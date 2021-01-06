using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using FreeScoreBoard.Classes;
using FreeScoreBoard.Core.DB;
using FreeScoreBoard.Core.DBControler;
using FreeScoreBoard.Core.Kontrolle;
using FreeScoreBoard.Core.Timer;
using FreeScoreBoard.Core.Variablen;
using FreeScoreBoard.Forms;
using FreeScoreBoard.Forms.Dialoge;
using FreeScoreBoard.Interpreter.Error;

namespace FreeScoreBoard.Core.Kontrolle
{
	public static class ClsKontrolfunktionen
	{
		private readonly static string Name = MethodBase.GetCurrentMethod().DeclaringType.Name;

		public static void TimeOutLeftChange(TSpiele spiel, string aoderb, string wert, bool nullen = false)
		{
			try
			{
				string mannschaft = string.Empty;
				switch (aoderb)
				{
					case "A":
						if (spiel != null)
						{
							mannschaft = spiel.IstMannA;
						}

						ClsDBVariablen.Instance.SetTextVariableWert("S39", wert);

						break;
					case "B":
						if (spiel != null)
						{
							mannschaft = spiel.IstMannB;
						}

						ClsDBVariablen.Instance.SetTextVariableWert("S40", wert);

						break;
				}

				// prüfen ob Ereignis gespeichert werden muss
				string SpielzeitStr = ClsDBVariablen.Instance.GetTextVariableWert("S08");
				if (ClsZeitkontroler.Instance.GetClockStatus(2)) //(ClsSpielzeitControler.Instance.Nachspielzeitlaeuft)
				{
					SpielzeitStr = SpielzeitStr + "+" + ClsDBVariablen.Instance.GetTextVariableWert("S35");
				}

				DateTime cpu = DateTime.Now;

				string freiSpielereignisse = ClsOptionsControler.Options3("Spielfreieereignisse").Value;

				if (spiel != null)
				{
					if (spiel.Spiel != 0 || (spiel.Spiel == 0 && freiSpielereignisse == "True"))
					{
						TEreignisse e = new TEreignisse();
						e.TurnierID = spiel.TurnierID;
						e.TurnierNr = spiel.TurnierNr;
						e.Spiel = spiel.Spiel;
						e.Mannschaft = mannschaft;
						e.Spielzeit = SpielzeitStr;
						e.Spielzeitrichtung = ClsTimerControler.Timer(ClsTimerControler.TimerID(1)).Countdown.ToString(); //ClsOptionsControler.Options3("Spielzeitrichtung").Value;
						e.CPUZeit = DateTime.Now;
						e.Ereignistyp = "16";

						if (nullen)
						{
							e.Mannschaft = string.Empty;
							e.Ereignistyp = "17";
						}

						e.Spieler = "";
						e.Details = wert;
						e.Spielabschnitt = ClsDBVariablen.Instance.GetBildVariableWert("S09"); // ClsSpielzeitControler.Instance.Spielabschnitt;
						ClsEreignisControler.AddEreignis(e);
					}
				}

				// update der Ereignistabelle
				ClsDBVariablen.Instance.SetTabellenVariableWert("T03", ClsTabellenfunktionen.EreignisseToVariable_Json(ClsGlobal.Instance.TurnierID));
			}
			catch (Exception ex)
			{
				ClsError.Error(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		public static void DownChange(TSpiele spiel, bool add, bool nullen = false, bool autoevent = false)
		{

		}

		public static void YardsToGoChange(TSpiele spiel, bool add, bool nullen = false, bool autoevent = false)
		{

		}

		public static void BallOnChange(TSpiele spiel, bool add, bool nullen = false, bool autoevent = false)
		{

		}

		public static void PossessionChange(TSpiele spiel, bool add, bool nullen = false, bool autoevent = false)
		{

		}

		public static void Dispose()
		{
			// throw new NotImplementedException();
		}

		public static void PaintballPlus(string Team)
		{
			try
			{
				string sollBild = ClsOptionsControler.Options3("Paintball Bild1").Value;

				if (Team == "A") // Team A
				{
					for (int i = 21; i > 16; i--)
					{
						string var = "B" + i;
						if (ClsDBVariablen.Instance.GetBildVariableWert(var) != sollBild)
						{
							ClsDBVariablen.Instance.SetBildVariableWert(var, sollBild);
							return;
						}
					}
				}
				else
				{
					// Team B
					for (int i = 26; i > 21; i--)
					{
						string var = "B" + i;
						if (ClsDBVariablen.Instance.GetBildVariableWert(var) != sollBild)
						{
							ClsDBVariablen.Instance.SetBildVariableWert(var, sollBild);
							return;
						}
					}
				}
			}
			catch (Exception ex)
			{
				ClsError.Error(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		public static void PaintballMinus(string Team)
		{
			try
			{
				string sollBild = ClsOptionsControler.Options3("Paintball Bild2").Value;

				if (Team == "A") // Team A
				{
					for (int i = 17; i < 22; i++)
					{
						string var = "B" + i;
						if (ClsDBVariablen.Instance.GetBildVariableWert(var) != sollBild)
						{
							ClsDBVariablen.Instance.SetBildVariableWert(var, sollBild);
							return;
						}
					}
				}
				else
				{
					// Team B
					for (int i = 22; i < 27; i++)
					{
						string var = "B" + i;
						if (ClsDBVariablen.Instance.GetBildVariableWert(var) != sollBild)
						{
							ClsDBVariablen.Instance.SetBildVariableWert(var, sollBild);
							return;
						}
					}
				}
			}
			catch (Exception ex)
			{
				ClsError.Error(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		public static void PaintballReset()
		{
			try
			{
				string sollBild = ClsOptionsControler.Options3("Paintball Bild1").Value;

				// Team A
				for (int i = 17; i < 22; i++)
				{
					string var = "B" + i;
					ClsDBVariablen.Instance.SetBildVariableWert(var, sollBild);
				}

				// Team B
				for (int i = 22; i < 27; i++)
				{
					string var = "B" + i;
					ClsDBVariablen.Instance.SetBildVariableWert(var, sollBild);
				}
			}
			catch (Exception ex)
			{
				ClsError.Error(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}
	}
}
