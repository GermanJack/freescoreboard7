using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using FreeScoreBoard.Core.DB;
using FreeScoreBoard.Core.Objekte;
//using FreeScoreBoard.Forms;
using FreeScoreBoard.Interpreter.Error;

namespace FreeScoreBoard.Core.DBControler
{
	public static class ClsTurnierControler
	{
		private static readonly string Name = MethodBase.GetCurrentMethod().DeclaringType.Name;

		/// <summary>
		/// Gibt die Turnierkopfdaten alle Turniere zurück
		/// </summary>
		/// <param name="Turniertyp">Turnier / System</param>
		/// <returns></returns>
		public static List<TKopf> Turniere(string Turniertyp)
		{
			try
			{
				List<TKopf> stl;

				using (fsbDB FSBDB = new fsbDB())
				{
					stl = (from stk in FSBDB.TKopf where stk.Turniertyp == Turniertyp select stk).ToList();
				}

				return stl;
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new TKopf[0].ToList();
			}
		}

		public static List<ClsTurnier> TurniereKomplett(string Turniertyp)
		{
			try
			{
				List<ClsTurnier> turnList = new List<ClsTurnier>();

				using (fsbDB FSBDB = new fsbDB())
				{
					List<TKopf> stl;

					if (Turniertyp == "")
					{
						stl = (from stk in FSBDB.TKopf select stk).ToList();
					}
					else
					{
						stl = (from stk in FSBDB.TKopf where stk.Turniertyp == Turniertyp select stk).ToList();
					}

					for (int i = 0; i < stl.Count; i++)
					{
						turnList.Add(TurnierKomplett((int)stl[i].ID));
					}
				}

				return turnList;
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new ClsTurnier[0].ToList();
			}
		}

		public static ClsTurnier TurnierKomplett(int TurnierID)
		{
			try
			{
				ClsTurnier turn = new ClsTurnier();

				turn.Kopf = Turnier(TurnierID);
				turn.Runden = Runden(TurnierID);
				turn.Gruppen = Gruppen(TurnierID);
				turn.Spiele = Spiele(TurnierID);
				turn.Tabellen = Tabellen(TurnierID);
				turn.Ereignisse = ClsEreignisControler.Ereignisse(TurnierID);
				turn.Torschuetzen = TorschuetzenList(TurnierID);

				return turn;
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new ClsTurnier();
			}
		}

		public static TKopf Turnier(int TurnierID)
		{
			try
			{
				TKopf stl;

				using (fsbDB FSBDB = new fsbDB())
				{
					stl = (from x in FSBDB.TKopf
						   where x.ID == TurnierID
						   select x).FirstOrDefault();
				}

				return stl;
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new TKopf();
			}
		}

		public static List<TTabellen> Tabellen(int TurnierID)
		{
			try
			{
				List<TTabellen> t;

				using (fsbDB FSBDB = new fsbDB())
				{
					t = (from x in FSBDB.TTabellen
						 where x.TurnierID == TurnierID
						 orderby x.Platz, x.Mannschaft
						 select x).ToList();
				}

				return t;
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new TTabellen[0].ToList();
			}
		}

		public static List<TTabellen> Tabellen(int TurnierID, int runde)
		{
			try
			{
				List<TTabellen> t;

				using (fsbDB FSBDB = new fsbDB())
				{
					t = (from x in FSBDB.TTabellen
						 where x.TurnierID == TurnierID && x.Runde == runde
						 orderby x.Platz, x.Mannschaft
						 select x).ToList();
				}

				return t;
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new TTabellen[0].ToList();
			}
		}

		public static List<TTabellen> Tabellen(int TurnierID, string gruppe)
		{
			try
			{
				List<TTabellen> t;

				using (fsbDB FSBDB = new fsbDB())
				{
					t = (from x in FSBDB.TTabellen
						 where x.TurnierID == TurnierID && x.Gruppe == gruppe
						 orderby x.Platz, x.Mannschaft
						 select x).ToList();
				}

				return t;
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new TTabellen[0].ToList();
			}
		}

