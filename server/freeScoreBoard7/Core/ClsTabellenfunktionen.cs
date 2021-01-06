using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FreeScoreBoard.Core.DB;
using FreeScoreBoard.Core.DBControler;
using FreeScoreBoard.Core.Objekte;
using FreeScoreBoard.Core.Variablen;
using FreeScoreBoard.Interpreter.Error;

namespace FreeScoreBoard.Core
{
	public class ClsTabellenfunktionen
	{
		private static string Name = MethodBase.GetCurrentMethod().DeclaringType.Name;

		public static void IniTableFilter()
		{
			List<ClsTableFilter> tfl = new List<ClsTableFilter>
			{

				// Tabelle T01
				new ClsTableFilter("T01", "Runde", ""),
				new ClsTableFilter("T01", "Gruppe", ""),
				new ClsTableFilter("T01", "SelPage", 1),

				// Spielplan T02
				new ClsTableFilter("T02", "Runde", ""),
				new ClsTableFilter("T02", "Gruppe", ""),
				new ClsTableFilter("T02", "Status", ""),
				new ClsTableFilter("T02", "Datum", ""),
				new ClsTableFilter("T02", "SelPage", 1),

				// Ereignisse T03
				new ClsTableFilter("T03", "Spiel", ""),
				new ClsTableFilter("T03", "Ereignistyp", ""),
				new ClsTableFilter("T03", "SelPage", 1),

				// Strafen Mannschaft1 T04
				new ClsTableFilter("T04", "SelPage", 1),

				// Strafen Mannschaft2 T05
				new ClsTableFilter("T05", "SelPage", 1),

				// Strafen Torschützen T06
				new ClsTableFilter("T06", "SelPage", 1)

			};

			ClsGlobal.Instance.TableFilterList = tfl;
		}

		public static void TabToVariable_Json(string Tab, int turnierID)
		{
			switch (Tab)
			{
				case "T01":
					SpielplanToVariable_Json(turnierID);
					break;
				case "T02":
					TabelleToVariable_Json(turnierID);
					break;
				case "T03":
					EreignisseToVariable_Json(turnierID);
					break;
				case "T04":
					//StrafenToVariable("a", "std");
					break;
				case "T05":
					//StrafenToVariable("b", "std");
					break;
				case "T06":
					TorschuetzeToVariable(turnierID);
					break;
			}
		}

