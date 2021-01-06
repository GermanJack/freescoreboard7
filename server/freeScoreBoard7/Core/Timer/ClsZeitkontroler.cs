using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FreeScoreBoard.Classes;
using FreeScoreBoard.Core.DB;
using FreeScoreBoard.Core.DBControler;
using FreeScoreBoard.Core.Display;
using FreeScoreBoard.Core.Variablen;
using FreeScoreBoard.Interpreter;
using FreeScoreBoard.Interpreter.Error;
//using FreeScoreBoard.Forms.Objekte;
using FreeScoreBoard.Server;

namespace FreeScoreBoard.Core.Timer
{
	public class ClsZeitkontroler
	{
		private int slideshowsek = 0;
		private int slideshowsekcounter = 0;
		private int slideschowpic = 0;
		private static readonly Lazy<ClsZeitkontroler> Lazy = new Lazy<ClsZeitkontroler>(() => new ClsZeitkontroler());

		public static ClsZeitkontroler Instance
		{
			get { return Lazy.Value; }
		}

		private readonly string Name = MethodBase.GetCurrentMethod().DeclaringType.Name;

		// Initialisieren des Heartbeats
		private ClsTimer mmTimer;

		// UhrList beinnhaltet alle Spieluhren
		private List<ClsClock> ClockList = new List<ClsClock>();

		private bool myHeartBeatStatus = false;

		public bool HeartBeatStatus
		{
			get
			{
				return this.myHeartBeatStatus;
			}

			set
			{
				try
				{
					this.HeartbeatOn(value);
					this.myHeartBeatStatus = value;
				}
				catch (Exception ex)
				{
					ClsError.CoreError(this.Name, MethodBase.GetCurrentMethod().ToString(), ex);
					this.myHeartBeatStatus = false;
					if (this.mmTimer != null)
					{
						this.myHeartBeatStatus = this.mmTimer.IsRunning;
					}
				}
			}
		}

		public List<ClsClock> PubClockList()
		{
			return this.ClockList;
		}