		public static TSpiele Spiel(int TurnierID, int spielNr)
		{
			try
			{
				TSpiele t;

				using (fsbDB FSBDB = new fsbDB())
				{
					t = (from x in FSBDB.TSpiele
						 where x.TurnierID == TurnierID && x.Spiel == spielNr
						 orderby x.Spiel
						 select x).FirstOrDefault();
				}

				return t;
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new TSpiele();
			}
		}

		public static TSpiele Spiel(int spielID)
		{
			try
			{
				TSpiele t;

				using (fsbDB FSBDB = new fsbDB())
				{
					t = (from x in FSBDB.TSpiele
						 where x.ID == spielID
						 orderby x.Spiel
						 select x).FirstOrDefault();
				}

				return t;
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new TSpiele();
			}
		}

		public static List<TSpiele> Spiele(int TurnierID)
		{
			try
			{
				List<TSpiele> t;

				using (fsbDB FSBDB = new fsbDB())
				{
					t = (from x in FSBDB.TSpiele
						 where x.TurnierID == TurnierID
						 orderby x.Spiel
						 select x).ToList();
				}

				return t;
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new TSpiele[0].ToList();
			}
		}

		public static List<TSpiele> Spiele(int TurnierID, int runde)
		{
			try
			{
				List<TSpiele> t;

				using (fsbDB FSBDB = new fsbDB())
				{
					t = (from x in FSBDB.TSpiele
						 where x.TurnierID == TurnierID && x.Runde == runde
						 orderby x.Spiel
						 select x).ToList();
				}

				return t;
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new TSpiele[0].ToList();
			}
		}

		public static List<TSpiele> Spiele(int TurnierID, int runde, string gruppe)
		{
			try
			{
				List<TSpiele> t;

				using (fsbDB FSBDB = new fsbDB())
				{
					t = (from x in FSBDB.TSpiele
						 where x.TurnierID == TurnierID && x.Runde == runde && x.Gruppe == gruppe
						 orderby x.Spiel
						 select x).ToList();
				}

				return t;
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new TSpiele[0].ToList();
			}
		}

		public static List<TSpiele> Spiele(int TurnierID, string gruppe)
		{
			try
			{
				List<TSpiele> t;

				using (fsbDB FSBDB = new fsbDB())
				{
					t = (from x in FSBDB.TSpiele
						 where x.TurnierID == TurnierID && x.Gruppe == gruppe
						 orderby x.Spiel
						 select x).ToList();
				}

				return t;
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new TSpiele[0].ToList();
			}
		}

		public static void DelTurnierKopf(int TurnierID)
		{
			List<TKopf> l;

			using (fsbDB FSBDB = new fsbDB())
			{
				l = (from x in FSBDB.TKopf
					 where x.ID == TurnierID
					 select x).ToList();

				foreach (TKopf x in l)
				{
					FSBDB.TKopf.Remove(x);
				}

				FSBDB.SaveChanges();
			}
		}

		public static void DelTabellen(int TurnierID)
		{
			List<TTabellen> l;

			using (fsbDB FSBDB = new fsbDB())
			{
				l = (from x in FSBDB.TTabellen
					 where x.TurnierID == TurnierID
					 select x).ToList();

				foreach (TTabellen x in l)
				{
					FSBDB.TTabellen.Remove(x);
				}

				FSBDB.SaveChanges();
			}
		}

		public static void DelSpiele(int TurnierID)
		{
			List<TSpiele> l;

			using (fsbDB FSBDB = new fsbDB())
			{
				l = (from x in FSBDB.TSpiele
					 where x.TurnierID == TurnierID
					 select x).ToList();

				foreach (TSpiele x in l)
				{
					FSBDB.TSpiele.Remove(x);
				}

				FSBDB.SaveChanges();
			}
		}

		public static void DelGruppen(int TurnierID)
		{
			List<TGruppen> l;

			using (fsbDB FSBDB = new fsbDB())
			{
				l = (from x in FSBDB.TGruppen
					 where x.TurnierID == TurnierID
					 select x).ToList();

				foreach (TGruppen x in l)
				{
					FSBDB.TGruppen.Remove(x);
				}

				FSBDB.SaveChanges();
			}
		}

