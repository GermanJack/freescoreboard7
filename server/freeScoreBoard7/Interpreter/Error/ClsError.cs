using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using FreeScoreBoard.Core;
using FreeScoreBoard.Server;

namespace FreeScoreBoard.Interpreter.Error
{
	public class ClsError
	{
		//public static event EventHandler ErrorEvent;

		public static void CoreError(string modul, string function, Exception ex)
		{
			string msg = ClsFunktionen.MakeErrorMessage(modul, function, ex);

			ClsCommand cmd = new ClsCommand();
			cmd.Domain = "Error";
			cmd.Value1 = msg;

			GlobalServerEvents.SendMessage(null, new Classes.ClsStringEventArgs(new JavaScriptSerializer().Serialize(cmd)));
		}

		internal static void Error(string modul, string function, Exception ex, bool silent = false)
		{
			string msg = ClsFunktionen.MakeErrorMessage(modul, function, ex);

			string nurLog = "";
			if (silent)
			{
				nurLog = "silent";
			}

			ClsCommand cmd = new ClsCommand();
			cmd.Domain = "Error";
			cmd.Value1 = msg;
			cmd.Value2 = nurLog;

			GlobalServerEvents.SendMessage(null, new Classes.ClsStringEventArgs(new JavaScriptSerializer().Serialize(cmd)));
		}
	}
}
