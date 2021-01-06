using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeScoreBoard.Core.DB;
using FreeScoreBoard.Core.DBControler;

namespace FreeScoreBoard.Core.Display
{
	public static class ClsPageSets
	{
		public static string[] PageSetNames()
		{
			List<string> dl = (from x in ClsDisplayControler.DisplayPageSets() select x.PageSetName).ToList();
			return dl.ToArray();
		}

		public static string GetPageSetNameFromID(int PageSetID)
		{
			string psn = (from x in ClsDisplayControler.DisplayPageSets() where x.ID == PageSetID select x.PageSetName).FirstOrDefault();
			return psn;
		}

		public static long[] PageSetIDs()
		{
			List<long> dl = (from x in ClsDisplayControler.DisplayPageSets() select x.ID).ToList();
			return dl.ToArray();
		}

		public static List<DisplayPageSet> PageSets()
		{
			List<DisplayPageSet> dl = (from x in ClsDisplayControler.DisplayPageSets() select x).ToList();
			return dl;
		}

		public static int NewPageSet(string newName, bool firstPage = true)
		{
			long s = (from x in ClsDisplayControler.DisplayPageSets() select x.Sort).Max();
			DisplayPageSet dps = new DisplayPageSet();
			dps.PageSetName = newName;
			dps.Sort = s + 1;
			long id = ClsDisplayControler.AddDisplayPageSet(dps);

			// add first Page
			if (firstPage)
			{
				ClsPages.NewPage((int)id, "Spiel");
			}

			return (int)id;
		}

		public static void CopyPageSet(int sourceID, string newName)
		{
			// pageset
			int id = NewPageSet(newName, false);

			// page
			List<DisplayPage> dpl = ClsDisplayControler.DisplayPagesForPageSet(sourceID);
			for (int i = 0; i < dpl.Count; i++)
			{
				ClsPages.CopyPage(sourceID, id, dpl[i].PageName, dpl[i].PageName);
			}
		}

		public static void RenamePageSet(int PageSetID, string newName)
		{
			List<DisplayPageSet> l = ClsDisplayControler.DisplayPageSets();
			DisplayPageSet dps = (from x in l where x.ID == PageSetID select x).FirstOrDefault();
			dps.PageSetName = newName;
			ClsDisplayControler.SaveDisplayPageSet(dps);
		}

		/// <summary>
		/// delets all objects, pages and the page set itself
		/// </summary>
		/// <param name="Name">name of the page set</param>
		public static void DeletePageSet(int PageSetID)
		{
			List<DisplayPageSet> l = ClsDisplayControler.DisplayPageSets();
			DisplayPageSet dps = (from x in l where x.ID == PageSetID select x).FirstOrDefault();
			ClsDisplayControler.DelDisplayPageSet(dps.ID);
		}

		public static int ActivePageSet()
		{
			int StartPageSet = Convert.ToInt32(ClsOptionsControler.Options3("StartPageSet").Value);
			List<long> psn = PageSetIDs().ToList();
			if (psn.Contains(StartPageSet))
			{
				return Convert.ToInt32(ClsOptionsControler.Options3("StartPageSet").Value);
			}
			else
			{
				return (int)psn[0];
			}
		}
	}
}
