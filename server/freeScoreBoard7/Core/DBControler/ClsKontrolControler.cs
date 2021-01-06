using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FreeScoreBoard.Core.DB;
using FreeScoreBoard.Interpreter.Error;

namespace FreeScoreBoard.Core.DBControler
{
	public static class ClsKontrolControler
	{
		private static readonly string Name = MethodBase.GetCurrentMethod().DeclaringType.Name;

		public static List<WebKontrols> WebKontrols()
		{
			List<WebKontrols> lst = new List<WebKontrols>();
			using (fsbDB FSBDB = new fsbDB())
			{
				lst = (from s in FSBDB.WebKontrols
					   select s).ToList();
			}

			return lst;
		}

		public static WebKontrols WebKontrol(long ID)
		{
			WebKontrols wk;
			using (fsbDB FSBDB = new fsbDB())
			{
				wk = (from s in FSBDB.WebKontrols
					  where s.ID == ID
					  select s).FirstOrDefault();
			}

			return wk;
		}

		public static void SaveWebKontrol(WebKontrols Kontrol)
		{
			try
			{
				WebKontrols ma;

				using (fsbDB FSBDB = new fsbDB())
				{
					ma = (from s in FSBDB.WebKontrols
						  where s.ID == Kontrol.ID
						  select s).FirstOrDefault();

					foreach (PropertyInfo pi in Kontrol.GetType().GetProperties())
					{
						if (pi.CanWrite)
						{
							pi.SetValue(ma, pi.GetValue(Kontrol, null), null);
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


		public static List<Kontrols> Kontrols()
		{
			List<Kontrols> lst = new List<Kontrols>();
			using (fsbDB FSBDB = new fsbDB())
			{
				lst = (from s in FSBDB.Kontrols
					   orderby s.Sort
					   select s).ToList();
			}

			return lst;
		}

		public static List<Kontrols> Kontrols(string Ort)
		{
			List<Kontrols> lst = new List<Kontrols>();
			using (fsbDB FSBDB = new fsbDB())
			{
				lst = (from s in FSBDB.Kontrols
					   where s.Ort == Ort
					   orderby s.Sort
					   select s).ToList();
			}

			return lst;
		}

		public static Kontrols Kontrol(int ID)
		{
			Kontrols ret;
			using (fsbDB FSBDB = new fsbDB())
			{
				ret = (from s in FSBDB.Kontrols
					   where s.ID == ID
					   orderby s.Sort
					   select s).Single();
			}

			return ret;
		}

		public static Kontrols Kontrol(string Name)
		{
			Kontrols ret;
			using (fsbDB FSBDB = new fsbDB())
			{
				ret = (from s in FSBDB.Kontrols
					   where s.Name == Name
					   orderby s.Sort
					   select s).Single();
			}

			return ret;
		}

		public static void SaveKontrol(Kontrols Kontrol)
		{
			try
			{
				Kontrols ma;

				using (fsbDB FSBDB = new fsbDB())
				{
					ma = (from s in FSBDB.Kontrols
						  where s.ID == Kontrol.ID
						  select s).FirstOrDefault();

					foreach (PropertyInfo pi in Kontrol.GetType().GetProperties())
					{
						if (pi.CanWrite)
						{
							pi.SetValue(ma, pi.GetValue(Kontrol, null), null);
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
