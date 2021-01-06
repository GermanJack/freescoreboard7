using FreeScoreBoard.Core.DB;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FreeScoreBoard.ExportImport
{
	[Serializable()]
	[XmlRoot("Tabellendarstellung")]
	public class ClsTabellendarstellung
	{
		[XmlElement("Anzeigetabelle")]
		public List<Anzeigetabelle> Anzeigetabelle { get; set; }
	}
}
