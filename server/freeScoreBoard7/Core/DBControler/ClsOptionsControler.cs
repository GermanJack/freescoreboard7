using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FreeScoreBoard.Core.DB;
using FreeScoreBoard.Interpreter.Error;

namespace FreeScoreBoard.Core.DBControler
{
	public static class ClsOptionsControler
	{
		private static readonly string Name = MethodBase.GetCurrentMethod().DeclaringType.Name;

		public static string Password(string Name)
		{
			try
			{
				User st;

				using (fsbDB FSBDB = new fsbDB())
				{
					st = (from x in FSBDB.User
						  where x.Name == Name
						  select x).FirstOrDefault();
				}

				return st.Password;
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return "";
			}
		}

		public static int SavePassword(string Name, string Password)
		{
			try
			{
				User st;

				using (fsbDB FSBDB = new fsbDB())
				{
					st = (from x in FSBDB.User
						  where x.Name == Name
						  select x).FirstOrDefault();

					st.Password = Password;

					FSBDB.SaveChanges();

				}

				return 0;

			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return 1;
			}
		}

		public static Strafen Strafe(string bezeichnung)
		{
			try
			{
				Strafen st;

				using (fsbDB FSBDB = new fsbDB())
				{
					st = (from x in FSBDB.Strafen
						  where x.Bezeichnung == bezeichnung
						  select x).FirstOrDefault();
				}

				return st;
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new Strafen();
			}
		}

		public static Strafen Strafe(int ID)
		{
			try
			{
				Strafen st;

				using (fsbDB FSBDB = new fsbDB())
				{
					st = (from x in FSBDB.Strafen
						  where x.ID == ID
						  select x).FirstOrDefault();
				}

				return st;
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new Strafen();
			}
		}

		public static List<Strafen> Strafen()
		{
			try
			{
				List<Strafen> st;

				using (fsbDB FSBDB = new fsbDB())
				{
					st = (from x in FSBDB.Strafen
						  select x).ToList();
				}

				return st;
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new Strafen[0].ToList();
			}
		}

