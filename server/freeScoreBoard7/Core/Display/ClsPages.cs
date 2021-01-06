using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FreeScoreBoard.Core.DB;
using FreeScoreBoard.Core.DBControler;
using FreeScoreBoard.Interpreter.Error;

namespace FreeScoreBoard.Core.Display
{
	public static class ClsPages
	{
		private static readonly string Name = MethodBase.GetCurrentMethod().DeclaringType.Name;

		/// <summary>
		/// Returns the Pagename with the lowest meta tag sort.
		/// </summary>
		/// <param name="SetName"></param>
		/// <returns></returns>
		public static int GetFirstPageName(int PageSetID)
		{
			List<DisplayPage> pl = PageObjectList(PageSetID);

			int ret = (int)pl[0].ID;
			int s0 = Convert.ToInt32(pl[0].Sort);
			for (int i = 0; i < pl.Count; i++)
			{
				int s1 = Convert.ToInt32(pl[i].Sort);
				if (s1 < s0)
				{
					ret = (int)pl[i].ID;
					s0 = s1;
				}
			}

			return ret;
		}

		///// <summary>
		///// Returns the highest meta tag sort.
		///// </summary>
		///// <param name="SetName"></param>
		///// <returns></returns>
		//public static string GetMaxPageSort(string SetName)
		//{
		//	List<DisplayPage> pl = PageObjectList(SetName);

		//	string ret = "0";
		//	int s0 = Convert.ToInt32(pl[0].Sort);
		//	for (int i = 0; i < pl.Count; i++)
		//	{
		//		int s1 = Convert.ToInt32(pl[i].Sort);
		//		if (s1 > s0)
		//		{
		//			ret = s1.ToString();
		//			s0 = s1;
		//		}
		//	}

		//	return ret;
		//}

		/// <summary>
		/// Returns a sorted StringArray of the Page Names.
		/// </summary>
		/// <param name="SetName"></param>
		/// <returns></returns>
		public static string[] PageNameList(string SetName)
		{
			List<DisplayPageSet> l = ClsDisplayControler.DisplayPageSets();
			DisplayPageSet dps = (from x in l where x.PageSetName == SetName select x).FirstOrDefault();

			List<DisplayPage> pl = ClsDisplayControler.DisplayPagesForPageSet(dps.ID).OrderBy(x => x.Sort).ToList();

			// create name array
			string[] fl = (from x in pl select x.PageName).ToArray();

			// return Pagelist
			return fl;
		}

		/// <summary>
		/// Returns a List of the Page Objects.
		/// </summary>
		/// <param name="SetName"></param>
		/// <returns></returns>
		public static List<DisplayPage> PageList(int SetID)
		{
			List<DisplayPageSet> l = ClsDisplayControler.DisplayPageSets();
			DisplayPageSet dps = (from x in l where x.ID == SetID select x).FirstOrDefault();

			List<DisplayPage> pl = ClsDisplayControler.DisplayPagesForPageSet(dps.ID).OrderBy(x => x.Sort).ToList();

			// return Pagelist
			return pl;
		}

