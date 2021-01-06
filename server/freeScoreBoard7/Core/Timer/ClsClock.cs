using System;
using System.Collections.Generic;
using System.Reflection;
using FreeScoreBoard.Classes;
using FreeScoreBoard.Core.DB;
using FreeScoreBoard.Core.DBControler;
using FreeScoreBoard.Core.Variablen;
using FreeScoreBoard.Interpreter.Error;
using FreeScoreBoard.Server;

namespace FreeScoreBoard.Core.Timer
{
	public class ClsClock : IDisposable
	{
		private long myID = 0;
		private bool myStatus = false;
		private DB.Timer myTimer;
		private int myTimerValue = 0;
		private bool myDynVisibility = false;
		private string myVar = "";
		private string myClockName = "";

		public ClsClock()
		{
		}

		public ClsClock(DB.Timer Timer)
		{
			this.SetTimer(Timer);

			// Tickevent einrichten
			GlobalServerEvents.Timer_Tick += this.Timer_tick;
		}

		public void SetTimer(DB.Timer Timer)
		{
			this.myID = Timer.Nr;
			this.myStatus = false;
			this.myTimer = Timer;
			this.myTimerValue = Timer.StartSekunden;
			this.myDynVisibility = Timer.DisplayDynamisch;
			this.myVar = Timer.Variable;
			this.myClockName = Timer.Name;

			// Zeitereignisse lesen
			this.ReadTimerEvents();

			// set Variables
			this.SetVariable(this.myTimer.StartSekunden);

		}

		public bool DynDisplay
		{
			get
			{
				return this.myDynVisibility;
			}
		}

		public string Variable
		{
			get
			{
				return this.myVar;
			}
		}

		public string ClockName
		{
			get
			{
				return this.myClockName;
			}
		}

		private void ReadTimerEvents()
		{
			bool countdown = this.myTimer.Countdown;
			string sorting = "ASC";
			if (countdown)
			{
				sorting = "DESC";
			}

			this.EventListe.Clear();
			this.EventListe.AddRange(ClsTimerControler.AktiveTimerEvents(this.myTimer.Nr, sorting));

		}

		private readonly string Name = MethodBase.GetCurrentMethod().DeclaringType.Name;

		private readonly List<Timerevent> EventListe = new List<Timerevent>();

		public int Zeit
		{
			get
			{
				return this.myTimerValue;
			}

			set
			{
				this.myTimerValue = value;
				this.SetVariable(value);
			}
		}

		public long ID
		{
			get { return this.myID; }
		}

		public bool Status
		{
			get { return this.myStatus; }
			set { this.myStatus = value; }
		}

		public void Timer_tick(object sender, EventArgs e)
		{
			try
			{
				this.Tick_Zeit();

				for (int s = 0; s < this.EventListe.Count; s++)
				{
					if (this.EventListe[s].Sekunden == this.myTimerValue && this.myStatus)
					{
						// Ereignis auslösen und "ClsZeitkontroler.ZeitAusloeser" ausführen
						this.EventAusloesen(this, new ClsObjectEventArgs(this.EventListe[s]));
					}
				}
			}
			catch (Exception ex)
			{
				ClsError.Error(this.Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		public void ZeitReset()
		{
			if (!this.myStatus)
			{
				DB.Timer t = ClsTimerControler.Timer((int)this.myTimer.ID);
				this.SetTimer(t);

				this.myTimerValue = this.myTimer.StartSekunden;
				this.SetVariable(this.myTimerValue);

				// Zeitereignisse lesen
				this.ReadTimerEvents();
			}
		}

		private void Tick_Zeit()
		{
			if (!this.myStatus)
			{
				return;
			}

			if (!this.AbhaengigkeitOK())
			{
				return;
			}

			if (!this.myTimer.Countdown)
			{
				this.myTimerValue += 1;
			}
			else
			{
				this.myTimerValue -= 1;
			}

			this.SetVariable(this.myTimerValue);
		}

		private void SetVariable(int Value)
		{
			string strvalue = Value.ToString();

			if (this.myTimer.MinutenDarstellung)
			{
				strvalue = ClsFunktionen.Sek2uhr(Value);
			}

			ClsDBVariablen.Instance.SetTextVariableWert(this.myTimer.Variable, strvalue);
		}

		public void Start()
		{
			try
			{
				// autoreset
				if (this.myTimer.AutoReset)
				{
					this.ZeitReset();
				}

				this.myStatus = true;
			}
			catch (Exception ex)
			{
				ClsError.Error(this.Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		public void Stop()
		{
			try
			{
				this.myStatus = false;
			}
			catch (Exception ex)
			{
				ClsError.Error(this.Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		private bool AbhaengigkeitOK()
		{
			if (this.myTimer.AbhängigeTimerNr == 0)
			{
				return true; // keine Abhängigkeit
			}

			bool AbhaengigerTimerStatus = ClsZeitkontroler.Instance.GetClockStatus(this.myTimer.AbhängigeTimerNr);

			if (AbhaengigerTimerStatus == this.myTimer.AbhängigeTimerStatus)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		//// event start-------------------------
		private void EventAusloesen(object o, ClsObjectEventArgs e)
		{
			if (Zeitereignis != null)
			{
				Zeitereignis(o, e);
			}
		}

		public event OwnObjectEventHandler Zeitereignis;

		//// event stop-------------------------------

		public void Dispose()
		{
			GlobalServerEvents.Timer_Tick -= this.Timer_tick; ;
		}
	}
}
