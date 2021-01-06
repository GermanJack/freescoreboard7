using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using FreeScoreBoard.Classes;
using FreeScoreBoard.Core;
using FreeScoreBoard.Core.DBControler;
using FreeScoreBoard.Core.Display;
using FreeScoreBoard.Core.Kontrolle;
using FreeScoreBoard.Core.Objekte;
using FreeScoreBoard.Core.Timer;
using FreeScoreBoard.Core.Turnier;
using FreeScoreBoard.Core.Variablen;
using FreeScoreBoard.Server;
using WebSocketSharp;

namespace FreeScoreBoard.Interpreter
{
	public class ClsCommands
	{
		//  Requests werden je Client session hier behandelt und die Daten dierekt nur an den anfragenden Client gesendet


		public static string Command(ClsCommand rb)
		{
			string ret = "";
			try
			{
				object[] o = new object[1];
				o[0] = rb;
				//Type.GetType("void").GetMethod(cmd).Invoke(null, o);
				Type thisType = typeof(ClsCommands); // Type.GetType("void");
				MethodInfo theMethod = thisType.GetMethod(rb.Command, new Type[] { typeof(ClsCommand) });
				if (theMethod != null)
				{
					theMethod.Invoke(null, new Object[] { rb });
				}

				return ret;
			}
			catch (Exception ex)
			{
				ret = "Fehler Command: " + rb.Command + " " + ex.InnerException;
				Console.Write(ret);
				return ret;
			}
		}


