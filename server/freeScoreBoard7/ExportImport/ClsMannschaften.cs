using FreeScoreBoard.Core.DB;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FreeScoreBoard.ExportImport
{
	[Serializable()]
	[XmlRoot("Optionen")]
	public class ClsMannschaften
	{
		[XmlElement("Mannschaften")]
		public List<Mannschaften> Mannschaften { get; set; }

		[XmlElement("Spieler")]
		public List<Spieler> Spieler { get; set; }
	}
}
