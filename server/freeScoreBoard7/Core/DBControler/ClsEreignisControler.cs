using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FreeScoreBoard.Core.DB;
using FreeScoreBoard.Interpreter.Error;

namespace FreeScoreBoard.Core.DBControler
{
	class ClsEreignisControler
	{
		private static readonly string Name = MethodBase.GetCurrentMethod().DeclaringType.Name;

		public static List<EreignissTyp> EreignissTypen()
		{
			List<EreignissTyp> lst = new List<EreignissTyp>();
			using (fsbDB FSBDB = new fsbDB())
			{
				lst = (from s in FSBDB.EreignissTyp
					   orderby s.Sort
					   select s).ToList();
			}

			return lst;
		}

		public static EreignissTyp EreignissTyp(int id)
		{
			EreignissTyp res = new EreignissTyp();
			using (fsbDB FSBDB = new fsbDB())
			{
				res = (from s in FSBDB.EreignissTyp
					   where s.ID == id
					   select s).FirstOrDefault();
			}

			return res;
		}

		public static EreignissTyp EreignissTyp(string Ereignistyp)
		{
			EreignissTyp res = new EreignissTyp();
			using (fsbDB FSBDB = new fsbDB())
			{
				res = (from s in FSBDB.EreignissTyp
					   where s.Nummer == Ereignistyp
					   select s).FirstOrDefault();
			}

			return res;
		}

		public static void SaveEreignissTyp(EreignissTyp Ereignisstyp)
		{
			try
			{
				EreignissTyp ma;

				using (fsbDB FSBDB = new fsbDB())
				{
					ma = (from s in FSBDB.EreignissTyp
						  where s.ID == Ereignisstyp.ID
						  select s).FirstOrDefault();

					foreach (PropertyInfo pi in Ereignisstyp.GetType().GetProperties())
					{
						if (pi.CanWrite)
						{
							pi.SetValue(ma, pi.GetValue(Ereignisstyp, null), null);
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

		public static void DelFreeEvents()
		{
			using (fsbDB FSBDB = new fsbDB())
			{
				List<TEreignisse> el = (from s in FSBDB.TEreignisse
										where s.Spiel == 0
										select s).ToList();
				for (int i = 0; i < el.Count; i++)
				{
					FSBDB.TEreignisse.Remove(el[i]);
					FSBDB.SaveChanges();
				}
			}
		}

		public static void DelEvent(int ID)
		{
			using (fsbDB FSBDB = new fsbDB())
			{
				TEreignisse e = (from s in FSBDB.TEreignisse
								 where s.ID == 0
								 select s).FirstOrDefault();
				FSBDB.TEreignisse.Remove(e);
				FSBDB.SaveChanges();
			}
		}

		public static void AddEreignis(TEreignisse ereignis)
		{
			try
			{
				// prüfen ob gespeichert werden muss
				EreignissTyp e = ClsEreignisControler.EreignissTyp(ereignis.Ereignistyp);
				if (!e.Log)
				{
					return;
				}

				using (fsbDB FSBDB = new fsbDB())
				{
					long newID = (from x in FSBDB.TEreignisse select x.ID).DefaultIfEmpty(0).Max() + 1;
					ereignis.ID = newID;
					FSBDB.TEreignisse.Add(ereignis);
					FSBDB.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		public static void SaveEreigniss(TEreignisse ereignis)
		{
			try
			{
				TEreignisse l;

				using (fsbDB FSBDB = new fsbDB())
				{
					l = (from x in FSBDB.TEreignisse
						 where x.ID == ereignis.ID
						 select x).FirstOrDefault();

					foreach (PropertyInfo pi in ereignis.GetType().GetProperties())
					{
						if (pi.CanWrite)
						{
							pi.SetValue(l, pi.GetValue(ereignis, null), null);
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


		public static void DelEreignis(int EreignisID)
		{
			TEreignisse l;

			using (fsbDB FSBDB = new fsbDB())
			{
				l = (from x in FSBDB.TEreignisse
					 where x.ID == EreignisID
					 select x).First();

				FSBDB.TEreignisse.Remove(l);

				FSBDB.SaveChanges();
			}
		}

		public static void DelEreignisse(int TurnierID)
		{
			List<TEreignisse> l;

			using (fsbDB FSBDB = new fsbDB())
			{
				l = (from x in FSBDB.TEreignisse
					 where x.TurnierID == TurnierID
					 select x).ToList();

				foreach (TEreignisse x in l)
				{
					FSBDB.TEreignisse.Remove(x);
				}

				FSBDB.SaveChanges();
			}
		}

		public static TEreignisse Ereignis(int EreignisID)
		{
			try
			{
				TEreignisse t;

				using (fsbDB FSBDB = new fsbDB())
				{
					t = (from x in FSBDB.TEreignisse
						 where x.ID == EreignisID
						 select x).First();
				}

				return t;
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new TEreignisse();
			}
		}

		public static List<TEreignisse> Ereignisse(int TurnierID)
		{
			try
			{
				List<TEreignisse> t;

				using (fsbDB FSBDB = new fsbDB())
				{
					t = (from x in FSBDB.TEreignisse
						 where x.TurnierID == TurnierID
						 orderby x.Spiel, x.CPUZeit
						 select x).ToList();
				}

				if (t.Any())
				{
					for (int i = 0; i < t.Count; i++)
					{
						t[i].Ereignistyp = ClsTextControler.TextByNameAndNumber("Ereignis", t[i].Ereignistyp);

						string opt_EreigniszeitGenau = ClsOptionsControler.Options3("EreigniszeitGenau").Value;
						if (opt_EreigniszeitGenau == "False")
						{
							t[i].Spielzeit = ClsTabellenfunktionen.ConvertSpielzeitToKurzzeit(t[i].Spielzeit, t[i].Spielzeitrichtung);
						}
					}
				}

				return t;
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new TEreignisse[0].ToList();
			}
		}

		public static int EreignisseMenge(int TurnierID)
		{
			try
			{
				int t;

				using (fsbDB FSBDB = new fsbDB())
				{
					t = (from x in FSBDB.TEreignisse
						 where x.TurnierID == TurnierID
						 orderby x.Spiel, x.CPUZeit
						 select x).Count();
				}

				return t;
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return 0;
			}
		}

		public static List<TEreignisse> Ereignisse(int TurnierID, int spielNr)
		{
			try
			{
				List<TEreignisse> t;

				using (fsbDB FSBDB = new fsbDB())
				{
					t = (from x in FSBDB.TEreignisse
						 where x.TurnierID == TurnierID && x.Spiel == spielNr
						 orderby x.Spiel, x.CPUZeit
						 select x).ToList();
				}

				return t;
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new TEreignisse[0].ToList();
			}
		}
	}
}