		#region PageSetfunktionen
		public static void NewPageSet(ClsCommand rb)
		{
			ClsPageSets.NewPageSet(rb.Value1);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "PageSets"))));
		}

		public static void CopyPageSet(ClsCommand rb)
		{
			int PageSetID = Convert.ToInt32(rb.PageSet);
			ClsPageSets.CopyPageSet(PageSetID, rb.Value1);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "PageSets"))));
		}

		public static void RenamePageSet(ClsCommand rb)
		{
			int PageSetID = Convert.ToInt32(rb.PageSet);
			ClsPageSets.RenamePageSet(PageSetID, rb.Value1);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "PageSets"))));
		}

		public static void DelPageSet(ClsCommand rb)
		{
			int PageSetID = Convert.ToInt32(rb.PageSet);
			ClsPageSets.DeletePageSet(PageSetID);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "PageSets"))));
		}
		#endregion

		#region Pagefunktionen
		public static void NewPage(ClsCommand rb)
		{
			int PageSetID = Convert.ToInt32(rb.PageSet);
			ClsPages.NewPage(PageSetID, rb.Value1);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "Pages", pageSet: rb.PageSet))));
		}

		public static void DelPage(ClsCommand rb)
		{
			int PageSetID = Convert.ToInt32(rb.PageSet);
			int PageID = Convert.ToInt32(rb.Page);
			ClsPages.DelPage(PageSetID, PageID);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "Pages", pageSet: rb.PageSet))));
		}

		public static void CopyPage(ClsCommand rb)
		{
			int PageSetID = Convert.ToInt32(rb.PageSet);
			int PageID = Convert.ToInt32(rb.Page);
			ClsPages.CopyPage(PageSetID, PageID, rb.Value1);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "Pages", pageSet: rb.PageSet))));
		}

		public static void RenamePage(ClsCommand rb)
		{
			int PageSetID = Convert.ToInt32(rb.PageSet);
			int PageID = Convert.ToInt32(rb.Page);
			ClsPages.RenamePage(PageSetID, PageID, rb.Value1);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "Pages", pageSet: rb.PageSet))));
		}

		public static void SetPageStyle(ClsCommand rb)
		{
			int PageSetID = Convert.ToInt32(rb.PageSet);
			int PageID = Convert.ToInt32(rb.Page);
			ClsPages.SetStyleWert(PageSetID, PageID, rb.Property, rb.Value1);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "Page", pageSet: rb.PageSet, page: rb.Page))));
		}

		public static void Pagefarbe(ClsCommand rb)
		{
			int PageSetID = Convert.ToInt32(rb.PageSet);
			int PageID = Convert.ToInt32(rb.Page);
			ClsPages.SetStyleWert(PageSetID, PageID, "background-color", rb.Value1);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "Page", pageSet: rb.PageSet, page: rb.Page))));
		}

		public static void MarkColor(ClsCommand rb)
		{
			int PageSetID = Convert.ToInt32(rb.PageSet);
			int PageID = Convert.ToInt32(rb.Page);
			ClsPages.SetMarkStyleWert(PageSetID, PageID, "border-color", rb.Value1);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "Page", pageSet: rb.PageSet, page: rb.Page))));
		}

		public static void SetPageGrid(ClsCommand rb)
		{
			int PageSetID = Convert.ToInt32(rb.PageSet);
			int PageID = Convert.ToInt32(rb.Page);
			ClsPages.SetMarkStyleWert(PageSetID, PageID, "Grid", rb.Value1);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "Page", pageSet: rb.PageSet, page: rb.Page))));
		}
		#endregion

		#region Objektfunktionen
		public static void NewDiv(ClsCommand rb)
		{
			int PageSetID = Convert.ToInt32(rb.PageSet);
			int PageID = Convert.ToInt32(rb.Page);
			ClsDivs.NewDiv(PageSetID, PageID);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "Divs", pageSet: rb.PageSet, page: rb.Page))));
		}

		public static void DelDivs(ClsCommand rb)
		{
			// ClsDivs.DeActivateDiv(rb.PageSet, rb.Page);
			ClsDivs.DelDiv(rb.Divs);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "Divs", pageSet: rb.PageSet, page: rb.Page))));
		}

		public static void CopyDivs(ClsCommand rb)
		{
			ClsDivs.CopyDiv(rb.Divs[0]);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "Divs", pageSet: rb.PageSet, page: rb.Page))));
		}

		public static void UpDivs(ClsCommand rb)
		{
			int PageID = Convert.ToInt32(rb.Page);
			int PageSetID = Convert.ToInt32(rb.PageSet);
			ClsDivs.MoveDivs(PageSetID, PageID, rb.Divs, Direction.DirectionType.up);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "Divs", pageSet: rb.PageSet, page: rb.Page))));
		}

		public static void DownDivs(ClsCommand rb)
		{
			int PageID = Convert.ToInt32(rb.Page);
			int PageSetID = Convert.ToInt32(rb.PageSet);
			ClsDivs.MoveDivs(PageSetID, PageID, rb.Divs, Direction.DirectionType.down);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "Divs", pageSet: rb.PageSet, page: rb.Page))));
		}

		public static void setDivStyle(ClsCommand rb)
		{
			ClsDivs.SetStyleValue(rb.Divs, rb.Property, rb.Value1);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "Divs", pageSet: rb.PageSet, page: rb.Page))));
		}

		public static void setDivStyleString(ClsCommand rb)
		{
			ClsDivs.SetStyleString(rb.Divs, rb.Value1);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "Divs", pageSet: rb.PageSet, page: rb.Page))));
		}

		public static void setDivText(ClsCommand rb)
		{
			ClsDivs.SetInnerHTML(rb.Divs, rb.Value1);
		}

		public static void setDivAttribute(ClsCommand rb)
		{
			ClsDivs.setDivAttribute(rb.PageSet, rb.Page, rb.Divs, rb.Property, rb.Value1);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "Divs", pageSet: rb.PageSet, page: rb.Page))));
		}

		public static void RefreshDivPos(ClsCommand rb)
		{
			//this.Sessions.BroadcastAsync("DD|SB|refreshDivPos|" + rb.PageSet + "|" + rb.Page + "|" + rb.Value1 + "|" + rb.Value2, null);
			//string sendstr = "DD|SB|refreshDivPos|" + rb.PageSet + "|" + rb.Page + "|" + rb.Value1 + "|" + rb.Value2;
			//GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(sendstr));

			ClsCommand cmd = new ClsCommand();
			cmd.Domain = "DD";
			cmd.Type = "SB";
			cmd.Command = "refreshDivPos";
			cmd.PageSet = rb.PageSet;
			cmd.Page = rb.Page;
			cmd.Value1 = rb.Value1;
			cmd.Value2 = rb.Value2;

			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(new JavaScriptSerializer().Serialize(cmd)));
		}

		public static void RefreshDivSiz(ClsCommand rb)
		{
			//this.Sessions.BroadcastAsync("DD|SB|refreshDivSiz|" + rb.PageSet + "|" + rb.Page + "|" + rb.Value1 + "|" + rb.Value2, null);
			//string sendstr = "DD|SB|refreshDivSiz|" + rb.PageSet + "|" + rb.Page + "|" + rb.Value1 + "|" + rb.Value2;
			//GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(sendstr));

			ClsCommand cmd = new ClsCommand();
			cmd.Domain = "DD";
			cmd.Type = "SB";
			cmd.Command = "refreshDivSiz";
			cmd.PageSet = rb.PageSet;
			cmd.Page = rb.Page;
			cmd.Value1 = rb.Value1;
			cmd.Value2 = rb.Value2;

			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(new JavaScriptSerializer().Serialize(cmd)));
		}
		#endregion

		#region Teams
		public static void TeamNew(ClsCommand rb)
		{
			Core.Mannschaften.ClsCommon.NewMannschaft(rb.Team);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "TeamList"))));
		}

		public static void TeamDel(ClsCommand rb)
		{
			Core.Mannschaften.ClsCommon.DelMannschaft(rb.Team);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "TeamList"))));
		}

		public static void TeamChange(ClsCommand rb)
		{
			Core.Mannschaften.ClsCommon.ChangeMannschaft(rb.Team, rb.Property, rb.Value1);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "TeamList"))));
		}

		public static void PlayerNew(ClsCommand rb)
		{
			Core.Mannschaften.ClsCommon.NewSpieler(rb.Team, rb.Player);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "PlayerList", team: rb.Team))));
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "PlayerList", team: "ALL"))));
		}

		public static void PlayerDel(ClsCommand rb)
		{
			Core.Mannschaften.ClsCommon.DelSpieler(rb.Player);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "PlayerList", team: rb.Team))));
		}

		public static void PlayerChange(ClsCommand rb)
		{
			Core.Mannschaften.ClsCommon.ChangeSpieler(rb.Player, rb.Property, rb.Value1);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "PlayerList", team: rb.Team))));
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "PlayerList", team: "ALL"))));
		}

		#endregion

		#region control

		public static void SwitchPage(ClsCommand rb)
		{
			int pageID = Convert.ToInt32(rb.Page);
			ClsDisplay.SetPage(ClsPageSets.ActivePageSet(), pageID);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "ActivePage"))));

			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "DivsActivePage", pageSet: rb.PageSet, page: rb.Page))));
		}

		public static void ToogleDisplay(ClsCommand rb)
		{
			ClsGlobalEvents.Fire_ToogleDisplay(null, null);
			//ClsDisplay.SetPage(ClsPageSets.ActivePageSet(), rb.Page);
		}

		public static void SetB05(ClsCommand rb)
		{
			string picfile = Core.DBControler.ClsOptionsControler.Options3("SlideshowBild" + rb.Value1).Value;
			ClsDBVariablen.Instance.SetBildVariableWert("B05", picfile);
		}

		public static void SetSpielrichtung(ClsCommand rb)
		{
			ClsDBVariablen.Instance.SetSpielrichtung(rb.Value1);
		}

		public static void PlayAudio(ClsCommand rb)
		{
			ClsGlobalEvents.Fire_PlayAudio("ClsCommands.PlayAudio", new ClsStringEventArgs(rb.Value1));
		}

		public static void SetMannschaft(ClsCommand rb)
		{
			// rb.Value1 = A or B
			// rb.Value2 = teamID
			// rb.Value3 = maual enterd team name

			string Mannschaft = rb.Value1;
			string mIDstr = rb.Value2;
			string nameneu = rb.Value3;
			Core.DB.Mannschaften m = null;
			if (mIDstr != "0")
			{

				long mID = Convert.ToInt64(mIDstr);
				m = Core.DBControler.ClsMannschaftenControler.Mannschaft(mID);
				nameneu = m.Name;
			}

			if (Mannschaft == "A")
			{
				ClsGlobal.Instance.MansschaftAID = mIDstr;
				ClsDBVariablen.Instance.SetTextVariableWert("S01", nameneu);
				if (m != null)
				{
					ClsDBVariablen.Instance.SetBildVariableWert("B01", m.Bild1);
					ClsDBVariablen.Instance.SetBildVariableWert("B03", m.Bild2);
					ClsDBVariablen.Instance.SetTextVariableWert("S03", m.Kurzname);
				}
				else
				{
					ClsDBVariablen.Instance.SetBildVariableWert("B01", "");
					ClsDBVariablen.Instance.SetBildVariableWert("B03", "");
					ClsDBVariablen.Instance.SetTextVariableWert("S03", "");
				}

				//GlobalServerEvents.SendMessage(null, new ClsStringEventArgs("|JN|TeamID|A|" + mIDstr));
				GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "TeamID", team: "A"))));

				GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "PlayerList", team: mIDstr, value1: Mannschaft))));
			}
			else
			{
				ClsGlobal.Instance.MansschaftBID = mIDstr;
				ClsDBVariablen.Instance.SetTextVariableWert("S02", nameneu);
				if (m != null)
				{
					ClsDBVariablen.Instance.SetBildVariableWert("B02", m.Bild1);
					ClsDBVariablen.Instance.SetBildVariableWert("B04", m.Bild2);
					ClsDBVariablen.Instance.SetTextVariableWert("S04", m.Kurzname);
				}
				else
				{
					ClsDBVariablen.Instance.SetBildVariableWert("B02", "");
					ClsDBVariablen.Instance.SetBildVariableWert("B04", "");
					ClsDBVariablen.Instance.SetTextVariableWert("S04", "");
				}

				//GlobalServerEvents.SendMessage(null, new ClsStringEventArgs("|JN|TeamID|B|" + mIDstr));
				GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "TeamID", team: "B"))));

				GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "PlayerList", team: mIDstr, value1: Mannschaft))));
			}
		}

		public static void SetPlayer(ClsCommand rb)
		{
			string playerID = rb.Value1;
			ClsGlobal.Instance.SpielerID = playerID;

			Core.DB.Spieler p = null;

			if (playerID != "0")
			{
				int pID = Convert.ToInt32(playerID);
				p = Core.DBControler.ClsMannschaftenControler.Spieler(pID);

				ClsDBVariablen.Instance.SetTextVariableWert("S36", p.Vorname + ", " + p.Nachname);
				ClsDBVariablen.Instance.SetTextVariableWert("S37", p.NickName);
				ClsDBVariablen.Instance.SetBildVariableWert("B14", p.Bild);
			}
			else
			{
				ClsDBVariablen.Instance.SetTextVariableWert("S36", "");
				ClsDBVariablen.Instance.SetTextVariableWert("S37", "");
				ClsDBVariablen.Instance.SetBildVariableWert("B14", "");
			}
		}

		public static void Tor(ClsCommand rb)
		{
			string Mannschaft = "A";
			if (rb.Value1 == "S06")
			{
				Mannschaft = "B";
			}

			int Add = 1;
			if (rb.Value2 == "-")
			{
				Add = -1;
			}

			int spielerID = 0;
			try
			{
				spielerID = Convert.ToInt32(rb.Value3);
			}
			catch
			{
				spielerID = 0;
			}

			ClsTore.Tore_change(spiel: ClsGlobal.Instance.AktivesSpiel, aoderb: Mannschaft, add: Add, spielerID);
		}

		public static void Tore_Reset(ClsCommand rb)
		{
			ClsTore.Tore_Reset(spiel: ClsGlobal.Instance.AktivesSpiel);
		}

		public static void Foul(ClsCommand rb)
		{
			string Mannschaft = "A";
			if (rb.Value1 == "S19")
			{
				Mannschaft = "B";
			}

			int Add = 1;
			if (rb.Value2 == "-")
			{
				Add = -1;
			}

			int spielerID = 0;
			try
			{
				spielerID = Convert.ToInt32(rb.Value3);
			}
			catch
			{
				spielerID = 0;
			}

			ClsFouls.Foul_change(ClsGlobal.Instance.AktivesSpiel, Mannschaft, Add, spielerID);
		}

		public static void Foul_Reset(ClsCommand rb)
		{
			ClsFouls.Foul_Reset(spiel: ClsGlobal.Instance.AktivesSpiel);
		}

		public static void VarCount(ClsCommand rb)
		{
			bool Add = true;
			if (rb.Value2 == "-")
			{
				Add = false;
			}

			string wert = ClsDBVariablen.Instance.GetTextVariableWert(rb.Value1);
			int numwert = Convert.ToInt32(wert);

			// cancel conditions
			// Schüielabschnitt nicht kleiner als 1
			if (rb.Value1 == "S09" & numwert <= 1 && !Add)
			{
				return;
			}

			// Paintballzähler M1 nicht kleiner als 0
			if (rb.Value1 == "S46" & numwert <= 0 && !Add)
			{
				return;
			}

			// Paintballzähler M1 nicht grösser als 4
			if (rb.Value1 == "S46" & numwert >= 5 && Add)
			{
				return;
			}

			// Paintballzähler M2 nicht kleiner als 0
			if (rb.Value1 == "S47" & numwert <= 0 && !Add)
			{
				return;
			}

			// Paintballzähler M2 nicht grösser als 4
			if (rb.Value1 == "S47" & numwert >= 5 && Add)
			{
				return;
			}

			if (Add)
			{
				numwert += 1;
			}

			if (!Add && numwert > 0)
			{
				numwert -= 1;
			}

			ClsDBVariablen.Instance.SetTextVariableWert(rb.Value1, numwert.ToString());

			if (rb.Value1 == "S46" || rb.Value1 == "S47")
			{
				ClsDBVariablen.Instance.SetPaintballPictures();
			}
		}

		public static void SetVariableDefault(ClsCommand rb)
		{
			string v = rb.Value1;
			string w = rb.Value2;
			ClsDBVariablen.Instance.SetVariablenDefault(v, w);

			// Variable neu an die Clients senden
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "SVariablen"))));
		}

		public static void VarReset(ClsCommand rb)
		{
			// Spielabschnitt
			if (rb.Value1 == "S09")
			{
				ClsDBVariablen.Instance.SetTextVariableWert(rb.Value1, "1");
			}

			// Paintballzähler M1
			if (rb.Value1 == "S46")
			{
				ClsDBVariablen.Instance.SetTextVariableWert(rb.Value1, "5");
				ClsDBVariablen.Instance.SetPaintballPictures();
			}

			// Paintballzähler M2
			if (rb.Value1 == "S47")
			{
				ClsDBVariablen.Instance.SetTextVariableWert(rb.Value1, "5");
				ClsDBVariablen.Instance.SetPaintballPictures();
			}

			// Saetze M1
			if (rb.Value1 == "S51")
			{
				ClsDBVariablen.Instance.SetTextVariableWert(rb.Value1, "0");
			}

			// Saetze M2
			if (rb.Value1 == "S52")
			{
				ClsDBVariablen.Instance.SetTextVariableWert(rb.Value1, "0");
			}
		}

		public static void SetPossession(ClsCommand rb)
		{
			string str = ClsOptionsControler.Options3("PossessionBild").Value;

			if (rb.Value1 == "1")
			{
				ClsDBVariablen.Instance.SetBildVariableWert("B15", str);
				ClsDBVariablen.Instance.SetBildVariableWert("B16", "");
			}

			if (rb.Value1 == "2")
			{
				ClsDBVariablen.Instance.SetBildVariableWert("B15", "");
				ClsDBVariablen.Instance.SetBildVariableWert("B16", str);
			}
		}

		public static void ResetAFootball(ClsCommand rb)
		{
			ClsDBVariablen.Instance.SetTextVariableWert("S39", ClsDBVariablen.Instance.GetTextVariable("S39").Default);

			ClsDBVariablen.Instance.SetTextVariableWert("S40", ClsDBVariablen.Instance.GetTextVariable("S40").Default);

			ClsDBVariablen.Instance.SetTextVariableWert("S42", ClsDBVariablen.Instance.GetTextVariable("S42").Default);

			ClsDBVariablen.Instance.SetTextVariableWert("S43", ClsDBVariablen.Instance.GetTextVariable("S43").Default);

			ClsDBVariablen.Instance.SetTextVariableWert("S41", ClsDBVariablen.Instance.GetTextVariable("S41").Default);
		}

		public static void TimerOn(ClsCommand rb)
		{
			long tNr = Convert.ToInt32(rb.Value1);
			ClsZeitkontroler.Instance.ClockStart(tNr);
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "TimerObjects"))));
		}

		public static void TimerOff(ClsCommand rb)
		{
			long tNr = Convert.ToInt32(rb.Value1);
			ClsZeitkontroler.Instance.ClockStop(tNr);
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "TimerObjects"))));
		}

		public static void TimerReset(ClsCommand rb)
		{
			long tNr = Convert.ToInt32(rb.Value1);
			ClsZeitkontroler.Instance.ResetUhr(tNr);
		}

		public static void TimerManipulate(ClsCommand rb)
		{
			long tNr = Convert.ToInt32(rb.Value1);
			int sec = Convert.ToInt32(rb.Value2);

			Core.Timer.ClsZeitkontroler.Instance.ManipulateClock(tNr, sec);
		}

		public static void SetTicker(ClsCommand rb)
		{
			ClsDBVariablen.Instance.SetTextVariableWert("S34", rb.Value1);
		}

		public static void SetTablefilter(ClsCommand rb)
		{
			// Filter setzen
			(from x in ClsGlobal.Instance.TableFilterList where x.Table == rb.Value1 && x.Field == rb.Value2 select x).FirstOrDefault().Value = rb.Value3;

			// Filter zurück an clients senden
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "TableFilter"))));

			// Tabelleninhalt setzetn
			switch (rb.Value1)
			{
				case "T01":
					ClsDBVariablen.Instance.SetTabellenVariableWert("T01", ClsTabellenfunktionen.TabelleToVariable_Json(ClsGlobal.Instance.TurnierID));
					break;
				case "T02":
					ClsDBVariablen.Instance.SetTabellenVariableWert("T02", ClsTabellenfunktionen.SpielplanToVariable_Json(ClsGlobal.Instance.TurnierID));
					break;
				case "T03":
					ClsDBVariablen.Instance.SetTabellenVariableWert("T03", ClsTabellenfunktionen.EreignisseToVariable_Json(ClsGlobal.Instance.TurnierID));
					break;
			}
		}

		public static void newPenalty(ClsCommand rb)
		{
			ClsStrafen.StrafeNeu(rb.Team, rb.Player, rb.Value1);
		}

		public static void Strafen_Reset(ClsCommand rb)
		{
			ClsStrafen.DeleteStrafen();
		}

		public static void delPenalty(ClsCommand rb)
		{
			ClsStrafen.Delete(rb.Team, rb.Value1);
		}

		public static void ManipulatePenalty(ClsCommand rb)
		{
			ClsStrafen.Manipulate(rb.Team, rb.Value1, rb.Value2);
		}
		#endregion

		#region Options

		public static void SetOptValue(ClsCommand rb)
		{
			Core.DB.Options3 o3 = ClsOptionsControler.Options3(rb.Value1);
			o3.Value = rb.Value2;
			ClsOptionsControler.SaveOptions3(o3);
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "Options"))));

			if (rb.Value1.Length > 12)
			{
				if (rb.Value1.Substring(0, 13) == "DisplayScreen")
				{
					ClsGlobalEvents.Fire_DisplayPropChanged(null, null);
				}
			}

			if (rb.Value1 == "StartPageSet")
			{
				// aktuelle Anzeige umschalten
				ClsGlobal.Instance.ActivePage = ClsPages.GetFirstPageName(ClsPageSets.ActivePageSet());

				ClsCommand cmd = new ClsCommand();
				cmd.Type = "JN";
				cmd.Command = "Reload";

				ClsServer.Instance.SendMessage(new JavaScriptSerializer().Serialize(cmd));

				// Liste der Anzeigeseiten aktualisieren
				GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "PageList"))));
			}
		}

		public static void SetTimer(ClsCommand rb)
		{
			Core.DB.Timer t = new JavaScriptSerializer().Deserialize<Core.DB.Timer>(rb.Value1);

			Core.DBControler.ClsTimerControler.SaveTimer(t);
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "Timers"))));
		}

		public static void SetTimerEvent(ClsCommand rb)
		{
			Core.DB.Timerevent t = new JavaScriptSerializer().Deserialize<Core.DB.Timerevent>(rb.Value1);
			Core.DBControler.ClsTimerControler.SaveTimerEvent(t);

			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "Timerevents", value1: t.TimerNr.ToString()))));
		}

		public static void AddTimerEvent(ClsCommand rb)
		{
			int tnr = Convert.ToInt32(rb.Value1);
			Core.DB.Timerevent m = new Core.DB.Timerevent();
			m.TimerNr = tnr;
			m.Active = true;
			m.Sekunden = 0;
			m.Eventtype = 0;
			//m.Name = "";
			m.Soundfile = "";
			m.Layer = "";
			m.AndereTimerNr = -1;

			Core.DBControler.ClsTimerControler.AddTimerEvent(m);
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "Timerevents", value1: rb.Value1))));
		}

		public static void DelTimerEvent(ClsCommand rb)
		{
			int tnr = Convert.ToInt32(rb.Value1);
			Core.DBControler.ClsTimerControler.DelTimerEvent(tnr);
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "Timerevents", value1: rb.Value2))));
		}

		public static void delPic(ClsCommand rb)
		{
			string file = Path.Combine(ClsMain.AppFolder, "web", "pictures", rb.Value1, rb.Value2);
			if (File.Exists(file))
			{
				File.Delete(file);
				GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "PictureList"))));
			}
		}

		public static void delSnd(ClsCommand rb)
		{
			string file = Path.Combine(ClsMain.AppFolder, "web", "sounds", rb.Value1);
			if (File.Exists(file))
			{
				File.Delete(file);
				GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "AudioFileList"))));
			}
		}

		public static void delFont(ClsCommand rb)
		{
			string file = Path.Combine(ClsMain.AppFolder, "web", "css", "fonts.css");

			// delete files
			string searchfolder = Path.Combine(ClsMain.AppFolder, "web", "fonts");
			string[] files = Directory.GetFiles(searchfolder, rb.Value1 + ".*");
			for (int i = 0; i < files.Count(); i++)
			{
				if (File.Exists(files[i]))
				{
					File.Delete(files[i]);

				}
			}

			// delet font-face entries
			// ??



			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "FontList"))));
		}

		public static void SetEvent(ClsCommand rb)
		{
			int id = Convert.ToInt32(rb.Value1);
			Core.DB.EreignissTyp erg = Core.DBControler.ClsEreignisControler.EreignissTyp(id);
			erg.Log = Convert.ToBoolean(rb.Value2);
			Core.DBControler.ClsEreignisControler.SaveEreignissTyp(erg);

			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "Events"))));
		}

		public static void AddEvent(ClsCommand rb)
		{

			Core.DB.TEreignisse e = new JavaScriptSerializer().Deserialize<Core.DB.TEreignisse>(rb.Value1);
			e.CPUZeit = DateTime.Now;
			e.Spielzeit = ClsFunktionen.Sek2uhr(Convert.ToInt32(e.Spielzeit));
			ClsEreignisControler.AddEreignis(e);

			// update der Ereignistabelle
			ClsDBVariablen.Instance.SetTabellenVariableWert("T03", ClsTabellenfunktionen.EreignisseToVariable_Json(ClsGlobal.Instance.TurnierID));

			// update der Torschützentabelle
			ClsTabellenfunktionen.TorschuetzeToVariable(ClsGlobal.Instance.TurnierID);
		}

		public static void SetPenaltie(ClsCommand rb)
		{
			Core.DB.Strafen s = new JavaScriptSerializer().Deserialize<Core.DB.Strafen>(rb.Value1);
			Core.DBControler.ClsOptionsControler.SaveStrafe(s);
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "Penalties", value1: rb.Value1))));
		}

		public static void AddPenaltie(ClsCommand rb)
		{
			Core.DB.Strafen n = new Core.DB.Strafen();
			n.Bezeichnung = rb.Value1;
			Core.DBControler.ClsOptionsControler.AddStrafe(n);
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "Penalties", value1: rb.Value1))));
		}

		public static void DelPenaltie(ClsCommand rb)
		{
			Core.DBControler.ClsOptionsControler.DelStrafe(rb.Value1);
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "Penalties", value1: rb.Value1))));
		}

		public static void ChangeTabSortOrder(ClsCommand rb)
		{
			int id = Convert.ToInt32(rb.Value1);
			Core.DB.TabellenSort t = Core.DBControler.ClsOptionsControler.Tabellensortierung(id);
			t.absteigend = !t.absteigend;
			Core.DBControler.ClsOptionsControler.SaveTabellensortierung(t);

			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "TabellenSort"))));
		}

		public static void LowerTabSortPrio(ClsCommand rb)
		{
			int id = Convert.ToInt32(rb.Value1);
			List<Core.DB.TabellenSort> tl = Core.DBControler.ClsOptionsControler.Tabellensortierung();
			Core.DB.TabellenSort t1 = (from x in tl where x.ID == id select x).FirstOrDefault();
			int mem = t1.Prio;
			Core.DB.TabellenSort t2 = (from x in tl where x.Prio == mem + 1 select x).FirstOrDefault();
			if (t2 != null)
			{
				t1.Prio += 1;
				t2.Prio -= 1;
				Core.DBControler.ClsOptionsControler.SaveTabellensortierung(t1);
				Core.DBControler.ClsOptionsControler.SaveTabellensortierung(t2);
			}

			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "TabellenSort"))));
		}

		#endregion

		#region Debug
		public static void SetHeartBeatStatus(ClsCommand rb)
		{
			ClsZeitkontroler.Instance.HeartBeatStatus = Convert.ToBoolean(rb.Value1);
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "HeartBeatStatus"))));
		}
		#endregion

		#region WebKontrols
		public static void SaveWebKontrol(ClsCommand rb)
		{
			Core.DB.WebKontrols wk = new JavaScriptSerializer().Deserialize<Core.DB.WebKontrols>(rb.Value1);
			Core.DBControler.ClsKontrolControler.SaveWebKontrol(wk);
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "WebKontrols"))));
		}

		public static void delFreeEreig(ClsCommand rb)
		{
			Core.DBControler.ClsEreignisControler.DelFreeEvents();

			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "TVariablen"))));
		}

		public static void SaveAnzeigetabelle(ClsCommand rb)
		{
			Core.DB.Anzeigetabelle at = new JavaScriptSerializer().Deserialize<Core.DB.Anzeigetabelle>(rb.Value1);
			Core.DBControler.ClsOptionsControler.SaveAnzeigetabelle(at);

			if (at.Tabelle == "T01")
			{
				ClsDBVariablen.Instance.SetTabellenVariableWert(at.Tabelle, ClsTabellenfunktionen.TabelleToVariable_Json(ClsGlobal.Instance.TurnierID));
			}

			if (at.Tabelle == "T02")
			{
				ClsDBVariablen.Instance.SetTabellenVariableWert(at.Tabelle, ClsTabellenfunktionen.SpielplanToVariable_Json(ClsGlobal.Instance.TurnierID));
			}

			if (at.Tabelle == "T03")
			{
				ClsDBVariablen.Instance.SetTabellenVariableWert(at.Tabelle, ClsTabellenfunktionen.EreignisseToVariable_Json(ClsGlobal.Instance.TurnierID));
			}

			if (at.Tabelle == "Strafen")
			{
				ClsTabellenfunktionen.StrafenToVariable(ClsGlobal.Instance.Strafen1, "A");
				ClsTabellenfunktionen.StrafenToVariable(ClsGlobal.Instance.Strafen2, "B");
			}

			if (at.Tabelle == "T06")
			{
				ClsTabellenfunktionen.TorschuetzeToVariable(ClsGlobal.Instance.TurnierID);
			}

			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "Anzeigetabellen"))));
		}

		public static void RecPerPageChange(ClsCommand rb)
		{
			ClsDBVariablen.Instance.SetTabellenVariableRecPerPage(rb.Value1, Convert.ToInt32(rb.Value2));
			if (rb.Value1 == "T01")
			{
				ClsDBVariablen.Instance.SetTabellenVariableWert(rb.Value1, ClsTabellenfunktionen.TabelleToVariable_Json(ClsGlobal.Instance.TurnierID));
			}

			if (rb.Value1 == "T02")
			{
				ClsDBVariablen.Instance.SetTabellenVariableWert(rb.Value1, ClsTabellenfunktionen.SpielplanToVariable_Json(ClsGlobal.Instance.TurnierID));
			}

			if (rb.Value1 == "T03")
			{
				ClsDBVariablen.Instance.SetTabellenVariableWert(rb.Value1, ClsTabellenfunktionen.EreignisseToVariable_Json(ClsGlobal.Instance.TurnierID));
			}

			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "TVariablen"))));
		}

		public static void TablePageSelection(ClsCommand rb)
		{
			ClsTableFilter tf = (from x
								 in ClsGlobal.Instance.TableFilterList
								 where x.Table == rb.Value1 && x.Field == "SelPage"
								 select x).FirstOrDefault();
			tf.IntValue = Convert.ToInt32(rb.Value2);

			if (rb.Value1 == "T01")
			{
				ClsDBVariablen.Instance.SetTabellenVariableWert(rb.Value1, ClsTabellenfunktionen.TabelleToVariable_Json(ClsGlobal.Instance.TurnierID));
			}

			if (rb.Value1 == "T02")
			{
				ClsDBVariablen.Instance.SetTabellenVariableWert(rb.Value1, ClsTabellenfunktionen.SpielplanToVariable_Json(ClsGlobal.Instance.TurnierID));
			}

			if (rb.Value1 == "T03")
			{
				ClsDBVariablen.Instance.SetTabellenVariableWert(rb.Value1, ClsTabellenfunktionen.EreignisseToVariable_Json(ClsGlobal.Instance.TurnierID));
			}
		}
		#endregion

		#region Turnier
		public static void NewSystem(ClsCommand rb)
		{
			ClsTurnier turnier = new JavaScriptSerializer().Deserialize<ClsTurnier>(rb.Value1);

			ClsTurnierverwaltung.TurnierSpeichern(turnier);

			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "TurniereKomplett"))));
		}

		public static void DelSystem(ClsCommand rb)
		{
			int turnierNr = Convert.ToInt32(rb.Value1);

			ClsTurnierverwaltung.TurnierLoeschen(turnierNr);

			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "TurniereKomplett"))));
		}

		public static void NewTurnier(ClsCommand rb)
		{
			ClsTurnier turnier = new JavaScriptSerializer().Deserialize<ClsTurnier>(rb.Value1);

			ClsTurnierverwaltung.TurnierSpeichern(turnier);

			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "TurniereKomplett"))));
		}

		public static void DelTurnier(ClsCommand rb)
		{
			int turnierNr = Convert.ToInt32(rb.Value1);

			ClsTurnierverwaltung.TurnierLoeschen(turnierNr);

			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "TurniereKomplett"))));
		}

		public static void SetTurnierID(ClsCommand rb)
		{
			int turnierID = Convert.ToInt32(rb.Value1);
			// ClsTurnierfunktionen.LoadTurnier(turnierID);
			ClsGlobal.Instance.TurnierID = turnierID;
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "turnierID"))));
		}

		public static void SetMatch(ClsCommand rb)
		{
			int spielID = Convert.ToInt32(rb.Property);
			int ToreA = Convert.ToInt32(rb.Value1);
			int ToreB = Convert.ToInt32(rb.Value2);
			string mod = rb.Value3;

			ClsSpielfunktionen.SetMatch(spielID, ToreA, ToreB, mod);
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(ClsRequests.DataRequest(new ClsCommand(command: "turnierID"))));
		}

		public static void SetNextMatch(ClsCommand rb)
		{
			int spielNr = Convert.ToInt32(rb.Value1);
			ClsSpielfunktionen.SetNext(ClsSpielfunktionen.MatchByNr(spielNr));
		}

		public static void NextToActual(ClsCommand rb)
		{
			string mod = rb.Value1;

			bool beenden = true;
			if (mod == "2")
			{
				// Spiel offen lassen
				beenden = false;
			}

			bool beideNichtDa = false;
			if (mod == "3")
			{
				beenden = true;
			}

			ClsSpielfunktionen.NextGame(beenden, beideNichtDa);
		}

		public static void DelEvent(ClsCommand rb)
		{
			int id = Convert.ToInt32(rb.Value1);
			ClsEreignisControler.DelEreignis(id);

			// update der Ereignistabelle
			ClsDBVariablen.Instance.SetTabellenVariableWert("T03", ClsTabellenfunktionen.EreignisseToVariable_Json(ClsGlobal.Instance.TurnierID));

			// update der Torschützentabelle
			ClsTabellenfunktionen.TorschuetzeToVariable(ClsGlobal.Instance.TurnierID);
		}

		public static void ReOpenMatch(ClsCommand rb)
		{
			Core.DB.TSpiele s = new JavaScriptSerializer().Deserialize<Core.DB.TSpiele>(rb.Value1);
			Core.DBControler.ClsTurnierControler.SaveSpielStatus(s, 2);

			// update der Ereignistabelle
			ClsDBVariablen.Instance.SetTabellenVariableWert("T02", ClsTabellenfunktionen.SpielplanToVariable_Json(ClsGlobal.Instance.TurnierID));
		}

		#endregion
	}
}
