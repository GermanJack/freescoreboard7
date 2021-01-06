using FreeScoreBoard.Core.DB;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FreeScoreBoard.ExportImport
{
	[Serializable()]
	[XmlRoot("Optionen")]
	public class ClsOptionen
	{
		[XmlElement("Optionen3")]
		public List<Options3> Options3 { get; set; }

		[XmlElement("HotKey")]
		public List<HotKey> HotKey { get; set; }

		[XmlElement("Kontrols")]
		public List<Kontrols> Kontrols { get; set; }

		[XmlElement("Strafen")]
		public List<Strafen> Strafen { get; set; }

		[XmlElement("TabellenSort")]
		public List<TabellenSort> TabellenSort { get; set; }

		[XmlElement("Timer")]
		public List<Timer> Timer { get; set; }

		[XmlElement("Timerevent")]
		public List<Timerevent> Timerevent { get; set; }
	}
}