		public static void DelRunden(int TurnierID)
		{
			List<TRunden> l;

			using (fsbDB FSBDB = new fsbDB())
			{
				l = (from x in FSBDB.TRunden
					 where x.TurnierID == TurnierID
					 select x).ToList();

				foreach (TRunden x in l)
				{
					FSBDB.TRunden.Remove(x);
				}

				FSBDB.SaveChanges();
			}
		}

		public static List<TRunden> Runden(int TurnierID)
		{
			try
			{
				List<TRunden> l;

				using (fsbDB FSBDB = new fsbDB())
				{
					l = (from x in FSBDB.TRunden
						 where x.TurnierID == TurnierID
						 select x).ToList();
				}

				return l;
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new TRunden[0].ToList();
			}
		}

		public static TRunden Runde(int TurnierID, int Runde)
		{
			try
			{
				TRunden l;

				using (fsbDB FSBDB = new fsbDB())
				{
					l = (from x in FSBDB.TRunden
						 where x.TurnierID == TurnierID && x.Runde == Runde
						 select x).FirstOrDefault();
				}

				return l;
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new TRunden();
			}
		}

		public static List<TGruppen> Gruppen(int TurnierID)
		{
			try
			{
				List<TGruppen> l;

				using (fsbDB FSBDB = new fsbDB())
				{
					l = (from x in FSBDB.TGruppen
						 where x.TurnierID == TurnierID
						 select x).ToList();
				}

				return l;
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new TGruppen[0].ToList();
			}
		}

		public static List<TGruppen> Gruppen(int TurnierID, int runde)
		{
			try
			{
				List<TGruppen> l;

				using (fsbDB FSBDB = new fsbDB())
				{
					l = (from x in FSBDB.TGruppen
						 where x.TurnierID == TurnierID && x.Runde == runde
						 select x).ToList();
				}

				return l;
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new TGruppen[0].ToList();
			}
		}

		public static TGruppen Gruppe(int TurnierID, string gruppe)
		{
			try
			{
				TGruppen l;

				using (fsbDB FSBDB = new fsbDB())
				{
					l = (from x in FSBDB.TGruppen
						 where x.TurnierID == TurnierID && x.Gruppe == gruppe
						 select x).FirstOrDefault();
				}

				return l;
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new TGruppen();
			}
		}

		public static int AddTurnierKopf(TKopf turnier)
		{
			try
			{
				using (fsbDB FSBDB = new fsbDB())
				{
					long newID = (from x in FSBDB.TKopf select x.ID).DefaultIfEmpty(0).Max() + 1;
					turnier.ID = newID;
					FSBDB.TKopf.Add(turnier);
					FSBDB.SaveChanges();
					return (int)turnier.ID;
				}
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return 0;
			}
		}

		//public static void AddEreignisse(TEreignisse ereigniss)
		//{
		//    try
		//    {
		//        using (fsbDB FSBDB = new fsbDB())
		//        {
		//            FSBDB.TEreignisse.Add(ereigniss);
		//            FSBDB.SaveChanges();
		//        }
		//    }
		//    catch (Exception ex)
		//    {
		//        ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
		//    }
		//}

