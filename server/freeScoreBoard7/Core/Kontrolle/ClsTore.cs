using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FreeScoreBoard.Classes;
using FreeScoreBoard.Core.DB;
using FreeScoreBoard.Core.DBControler;
using FreeScoreBoard.Core.Timer;
using FreeScoreBoard.Core.Variablen;
using FreeScoreBoard.Interpreter.Error;

namespace FreeScoreBoard.Core.Kontrolle
{
	public static class ClsTore
	{
		private readonly static string Name = MethodBase.GetCurrentMethod().DeclaringType.Name;

		public static void Tore_Reset(TSpiele spiel)
		{
			ClsDBVariablen.Instance.SetTextVariableWert("S05", "0");
			ClsDBVariablen.Instance.SetTextVariableWert("S06", "0");

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
					Ereignistyp = "10",
					Details = spiel.ToreA.ToString() + ":" + spiel.ToreB.ToString(),
					Spielabschnitt = ClsDBVariablen.Instance.GetBildVariableWert("S09")
				};

				ClsEreignisControler.AddEreignis(e);
			}
		}

		public static async void Tore_change(TSpiele spiel, string aoderb, int add, int spielerID = 0)
		{
			try
			{
				string mannschaft = "";
				switch (aoderb)
				{
					case "A":
						if (spiel != null)
						{
							mannschaft = spiel.IstMannA;
						}

						int tore1 = spiel.ToreA + add;

						if (spiel != null)
						{
							spiel.ToreA = tore1;
						}

						ClsDBVariablen.Instance.SetTextVariableWert("S05", tore1.ToString());

						break;
					case "B":
						if (spiel != null)
						{
							mannschaft = spiel.IstMannB;
						}

						int tore2 = spiel.ToreB + add;

						if (spiel != null)
						{
							spiel.ToreB = tore2;
						}

						ClsDBVariablen.Instance.SetTextVariableWert("S06", tore2.ToString());

						break;
				}

				await Task.Run(() => Tor_nebenereignisse(spiel, aoderb, add, mannschaft, spielerID));
			}
			catch (Exception ex)
			{
				ClsError.Error(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		private static void Tor_nebenereignisse(TSpiele spiel, string aoderb, int add, string mannschaft, int spielerID)
		{
			try
			{
				ClsDBVariablen.Instance.SetTextVariableWert("S12", ClsFunktionen.Setspielstand());

				// spiel speichern
				if (spiel != null)
				{
					if (spiel.Spiel != 0)
					{
						ClsTurnierControler.SaveSpiel(ClsGlobal.Instance.AktivesSpiel);
					}
				}

				// prüfen ob Ereignis gespeichert werden muss
				string SpielzeitStr = ClsDBVariablen.Instance.GetTextVariableWert("S08");
				if (ClsZeitkontroler.Instance.GetClockStatus(2)) // ClsSpielzeitControler.Instance.Nachspielzeitlaeuft)
				{
					SpielzeitStr = SpielzeitStr + "+" + ClsDBVariablen.Instance.GetTextVariableWert("S35");
				}

				DateTime cpu = DateTime.Now;

				string freiSpielereignisse = ClsOptionsControler.Options3("Spielfreieereignisse").Value;


				string Spielername = "";
				if (spielerID != 0)
				{
					Spieler spieler = ClsMannschaftenControler.Spieler(spielerID);
					Spielername = spieler.Nachname + ", " + spieler.Vorname;
				}

				if (spiel.Spiel != 0 || (spiel.Spiel == 0 && freiSpielereignisse == "True"))
				{
					TEreignisse e = new TEreignisse
					{
						TurnierID = spiel.TurnierID,
						TurnierNr = spiel.TurnierNr,
						Spiel = spiel.Spiel,
						Mannschaft = mannschaft,
						Spieler = Spielername,
						Spielzeit = SpielzeitStr,
						Spielzeitrichtung = ClsTimerControler.Timer(ClsTimerControler.TimerID(1)).Countdown.ToString(), //ClsOptionsControler.Options3("Spielzeitrichtung").Value,
						CPUZeit = DateTime.Now,
						Ereignistyp = "05"
					};


					ClsEreignisControler.AddEreignis(e);
				}

				// update der Ereignistabelle
				ClsDBVariablen.Instance.SetTabellenVariableWert("T03", ClsTabellenfunktionen.EreignisseToVariable_Json(ClsGlobal.Instance.TurnierID));

				// update der Torschützentabelle
				ClsTabellenfunktionen.TorschuetzeToVariable(ClsGlobal.Instance.TurnierID);

				// einstellen des Torschützen als aktive gewählten Spieler
				ClsDBVariablen.Instance.SetTextVariableWert("S38", spielerID.ToString());

				// prüfen ob ein Torton abgespielt werden soll
				if (add > 0)
				{
					string torton = string.Empty;

					if (ClsOptionsControler.Options3("individualTorsound").Value != "True")
					{
						torton = ClsOptionsControler.Options3("Tor").Value;
					}
					else
					{
						DB.Mannschaften m = new DB.Mannschaften();
						if (aoderb == "A")
						{
							m = ClsMannschaftenControler.Mannschaft(spiel.IstMannA);
						}
						else
						{
							m = ClsMannschaftenControler.Mannschaft(spiel.IstMannB);
						}

						if (m != null)
						{
							torton = m.Torton;
						}
					}

					if (torton != string.Empty)
					{
						ClsSoundplayer.Instance.TPlay(torton + "|" + "30");
					}
				}
			}
			catch (Exception ex)
			{
				ClsError.Error(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}


	}
}