		public static void AddStrafe(Strafen strafe)
		{
			try
			{
				using (fsbDB FSBDB = new fsbDB())
				{
					long newID = (from x in FSBDB.Strafen select x.ID).DefaultIfEmpty(0).Max() + 1;
					strafe.ID = newID;
					FSBDB.Strafen.Add(strafe);
					FSBDB.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		public static void SaveStrafe(Strafen strafe)
		{
			try
			{
				Strafen st;

				using (fsbDB FSBDB = new fsbDB())
				{
					st = (from x in FSBDB.Strafen
						  where x.ID == strafe.ID
						  select x).FirstOrDefault();

					foreach (PropertyInfo pi in strafe.GetType().GetProperties())
					{
						if (pi.CanWrite)
						{
							pi.SetValue(st, pi.GetValue(strafe, null), null);
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

		public static void DelStrafe(string bezeichnung)
		{
			try
			{
				Strafen st;

				using (fsbDB FSBDB = new fsbDB())
				{
					st = (from x in FSBDB.Strafen
						  where x.Bezeichnung == bezeichnung
						  select x).FirstOrDefault();

					FSBDB.Strafen.Remove(st);
					FSBDB.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		////---------------------------------------------------------------------------

		////public List<string> Options1Strings(string gruppentyp)
		////{
		////    try
		////    {
		////        List<string> o1;
		////
		////        using (fsbDB FSBDB = new fsbDB())
		////        {
		////            e = (from Options1 g1 in FSBDB.Options1
		////                           where g1.Grouptype == gruppentyp
		////                           orderby g1.GroupSeq
		////                           select g1.Group).ToList();
		////        }
		////
		////        return o1;
		////    }
		////    catch (Exception ex)
		////    {
		////        ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
		////        return new string[0].ToList();
		////    }
		////}

		public static List<Options3> Options3()
		{
			try
			{
				List<Options3> o3;

				using (fsbDB FSBDB = new fsbDB())
				{
					o3 = (from Options3 g2 in FSBDB.Options3 select g2).ToList();
				}

				return o3;
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new Options3[0].ToList();
			}
		}

		public static List<Options1> Options1()
		{
			try
			{
				List<Options1> o1;

				using (fsbDB FSBDB = new fsbDB())
				{
					o1 = (from Options1 g2 in FSBDB.Options1 select g2).ToList();
				}

				return o1;
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new Options1[0].ToList();
			}
		}

		public static List<Options2> Options2(int Group)
		{
			try
			{
				List<Options2> o2;

				using (fsbDB FSBDB = new fsbDB())
				{
					o2 = (from Options2 g2 in FSBDB.Options2 where g2.GroupNr == Group select g2).ToList();
				}

				return o2;
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new Options2[0].ToList();
			}
		}

		public static List<Options21> Options21(string type)
		{
			try
			{
				List<Options21> o21;

				using (fsbDB FSBDB = new fsbDB())
				{
					o21 = (from Options21 g2 in FSBDB.Options21 where g2.Type == type select g2).ToList();
				}

				return o21;
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new Options21[0].ToList();
			}
		}

		public static Options3 Options3(string option, string requestor = "")
		{
			try
			{
				Options3 o3;

				using (fsbDB FSBDB = new fsbDB())
				{
					o3 = (from Options3 g2 in FSBDB.Options3
						  where g2.Prop == option
						  select g2).FirstOrDefault();
				}

				return o3;
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString() + " > " + requestor, ex);
				return new Options3();
			}
		}

		public static void SaveOptions3(Options3 option)
		{
			try
			{
				Options3 o3;

				using (fsbDB FSBDB = new fsbDB())
				{
					o3 = (from Options3 g2 in FSBDB.Options3
						  where g2.ID == option.ID
						  select g2).FirstOrDefault();

					if (o3 != null)
					{
						foreach (PropertyInfo pi in option.GetType().GetProperties())
						{
							if (pi.CanWrite)
							{
								pi.SetValue(o3, pi.GetValue(option, null), null);
							}
						}
					}
					else
					{
						FSBDB.Options3.Add(option);
					}

					FSBDB.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		////-------------------------------------------------------------------------------

		public static List<TabellenSort> Tabellensortierung()
		{
			try
			{
				List<TabellenSort> ts;

				using (fsbDB FSBDB = new fsbDB())
				{
					ts = (from x in FSBDB.TabellenSort
						  orderby x.Prio
						  select x).ToList();
				}

				return ts;
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new TabellenSort[0].ToList();
			}
		}

		public static TabellenSort Tabellensortierung(string tabellenfeld)
		{
			try
			{
				TabellenSort ts;

				using (fsbDB FSBDB = new fsbDB())
				{
					ts = (from x in FSBDB.TabellenSort
						  where x.Feld == tabellenfeld
						  select x).FirstOrDefault();
				}

				return ts;
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new TabellenSort();
			}
		}

		public static TabellenSort Tabellensortierung(int id)
		{
			try
			{
				TabellenSort ts;

				using (fsbDB FSBDB = new fsbDB())
				{
					ts = (from x in FSBDB.TabellenSort
						  where x.ID == id
						  select x).FirstOrDefault();
				}

				return ts;
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new TabellenSort();
			}
		}

		public static void SaveTabellensortierung(TabellenSort sortierung)
		{
			try
			{
				TabellenSort ts;

				using (fsbDB FSBDB = new fsbDB())
				{
					ts = (from x in FSBDB.TabellenSort
						  where x.ID == sortierung.ID
						  select x).FirstOrDefault();

					foreach (PropertyInfo pi in sortierung.GetType().GetProperties())
					{
						if (pi.CanWrite)
						{
							pi.SetValue(ts, pi.GetValue(sortierung, null), null);
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

		////----------------------------------------------------------------------------------

		public static List<Anzeigetabelle> AnzeigetabelleAll()
		{
			try
			{
				List<Anzeigetabelle> l;

				using (fsbDB FSBDB = new fsbDB())
				{
					l = (from t in FSBDB.Anzeigetabelle
						 orderby t.Position
						 select t).ToList();
				}

				return l;
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new Anzeigetabelle[0].ToList();
			}
		}

		public static List<Anzeigetabelle> Anzeigetabelle(string tabelle, string variante)
		{
			try
			{
				List<Anzeigetabelle> l;

				using (fsbDB FSBDB = new fsbDB())
				{
					l = (from t in FSBDB.Anzeigetabelle
						 where t.Tabelle == tabelle && t.Variante == variante
						 orderby t.Position
						 select t).ToList();
				}

				return l;
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new Anzeigetabelle[0].ToList();
			}
		}

		public static List<Anzeigetabelle> Anzeigetabelle(string tabelle)
		{
			try
			{
				List<Anzeigetabelle> l;

				using (fsbDB FSBDB = new fsbDB())
				{
					l = (from t in FSBDB.Anzeigetabelle
						 where t.Tabelle == tabelle
						 orderby t.Position
						 select t).ToList();
				}

				return l;
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new Anzeigetabelle[0].ToList();
			}
		}

		public static void AddAnzeigetabelle(Anzeigetabelle anzeigetabelle)
		{
			try
			{
				using (fsbDB FSBDB = new fsbDB())
				{
					long newID = (from x in FSBDB.Anzeigetabelle select x.ID).DefaultIfEmpty(0).Max() + 1;
					anzeigetabelle.ID = newID;
					FSBDB.Anzeigetabelle.Add(anzeigetabelle);
					FSBDB.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		public static void DelAnzeigetabelle(int id)
		{
			try
			{
				Anzeigetabelle at;

				using (fsbDB FSBDB = new fsbDB())
				{
					at = (from x in FSBDB.Anzeigetabelle
						  where x.ID == id
						  select x).FirstOrDefault();

					FSBDB.Anzeigetabelle.Remove(at);
					FSBDB.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		public static void SaveAnzeigetabelle(Anzeigetabelle anzeigetabelle)
		{
			try
			{
				Anzeigetabelle at;

				using (fsbDB FSBDB = new fsbDB())
				{
					at = (from x in FSBDB.Anzeigetabelle
						  where x.ID == anzeigetabelle.ID
						  select x).FirstOrDefault();

					foreach (PropertyInfo pi in anzeigetabelle.GetType().GetProperties())
					{
						if (pi.CanWrite)
						{
							pi.SetValue(at, pi.GetValue(anzeigetabelle, null), null);
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
	}
}