		public static string SpielplanToVariable_Json(int turnierID)
		{
			try
			{
				bool kname = Convert.ToBoolean(ClsOptionsControler.Options3("kurznamen").Value);

				string Runde = ClsGlobal.Instance.TableFilterList.Find(x => x.Table == "T02" && x.Field == "Runde").Value;
				string Gruppe = ClsGlobal.Instance.TableFilterList.Find(x => x.Table == "T02" && x.Field == "Gruppe").Value;
				string Status = ClsGlobal.Instance.TableFilterList.Find(x => x.Table == "T02" && x.Field == "Status").Value;
				string Datum = ClsGlobal.Instance.TableFilterList.Find(x => x.Table == "T02" && x.Field == "Datum").Value;

				if (string.IsNullOrEmpty(Runde))
				{
					Runde = "0";
				}

				int runde = Convert.ToInt32(Runde);

				// Create a DataTable for buildup
				DataTable tempTable = new DataTable();

				// Spalte je Anzeigespalte esretllen (Überschriften)
				List<Anzeigetabelle> cols1 = ClsOptionsControler.Anzeigetabelle("T02", "std");
				List<Anzeigetabelle> cols = (from x in cols1 where x.Sichtbar == 1 select x).ToList();

				string[] spalten = new string[cols.Count + 1];
				if (cols.Any())
				{
					spalten[0] = "ID";
					for (int i = 0; i < cols.Count; i++)
					{
						spalten[i + 1] = cols[i].DBFeld;
					}
				}

				// Zeilen Hinzufügen
				// Tabelle lesen
				List<TSpiele> tab = ClsTurnierControler.Spiele(turnierID);

				// Daten filtern
				if (Runde != "0")
				{
					tab = (from t in tab
						   where t.Runde == runde
						   select t).ToList();
				}

				if (!string.IsNullOrEmpty(Gruppe))
				{
					tab = (from t in tab
						   where t.Gruppe == Gruppe
						   select t).ToList();
				}

				if (!string.IsNullOrEmpty(Status))
				{
					tab = (from t in tab
						   where t.Status == Convert.ToInt32(Status)
						   select t).ToList();
				}

				if (!string.IsNullOrEmpty(Datum))
				{
					tab = (from t in tab
							   //where t.Datum == Convert.ToDateTime(ClsGlobal.Instance.FilterS.Date)
						   where t.Datum == Datum
						   select t).ToList();
				}

				// Daten sortieren
				tab = (from ot in tab
					   orderby ot.Spiel
					   select ot).ToList();

				// Werte übersetzen
				if (tab.Any())
				{
					for (int i = 0; i < tab.Count; i++)
					{
						if (kname)
						{
							tab[i].IstMannA = Kurzname(tab[i].IstMannA);
							tab[i].IstMannB = Kurzname(tab[i].IstMannB);
						}
					}
				}

				// Mache aus List ein Datatable
				using (var reader = FastMember.ObjectReader.Create(tab, spalten))
				{
					tempTable.Load(reader);
				}

				// Spalten übersetzen
				for (int i = 0; i < tempTable.Columns.Count; i++)
				{
					// Anzeigename
					string text = (from x in cols where x.DBFeld == tempTable.Columns[i].ColumnName select x.Anzeigename).FirstOrDefault();
					if (string.IsNullOrEmpty(text))
					{
						//text = ClsLocalisationFunctions.Tabellenfeld("Spielplan", tempTable.Columns[i].ColumnName);
						text = ClsTextControler.TextByNameAndNumber("Spielplan", tempTable.Columns[i].ColumnName);
					}

					if (!string.IsNullOrEmpty(text))
					{
						tempTable.Columns[i].ColumnName = text;
					}
				}

				return ConvertDatatabletoString(tempTable);
			}
			catch (Exception ex)
			{
				ClsError.Error(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return null;
			}
		}

		public static string TabelleToVariable_Json(int turnierID)
		{
			try
			{
				ClsTabellenVariabeln tv = ClsDBVariablen.Instance.GetTabellenVariable("T01");

				bool kname = Convert.ToBoolean(ClsOptionsControler.Options3("kurznamen").Value);

				string Runde = ClsGlobal.Instance.TableFilterList.Find(x => x.Table == "T01" && x.Field == "Runde").Value;
				string Gruppe = ClsGlobal.Instance.TableFilterList.Find(x => x.Table == "T01" && x.Field == "Gruppe").Value;

				if (string.IsNullOrEmpty(Runde))
				{
					Runde = "0";
				}

				int runde = Convert.ToInt32(Runde);

				// Create a DataTable for buildup
				DataTable tempTable = new DataTable();

				// Spalte je Anzeigespalte erstellen
				List<Anzeigetabelle> cols1 = ClsOptionsControler.Anzeigetabelle("T01", "std");
				List<Anzeigetabelle> cols = (from x in cols1 where x.Sichtbar == 1 select x).ToList();

				string[] spalten = new string[cols.Count + 1];
				if (cols.Any())
				{
					spalten[0] = "ID";
					for (int i = 0; i < cols.Count; i++)
					{
						spalten[i + 1] = cols[i].DBFeld;
					}
				}

				// Tabelle lesen
				List<TTabellen> tab = ClsTurnierControler.Tabellen(turnierID);

				// Daten filtern
				if (Runde != "0")
				{
					tab = (from t in tab
						   where t.Runde == runde
						   select t).ToList();
				}

				if (!string.IsNullOrEmpty(Gruppe))
				{
					tab = (from t in tab
						   where t.Gruppe == Gruppe
						   select t).ToList();
				}

				// Daten sortieren
				tab = (from t in tab
					   orderby t.Runde, t.Gruppe, t.Platz
					   select t).ToList();

				//// Seite berechnen
				int recPerPage = tv.RecPerPage;
				int skip = calacSkipRecords("T02", recPerPage, (decimal)tab.Count());

				// tabseite
				tab = (from t in tab select t).Skip(skip).Take(recPerPage).ToList();

				// Werte übersetzen
				if (tab.Any())
				{
					for (int i = 0; i < tab.Count; i++)
					{
						if (kname)
						{
							tab[i].Mannschaft = Kurzname(tab[i].Mannschaft);
						}
					}
				}

				// Mache aus List ein Datatable
				using (var reader = FastMember.ObjectReader.Create(tab, spalten))
				{
					tempTable.Load(reader);
				}

				// Spalten übersetzen
				for (int i = 0; i < tempTable.Columns.Count; i++)
				{
					// Anzeigename
					string text = (from x in cols where x.DBFeld == tempTable.Columns[i].ColumnName select x.Anzeigename).FirstOrDefault();
					if (string.IsNullOrEmpty(text))
					{
						//text = ClsLocalisationFunctions.Tabellenfeld("Tabelle", tempTable.Columns[i].ColumnName);
						text = ClsTextControler.TextByNameAndNumber("Tabelle", tempTable.Columns[i].ColumnName);
					}

					if (!string.IsNullOrEmpty(text))
					{
						tempTable.Columns[i].ColumnName = text;
					}
				}

				return ConvertDatatabletoString(tempTable);

				// tempTable.Dispose();
			}
			catch (Exception ex)
			{
				ClsError.Error(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return null;
			}
		}

		public static string EreignisseToVariable_Json(int turnierID)
		{
			try
			{
				ClsTabellenVariabeln tv = ClsDBVariablen.Instance.GetTabellenVariable("T03");

				string SpielNr = ClsGlobal.Instance.TableFilterList.Find(x => x.Table == "T03" && x.Field == "Spiel").Value;

				if (SpielNr == string.Empty)
				{
					SpielNr = "0";
				}

				int spielNr = Convert.ToInt32(SpielNr);

				string Ereignisart = ClsGlobal.Instance.TableFilterList.Find(x => x.Table == "T03" && x.Field == "Ereignistyp").Value;

				// Create a DataTable for buildup
				DataTable tempTable = new DataTable();

				// Spalte je Anzeigespalte erstellen
				List<Anzeigetabelle> cols1 = ClsOptionsControler.Anzeigetabelle("T03", "std");
				List<Anzeigetabelle> cols = (from x in cols1 where x.Sichtbar == 1 select x).ToList();

				string[] spalten = new string[cols.Count + 1];
				if (cols.Any())
				{
					spalten[0] = "ID";
					for (int i = 0; i < cols.Count; i++)
					{
						spalten[i + 1] = cols[i].DBFeld;
					}
				}

				// Daten lesen
				List<TEreignisse> tab = ClsEreignisControler.Ereignisse(turnierID);

				// Daten filtern
				if (Ereignisart != string.Empty)
				{
					tab = (from t in tab
						   where t.Ereignistyp == Ereignisart.Substring(0, 2)
						   select t).ToList();
				}

				if (SpielNr != "0")
				{
					tab = (from t in tab
						   where t.Spiel == spielNr
						   select t).ToList();
				}

				// Daten sortieren
				//if (zeitrichtung == "0")
				//{
				//    tab = (from t in tab orderby t.Spiel, t.Spielabschnitt, t.Spielzeit select t).ToList();
				//}
				//else
				//{
				//    tab = (from t in tab orderby t.Spiel, t.Spielabschnitt, t.Spielzeit descending select t).ToList();
				//}

				tab = (from t in tab orderby t.CPUZeit select t).ToList();

				//// Seite berechnen
				int recPerPage = tv.RecPerPage;
				int skip = calacSkipRecords("T03", recPerPage, (decimal)tab.Count());

				// tabseite
				tab = (from t in tab select t).Skip(skip).Take(recPerPage).ToList();

				// Werte übersetzen
				if (tab.Any())
				{
					for (int i = 0; i < tab.Count; i++)
					{
						//tab[i].Ereignistyp = ClsLocalisationFunctions.Keytext("Ereignis", tab[i].Ereignistyp);
						if (!tab[i].Ereignistyp.Contains("-"))
						{
							tab[i].Ereignistyp = ClsTextControler.TextByNameAndNumber("Ereignis", tab[i].Ereignistyp);
						}
						//if (kname)
						//{
						//	tab[i].Mannschaft = Kurzname(tab[i].Mannschaft);
						//}

						string opt_EreigniszeitGenau = ClsOptionsControler.Options3("EreigniszeitGenau").Value;
						if (opt_EreigniszeitGenau == "False")
						{
							tab[i].Spielzeit = ConvertSpielzeitToKurzzeit(tab[i].Spielzeit, tab[i].Spielzeitrichtung);
						}
					}
				}

				// Mache aus List ein Datatable
				using (var reader = FastMember.ObjectReader.Create(tab, spalten))
				{
					tempTable.Load(reader);
				}

				// Spalten übersetzen
				for (int i = 0; i < tempTable.Columns.Count; i++)
				{
					// Anzeigename
					string text = (from x in cols where x.DBFeld == tempTable.Columns[i].ColumnName select x.Anzeigename).FirstOrDefault();
					if (string.IsNullOrEmpty(text))
					{
						//text = ClsLocalisationFunctions.Tabellenfeld("Ereignisse", tempTable.Columns[i].ColumnName);
						text = ClsTextControler.TextByNameAndNumber("Ereignisse", tempTable.Columns[i].ColumnName);
					}

					if (!string.IsNullOrEmpty(text))
					{
						tempTable.Columns[i].ColumnName = text;
					}
				}

				// Tabellenseite berechnen
				// gewählte seite = ???
				// seiten = anz datensätze / datensätze je Seite
				// startindex = 	datensätze je Seite * 	gewählte seite	  + 1
				// endindex =		datensätze je Seite * 	(gewählte seite	  + 1 ) - 1
				// schleife zum filten von tempTable

				//			var custQuery2 =
				//(from cust in db.Customers
				// orderby cust.ContactName
				// select cust)
				//.Skip(50).Take(10);	<------- skip und take!!!

				//			foreach (var custRecord in custQuery2)
				//			{
				//				Console.WriteLine(custRecord.ContactName);
				//			}

				return ConvertDatatabletoString(tempTable);
			}
			catch (Exception ex)
			{
				ClsError.Error(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return null;
			}
		}

		public static void TorschuetzeToVariable(int turnierID)
		{
			try
			{
				bool kname = Convert.ToBoolean(ClsOptionsControler.Options3("kurznamen").Value);

				// Create a DataTable for buildup
				DataTable tempTable = new DataTable();

				// Spalte je Anzeigespalte erstellen
				List<Anzeigetabelle> cols1 = ClsOptionsControler.Anzeigetabelle("T06", "std");
				List<Anzeigetabelle> cols = (from x in cols1 where x.Sichtbar == 1 select x).ToList();

				string[] spalten = new string[cols.Count];
				if (cols.Any())
				{
					for (int i = 0; i < cols.Count; i++)
					{
						spalten[i] = cols[i].DBFeld;
					}
				}

				// Tabelle lesen
				List<ClsTorschuetze> tab = ClsTurnierControler.TorschuetzenList(turnierID);

				// Werte übersetzen
				if (tab.Any())
				{
					for (int i = 0; i < tab.Count; i++)
					{
						if (kname)
						{
							tab[i].Mannschaft = Kurzname(tab[i].Mannschaft);
						}
					}
				}

				// Mache aus List ein Datatable
				using (var reader = FastMember.ObjectReader.Create(tab, spalten))
				{
					tempTable.Load(reader);
				}

				// Spalten übersetzen
				for (int i = 0; i < tempTable.Columns.Count; i++)
				{
					// Anzeigename
					string text = (from x in cols where x.DBFeld == tempTable.Columns[i].ColumnName select x.Anzeigename).FirstOrDefault();
					if (string.IsNullOrEmpty(text))
					{
						//text = ClsLocalisationFunctions.Tabellenfeld("Torschuetzen", tempTable.Columns[i].ColumnName);
						text = ClsTextControler.TextByNameAndNumber("Torschuetzen", tempTable.Columns[i].ColumnName);
					}

					if (!string.IsNullOrEmpty(text))
					{
						tempTable.Columns[i].ColumnName = text;
					}
				}

				ClsDBVariablen.Instance.SetTabellenVariableWert("T06", ConvertDatatabletoString(tempTable));

				// return tempTable;

				// tempTable.Dispose();
			}
			catch (Exception ex)
			{
				ClsError.Error(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="items">from ClsGlobal</param>
		/// <param name="aoderb">in upper case</param>
		/// <param name="variante"></param>
		public static void StrafenToVariable(List<ClsZeitstrafe> items, string aoderb, string variante = "std")
		{
			try
			{
				bool kname = Convert.ToBoolean(ClsOptionsControler.Options3("kurznamen").Value);

				// Create a DataTable for buildup
				DataTable tempTable = new DataTable();

				// Spalte je Anzeigespalte erstellen
				List<Anzeigetabelle> cols1 = ClsOptionsControler.Anzeigetabelle("Strafen", variante);
				List<Anzeigetabelle> cols = (from x in cols1 where x.Sichtbar == 1 select x).ToList();

				string[] spalten = new string[cols.Count];
				if (cols.Any())
				{
					// spalten[0] = "ID";
					for (int i = 0; i < cols.Count; i++)
					{
						spalten[i] = cols[i].DBFeld;
					}
				}

				// Mache aus List ein Datatable
				using (var reader = FastMember.ObjectReader.Create(items, spalten))
				{
					tempTable.Load(reader);
				}

				// Spalten übersetzen
				for (int i = 0; i < tempTable.Columns.Count; i++)
				{
					// Anzeigename
					string text = (from x in cols where x.DBFeld == tempTable.Columns[i].ColumnName select x.Anzeigename).FirstOrDefault();
					if (string.IsNullOrEmpty(text))
					{
						//text = ClsLocalisationFunctions.Tabellenfeld("Strafen", tempTable.Columns[i].ColumnName);
						text = ClsTextControler.TextByNameAndNumber("Strafen", tempTable.Columns[i].ColumnName);
					}

					if (!string.IsNullOrEmpty(text))
					{
						tempTable.Columns[i].ColumnName = text;
					}
				}

				if (aoderb == "A")
				{
					ClsDBVariablen.Instance.SetTabellenVariableWert("T04", ConvertDatatabletoString(tempTable));
				}
				else
				{
					ClsDBVariablen.Instance.SetTabellenVariableWert("T05", ConvertDatatabletoString(tempTable));
				}
			}
			catch (Exception ex)
			{
				ClsError.Error(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		private void LoadSpieleToTE()
		{
			try
			{
				List<TSpiele> tee = ClsTurnierControler.Spiele(ClsGlobal.Instance.TurnierID);

				if (!tee.Any())
				{
					return;
				}


			}
			catch (Exception ex)
			{
				ClsError.Error(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		private static string ConvertDatatabletoString(DataTable dt)
		{
			System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
			List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
			Dictionary<string, object> row;
			foreach (DataRow dr in dt.Rows)
			{
				row = new Dictionary<string, object>();
				foreach (DataColumn col in dt.Columns)
				{
					row.Add(col.ColumnName, dr[col]);
				}

				rows.Add(row);
			}

			return serializer.Serialize(rows);
		}

		private static string Kurzname(string langname)
		{
			Core.DB.Mannschaften m = ClsMannschaftenControler.Mannschaft(langname);
			if (m == null)
			{
				return langname;
			}

			if (m.Kurzname == string.Empty)
			{
				return langname;
			}

			return m.Kurzname;
		}

		public static string ConvertSpielzeitToKurzzeit(string Spielzeit, string Spielzeitrichtung)
		{
			try
			{
				if (!Spielzeit.Contains(":"))
				{
					return Spielzeit;
				}

				string spielzeitstr = Spielzeit;
				string ausgabestr = string.Empty;

				// array wegen möglicher Nachspielzeit
				string[] spielzeitstrarr = spielzeitstr.Split('+');
				for (int j = 0; j < spielzeitstrarr.Length; j += 1)
				{
					string zeitstr = spielzeitstrarr[j];
					string minstr = zeitstr.Substring(0, zeitstr.IndexOf(":"));
					string sekstr = zeitstr.Substring(zeitstr.IndexOf(":") + 1, 2);

					if (sekstr == "00")
					{
						minstr += @"'";

					}
					else
					{
						int min = Convert.ToInt32(minstr) + 1;
						minstr = min.ToString() + @"'";
					}

					if (j == 0)
					{
						// j = 0 = Hauptspielzeit
						if (Spielzeitrichtung == "True")
						{
							minstr += @"↓";
						}
					}
					else
					{
						// j = 0 = Nachspielzeit
						ausgabestr += @" + ";

					}

					ausgabestr += minstr;
				}

				return ausgabestr;
			}
			catch (Exception ex)
			{
				ClsError.Error(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return "";
			}
		}

		private static int calacSkipRecords(string TableVariable, int recPerPage, decimal recCount)
		{
			// Seite berechnen
			// gewählte seite = ???
			int selpage = (from x
						   in ClsGlobal.Instance.TableFilterList
						   where x.Table == TableVariable && x.Field == "SelPage"
						   select x).FirstOrDefault().IntValue;

			// seiten = anz datensätze / datensätze je Seite
			decimal d = recCount / (decimal)recPerPage;
			ClsDBVariablen.Instance.SetTabellenVariableAmountOfPages(TableVariable, Math.Ceiling(d));

			// überspringenindex = 	datensätze je Seite * 	gewählte seite
			int skip = recPerPage * (selpage - 1);

			//// endindex =		datensätze je Seite * 	(gewählte seite	  + 1 ) - 1
			//int end = recPerPage * selpage - 1;

			return skip;
		}
	}
}
