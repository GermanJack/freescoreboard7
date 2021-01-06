using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FreeScoreBoard.Core.DB;
using FreeScoreBoard.Core.DBControler;
using FreeScoreBoard.Core.Kontrolle;
using FreeScoreBoard.Core.Timer;
using FreeScoreBoard.Core.Variablen;
using FreeScoreBoard.Interpreter.Error;

namespace FreeScoreBoard.Core.Turnier
{
	public class ClsTurnierfunktionen
	{
		private readonly static string Name = MethodBase.GetCurrentMethod().DeclaringType.Name;

		private static void Turnierwechsel(int turnierID)
		{
			// Tabellenfilter
			ClsTabellenfunktionen.IniTableFilter();

			// TurnierTabellen
			ClsDBVariablen.Instance.SetTabellenVariableWert("T01", ClsTabellenfunktionen.TabelleToVariable_Json(turnierID));

			// te - TurnierErgebnisse(spielplan)
			ClsDBVariablen.Instance.SetTabellenVariableWert("T02", ClsTabellenfunktionen.SpielplanToVariable_Json(turnierID));

			// tg - TurnierGruppen (und Combofelder zum filtern der Tabelleninhalten füllen)
			////this.LoadGruppenToTG();

			// tn - TurnierEreignisse
			ClsDBVariablen.Instance.SetTabellenVariableWert("T03", ClsTabellenfunktionen.EreignisseToVariable_Json(turnierID));

			// th - Torjäger
			ClsTabellenfunktionen.TorschuetzeToVariable(turnierID);
		}

		public static int LoadTurnier(int turnierID)
		{
			try
			{
				TKopf k = ClsTurnierControler.Turnier(turnierID);
				if (k == null)
				{
					k = new TKopf();
					k.TurnierNr = 0;
				}

				ClsDBVariablen.Instance.SetTextVariableWert("S13", k.TurnierNr.ToString());

				if (turnierID == 0)
				{
					// Turniername auf "freies Spielen" setzen
					ClsDBVariablen.Instance.SetTextVariableWert("S14", "freies Spielen");

					// Spiel 0 initialisieren
					SetSpiel0();

					// Spiel next initialisieren
					TSpiele nextspiel = null;
					ClsGlobal.Instance.NextSpiel = nextspiel;
					ClsDBVariablen.Instance.SetTextVariableWert("S16", "");
					ClsDBVariablen.Instance.SetTextVariableWert("S48", "0"); // Runde_next
					ClsDBVariablen.Instance.SetTextVariableWert("S49", "0"); // Gruppe_next
					ClsDBVariablen.Instance.SetTextVariableWert("S50", "0"); // Spiel_next

					// Spielabschnitt auf 1 setzten
					ClsSpielAbschnittControler.Instance.Spielabschnitt = 1;

					// Zeiten initialisieren
					ClsZeitkontroler.Instance.ResetAlleUhren();

					// Tabellenvariablen füllen
					Turnierwechsel(turnierID);

					return 0;
				}

				// prüfen ob turnier bereits beendet
				if (k.status == 3)
				{
					// MessageBox.Show("Turnier ist bereits beendet und kann nicht geladen werden.", "Meldung...");
					////ClsTranslateControls.ShowMessage("M0014", "Meldung", new object[] { "\n" }, MessageBoxButtons.OK);
					ClsDBVariablen.Instance.SetTextVariableWert("S48", ""); // Runde_next
					ClsDBVariablen.Instance.SetTextVariableWert("S49", ""); // Gruppe_next
					ClsDBVariablen.Instance.SetTextVariableWert("S50", ""); // Spiel_next
				}

				// neues Turnier laden
				// tk - TurnierKopf
				TKopf tkk = ClsTurnierControler.Turnier(turnierID);
				if (tkk != null)
				{
					// tk.Add(tkk);
					ClsDBVariablen.Instance.SetTextVariableWert("S14", tkk.Beschreibung);
				}

				// aktuelle Runde ermitteln
				List<TRunden> runden = ClsTurnierControler.Runden(turnierID);
				int runde = (from r in runden
							 where r.TurnierID == ClsGlobal.Instance.TurnierID && r.status == 1
							 select r.Runde).FirstOrDefault();

				// Naechste Begegnungen
				ClsSpielfunktionen.SetNext(ClsSpielfunktionen.FindNext(0, turnierID));


				// aktives spiel auf null setzen
				ClsGlobal.Instance.AktivesSpiel = null;

				// Tabellenvariablen füllen
				Turnierwechsel(turnierID);

				return 0;
			}
			catch (Exception ex)
			{
				ClsError.Error(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return 0;
			}
		}

		private static void SetSpiel0()
		{
			// Spiel 0 initialisieren
			TSpiele spiel = new TSpiele
			{
				Spiel = 0,
				Runde = 0,
				Gruppe = "0",
			};

			ClsGlobal.Instance.AktivesSpiel = spiel;
			ClsSpielfunktionen.SetManName("A", "Heim");
			ClsSpielfunktionen.SetManName("B", "Gast");
			ClsDBVariablen.Instance.SetTextVariableWert("S05", "0");
			ClsDBVariablen.Instance.SetTextVariableWert("S06", "0");
			ClsDBVariablen.Instance.SetTextVariableWert("S15", "Heim : Gast");
			ClsDBVariablen.Instance.SetTextVariableWert("S12", ClsFunktionen.Setspielstand());
		}


	}
}
