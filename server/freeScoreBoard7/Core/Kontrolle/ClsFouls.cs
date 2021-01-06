using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FreeScoreBoard.Core.DB;
using FreeScoreBoard.Core.DBControler;
using FreeScoreBoard.Core.Timer;
using FreeScoreBoard.Core.Variablen;
using FreeScoreBoard.Interpreter.Error;

namespace FreeScoreBoard.Core.Kontrolle
{
	public static class ClsFouls
	{
		private readonly static string Name = MethodBase.GetCurrentMethod().DeclaringType.Name;

		public static void Foul_Reset(TSpiele spiel)
		{
			ClsDBVariablen.Instance.SetTextVariableWert("S18", "0");
			ClsDBVariablen.Instance.SetTextVariableWert("S19", "0");

			// Ereignis schreiben
			string SpielzeitStr = ClsDBVariablen.Instance.GetTextVariableWert("S08");
			if (ClsZeitkontroler.Instance.GetClockStatus(2)) // ClsSpielzeitControler.Instance.Nachspielzeitlaeuft)
			{
				SpielzeitStr = SpielzeitStr + "+" + ClsDBVariablen.Instance.GetTextVariableWert("S35");
			}

			DateTime cpu = DateTime.Now;

			string freiSpielereignisse = ClsOptionsControler.Options3("Spielfreieereignisse").Value;

			if (spiel.Spiel != 0 || (spiel.Spiel == 0 && freiSpielereignisse == "True"))
			{
				TEreignisse e = new TEreignisse
				{
					TurnierID = spiel.TurnierID,
					TurnierNr = spiel.TurnierNr,
					Spiel = spiel.Spiel,
					Mannschaft = "",
					Spielzeit = SpielzeitStr,
					Spielzeitrichtung = ClsTimerControler.Timer(ClsTimerControler.TimerID(1)).Countdown.ToString(), //ClsOptionsControler.Options3("Spielzeitrichtung").Value,
					CPUZeit = DateTime.Now,
					Ereignistyp = "11",
					Details = spiel.ToreA.ToString() + ":" + spiel.ToreB.ToString(),
					Spielabschnitt = ClsDBVariablen.Instance.GetBildVariableWert("S09")
				};

				ClsEreignisControler.AddEreignis(e);
			}
		}

		public static async void Foul_change(TSpiele spiel, string aoderb, int add, int spielerID = 0)
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

						int foul1 = Convert.ToInt32("0" + ClsDBVariablen.Instance.GetTextVariableWert("S18")) + add;

						if (foul1 < 0)
						{
							foul1 = 0;
						}

						ClsDBVariablen.Instance.SetTextVariableWert("S18", foul1.ToString());

						break;
					case "B":
						if (spiel != null)
						{
							mannschaft = spiel.IstMannB;
						}

						int foul2 = Convert.ToInt32("0" + ClsDBVariablen.Instance.GetTextVariableWert("S19")) + add;

						if (foul2 < 0)
						{
							foul2 = 0;
						}

						ClsDBVariablen.Instance.SetTextVariableWert("S19", foul2.ToString());

						break;
				}

				await Task.Run(() => Foul_nebenereignisse(spiel, aoderb, add, mannschaft, spielerID));
			}
			catch (Exception ex)
			{
				ClsError.Error(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		public static void Foul_nebenereignisse(TSpiele spiel, string aoderb, int add, string mannschaft, int spielerID = 0)
		{
			try
			{
				string Spielername = "";
				if (spielerID != 0)
				{
					Spieler sp = ClsMannschaftenControler.Spieler(spielerID);
					Spielername = sp.Nachname + ", " + sp.Vorname;
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
						TEreignisse e = new TEreignisse
						{
							TurnierID = spiel.TurnierID,
							TurnierNr = spiel.TurnierNr,
							Spiel = spiel.Spiel,
							Mannschaft = mannschaft,
							Spielzeit = SpielzeitStr,
							Spielzeitrichtung = ClsTimerControler.Timer(ClsTimerControler.TimerID(1)).Countdown.ToString(), //ClsOptionsControler.Options3("Spielzeitrichtung").Value,
							CPUZeit = DateTime.Now,
							Ereignistyp = "06"
						};

						e.Spieler = Spielername;
						e.Details = add.ToString();
						e.Spielabschnitt = ClsDBVariablen.Instance.GetBildVariableWert("S09"); // ClsSpielzeitControler.Instance.Spielabschnitt;
						ClsEreignisControler.AddEreignis(e);
					}
				}

				ClsDBVariablen.Instance.SetTabellenVariableWert("T03", ClsTabellenfunktionen.EreignisseToVariable_Json(ClsGlobal.Instance.TurnierID));
				ClsDBVariablen.Instance.SetTextVariableWert("S38", spielerID.ToString());

				if (ClsGlobal.Instance.TurnierID != 0)
				{
					//// ClsTurnierControler.SaveFoul(ClsGlobal.Instance.AktivesSpiel);
					//// Foulstand wird bisher nicht an spiel gespeichert
					//// Fouls werden während des spiels manchmal genullt z.B.: 5 Fouls = 1 x 7 Meter starfstoß
				}
			}
			catch (Exception ex)
			{
				ClsError.Error(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}


	}
}
