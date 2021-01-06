using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeScoreBoard.Interpreter
{
	[Serializable]
	public class ClsCommand
	{
		public ClsCommand() { }

		public ClsCommand(
			string domain = "",
			string type = "",
			string pageSet = "",
			string page = "",
			string command = "",
			string property = "",
			string value1 = "",
			string value2 = "",
			string value3 = "",
			string team = "",
			string player = "")
		{
			this.Domain = domain;
			this.Type = type;
			this.PageSet = pageSet;
			this.Page = page;
			this.Command = command;
			this.Property = property;
			this.Value1 = value1;
			this.Value2 = value2;
			this.Value3 = value3;
			this.Team = team;
			this.Player = player;
		}

		public ClsCommand(
			string domain,
			string type,
			string pageSet,
			string page,
			string[] divs,
			string command,
			string property,
			string value1 = "",
			string value2 = "",
			string value3 = "",
			string team = "",
			string player = "")
		{
			this.Domain = domain;
			this.Type = type;
			this.PageSet = pageSet;
			this.Page = page;
			this.Divs = divs;
			this.Command = command;
			this.Property = property;
			this.Value1 = value1;
			this.Value2 = value2;
			this.Value3 = value3;
			this.Team = team;
			this.Player = player;
		}


		public string Domain { get; set; }
		public string Type { get; set; }
		public string PageSet { get; set; }
		public string Page { get; set; }
		public string[] Divs { get; set; }
		public string Command { get; set; }
		public string Property { get; set; }
		public string Value1 { get; set; }
		public string Value2 { get; set; }
		public string Value3 { get; set; }
		public string Team { get; set; }
		public string Player { get; set; }
	}
}
