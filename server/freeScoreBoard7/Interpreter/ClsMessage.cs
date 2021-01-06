using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using FreeScoreBoard.Forms;
using FreeScoreBoard.Server;

namespace FreeScoreBoard.Interpreter
{
	public class ClsMessage
	{
		public static void SendMessage(string Message, string Titel)
		{
			ClsCommand cmd = new ClsCommand();
			cmd.Domain = "Message";
			cmd.Value1 = Message;
			cmd.Value2 = Titel;

			GlobalServerEvents.SendMessage(null, new Classes.ClsStringEventArgs(new JavaScriptSerializer().Serialize(cmd)));
		}


	}
}
