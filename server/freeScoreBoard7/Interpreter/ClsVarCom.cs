using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using FreeScoreBoard.Core;
using FreeScoreBoard.Core.DB;
using FreeScoreBoard.Core.DBControler;
using FreeScoreBoard.Core.Variablen;

namespace FreeScoreBoard.Interpreter
{
	public static class ClsVarCom
	{
		public static string UpdateDivContentStr(string ID, string Wert)
		{
			ClsCommand cmd = new ClsCommand();
			cmd.Domain = "AN";
			cmd.Command = "textvar";
			cmd.Property = ID;
			cmd.Value1 = Wert;

			return new JavaScriptSerializer().Serialize(cmd);
		}

		public static string UpdateDivContentPic(string ID, string Wert)
		{
			ClsCommand cmd = new ClsCommand();
			cmd.Domain = "AN";
			cmd.Command = "picvar";
			cmd.Property = ID;
			cmd.Value1 = Wert;

			return new JavaScriptSerializer().Serialize(cmd);
		}

		public static string UpdateDivContentTab(string ID, string Wert)
		{
			ClsCommand cmd = new ClsCommand();
			cmd.Domain = "AN";
			cmd.Command = "tabvar";
			cmd.Property = ID;
			cmd.Value1 = Wert;

			return new JavaScriptSerializer().Serialize(cmd);
		}

		public static void ChangeTextVariable(string Variable, string Wert)
		{
			// Abbruch wenn richtiger Wert bereits in Variable
			if (ClsDBVariablen.Instance.GetTextVariableWert(Variable) == Wert)
			{
				return;
			}

			// Wert setzten
			ClsDBVariablen.Instance.SetTextVariableWert(Variable, Wert);

			// Prüfen ob Torschütze erfasst werden muß
			if (new[] { "S05", "S06" }.Contains(Variable))
			{
				Options3 o3 = ClsOptionsControler.Options3("Torschütze");
				if (Convert.ToBoolean(o3.Value))
				{

				}
			}

			// Prüfen ob Foulspieler erfasst werden muß
			if (new[] { "S18", "S19" }.Contains(Variable))
			{
				Options3 o3 = ClsOptionsControler.Options3("Foulspieler");
				if (Convert.ToBoolean(o3.Value))
				{

				}
			}

			// Prüfen ob Ereignis geschrieben werden muß
			bool log = ClsVariablenControler.LogChange(Variable);
			if (log)
			{

			}
		}

		public static string ChangeDivVisibilityString(string ID, string Wert)
		{
			ClsCommand cmd = new ClsCommand();
			cmd.Domain = "AN";
			cmd.Command = "DivVisibility";
			cmd.Property = ID;
			cmd.Value1 = Wert;

			return new JavaScriptSerializer().Serialize(cmd);
		}

		public static string PlayAudioString(string File)
		{
			ClsCommand cmd = new ClsCommand();
			cmd.Domain = "AN";
			cmd.Command = "toogleAudio";
			cmd.Value1 = File;

			return new JavaScriptSerializer().Serialize(cmd);
		}

		public static string AllAudioStopString()
		{
			ClsCommand cmd = new ClsCommand();
			cmd.Domain = "AN";
			cmd.Command = "allAudioStop";

			return new JavaScriptSerializer().Serialize(cmd);
		}
	}
}
