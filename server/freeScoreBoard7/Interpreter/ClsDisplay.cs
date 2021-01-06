using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using FreeScoreBoard.Core;
using FreeScoreBoard.Core.Display;
using FreeScoreBoard.Server;

namespace FreeScoreBoard.Interpreter
{
	public class ClsDisplay
	{
		public static void SetPage(int PageSetID, int PageID)
		{
			string PageSetName = ClsPageSets.GetPageSetNameFromID(PageSetID);
			string PageName = ClsPages.GetPageNameFromID(PageID);


			ClsGlobal.Instance.ActivePage = PageID;

			StringBuilder uri = new StringBuilder();
			uri.Append("http://");
			uri.Append(ClsServer.Instance.IP);
			uri.Append(":");
			uri.Append(ClsServer.Instance.HttpsvPort);
			uri.Append("/");
			uri.Append(ClsMain.WebDesignsFolder);
			uri.Append("/");
			uri.Append(PageSetName);
			uri.Append("/");
			uri.Append(PageName);
			uri.Append(".html");
			uri.Append("?wsp=");
			uri.Append(ClsServer.Instance.WsPort);

			string x = uri.ToString();

			ClsCommand cmd = new ClsCommand();
			cmd.Domain = "AN";
			cmd.Command = "gotopage";
			cmd.Value1 = uri.ToString();

			//GlobalServerEvents.SendMessage(null, new Classes.ClsStringEventArgs("AN|gotopage|" + uri.ToString()));
			GlobalServerEvents.SendMessage(null, new Classes.ClsStringEventArgs(new JavaScriptSerializer().Serialize(cmd)));
		}
	}
}