		public static void AddTabellen(TTabellen tabelleneintrag)
		{
			try
			{
				using (fsbDB FSBDB = new fsbDB())
				{
					long newID = (from x in FSBDB.TTabellen select x.ID).DefaultIfEmpty(0).Max() + 1;
					tabelleneintrag.ID = newID;
					FSBDB.TTabellen.Add(tabelleneintrag);
					FSBDB.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		public static void AddSpiele(TSpiele spiel)
		{
			try
			{
				using (fsbDB FSBDB = new fsbDB())
				{
					long newID = (from x in FSBDB.TSpiele select x.ID).DefaultIfEmpty(0).Max() + 1;
					spiel.ID = newID;
					FSBDB.TSpiele.Add(spiel);
					FSBDB.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		public static void AddGruppen(TGruppen gruppe)
		{
			try
			{
				using (fsbDB FSBDB = new fsbDB())
				{
					long newID = (from x in FSBDB.TGruppen select x.ID).DefaultIfEmpty(0).Max() + 1;
					gruppe.ID = newID;
					FSBDB.TGruppen.Add(gruppe);
					FSBDB.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		public static void AddRunden(TRunden runde)
		{
			try
			{
				using (fsbDB FSBDB = new fsbDB())
				{
					long newID = (from x in FSBDB.TRunden select x.ID).DefaultIfEmpty(0).Max() + 1;
					runde.ID = newID;
					FSBDB.TRunden.Add(runde);
					FSBDB.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		public static void SaveSpiel(TSpiele spiel)
		{
			try
			{
				TSpiele l;

				using (fsbDB FSBDB = new fsbDB())
				{
					l = (from x in FSBDB.TSpiele
						 where x.ID == spiel.ID
						 select x).FirstOrDefault();

					foreach (PropertyInfo pi in spiel.GetType().GetProperties())
					{
						if (pi.CanWrite)
						{
							pi.SetValue(l, pi.GetValue(spiel, null), null);
						}
					}

					FSBDB.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		public static void SaveTKopf(TKopf TurnierKopf)
		{
			try
			{
				TKopf l;

				using (fsbDB FSBDB = new fsbDB())
				{
					l = (from x in FSBDB.TKopf
						 where x.ID == TurnierKopf.ID
						 select x).FirstOrDefault();

					foreach (PropertyInfo pi in TurnierKopf.GetType().GetProperties())
					{
						if (pi.CanWrite)
						{
							pi.SetValue(l, pi.GetValue(TurnierKopf, null), null);
						}
					}

					FSBDB.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		public static void SaveTabellen(TTabellen tabelle)
		{
			try
			{
				TTabellen l;

				using (fsbDB FSBDB = new fsbDB())
				{
					l = (from x in FSBDB.TTabellen
						 where x.ID == tabelle.ID
						 select x).FirstOrDefault();

					foreach (PropertyInfo pi in tabelle.GetType().GetProperties())
					{
						if (pi.CanWrite)
						{
							pi.SetValue(l, pi.GetValue(tabelle, null), null);
						}
					}

					FSBDB.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		/// <summary>
		/// Speichert nur das Feld DirekterPunktVergleich
		/// </summary>
		/// <param name="tabelle"></param>
		/// <param name="dV"></param>
		public static void SaveTabellenDVP(TTabellen tabelle, int dV)
		{
			try
			{
				TTabellen l;

				using (fsbDB FSBDB = new fsbDB())
				{
					l = (from x in FSBDB.TTabellen
						 where x.ID == tabelle.ID
						 select x).FirstOrDefault();

					l.Direkterpunktvergleich = dV;

					FSBDB.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		/// <summary>
		/// Speichert nur das Feld DirekterErweiterterVergleich
		/// </summary>
		/// <param name="tabelle"></param>
		/// <param name="dV"></param>
		public static void SaveTabellenDVPV(TTabellen tabelle, int dV)
		{
			try
			{
				TTabellen l;

				using (fsbDB FSBDB = new fsbDB())
				{
					l = (from x in FSBDB.TTabellen
						 where x.ID == tabelle.ID
						 select x).FirstOrDefault();

					l.Direktererweitertervergleich = dV;

					FSBDB.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		////public static void SaveFoul(TSpiele spiel)
		////{
		////    // es muss die datenbankklasse für Spiele erweitert werden
		////    ////try
		////    ////{
		////    ////    TSpiele e;
		////    ////    
		////    ////    using (fsbDB FSBDB = new fsbDB())
		////    ////    {
		////    ////        e = (from s in FSBDB.TSpiele where s.TurnierID == spiel.TurnierID && s.Spiel == spiel.Spiel select s).FirstOrDefault();
		////    ////        e.FoulA = Convert.ToInt32(ClsVariablen.Instance.Foul1);
		////    ////        e.FoulB = Convert.ToInt32(ClsVariablen.Instance.Foul2);
		////    ////        FSBDB.SaveChanges();
		////    ////    }
		////    ////}
		////    ////catch (Exception ex)
		////    ////{
		////    ////    ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
		////    ////}
		////}




		public static void SaveSpielStatus(TSpiele spiel, int status)
		{
			try
			{
				TSpiele e;

				using (fsbDB FSBDB = new fsbDB())
				{
					e = (from s in FSBDB.TSpiele
						 where s.TurnierID == spiel.TurnierID && s.Spiel == spiel.Spiel
						 select s).Single();

					e.Status = status;
					FSBDB.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		public static void SaveGruppenStatus(int TurnierID, string gruppe, int status)
		{
			try
			{
				TGruppen e;

				using (fsbDB FSBDB = new fsbDB())
				{
					e = (from g in FSBDB.TGruppen
						 where g.TurnierID == TurnierID && g.Gruppe == gruppe
						 select g).FirstOrDefault();

					e.status = status;
					FSBDB.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		public static void SaveRundenStatus(int TurnierID, int runde, int status)
		{
			try
			{
				TRunden e;

				using (fsbDB FSBDB = new fsbDB())
				{
					e = (from g in FSBDB.TRunden
						 where g.TurnierID == TurnierID && g.Runde == runde
						 select g).FirstOrDefault();

					e.status = status;
					FSBDB.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		public static void SaveTurnierStatus(int TurnierID, int status)
		{
			try
			{
				TKopf e;

				using (fsbDB FSBDB = new fsbDB())
				{
					e = (from g in FSBDB.TKopf
						 where g.ID == TurnierID
						 select g).FirstOrDefault();

					if (e != null)
					{
						e.status = status;
						FSBDB.SaveChanges();
					}
				}
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		public static DataTable Torschuetzen(int TurnierID)
		{
			try
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Mannschaft");
				dt.Columns.Add("Spieler");
				dt.Columns.Add("Tore");

				using (fsbDB FSBDB = new fsbDB())
				{
					var x = (
								from TEreignisse e
								in FSBDB.TEreignisse
								where e.TurnierID == TurnierID && e.Ereignistyp == "05"
								group e by new { e.Mannschaft, e.Spieler } into grp
								select new
								{
									grp.Key.Mannschaft,
									grp.Key.Spieler,
									Tore = grp.Select(e => e.Ereignistyp).Count()
								}

							).OrderByDescending(y => y.Tore).ToList();

					for (int i = 0; i < x.Count; i++)
					{
						DataRow dr = dt.NewRow();
						dr[0] = x[i].Mannschaft;
						dr[1] = x[i].Spieler;
						dr[2] = x[i].Tore;
						dt.Rows.Add(dr);
					}
				}

				return dt;
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new DataTable();
			}
		}

		public static List<ClsTorschuetze> TorschuetzenList(int TurnierID)
		{
			try
			{
				List<ClsTorschuetze> ret = new List<ClsTorschuetze>();
				using (fsbDB FSBDB = new fsbDB())
				{
					var x = (
								from TEreignisse e
								in FSBDB.TEreignisse
								where e.TurnierID == TurnierID && e.Ereignistyp == "05"
								group e by new { e.Mannschaft, e.Spieler } into grp
								select new
								{
									grp.Key.Mannschaft,
									grp.Key.Spieler,
									Tore = grp.Select(e => e.Ereignistyp).Count()
								}

							).OrderByDescending(y => y.Tore).ToList();

					for (int i = 0; i < x.Count; i++)
					{
						ClsTorschuetze nt = new ClsTorschuetze();
						nt.Mannschaft = x[i].Mannschaft;
						nt.Spieler = x[i].Spieler;
						nt.Tore = x[i].Tore;
						ret.Add(nt);
					}
				}

				return ret;
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new ClsTorschuetze[0].ToList();
			}
		}
	}
}
