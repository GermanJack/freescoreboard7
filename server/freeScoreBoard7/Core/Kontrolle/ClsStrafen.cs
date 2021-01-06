using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FreeScoreBoard.Classes;
using FreeScoreBoard.Core.DB;
using FreeScoreBoard.Core.DBControler;
using FreeScoreBoard.Core.Objekte;
using FreeScoreBoard.Core.Timer;
using FreeScoreBoard.Core.Variablen;
using FreeScoreBoard.Interpreter.Error;

namespace FreeScoreBoard.Core.Kontrolle
{
	public static class ClsStrafen
	{
		private readonly static string Name = MethodBase.GetCurrentMethod().DeclaringType.Name;

		public static void StrafeNeu(string aoderb, string SpielerID, string StrafenID)
		{
			try
			{
				int strafenID = Convert.ToInt32(StrafenID);

				DateTime cpu = DateTime.Now;

				string mannschaft = string.Empty;
				if (aoderb == "A")
				{
					mannschaft = ClsGlobal.Instance.AktivesSpiel.IstMannA;
				}
				else
				{
					mannschaft = ClsGlobal.Instance.AktivesSpiel.IstMannB;
				}

				string spielerName = ClsFunktionen.MakeSpielerNameOhneSID(SpielerID);

				// Strafdaten
				Strafen strafe = ClsOptionsControler.Strafe(strafenID);

				// 03 = Zeitstrafe
				if (strafe.Art == "03")
				{
					// Zeitstarfe
					ClsZeitstrafe z = new ClsZeitstrafe();

					z.Mannschaft = mannschaft;
					z.SpielerID = SpielerID;
					z.Spieler = spielerName;
					z.Zeit = strafe.Sekunden;
					z.Minuten = ClsFunktionen.Sek2uhr(z.Zeit);
					z.Bezeichnung = strafe.Bezeichnung;

					if (aoderb == "A")
					{
						if (ClsGlobal.Instance.Strafen1.Any())
						{
							z.ID = (from x in ClsGlobal.Instance.Strafen1 select x.ID).Max() + 1;
						}
						else
						{
							z.ID = 1;
						}

						ClsGlobal.Instance.Strafen1.Add(z);

						ClsTabellenfunktionen.StrafenToVariable(ClsGlobal.Instance.Strafen1, aoderb, "std");
					}
					else if (aoderb == "B")
					{
						if (ClsGlobal.Instance.Strafen2.Any())
						{
							z.ID = (from x in ClsGlobal.Instance.Strafen2 select x.ID).Max() + 1;
						}
						else
						{
							z.ID = 1;
						}

						ClsGlobal.Instance.Strafen2.Add(z);

						ClsTabellenfunktionen.StrafenToVariable(ClsGlobal.Instance.Strafen2, aoderb, "std");
					}

					// Ereignis
					if (
						(ClsGlobal.Instance.TurnierID == 0 && ClsOptionsControler.Options3("Spielfreieereignisse").Value == "True")
						||
						(ClsGlobal.Instance.TurnierID != 0 && ClsGlobal.Instance.AktivesSpiel != null)
					   )
					{
						string SpielzeitStr = ClsDBVariablen.Instance.GetTextVariableWert("S08");
						if (ClsZeitkontroler.Instance.GetClockStatus(2))
						{
							SpielzeitStr = SpielzeitStr + "+" + ClsDBVariablen.Instance.GetTextVariableWert("S35");
						}

						string Spielzeitrichtung = ClsTimerControler.Timer(ClsTimerControler.TimerID(1)).Countdown.ToString();

						TEreignisse e = new TEreignisse
						{
							TurnierID = ClsGlobal.Instance.TurnierID,
							TurnierNr = 0,
							Spiel = (int)ClsGlobal.Instance.AktivesSpiel.Spiel,
							Mannschaft = mannschaft,
							Spielzeit = SpielzeitStr,
							Spielzeitrichtung = Spielzeitrichtung,
							CPUZeit = DateTime.Now,
							Ereignistyp = "07",
							Spieler = spielerName,
							Details = strafe.Bezeichnung,
							Spielabschnitt = ClsDBVariablen.Instance.GetTextVariableWert("S09")
						};

						ClsEreignisControler.AddEreignis(e);
					}

					ClsDBVariablen.Instance.SetTabellenVariableWert("T03", ClsTabellenfunktionen.EreignisseToVariable_Json(ClsGlobal.Instance.TurnierID));

					// Spieler wählen
					ClsDBVariablen.Instance.SetTextVariableWert("S38", SpielerID);
				}
			}
			catch (Exception ex)
			{
				ClsError.Error(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		public static void DeleteStrafen()
		{
			try
			{
				ClsGlobal.Instance.Strafen1.Clear();
				ClsGlobal.Instance.Strafen2.Clear();

				ClsTabellenfunktionen.StrafenToVariable(ClsGlobal.Instance.Strafen1, "A", "std");

				ClsTabellenfunktionen.StrafenToVariable(ClsGlobal.Instance.Strafen2, "B", "std");
			}
			catch (Exception ex)
			{
				ClsError.Error(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		public static void Dotick(List<ClsZeitstrafe> zeitstrafen, string aoderb)
		{
			try
			{
				//// bool removed = false;

				if (zeitstrafen.Count > 0)
				{
					zeitstrafen.RemoveAll(x => x.Zeit == 0);
					//// removed = true;
				}

				string parStraf = ClsOptionsControler.Options3("Parallelstrafen").Value;
				int parStrafInt = Convert.ToInt32(parStraf);

				if (parStrafInt == 0 || parStrafInt > zeitstrafen.Count)
				{
					parStrafInt = zeitstrafen.Count;
				}

				for (int i = 0; i < parStrafInt; i++)
				{
					if (zeitstrafen[i].Zeit > 0)
					{
						zeitstrafen[i].Zeit -= 1;
						zeitstrafen[i].Minuten = ClsFunktionen.Sek2uhr(zeitstrafen[i].Zeit);
						if (zeitstrafen[i].Zeit == 0)
						{
							string endeton = ClsOptionsControler.Strafe(zeitstrafen[i].Bezeichnung).EndeTon;
							ClsSoundplayer.Instance.TPlay(endeton + "|" + "20");
						}

						Strafen straf = ClsOptionsControler.Strafe(zeitstrafen[i].Bezeichnung);
						if (zeitstrafen[i].Zeit <= straf.Countdowndauer && zeitstrafen[i].Zeit != 0)
						{
							ClsSoundplayer.Instance.TPlay(straf.TonCountdown + "|" + "21");
						}
					}
				}

				ClsTabellenfunktionen.StrafenToVariable(zeitstrafen, aoderb, "std");

			}
			catch (Exception ex)
			{
				ClsError.Error(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		public static void Delete(string aoderb, string StrafID)
		{
			int sid = Convert.ToInt32(StrafID);
			if (aoderb == "A")
			{
				if (ClsGlobal.Instance.Strafen1.Count > 0)
				{
					ClsZeitstrafe zs = (from x in ClsGlobal.Instance.Strafen1 where x.ID == sid select x).Single();
					if (zs != null)
					{
						ClsGlobal.Instance.Strafen1.Remove(zs);
					}

					ClsTabellenfunktionen.StrafenToVariable(ClsGlobal.Instance.Strafen1, "A", "std");
				}
			}
			else
			{
				if (ClsGlobal.Instance.Strafen2.Count > 0)
				{
					ClsZeitstrafe zs = (from x in ClsGlobal.Instance.Strafen2 where x.ID == sid select x).Single();
					if (zs != null)
					{
						ClsGlobal.Instance.Strafen2.Remove(zs);
					}

					ClsTabellenfunktionen.StrafenToVariable(ClsGlobal.Instance.Strafen2, "A", "std");
				}
			}
		}

		public static void Manipulate(string aoderb, string StrafID, string Time)
		{
			int sid = Convert.ToInt32(StrafID);
			int sec = Convert.ToInt32(Time);
			if (aoderb == "A")
			{
				ClsZeitstrafe zs = (from x in ClsGlobal.Instance.Strafen1 where x.ID == sid select x).Single();


				if (zs != null)
				{
					zs.Zeit = sec;
					zs.Minuten = ClsFunktionen.Sek2uhr(sec);
				}

				ClsTabellenfunktionen.StrafenToVariable(ClsGlobal.Instance.Strafen1, "A", "std");
			}
			else
			{
				ClsZeitstrafe zs = (from x in ClsGlobal.Instance.Strafen2 where x.ID == sid select x).Single();


				if (zs != null)
				{
					zs.Zeit = sec;
					zs.Minuten = ClsFunktionen.Sek2uhr(sec);
				}

				ClsTabellenfunktionen.StrafenToVariable(ClsGlobal.Instance.Strafen2, "B", "std");
			}
		}
	}
}
