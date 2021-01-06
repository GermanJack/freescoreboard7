using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using FreeScoreBoard.Core.DB;
using FreeScoreBoard.Core.DBControler;
using FreeScoreBoard.Core.Objekte;
using FreeScoreBoard.Core.Variablen;
using FreeScoreBoard.Interpreter.Error;
using static FreeScoreBoard.Core.Objekte.Direction;

namespace FreeScoreBoard.Core.Display
{
	public static class ClsDivs
	{
		private readonly static string Name = MethodBase.GetCurrentMethod().DeclaringType.Name;

		/// <summary>
		/// returns a z-index Descending list of Displayobjects 
		/// </summary>
		/// <param name="SetName"></param>
		/// <param name="Page"></param>
		/// <param name="MarkierIDs"></param>
		/// <returns></returns>
		public static List<DisplayObject> DivList(int SetID, int PageID, string[] MarkierIDs = null)
		{
			try
			{
				if (SetID == 0)
				{
					return new List<DisplayObject>();
				}

				List<DisplayPageSet> dpsl = ClsDisplayControler.DisplayPageSets();
				DisplayPageSet dps = (from x in dpsl where x.ID == SetID select x).FirstOrDefault();

				List<DisplayPage> dpl = ClsDisplayControler.DisplayPagesForPageSet(dps.ID);
				DisplayPage dp = (from x in dpl where x.ID == PageID select x).FirstOrDefault();

				if (dp != null)
				{
					List<DisplayObject> ol = ClsDisplayControler.DisplayObjectsForPage(dp.ID).OrderByDescending(x => x.Zindex).ToList();
					return ol;
				}
				else
				{
					return new List<DisplayObject>();
				}
			}
			catch (Exception ex)
			{
				ClsError.Error(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new List<DisplayObject>();
			}
		}

		//public static void NewDiv(int PageSetID, int PageID)
		//{
		//	List<DisplayPageSet> dpsl = ClsDisplayControler.DisplayPageSets();
		//	DisplayPageSet dps = (from x in dpsl where x.PageSetName == SetName select x).FirstOrDefault();

		//	List<DisplayPage> dpl = ClsDisplayControler.DisplayPagesForPageSet(dps.ID);
		//	DisplayPage dp = (from x in dpl where x.PageName == Page select x).FirstOrDefault();

		//	NewDiv(PageSetID, PageID);
		//}

		public static void NewDiv(
			long SetID,
			long PageID,
			string varText = "S00",
			string varPicture = "B00",
			string varTable = "T00",
			string Style = "",
			string innerText = "",
			long Speed = 0,
			string TableStyle = "")
		{
			// zIndex berechnen
			long nextno = 0;
			if (ClsDisplayControler.DisplayObjectsForPage(PageID).Count > 0)
			{
				long max = (from x in ClsDisplayControler.DisplayObjectsForPage(PageID) select x.Zindex).Max();
				nextno = max + 1;
			}

			// pos berechnen
			string left = "10vw";
			string top = "10vh";
			if (Style != "")
			{
				using (ClsStyleGenerator sg = new ClsStyleGenerator())
				{
					sg.ParseStyleStringJson(Style);
					top = sg.GetStyle("top");
					left = sg.GetStyle("left");
				}
			}

			List<DisplayObject> ol = ClsDisplayControler.DisplayObjectsForPage(PageID);
			if (ol.Count > 0)
			{
				for (int i = 0; i < ol.Count; i++)
				{
					using (ClsStyleGenerator sg = new ClsStyleGenerator())
					{
						sg.ParseStyleStringJson(ol[i].style);
						string t = sg.GetStyle("top");
						string l = sg.GetStyle("left");
						if (t == top && l == left)
						{
							decimal to = Convert.ToDecimal(top.Substring(0, top.Length - 2), CultureInfo.InvariantCulture);
							decimal le = Convert.ToDecimal(left.Substring(0, left.Length - 2), CultureInfo.InvariantCulture);
							top = (to + 1).ToString(CultureInfo.InvariantCulture) + "vh";
							left = (le + 1).ToString(CultureInfo.InvariantCulture) + "vw";
						}
					}
				}
			}

			DisplayObject o = new DisplayObject();

			o.PageSetNo = SetID;
			o.PageNo = PageID;
			if (innerText == "")
			{
				o.innerText = "Box " + nextno;
			}
			else
			{
				o.innerText = innerText;
			}
			o.textid = varText;
			o.bgid = varPicture;
			o.tableid = varTable;
			o.Zindex = nextno;
			o.Speed = Speed;
			o.TableStyle = TableStyle;

			string s = "";
			using (ClsStyleGenerator sg = new ClsStyleGenerator())
			{
				sg.SetStyle("position", "absolute");
				sg.SetStyle("top", top);
				sg.SetStyle("left", left);
				sg.SetStyle("height", "10vh");
				sg.SetStyle("width", "10vw");
				sg.SetStyle("display", "flex");
				sg.SetStyle("justify-content", "center");
				sg.SetStyle("align-items", "center");
				sg.SetStyle("background-color", "#56884e");
				sg.SetStyle("z-index", nextno.ToString());
				sg.SetStyle("visibility", "visible");
				sg.SetStyle("border-color", "#000000");
				sg.SetStyle("border-style", "solid");
				sg.SetStyle("border-width", "1px");
				sg.SetStyle("border-radius", "0px");
				sg.SetStyle("color", "black");
				sg.SetStyle("font-family", "Arial");
				sg.SetStyle("font-size", "5vh");
				sg.SetStyle("font-style", "normal");
				sg.SetStyle("font-weight", "normal");
				sg.SetStyle("white-space", "nowrap");
				sg.SetStyle("background-position", "center");
				sg.SetStyle("background-repeat", "no-repeat");
				sg.SetStyle("background-size", "contain");
				sg.SetStyle("background-position", "center");
				sg.SetStyle("background-repeat", "no-repeat");
				sg.SetStyle("box-sizing", "border-box");

				s = sg.GetStyleStringJson();
			}

			if (Style == "")
			{
				o.style = s;
			}
			else
			{
				using (ClsStyleGenerator sg = new ClsStyleGenerator())
				{
					sg.ParseStyleStringJson(Style);
					sg.SetStyle("top", top);
					sg.SetStyle("left", left);
					sg.SetStyle("z-index", nextno.ToString());
					o.style = sg.GetStyleStringJson();
				}
			}

			ClsDisplayControler.AddDisplayObject(o);

			SetDivInfo(o.ID);
		}

		public static void DelDiv(string[] DivIDs)
		{
			for (int i = 0; i < DivIDs.Length; i++)
			{
				long id = Convert.ToInt64(DivIDs[i]);
				DisplayObject o = ClsDisplayControler.DisplayObject(id);
				ClsDisplayControler.DelDisplayObject(o.ID);
			}
		}

		internal static void SetStyleString(string[] div, string value1)
		{
			long id = Convert.ToInt64(div[0]);
			DisplayObject o = ClsDisplayControler.DisplayObject(id);

			o.style = value1;

			ClsDisplayControler.SaveDisplayObject(o);
		}

		public static void CopyDiv(string DivID)
		{
			long id = Convert.ToInt64(DivID);
			DisplayObject o = ClsDisplayControler.DisplayObject(id);

			NewDiv(
				o.PageSetNo,
				o.PageNo,
				o.textid,
				o.bgid,
				o.tableid,
				o.style,
				o.innerText,
				o.Speed
				);
		}

		public static void MoveDivs(int SetID, int PageID, string[] DivIDs, DirectionType Direction)
		{
			if (DivIDs.Length == 0)
			{
				return;
			}

			// list all objects and sort it by zindex
			List<DisplayObject> moveDivs = new List<DisplayObject>();
			for (int i = 0; i < DivIDs.Length; i++)
			{
				long id = Convert.ToInt64(DivIDs[i]);
				DisplayObject tempo = ClsDisplayControler.DisplayObject(id);
				moveDivs.Add(tempo);
			}

			if (Direction == DirectionType.up)
			{
				// bewegung muss mit oberstem gewählten Objekt starten
				moveDivs.Sort((a, b) => b.Zindex.CompareTo(a.Zindex));
			}
			else
			{
				// bewegung muss mit unterstem gewählten Objekt starten 
				moveDivs.Sort((a, b) => a.Zindex.CompareTo(b.Zindex));
			}

			// loop over all moveDivs
			for (int i = 0; i < moveDivs.Count(); i++)
			{
				MoveDivUpDown(SetID, PageID, moveDivs[i].ID, Direction);
			}
		}

		private static void MoveDivUpDown(int SetID, int PageID, long DivID, DirectionType direction)
		{
			// get already sorted by z-index object List
			List<DisplayObject> divList = DivList(SetID, PageID);
			if (divList == null)
			{
				return;
			}

			// get object to move
			DisplayObject currentDiv = (from x in divList where x.ID == DivID select x).FirstOrDefault();
			if (currentDiv == null)
			{
				return;
			}

			// get index of object to move
			int ind = divList.IndexOf(currentDiv);

			// up
			if (direction == DirectionType.up)
			{
				if (ind == 0)
				{
					// already at top
					return;
				}

				DisplayObject aboveDiv = divList[ind - 1];
				long zNeu = aboveDiv.Zindex;

				// z in dem darüber liegenden ándern
				using (ClsStyleGenerator sg = new ClsStyleGenerator())
				{
					sg.ParseStyleStringJson(aboveDiv.style);
					sg.SetStyle("z-index", currentDiv.Zindex.ToString());
					aboveDiv.style = sg.GetStyleStringJson();
				}

				aboveDiv.Zindex = currentDiv.Zindex;
				ClsDisplayControler.SaveDisplayObject(aboveDiv);

				// z in dem gewáhlten ándern
				using (ClsStyleGenerator sg = new ClsStyleGenerator())
				{
					sg.ParseStyleStringJson(currentDiv.style);
					sg.SetStyle("z-index", zNeu.ToString());
					currentDiv.style = sg.GetStyleStringJson();
				}

				currentDiv.Zindex = zNeu;
				ClsDisplayControler.SaveDisplayObject(currentDiv);
			}

			// down
			if (direction == DirectionType.down)
			{
				if (ind == divList.Count - 1)
				{
					// already at bottom
					return;
				}

				DisplayObject belowDiv = divList[ind + 1];
				long zNeu = belowDiv.Zindex;

				// z in dem darunter liegenden ándern
				using (ClsStyleGenerator sg = new ClsStyleGenerator())
				{
					sg.ParseStyleStringJson(belowDiv.style);
					sg.SetStyle("z-index", currentDiv.Zindex.ToString());
					belowDiv.style = sg.GetStyleStringJson();
				}

				belowDiv.Zindex = currentDiv.Zindex;
				ClsDisplayControler.SaveDisplayObject(belowDiv);

				// z in dem gewáhlten ándern
				using (ClsStyleGenerator sg = new ClsStyleGenerator())
				{
					sg.ParseStyleStringJson(currentDiv.style);
					sg.SetStyle("z-index", zNeu.ToString());
					currentDiv.style = sg.GetStyleStringJson();
				}

				currentDiv.Zindex = zNeu;
				ClsDisplayControler.SaveDisplayObject(currentDiv);
			}
		}

		/// <summary>
		/// Setzt einen StyleValue fúr ein oder mehrere Divs
		/// </summary>
		/// <param name="PageSetID"></param>
		/// <param name="PageID"></param>
		/// <param name="DivIDs">Komma seperierte Liste von DivIDs</param>
		/// <param name="StyleAttribute"></param>
		/// <param name="StyleValue"></param>
		public static void SetStyleValue(string[] DivIDs, string StyleAttribute, string StyleValue)
		{
			for (int i = 0; i < DivIDs.Length; i++)
			{
				if (DivIDs[i] == "")
				{
					continue;
				}

				long id = Convert.ToInt64(DivIDs[i]);

				DisplayObject o = ClsDisplayControler.DisplayObject(id);

				using (ClsStyleGenerator sg = new ClsStyleGenerator())
				{
					sg.ParseStyleStringJson(o.style);
					sg.SetStyle(StyleAttribute, StyleValue);
					// o.style = sg.GetStyleString();
					o.style = sg.GetStyleStringJson();
				}

				ClsDisplayControler.SaveDisplayObject(o);
			}

			//ActivateDiv(SetName, Page, DivIDs[0]);
		}

		/// <summary>
		/// Setzt den Inhalt fúr ein oder mehrere Divs
		/// </summary>
		/// <param name="SetName"></param>
		/// <param name="Page"></param>
		/// <param name="DivIDs"></param>
		/// <param name="Wert"></param>
		public static void SetInnerHTML(string[] DivIDs, string Wert)
		{
			for (int i = 0; i < DivIDs.Length; i++)
			{
				long id = Convert.ToInt64(DivIDs[i]);
				DisplayObject o = ClsDisplayControler.DisplayObject(id);

				o.innerText = Wert;

				ClsDisplayControler.SaveDisplayObject(o);

				SetDivInfo(o.ID);
			}
		}

		public static void Dispose()
		{
			//throw new NotImplementedException();
		}

		internal static void setDivAttribute(string SetName, string Page, string[] DivIDs, string Attribute, string Value)
		{
			for (int i = 0; i < DivIDs.Length; i++)
			{
				long id = Convert.ToInt64(DivIDs[i]);
				DisplayObject o = ClsDisplayControler.DisplayObject(id);

				if (Attribute == "textid")
				{
					o.textid = Value;
				}

				if (Attribute == "bgid")
				{
					o.bgid = Value;
				}

				if (Attribute == "tableid")
				{
					o.tableid = Value;
				}

				if (Attribute == "Speed")
				{
					o.Speed = Convert.ToInt64(Value);
				}

				if (Attribute == "innerText")
				{
					o.innerText = Value;
				}

				if (Attribute == "TableStyle")
				{
					o.TableStyle = Value;
				}

				ClsDisplayControler.SaveDisplayObject(o);

				SetDivInfo(o.ID);
			}
		}

		private static void SetDivInfo(long divid)
		{
			DisplayObject o = ClsDisplayControler.DisplayObject(divid);

			// Inhalts Info Text setzten
			o.Info = "leer";

			if (!string.IsNullOrEmpty(o.innerText))
			{
				o.Info = o.innerText;
				if (o.innerText.Length > 10)
					o.Info = o.innerText.Substring(0, 10) + "...";
			}

			if (o.bgid != "B00")
			{
				o.Info = ClsDBVariablen.Instance.GetBildVariable(o.bgid).Variable;
			}

			if (o.textid != "S00")
			{
				o.Info = ClsDBVariablen.Instance.GetTextVariable(o.textid).Variable;
			}

			if (o.tableid != "T00")
			{
				o.Info = ClsDBVariablen.Instance.GetTabellenVariable(o.tableid).Variable;
			}

			ClsDisplayControler.SaveDisplayObject(o);
		}
	}
}
