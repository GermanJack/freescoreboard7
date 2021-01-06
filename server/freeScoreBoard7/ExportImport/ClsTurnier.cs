using FreeScoreBoard.Core.DB;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FreeScoreBoard.ExportImport
{
	[Serializable()]
	[XmlRoot("Turnier")]
	public class ClsTurnier
	{
		[XmlElement("TKopf")]
		public TKopf TKopf { get; set; }

		[XmlElement("TRunden")]
		public List<TRunden> TRunden { get; set; }

		[XmlElement("TGruppen")]
		public List<TGruppen> TGruppen { get; set; }

		[XmlElement("TSpiele")]
		public List<TSpiele> TSpiele { get; set; }

		[XmlElement("TEreignisse")]
		public List<TEreignisse> TEreignisse { get; set; }

		[XmlElement("TTabellen")]
		public List<TTabellen> TTabellen { get; set; }
	}
}