		/// <summary>
		/// Ein-/Ausschalten des Heartbeats
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void HeartbeatOn(bool status)
		{
			try
			{
				if (status)
				{
					this.mmTimer = new ClsTimer();
					this.mmTimer.Tick += this.Timer_Tick;
					this.mmTimer.Mode = Timer.TimerMode.Periodic;
					this.mmTimer.Resolution = 1;
					this.mmTimer.Period = 1000;
					//this.mmTimer.SynchronizingObject = this;
					this.mmTimer.Start();
				}
				else
				{
					if (this.mmTimer != null)
					{
						this.mmTimer.Stop();
						this.mmTimer.Tick -= this.Timer_Tick;
						this.mmTimer = null;

					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// - Auslösen des globalen Tick ereignisses 
		/// - Versorgen der Varialen S45 mit der Uhrzeit
		/// - Slideshow steueren
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Timer_Tick(object sender, System.EventArgs e)
		{
			try
			{
				GlobalServerEvents.Fire_Timer_Tick(this, EventArgs.Empty);

				// slideshow
				this.slideshowsek = Convert.ToInt32(ClsOptionsControler.Options3("Slideshowzeit").Value);
				if (this.slideshowsekcounter >= this.slideshowsek)
				{
					this.slideshowsekcounter = 0;
				}

				if (this.slideshowsekcounter == 0)
				{
					string[] picfiles = Directory.GetFiles(Path.Combine(ClsMain.PictureFolder, "Slideshow"));
					if (picfiles.Count() > 0)
					{
						string pic = "Slideshow/" + Path.GetFileName(picfiles[this.slideschowpic]);
						ClsDBVariablen.Instance.SetBildVariableWert("B29", pic);
					}

					if (this.slideschowpic == picfiles.Count() - 1)
					{
						this.slideschowpic = 0;
					}

					this.slideschowpic += 1;
				}

				this.slideshowsekcounter += 1;

				// slideshow ende

				// S45 mit Uhrzeit versorgen mit sekündlich wechselndem Doppelpunkt
				string zeit = DateTime.Now.ToShortTimeString();
				string altezeit = Variablen.ClsDBVariablen.Instance.GetTextVariableWert("S45");
				if (string.IsNullOrEmpty(altezeit))
				{
					altezeit = zeit;
				}

				if (altezeit.Substring(2, 1) == ":")
				{
					zeit = zeit.Replace(":", " ");
				}

				// Uhren in Zeitstrafen bedienen
				if (this.GetClockStatus(1))
				{
					if (ClsGlobal.Instance.Strafen1.Count > 0)
					{
						Kontrolle.ClsStrafen.Dotick(ClsGlobal.Instance.Strafen1, "A");
					}

					if (ClsGlobal.Instance.Strafen2.Count > 0)
					{
						Kontrolle.ClsStrafen.Dotick(ClsGlobal.Instance.Strafen2, "B");
					}
				}

				Variablen.ClsDBVariablen.Instance.SetTextVariableWert("S45", zeit);
			}
			catch (Exception ex)
			{
				ClsError.Error(this.Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		/// <summary>
		/// Initialisieren der Spieluhren mit Parametern aus der Datenbank
		/// </summary>
		public void IniUhren()
		{
			List<DB.Timer> tl = ClsTimerControler.Timers();

			if (tl.Count == 0)
			{
				return;
			}

			tl.Sort((a, b) => a.Nr.CompareTo(b.Nr));

			for (int i = 0; i < tl.Count; i++)
			{
				ClsClock uhr = new ClsClock(tl[i]);
				uhr.Zeitereignis += this.ZeitAusloeser;
				this.ClockList.Add(uhr);
			}
		}

		public bool GetClockStatus(long ClockID)
		{
			ClsClock clock = (from x in this.ClockList where x.ID == ClockID select x).FirstOrDefault();
			return clock.Status;
		}

		public void ClockStart(long ClockID)
		{
			ClsClock clock = (from x in this.ClockList where x.ID == ClockID select x).FirstOrDefault();
			clock.Status = true;
			Saveereignis(ClockID.ToString() + "_True");

			if (clock.DynDisplay)
			{
				ClsServer.Instance.SendMessage(ClsVarCom.ChangeDivVisibilityString(clock.Variable, "visible"));
			}
		}

		public void ClockStop(long ClockID)
		{
			ClsClock clock = (from x in this.ClockList where x.ID == ClockID select x).FirstOrDefault();
			clock.Status = false;
			Saveereignis(ClockID.ToString() + "_False");
			if (clock.DynDisplay)
			{
				ClsServer.Instance.SendMessage(ClsVarCom.ChangeDivVisibilityString(clock.Variable, "hidden"));
			}
		}

		/// <summary>
		/// Stellt die Uhr auf die in der DB gespeichte Zeit zurück
		/// </summary>
		/// <param name="Nr">mit 1 Startende Nummer der Uhr</param>
		public void ResetUhr(long ClockID)
		{
			ClsClock clock = (from x in this.ClockList where x.ID == ClockID select x).FirstOrDefault();
			clock.ZeitReset();
		}

		public void ResetAlleUhren()
		{
			for (int i = 0; i < this.ClockList.Count; i++)
			{
				ClsClock clock = this.ClockList[i];
				clock.ZeitReset();
			}
		}

		public void ManipulateClock(long ClockID, int Seconds)
		{
			ClsClock clock = (from x in this.ClockList where x.ID == ClockID select x).FirstOrDefault();
			clock.Zeit = Seconds;
		}

		public void Saveereignis(string ereignistyp)
		{
			long turnierID = ClsGlobal.Instance.TurnierID;
			int aktivesSpiel = 0;

			int timerID = ClsTimerControler.TimerID(1);
			bool countdown = ClsTimerControler.Timer(timerID).Countdown;

			if (ClsGlobal.Instance.AktivesSpiel != null)
			{
				aktivesSpiel = ClsGlobal.Instance.AktivesSpiel.Spiel;
			}

			if ((turnierID == 0 && ClsOptionsControler.Options3("Spielfreieereignisse").Value == "True") || turnierID != 0)
			{
				string SpielzeitStr = ClsDBVariablen.Instance.GetTextVariableWert("S08");

				// Nachspielzeit
				if (this.GetClockStatus(2) == true)
				{
					SpielzeitStr = SpielzeitStr + "+" + ClsDBVariablen.Instance.GetTextVariableWert("S35");
				}

				TEreignisse e = new TEreignisse
				{
					TurnierID = (int)turnierID,
					TurnierNr = 0,
					Spiel = aktivesSpiel,
					Mannschaft = string.Empty,
					Spielzeit = SpielzeitStr,
					Spielzeitrichtung = countdown.ToString(),
					CPUZeit = DateTime.Now,
					Ereignistyp = ereignistyp,
					Spieler = string.Empty,
					Details = "",
					Spielabschnitt = ClsDBVariablen.Instance.GetTextVariableWert("S09")
				};
				ClsEreignisControler.AddEreignis(e);
				ClsDBVariablen.Instance.SetTabellenVariableWert("T03", ClsTabellenfunktionen.EreignisseToVariable_Json((int)turnierID));
			}
		}

		private void ZeitAusloeser(object sender, ClsObjectEventArgs e)
		{
			Timerevent TimerEvent = (Timerevent)e.ObjectArgument;
			switch (TimerEvent.Eventtype)
			{
				case 0:
					{
						// Timer stoppen
						this.ClockStop(TimerEvent.TimerNr);
						GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "TimerObjects"))));
						break;
					}
				case 1:
					{
						// anderen Timer starten
						if (TimerEvent.AndereTimerNr > 0)
						{
							this.ClockStart(TimerEvent.AndereTimerNr);
							GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "TimerObjects"))));
						}

						break;
					}
				case 2:
					{
						// anderen Timer stoppen
						if (TimerEvent.AndereTimerNr > 0)
						{
							this.ClockStop(TimerEvent.AndereTimerNr);
							GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "TimerObjects"))));
						}

						break;
					}
				case 3:
					{
						// Audio abspielen
						ClsSoundplayer.Instance.TPlay(TimerEvent.Soundfile + "|" + 75);
						break;
					}
				case 4:
					{
						// Layer wechsel
						if (!string.IsNullOrEmpty(TimerEvent.Layer))
						{
							long id = Convert.ToInt64(TimerEvent.Layer);

							DisplayPageSet ps = (from x in ClsDisplayControler.DisplayPageSets() where x.ID == ClsPageSets.ActivePageSet() select x).FirstOrDefault();

							// prüfen ob ID tatsächlich noch existiert
							long p = (from x in ClsDisplayControler.DisplayPagesForPageSet(ps.ID) where x.ID == id select x.ID).FirstOrDefault();

							if (p != 0)
							{
								ClsDisplay.SetPage(ClsPageSets.ActivePageSet(), (int)p);

								GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "ActivePage"))));

								GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "DivsActivePage", pageSet: ps.PageSetName, page: p.ToString()))));
							}
						}

						break;
					}
			}
		}

		//// event start-------------------------
		private static void EventAusloesen(object o, ClsStringEventArgs e)
		{
			if (Zeitereignis != null)
			{
				Zeitereignis(o, e);
			}
		}

		public static event OwnStringEventHandler Zeitereignis;

		public static void Dispose()
		{
		}
		//// event stop-------------------------------
	}
}
