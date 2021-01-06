using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FreeScoreBoard.Core.DB;
using FreeScoreBoard.Core.DBControler;
using FreeScoreBoard.Core.Variablen;
using FreeScoreBoard.Interpreter;
using FreeScoreBoard.Interpreter.Error;

namespace FreeScoreBoard.Core.Turnier
{
	public class ClsSpielfunktionen
	{
		private readonly static string Name = MethodBase.GetCurrentMethod().DeclaringType.Name;

		/// <summary>
		/// Gibt das TSpiele-Objekt zurück für die SpielNr innerhalb des Turnieres. (SpielNR is nicht gleich SpielID)
		/// </summary>
		/// <param name="SpielNr"></param>
		/// <returns></returns>
		public static TSpiele MatchByNr(int SpielNr)
		{
			return ClsTurnierControler.Spiel(ClsGlobal.Instance.TurnierID, SpielNr);
		}

		/// <summary>
		/// Übernimmt nächstes Spiel als aktuelles Spiel
		/// </summary>
		/// <param name="letzesBeenden"></param>
		/// <param name="beidenichts"></param>
		public static void NextGame(bool letzesBeenden = true, bool beidenichts = false)
		{
			try
			{
				bool rundenwechsel = false;

				// letztes spiel beenden
				if (ClsGlobal.Instance.AktivesSpiel != null && letzesBeenden == true)
				{
					int ret = 0;
					ret = Spiel_beenden(ClsGlobal.Instance.AktivesSpiel, beidenichts);

					if (ret == 0)
					{
						return;
					}

					rundenwechsel |= ret == 2;
				}

				// Anzeige löschen
				ClsGlobal.Instance.AktivesSpiel = null;
				ClsDBVariablen.Instance.SetTextVariableWert("S05", "0");  // Tore1
				ClsDBVariablen.Instance.SetTextVariableWert("S06", "0");  // Tore2
				ClsDBVariablen.Instance.SetTextVariableWert("S09", "1");  // Spielabschnitt

				SetManName("A", "");
				SetManName("B", "");
				ClsDBVariablen.Instance.SetTextVariableWert("S05", "0"); // Tore1
				ClsDBVariablen.Instance.SetTextVariableWert("S06", "0"); // Tore2

				if (ClsGlobal.Instance.NextSpiel == null)
				{
					if (rundenwechsel == true)
					{
						SetNext(FindNext(0, ClsGlobal.Instance.TurnierID));
					}
					else
					{
						return;
					}
				}
				else
				{
					// neues Spiel als aktives Spiel setzten
					ClsGlobal.Instance.AktivesSpiel = ClsGlobal.Instance.NextSpiel;

					// Anzeige setzten
					SetManName("A", ClsGlobal.Instance.AktivesSpiel.IstMannA);
					SetManName("B", ClsGlobal.Instance.AktivesSpiel.IstMannB);
					////CtrlSpielInfo1.Spiel = myaktivesSpiel;

					if (ClsGlobal.Instance.AktivesSpiel.ToreA != 0 || ClsGlobal.Instance.AktivesSpiel.ToreB != 0)
					{
						ClsMessage.SendMessage("Das Spiel war schon einma aktiv.\nDie bereits erziehlten Tore werden übernommen.", "Achtung...");
						////ClsTranslateControls.ShowMessage("M0015", "Achtung", new object[] { "\n" }, MessageBoxButtons.OK);
						ClsDBVariablen.Instance.SetTextVariableWert("S05", ClsGlobal.Instance.AktivesSpiel.ToreA.ToString());   // Tore1
						ClsDBVariablen.Instance.SetTextVariableWert("S06", ClsGlobal.Instance.AktivesSpiel.ToreB.ToString());   // Tore2
					}

					ClsDBVariablen.Instance.SetTextVariableWert("S05", ClsGlobal.Instance.AktivesSpiel.ToreA.ToString());    // Tore1
					ClsDBVariablen.Instance.SetTextVariableWert("S06", ClsGlobal.Instance.AktivesSpiel.ToreA.ToString());    // Tore2
					ClsDBVariablen.Instance.SetTextVariableWert("S15", ClsGlobal.Instance.AktivesSpiel.IstMannA + " : " + ClsGlobal.Instance.AktivesSpiel.IstMannB);    // Begegnung

					if (ClsGlobal.Instance.AktivesSpiel.SPlatz != 0)
					{
						ClsDBVariablen.Instance.SetTextVariableWert("S20", "Spiel um Platz: " + ClsGlobal.Instance.AktivesSpiel.SPlatz.ToString()); // Spieleinfo
					}

					// Nextebegegnung setzen
					if (rundenwechsel == false)
					{
						// naechstes Spiel
						SetNext(FindNext(1, ClsGlobal.Instance.TurnierID));

						// wenn spiele nicht beendet werden, würde next = aktiv werden
						if (ClsGlobal.Instance.NextSpiel != null)
						{
							if (ClsGlobal.Instance.NextSpiel.Spiel == ClsGlobal.Instance.AktivesSpiel.Spiel)
							{
								SetNext(FindNext(2, ClsGlobal.Instance.TurnierID));
							}
						}

						// an ende einer Rund würde next = null werden
						// dann müssen die nicht beendeten spiele dran kommen
						if (ClsGlobal.Instance.NextSpiel == null)
						{
							SetNext(FindNext(0, (int)ClsGlobal.Instance.TurnierID));
							if (ClsGlobal.Instance.NextSpiel.Spiel == ClsGlobal.Instance.AktivesSpiel.Spiel)
							{
								SetNext(null);
							}
						}
					}
					else
					{
						SetNext(FindNext(0, (int)ClsGlobal.Instance.TurnierID));
					}

					ClsDBVariablen.Instance.SetTabellenVariableWert("T02", ClsTabellenfunktionen.SpielplanToVariable_Json((int)ClsGlobal.Instance.TurnierID));
				}
			}
			catch (Exception ex)
			{
				ClsError.Error(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		/// <summary>
		/// Spielendebehandlung
		/// </summary>
		/// <param name="spiel"></param>
		/// <param name="beidenichts"></param>
		/// <returns></returns>
		public static int Spiel_beenden(TSpiele spiel, bool beidenichts = false)
		{
			try
			{
				bool rundenwechsel = false;
				if (beidenichts == false)
				{
					// wenn nur 2 Mannschaften in Gruppe dann Unentschieden nicht erlaubt
					List<TSpiele> sl1 = ClsTurnierControler.Spiele(ClsGlobal.Instance.TurnierID);
					int anzGrMan = (from TSpiele tee in sl1
									where tee.Gruppe == spiel.Gruppe
									select tee).Count();

					if (anzGrMan == 1 && spiel.ToreA == spiel.ToreB)
					{
						ClsMessage.SendMessage("In dieser Gruppe ist kein Unentschieden erlaubt.\nSpiel nicht beendet.", "Geht nicht...");
						////ClsTranslateControls.ShowMessage("M0016", "Meldung", new object[] { "\n" }, MessageBoxButtons.OK);
						return 0;
					}
				}

				// Spiel beenden
				if (beidenichts == false)
				{
					ClsTurnierControler.SaveSpielStatus(spiel, 3);
				}
				else
				{
					ClsTurnierControler.SaveSpielStatus(spiel, 4);
				}

				ClsDBVariablen.Instance.SetTabellenVariableWert("T02", ClsTabellenfunktionen.SpielplanToVariable_Json(ClsGlobal.Instance.TurnierID));

				// Punkte vergeben
				if (beidenichts == false)
				{
					SpielPunkteSpeichern(ClsGlobal.Instance.TurnierID, spiel.Spiel);
				}

				// Tabelle berechnen
				TabCalc(ClsGlobal.Instance.TurnierID, spiel.Gruppe);

				// prüfen ob Spiel um platz wenn ja, platz in Tabelle eintragen
				TKopf tkk = ClsTurnierControler.Turnier(ClsGlobal.Instance.TurnierID);
				if (tkk.Platzierungstyp == "P")
				{
					if (spiel.SPlatz != 0)
					{
						if (spiel.ToreA > spiel.ToreB)
						{
							SaveTabellenplatz(spiel.Gruppe, spiel.IstMannA, spiel.SPlatz);
						}
						else if (spiel.ToreA < spiel.ToreB)
						{
							SaveTabellenplatz(spiel.Gruppe, spiel.IstMannB, spiel.SPlatz);
						}
					}

					if (spiel.VPlatz != 0)
					{
						if (spiel.ToreA > spiel.ToreB)
						{
							SaveTabellenplatz(spiel.Gruppe, spiel.IstMannB, spiel.VPlatz);
						}
						else if (spiel.ToreA < spiel.ToreB)
						{
							SaveTabellenplatz(spiel.Gruppe, spiel.IstMannA, spiel.VPlatz);
						}
					}
				}

				// Prüfen ob alle Spiele der Gruppe beendet sind
				List<TSpiele> spiele1 = ClsTurnierControler.Spiele(ClsGlobal.Instance.TurnierID, spiel.Gruppe);
				int g = (from grp in spiele1
						 where grp.Status < 3
						 select grp).Count();
				if (g == 0)
				{
					// Gruppe abgeschlossen

					// wenn alle Spiele beendet dann Gruppe als beendet buchen
					ClsTurnierControler.SaveGruppenStatus(ClsGlobal.Instance.TurnierID, spiel.Gruppe, 3);

					// Mannschaften in folgerunden eintragen
					IstMannschaftenGI(spiel.Gruppe);

					// prüfen ob alle gruppen in Runde beendet
					List<TSpiele> spiele = ClsTurnierControler.Spiele(ClsGlobal.Instance.TurnierID, spiel.Runde);
					int r = (from x in spiele
							 where x.Status < 3
							 select x).Count();
					if (r == 0)
					{
						rundenwechsel = true;

						// Runde als beendet buchen
						ClsTurnierControler.SaveRundenStatus(ClsGlobal.Instance.TurnierID, GetActualRoundNo(ClsGlobal.Instance.TurnierID), 3);

						// istmanschaften bei Gruppenübergereifender Folgerunde
						IstMannschaftenGegenueber(spiel);

						// wenn Folgerunde existent 
						if (tkk.Matrix != "AdHoc")
						{
							int rundenAnz = ClsTurnierControler.Runden(ClsGlobal.Instance.TurnierID).Count;
							if (spiel.Runde < rundenAnz)
							{
								int neueRunde = spiel.Runde;
								neueRunde += 1;
								ClsDBVariablen.Instance.SetTextVariableWert("S17", neueRunde.ToString());
								// ClsLocalisationFunctions.Keytext("Text", "01") + " " + runde;
								ClsMessage.SendMessage("Runde beendet", "rundenwechsel...");
								ClsTurnierControler.SaveRundenStatus(ClsGlobal.Instance.TurnierID, neueRunde, 1);
							}
						}
					}

					// prüfen ob alle Spiele in allen Runden in Turnier beendet
					List<TSpiele> spiele3 = ClsTurnierControler.Spiele(ClsGlobal.Instance.TurnierID);
					int ts = (from grp in spiele3
							  where grp.Status < 3
							  select grp).Count();

					if (ts == 0)
					{
						ClsTurnierControler.SaveTurnierStatus(ClsGlobal.Instance.TurnierID, 3);

						// ClsFunktionen.fillTreeViewdetails( turniernr, tvBrowser);
						ClsMessage.SendMessage("Turnier beendet.", "Geschaft...");
						////ClsTranslateControls.ShowMessage("M0017", "Meldung", new object[] { "\n" }, MessageBoxButtons.OK);
					}
				}

				ClsDBVariablen.Instance.SetTabellenVariableWert("T01", ClsTabellenfunktionen.TabelleToVariable_Json(ClsGlobal.Instance.TurnierID));
				if (rundenwechsel == true)
				{
					return 2;
				}

				return 1;
			}
			catch (Exception ex)
			{
				ClsError.Error(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return 0;
			}
		}

		/// <summary>
		/// Setzt ein Spiel als nächst anstehendes Spiel
		/// </summary>
		/// <param name="rb"></param>
		public static void SetNext(TSpiele spiel)
		{
			try
			{
				ClsGlobal.Instance.NextSpiel = spiel;
			}
			catch (Exception ex)
			{
				ClsError.Error(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		/// <summary>
		/// findet das nächste Spiel
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="turnierID"></param>
		/// <returns></returns>
		public static TSpiele FindNext(int pos, int turnierID)
		{
			try
			{
				TKopf turnierk = ClsTurnierControler.Turnier(turnierID);
				if (turnierk.status == 3)
				{
					return null;
				}

				List<TSpiele> spiele1;
				if (turnierk.Matrix != "AdHoc")
				{
					spiele1 = ClsTurnierControler.Spiele(turnierID, GetActualRoundNo(turnierID));
				}
				else
				{
					spiele1 = ClsTurnierControler.Spiele(turnierID);
				}

				List<TSpiele> spiele2 = (from x in spiele1 where x.Status == 2 select x).ToList();
				if (!spiele2.Any())
				{
					// MessageBox.Show("Es ist kein Spiel mehr offen.", "Meldung...");
					////ClsTranslateControls.ShowMessage("M0018", "Meldung", new object[] { "\n" }, MessageBoxButtons.OK);
					return null;
				}

				if (pos >= spiele2.Count)
				{
					return null;
				}
				else
				{
					return spiele2[pos];
				}
			}
			catch (Exception ex)
			{
				ClsError.Error(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new TSpiele();
			}
		}

		public static void SetManName(string aoderb, string nameneu)
		{
			try
			{
				DB.Mannschaften m = ClsMannschaftenControler.Mannschaft(nameneu);
				int turnierID = ClsGlobal.Instance.TurnierID;

				switch (aoderb)
				{
					case "A":
						{
							ClsDBVariablen.Instance.SetTextVariableWert("S01", nameneu);
							if (m != null)
							{
								ClsDBVariablen.Instance.SetBildVariableWert("B01", m.Bild1);
								ClsDBVariablen.Instance.SetBildVariableWert("B03", m.Bild2);
								ClsDBVariablen.Instance.SetTextVariableWert("S03", m.Kurzname);
							}
							else
							{
								ClsDBVariablen.Instance.SetBildVariableWert("B01", "");
								ClsDBVariablen.Instance.SetBildVariableWert("B03", "");
								ClsDBVariablen.Instance.SetTextVariableWert("S03", "");
							}

							if (turnierID == 0 && ClsGlobal.Instance.AktivesSpiel != null)
							{
								ClsGlobal.Instance.AktivesSpiel.IstMannA = nameneu;
							}

							break;
						}

					case "B":
						{
							ClsDBVariablen.Instance.SetTextVariableWert("S02", nameneu);
							if (m != null)
							{
								ClsDBVariablen.Instance.SetBildVariableWert("B02", m.Bild1);
								ClsDBVariablen.Instance.SetBildVariableWert("B04", m.Bild2);
								ClsDBVariablen.Instance.SetTextVariableWert("S04", m.Kurzname);
							}
							else
							{
								ClsDBVariablen.Instance.SetBildVariableWert("B02", "");
								ClsDBVariablen.Instance.SetBildVariableWert("B04", "");
								ClsDBVariablen.Instance.SetTextVariableWert("S04", "");
							}

							if (turnierID == 0 && ClsGlobal.Instance.AktivesSpiel != null)
							{
								ClsGlobal.Instance.AktivesSpiel.IstMannB = nameneu;
							}

							break;
						}

					default:
						{
							break;
						}
				}

				ClsDBVariablen.Instance.SetTextVariableWert("S12", ClsFunktionen.Setspielstand());
			}
			catch (Exception ex)
			{
				ClsError.Error(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		private static void SpielPunkteSpeichern(int turnierID, int spielNr)
		{
			try
			{
				TSpiele spiel = ClsTurnierControler.Spiel(turnierID, spielNr);

				// Feststellen wer gewinner ist und Punkte verteilen
				int sPunkte = Convert.ToInt32(ClsOptionsControler.Options3("Siegerpunkte").Value);
				int vPunkte = Convert.ToInt32(ClsOptionsControler.Options3("Verliererpunkte").Value);
				int uPunkte = Convert.ToInt32(ClsOptionsControler.Options3("Unentschiedenpunkte").Value);

				if (spiel.ToreA == spiel.ToreB)
				{
					spiel.PunkteA = uPunkte;
					spiel.PunkteB = uPunkte;
				}
				else if (spiel.ToreA < spiel.ToreB)
				{
					spiel.PunkteA = vPunkte;
					spiel.PunkteB = sPunkte;
				}
				else if (spiel.ToreA > spiel.ToreB)
				{
					spiel.PunkteA = sPunkte;
					spiel.PunkteB = vPunkte;
				}

				ClsTurnierControler.SaveSpiel(spiel);
			}
			catch (Exception ex)
			{
				ClsError.Error(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		/// <summary>
		/// aktuelle Runde ermitteln
		/// </summary>
		/// <param name="turnierID"></param>
		/// <returns></returns>
		private static int GetActualRoundNo(int turnierID)
		{
			List<TRunden> runden = ClsTurnierControler.Runden(turnierID);
			int runde = (from r in runden
						 where r.status == 1
						 select r.Runde).FirstOrDefault();
			return runde;
		}

		/// <summary>
		/// Nach Gruppenplazierung die Mannschften in den Folgerunden eintragen.
		/// </summary>
		/// <param name="spiel">todo: describe spiel parameter on IstMannschaftenGI</param>
		private static void IstMannschaftenGI(string Gruppe)
		{
			try
			{
				// platz in Gruppe ermitteln über order by und mannschaften in folgerunden eintragen
				List<TTabellen> e = ClsTurnierControler.Tabellen(ClsGlobal.Instance.TurnierID, Gruppe);
				if (e.Any())
				{
					for (int i = 0; i < e.Count; i++)
					{
						if (e[i] != null)
						{
							string suchstr = e[i].Platz.ToString() + "ter-" + Gruppe;

							// hole alle Spiele des Turniers
							List<TSpiele> s0 = ClsTurnierControler.Spiele(ClsGlobal.Instance.TurnierID);

							// filter Spiele die den Status kleiner 2 haben
							List<TSpiele> s1 = (from s in s0
												where s.Status < 2
												select s).ToList();

							// suche Spiele die SuchStr in MannA haben 
							List<TSpiele> p1 = (from s in s1
												where s.IstMannA == suchstr
												select s).ToList();
							if (p1.Any())
							{
								for (int j = 0; j < p1.Count; j++)
								{
									p1[j].IstMannA = e[i].Mannschaft;
									p1[j].Status = p1[j].Status + 1;
									ClsTurnierControler.SaveSpiel(p1[j]);
								}
							}

							// suche Spiele die SuchStr in MannB haben 
							List<TSpiele> p2 = (from s in s1
												where s.IstMannB == suchstr
												select s).ToList();
							if (p2.Any())
							{
								for (int j = 0; j < p2.Count; j++)
								{
									p2[j].IstMannB = e[i].Mannschaft;
									p2[j].Status = p2[j].Status + 1;
									ClsTurnierControler.SaveSpiel(p2[j]);
								}
							}

							// suche Tabelleneinträge der folgerunde und ersetzte Mannschftsnamen
							List<TTabellen> tab = ClsTurnierControler.Tabellen(ClsGlobal.Instance.TurnierID);
							TTabellen gefunden = (from x in tab
												  where x.Mannschaft == suchstr
												  select x).FirstOrDefault();
							if (gefunden != null)
							{
								gefunden.Mannschaft = e[i].Mannschaft;
								ClsTurnierControler.SaveTabellen(gefunden);
							}
						}
					}

					ClsDBVariablen.Instance.SetTabellenVariableWert("T02", ClsTabellenfunktionen.SpielplanToVariable_Json(ClsGlobal.Instance.TurnierID));
				}
			}
			catch (Exception ex)
			{
				ClsError.Error(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		/// <summary>
		/// Nach Rundenplazierung die Mannschften in den Folgerunden eintragen.
		/// </summary>
		/// <param name="spiel">todo: describe spiel parameter on IstMannschaftenGegenueber</param>
		private static void IstMannschaftenGegenueber(TSpiele spiel)
		{
			try
			{
				// Gruppenübergereifende Plaetze ermitten und mannschaften in folgerunden eintragen (1ter 1ter, 2ter 1ter,...)
				int grpManAnz = ClsTurnierControler.Tabellen((int)ClsGlobal.Instance.TurnierID, spiel.Gruppe).Count;

				// So oft wie Mannschaften je Gruppe (Anzeahl der Mannschaften wird al Platzzaehler verwendet)
				for (int p = 1; p <= grpManAnz; p += 1)
				{
					// alle Tabelleneintraege der Runde
					List<TTabellen> erg1 = ClsTurnierControler.Tabellen((int)ClsGlobal.Instance.TurnierID, spiel.Runde);

					// Alle x-platzierten lesen
					List<TTabellen> erg = (from d in erg1
										   where d.Platz == p
										   select d).ToList();
					if (erg != null)
					{
						// nach vorgaben sortieren
						erg = ClsFunktionen.STTabellem(erg);

						// erg enthaelt nun alle x-platzierten nach sortiervorgabe
						int platz = 1;

						for (int i = 0; i < erg.Count; i++)
						{
							string suchstr = platz.ToString() + "ter " + p.ToString() + "ter-Runde " + spiel.Runde.ToString();
							List<TSpiele> s0 = ClsTurnierControler.Spiele(ClsGlobal.Instance.TurnierID);
							List<TSpiele> s1 = (from s in s0
												where s.Status < 2
												select s).ToList();

							List<TSpiele> p1 = (from s in s1
												where s.IstMannA == suchstr
												select s).ToList();
							if (p1.Any())
							{
								for (int j = 0; j < p1.Count; j++)
								{
									p1[j].IstMannA = erg[i].Mannschaft;
									p1[j].Status = p1[j].Status + 1;
									ClsTurnierControler.SaveSpiel(p1[j]);
								}
							}

							List<TSpiele> p2 = (from s in s1
												where s.IstMannB == suchstr
												select s).ToList();
							if (p2.Any())
							{
								for (int j = 0; j < p1.Count; j++)
								{
									p2[j].IstMannB = erg[i].Mannschaft;
									p2[j].Status = p2[j].Status + 1;
									ClsTurnierControler.SaveSpiel(p2[j]);
								}
							}

							platz += 1;

							// suche Tabelleneinträge der folgerunde und ersetzte Mannschftsnamen
							List<TTabellen> tab = ClsTurnierControler.Tabellen(ClsGlobal.Instance.TurnierID);
							TTabellen gefunden = (from x in tab
												  where x.Mannschaft == suchstr
												  select x).FirstOrDefault();
							if (gefunden != null)
							{
								gefunden.Mannschaft = erg[i].Mannschaft;
								ClsTurnierControler.SaveTabellen(gefunden);
							}
						}

						ClsDBVariablen.Instance.SetTabellenVariableWert("T02", ClsTabellenfunktionen.SpielplanToVariable_Json(ClsGlobal.Instance.TurnierID));
					}
				}
			}
			catch (Exception ex)
			{
				ClsError.Error(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		private static void SaveTabellenplatz(string Gruppe, string mannschaft, int platz)
		{
			try
			{
				List<TTabellen> e1 = ClsTurnierControler.Tabellen(ClsGlobal.Instance.TurnierID, Gruppe);
				TTabellen e = (from t in e1
							   where t.Mannschaft == mannschaft
							   select t).FirstOrDefault();

				e.fixPlatz = platz;
				ClsTurnierControler.SaveTabellen(e);
			}
			catch (Exception ex)
			{
				ClsError.Error(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		private static void TabCalc(int turnierID, string gruppe)
		{
			try
			{
				// Tabellenupdate
				List<TTabellen> mannschaften = ClsTurnierControler.Tabellen(turnierID, gruppe);
				if (mannschaften.Any())
				{
					List<TTabellen> unsorted1 = Settablevalues(turnierID, gruppe, mannschaften);
					for (int i = 0; i < unsorted1.Count; i++)
					{
						unsorted1[i].Direkterpunktvergleich = unsorted1.Count;
						unsorted1[i].Direktererweitertervergleich = unsorted1.Count;
						ClsTurnierControler.SaveTabellen(unsorted1[i]);
					}

					// Dierektervergleich 
					// ermittel ob es gleiche punktanzahl gibt
					List<int> test = unsorted1.GroupBy(x => x.Punkte).Select(y => y.Key).ToList();

					// alle pläzte duchgehen und prüfen ob es noch weitere Plaetze mit gleicher punktzahl gibt
					for (int i = 0; i < test.Count; i++)
					{
						List<TTabellen> newlist = ClsTurnierControler.Tabellen(turnierID, gruppe);
						List<TTabellen> dist = (from x in newlist where x.Punkte == test[i] select x).ToList();
						if (dist.Count > 1)
						{
							// nur über Punkte ausrechnen
							List<TTabellen> unsorted = new List<TTabellen>(Settablevalues(turnierID, gruppe, dist).ToList());
							List<TTabellen> sorted = unsorted.OrderByDescending(x => x.Punkte).ToList();
							for (int p = 0; p < sorted.Count; p += 1)
							{
								TTabellen row = sorted[p];
								ClsTurnierControler.SaveTabellenDVP(row, p);
							}

							// über Punkte und Tore ausrechnen
							sorted = unsorted.OrderByDescending(x => x.Punkte).ThenByDescending(y => y.Tore).ThenBy(z => z.Gegentore).ToList();
							for (int p = 0; p < sorted.Count; p += 1)
							{
								TTabellen row = sorted[p];
								ClsTurnierControler.SaveTabellenDVPV(row, p);
							}
						}
					}
				}

				// Gruppenplaetze eintragen
				List<TTabellen> tabelle = ClsTurnierControler.Tabellen(turnierID, gruppe);

				if (tabelle.Any())
				{
					// Nach Vorgaben sortieren
					List<TTabellen> sortedtabelle = ClsFunktionen.STTabellem(tabelle);

					// Plaetze eintragen
					int platz = 1;
					for (int i = 0; i < sortedtabelle.Count; i++)
					{
						if (sortedtabelle[i] != null)
						{
							sortedtabelle[i].Platz = platz;
							ClsTurnierControler.SaveTabellen(sortedtabelle[i]);
							platz += 1;
						}
					}
				}
			}
			catch (Exception ex)
			{
				ClsError.Error(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		private static List<TTabellen> Settablevalues(int turnierID, string gruppe, List<TTabellen> mannschaften)
		{
			try
			{
				List<string> mannamen = mannschaften.Select(x => x.Mannschaft).ToList();

				// Tabellenupdate
				if (mannschaften.Any())
				{
					for (int i = 0; i < mannschaften.Count; i++)
					{
						// Spiele lesen
						List<TSpiele> spiele1 = ClsTurnierControler.Spiele(turnierID, gruppe);
						List<TSpiele> spiele = (from e in spiele1
												where (e.IstMannA == mannschaften[i].Mannschaft && mannamen.Contains(e.IstMannB))
													  ||
													  (e.IstMannB == mannschaften[i].Mannschaft && mannamen.Contains(e.IstMannA))
												select e).ToList();

						// Spiele Auswerten
						int punkte =
							(from TSpiele e in spiele
							 where e.IstMannA == mannschaften[i].Mannschaft
							 select e.PunkteA).Sum()
							+
							(from TSpiele e in spiele
							 where e.IstMannB == mannschaften[i].Mannschaft
							 select e.PunkteB).Sum();

						int tore =
							(from TSpiele e in spiele
							 where e.IstMannA == mannschaften[i].Mannschaft
							 select e.ToreA).Sum()
							+
							(from TSpiele e in spiele
							 where e.IstMannB == mannschaften[i].Mannschaft
							 select e.ToreB).Sum();

						int gegentore =
							(from TSpiele e in spiele
							 where e.IstMannA == mannschaften[i].Mannschaft
							 select e.ToreB).Sum()
							+
							(from TSpiele e in spiele
							 where e.IstMannB == mannschaften[i].Mannschaft
							 select e.ToreA).Sum();

						int spielanz =
							(from TSpiele e in spiele
							 where e.IstMannA == mannschaften[i].Mannschaft
								&& e.Status == 3
							 select e).Count()
							+
							(from TSpiele e in spiele
							 where e.IstMannB == mannschaften[i].Mannschaft
								&& e.Status == 3
							 select e).Count();

						// gegentore wenn beide nicht da
						int anz = (from e in spiele1
								   where (e.IstMannA == mannschaften[i].Mannschaft && e.Status == 4)
								   ||
								   (e.IstMannB == mannschaften[i].Mannschaft && e.Status == 4)
								   select e).Count();
						gegentore += (anz * 5);

						mannschaften[i].Punkte = punkte;
						mannschaften[i].Tore = tore;
						mannschaften[i].Gegentore = gegentore;
						mannschaften[i].Spiele = spielanz;
						mannschaften[i].Torverhaeltnis = tore - gegentore;
					}
				}

				return mannschaften;
			}
			catch (Exception ex)
			{
				ClsError.Error(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new List<TTabellen>();
			}
		}

		/// <summary>
		/// manuelles setzen von Toren und/oder beenden des Spiels
		/// </summary>
		public static void SetMatch(int SpielID, int ToreA, int ToreB, string SpielendeOpt)
		{
			TSpiele spiel = ClsTurnierControler.Spiel(SpielID);
			spiel.ToreA = ToreA;
			spiel.ToreB = ToreB;
			ClsTurnierControler.SaveSpiel(spiel);

			if (SpielendeOpt == "1")
			{
				Spiel_beenden(spiel);
			}

			if (SpielendeOpt == "2")
			{
				// nichts notwendig da Spiel offen bleiben muss
			}

			if (SpielendeOpt == "3")
			{
				Spiel_beenden(spiel, true);
			}
		}
	}
}
