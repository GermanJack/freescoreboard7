using System.Collections.Generic;
using System.Linq;
using FreeScoreBoard.Core.DB;

namespace FreeScoreBoard.Core.DBControler
{
	public static class ClsVariablenControler
	{
		public static List<DB.Variablen> Variablen(string Typ)
		{
			List<DB.Variablen> lst = new List<DB.Variablen>();
			using (fsbDB FSBDB = new fsbDB())
			{
				lst = (from s in FSBDB.Variablen
					   where s.Typ == Typ
					   select s).ToList();
			}

			return lst;
		}

		public static List<DB.Variablen> Variablen()
		{
			List<DB.Variablen> lst = new List<DB.Variablen>();
			using (fsbDB FSBDB = new fsbDB())
			{
				lst = (from s in FSBDB.Variablen
					   select s).ToList();
			}

			return lst;
		}

		/// <summary>
		/// Speichert den dafaultwert für die Variable.
		/// Für Tabellen ist der Default Wert die anzahl der Zeilen je Anzeigeseite
		/// </summary>
		/// <param name="Variable"></param>
		/// <param name="Wert"></param>
		public static void SaveDefault(string Variable, string Wert)
		{
			using (fsbDB FSBDB = new fsbDB())
			{
				DB.Variablen v = (from s in FSBDB.Variablen where s.Name == Variable select s).FirstOrDefault();
				if (v == null)
				{
					return;
				}

				v.Default = Wert;
				FSBDB.SaveChanges();
			}
		}

		public static string Default(string Variable)
		{
			using (fsbDB FSBDB = new fsbDB())
			{
				DB.Variablen v = (from s in FSBDB.Variablen where s.Name == Variable select s).FirstOrDefault();
				if (v == null)
				{
					return "";
				}

				return v.Default;
			}
		}

		public static bool LogChange(string Variable)
		{
			using (fsbDB FSBDB = new fsbDB())
			{
				DB.Variablen v = (from s in FSBDB.Variablen where s.Name == Variable select s).FirstOrDefault();
				if (v == null)
				{
					return false;
				}

				return v.LogChange;
			}
		}
	}
}
