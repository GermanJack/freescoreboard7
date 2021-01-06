using System.ComponentModel;
using System.Data;

namespace FreeScoreBoard.Core.Variablen
{
	public class ClsTabellenVariabeln
	{
		[Description("ID = Variablennummer")]
		public string ID { get; set; }
		[Description("Wert = Variableninhalt")]
		public string Wert { get; set; }
		[Description("Variable = Textbeschreibung der Variablennummer")]
		public string Variable { get; set; }
		public int RecPerPage { get; set; }
		public int AmountOfPages { get; set; }


		public override string ToString()
		{
			return this.Variable;
		}
	}
}
