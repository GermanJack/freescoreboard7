using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeScoreBoard.Core.DB;

namespace FreeScoreBoard.Core.Objekte
{
	public class ClsTurnier
	{
		public TKopf Kopf { get; set; }

		public List<TRunden> Runden { get; set; }

		public List<TGruppen> Gruppen { get; set; }

		public List<TSpiele> Spiele { get; set; }

		public List<TTabellen> Tabellen { get; set; }

		public List<TEreignisse> Ereignisse { get; set; }

		public List<ClsTorschuetze> Torschuetzen { get; set; }

	}
}
