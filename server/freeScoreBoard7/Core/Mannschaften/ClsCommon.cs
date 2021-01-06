using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FreeScoreBoard.Core.DBControler;
using FreeScoreBoard.Server;

namespace FreeScoreBoard.Core.Mannschaften
{
	public class ClsCommon
	{
		public static string[] Mannschaftsnamen()
		{
			List<DB.Mannschaften> ml = ClsMannschaftenControler.Mannschaften();
			List<string> mnl = (from m in ml select m.Name).ToList();

			return mnl.ToArray();
		}

		public static List<DB.Mannschaften> Mannschaften()
		{
			List<DB.Mannschaften> ml = ClsMannschaftenControler.Mannschaften();
			ml.Sort((a, b) => string.Compare(a.Name, b.Name));
			return ml;
		}

		public static void NewMannschaft(string Mannschaft)
		{
			if (Mannschaft?.Length == 0)
			{
				return;
			}

			DB.Mannschaften nm = new DB.Mannschaften
			{
				Name = Mannschaft
			};
			ClsMannschaftenControler.AddMannschaft(nm);
		}

		public static void DelMannschaft(string MannschaftsID)
		{
			long mid = Convert.ToInt32(MannschaftsID);
			ClsMannschaftenControler.DelMannschaft(mid);
		}

		public static void ChangeMannschaft(string MannschaftsID, string Feld, string Wert)
		{
			long mid = Convert.ToInt32(MannschaftsID);

			DB.Mannschaften mannschaft = ClsMannschaftenControler.Mannschaft(mid);
			foreach (PropertyInfo pi in mannschaft.GetType().GetProperties())
			{
				if (pi.Name == Feld)
				{
					pi.SetValue(mannschaft, Wert, null);
				}
			}

			ClsMannschaftenControler.SaveMannschaft(mannschaft);
		}

		public static List<DB.Spieler> SpielerListe(string MannschaftsID)
		{
			if (string.IsNullOrEmpty(MannschaftsID))
			{
				return new List<DB.Spieler>();
			}

			int mid = Convert.ToInt32(MannschaftsID);
			List<DB.Spieler> sl = ClsMannschaftenControler.Spielers(mid);
			sl.Sort((a, b) => string.Compare(a.Nachname, b.Nachname));
			return sl;
		}

		public static List<DB.Spieler> SpielerListe()
		{
			List<DB.Spieler> sl = ClsMannschaftenControler.Spielers();
			sl.Sort((a, b) => string.Compare(a.Nachname, b.Nachname));
			return sl;
		}

		public static void NewSpieler(string MannschaftsID, string SpielerName)
		{
			if (SpielerName?.Length == 0)
			{
				return;
			}

			long mid = 0;
			try
			{
				mid = Convert.ToInt32(MannschaftsID);
			}
			catch
			{
				mid = 0;
			}

			DB.Spieler nm = new DB.Spieler
			{
				MannschaftsID = mid,
				Nachname = SpielerName
			};
			ClsMannschaftenControler.AddSpieler(nm);
		}

		public static void DelSpieler(string SpielerID)
		{
			if (SpielerID?.Length == 0)
			{
				return;
			}

			int sid = Convert.ToInt32(SpielerID);

			ClsMannschaftenControler.DelSpieler(sid);
		}

		public static void ChangeSpieler(string SpielerID, string Feld, string Wert)
		{
			if (SpielerID?.Length == 0)
			{
				return;
			}

			int sid = Convert.ToInt32(SpielerID);

			DB.Spieler spieler = ClsMannschaftenControler.Spieler(sid);
			foreach (PropertyInfo pi in spieler.GetType().GetProperties())
			{
				if (pi.Name == Feld)
				{
					if (Feld == "MannschaftsID")
					{
						pi.SetValue(spieler, Convert.ToInt64(Wert), null);
					}
					else
					{
						pi.SetValue(spieler, Wert, null);
					}
				}
			}

			ClsMannschaftenControler.SaveSpieler(spieler);
		}
	}
}