		/// <summary>
		/// Returns a un-sorted classlist of the Pages.
		/// </summary>
		/// <param name="SetName"></param>
		/// <returns></returns>
		public static List<DisplayPage> PageObjectList(int PageSetID)
		{
			try
			{
				List<DisplayPageSet> l = ClsDisplayControler.DisplayPageSets();
				DisplayPageSet dps = (from x in l where x.ID == PageSetID select x).FirstOrDefault();

				List<DisplayPage> pl = ClsDisplayControler.DisplayPagesForPageSet(dps.ID);


				return pl;
			}
			catch (Exception ex)
			{
				ClsError.Error(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new List<DisplayPage>();
			}
		}

		/// <summary>
		/// Returns a Style Value of the Page.
		/// </summary>
		/// <param name="SetName"></param>
		/// <param name="Page"></param>
		/// /// <param name="StyleProperty"></param>
		/// <returns></returns>
		public static string GetStyleValue(string SetName, string Page, string StyleProperty)
		{
			List<DisplayPageSet> l = ClsDisplayControler.DisplayPageSets();
			DisplayPageSet dps = (from x in l where x.PageSetName == SetName select x).FirstOrDefault();

			List<DisplayPage> pl = ClsDisplayControler.DisplayPagesForPageSet(dps.ID);
			DisplayPage dp = (from x in pl where x.PageName == Page select x).FirstOrDefault();

			string ret = "";
			using (ClsStyleGenerator sg = new ClsStyleGenerator())
			{
				sg.ParseStyleString(dp.Style);
				ret = sg.GetStyle(StyleProperty);
			}

			return ret;
		}

		/// <summary>
		/// Return the Page properties.
		/// </summary>
		/// <param name="SetName"></param>
		/// <param name="Title"></param>
		public static DisplayPage Page(int SetID, int PageID)
		{
			try
			{
				List<DisplayPageSet> l = ClsDisplayControler.DisplayPageSets();
				DisplayPageSet dps = (from x in l where x.ID == SetID select x).FirstOrDefault();

				if (dps != null)
				{
					List<DisplayPage> pl = ClsDisplayControler.DisplayPagesForPageSet(dps.ID);
					DisplayPage dp = (from x in pl where x.ID == PageID select x).FirstOrDefault();

					return dp;
				}

				return new DisplayPage();
			}
			catch (Exception ex)
			{
				ClsError.Error(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new DisplayPage();
			}
		}

		public static string GetPageNameFromID(int PageID)
		{
			DisplayPage dp = ClsDisplayControler.DisplayPage(PageID);
			if (dp == null)
			{
				return "";
			}

			return dp.PageName;
		}

		/// <summary>
		/// Creates new Page.
		/// </summary>
		/// <param name="SetName"></param>
		/// <param name="newName"></param>
		public static long NewPage(int PageSetID, string newName, string Style = "", string MarkColor = "rgba(208, 2, 27, 1)", long Grid = 20)
		{
			if (Style == "")
			{
				using (ClsStyleGenerator sg = new ClsStyleGenerator())
				{
					sg.SetStyle("margin", "0px");
					sg.SetStyle("width", "100vw");
					sg.SetStyle("height", "100vh");
					sg.SetStyle("background-color", "rgba(0, 0, 0, 1)");
					sg.SetStyle("background-repeat", "no-repeat");
					sg.SetStyle("background-position", "center");
					sg.SetStyle("background-image", "");
					sg.SetStyle("background-size", "contain");

					Style = sg.GetStyleStringJson();
				}
			}

			// get max page sort
			List<DisplayPageSet> l = ClsDisplayControler.DisplayPageSets();
			List<DisplayPage> dpl = ClsDisplayControler.DisplayPagesForPageSet(PageSetID);
			long max = 0;
			if (dpl.Count > 0)
			{
				max = (from x in dpl select x.Sort).Max();
			}
			long newmax = max + 1;

			DisplayPage dp = new DisplayPage();
			dp.PageSetNo = PageSetID;
			dp.PageName = newName;
			dp.Style = Style;
			dp.Sort = newmax;
			dp.MarkColor = MarkColor;
			dp.Grid = Grid;

			ClsDisplayControler.AddDisplayPage(dp);

			dpl = ClsDisplayControler.DisplayPagesForPageSet(PageSetID);
			long dpid = (from x in dpl where x.PageName == newName select x.ID).FirstOrDefault();
			return dpid;
		}

		/// <summary>
		/// Delete Page.
		/// </summary>
		/// <param name="PageSetID"></param>
		/// <param name="PageID"></param>
		public static void DelPage(int PageSetID, int PageID)
		{
			ClsDisplayControler.DelDisplayPage(PageID);
		}

		/// <summary>
		/// Copy Page within a PageSet.
		/// </summary>
		/// <param name="PageSetID"></param>
		/// <param name="PageID"></param>
		/// <param name="newName"></param>
		public static void CopyPage(int PageSetID, int PageID, string newName)
		{
			List<DisplayPageSet> l = ClsDisplayControler.DisplayPageSets();
			DisplayPageSet dps = (from x in l where x.ID == PageSetID select x).FirstOrDefault();
			DisplayPage dp = (from x in ClsDisplayControler.DisplayPagesForPageSet(dps.ID) where x.ID == PageID select x).FirstOrDefault();

			long dpid = NewPage(PageSetID, newName, dp.Style, dp.MarkColor, dp.Grid);

			// copy objects
			List<DisplayObject> ol = ClsDisplayControler.DisplayObjectsForPage(dp.ID);
			for (int i = 0; i < ol.Count; i++)
			{
				ClsDivs.NewDiv(dps.ID, dpid, ol[i].textid, ol[i].bgid, ol[i].tableid, ol[i].style, ol[i].innerText, ol[i].Speed, ol[i].TableStyle);
			}
		}

		/// <summary>
		/// Copy Page cross PageSet.
		/// </summary>
		/// <param name="SetName"></param>
		/// <param name="oldName"></param>
		/// <param name="newName"></param>
		public static void CopyPage(int SourcePageSetID, int TargetPageSetID, string oldPageName, string newPageName)
		{
			List<DisplayPageSet> dpsl = ClsDisplayControler.DisplayPageSets();

			DisplayPage sdp = (from x in ClsDisplayControler.DisplayPagesForPageSet(SourcePageSetID) where x.PageName == oldPageName select x).FirstOrDefault();

			long dpid = NewPage(TargetPageSetID, newPageName, sdp.Style, sdp.MarkColor, sdp.Grid);

			// copy objects
			List<DisplayObject> ol = ClsDisplayControler.DisplayObjectsForPage(sdp.ID);
			for (int i = 0; i < ol.Count; i++)
			{
				ClsDivs.NewDiv(TargetPageSetID, dpid, ol[i].textid, ol[i].bgid, ol[i].tableid, ol[i].style, ol[i].innerText, ol[i].Speed, ol[i].TableStyle);
			}
		}

		/// <summary>
		/// Rename Page.
		/// </summary>
		/// <param name="PageSetID"></param>
		/// <param name="PageID"></param>
		/// <param name="newName"></param>
		public static void RenamePage(int PageSetID, int PageID, string newName)
		{
			List<DisplayPageSet> l = ClsDisplayControler.DisplayPageSets();
			DisplayPageSet dps = (from x in l where x.ID == PageSetID select x).FirstOrDefault();
			DisplayPage dp = (from x in ClsDisplayControler.DisplayPagesForPageSet(dps.ID) where x.ID == PageID select x).FirstOrDefault();

			dp.PageName = newName;

			ClsDisplayControler.SaveDisplayPage(dp);
		}

		/// <summary>
		/// Set Style property of Page.
		/// </summary>
		/// <param name="SetName"></param>
		/// <param name="Page"></param>
		/// <param name="StyleProperty"></param>
		/// <param name="StyleWert"></param>
		public static void SetStyleWert(int PageSetID, int PageID, string StyleProperty, string StyleWert)
		{
			List<DisplayPageSet> l = ClsDisplayControler.DisplayPageSets();
			DisplayPageSet dps = (from x in l where x.ID == PageSetID select x).FirstOrDefault();
			DisplayPage dp = (from x in ClsDisplayControler.DisplayPagesForPageSet(dps.ID) where x.ID == PageID select x).FirstOrDefault();


			using (ClsStyleGenerator sg = new ClsStyleGenerator())
			{
				sg.ParseStyleStringJson(dp.Style);
				sg.SetStyle(StyleProperty, StyleWert);
				dp.Style = sg.GetStyleStringJson();
			}

			ClsDisplayControler.SaveDisplayPage(dp);
		}

		/// <summary>		  --------------------------------
		/// Saves the style property of mark Div.
		/// </summary>
		/// <param name="SetName"></param>
		/// <param name="Page"></param>
		/// <param name="StyleProperty"></param>
		/// <param name="StyleWert"></param>
		public static void SetMarkStyleWert(int PageSetID, int PageID, string StyleProperty, string StyleWert)
		{
			DisplayPage dp = (from x in ClsDisplayControler.DisplayPagesForPageSet(PageSetID) where x.ID == PageID select x).FirstOrDefault();

			if (StyleProperty == "border-color")
			{
				dp.MarkColor = StyleWert;
			}

			if (StyleProperty == "Grid")
			{
				dp.Grid = Convert.ToInt64(StyleWert);
			}

			ClsDisplayControler.SaveDisplayPage(dp);
		}
	}
}
