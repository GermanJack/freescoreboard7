namespace FreeScoreBoard.Core.DB
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	[Table("Anzeigetabelle")]
	public partial class Anzeigetabelle
	{
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public long ID { get; set; }

		[StringLength(2147483647)]
		public string Tabelle { get; set; }

		[StringLength(2147483647)]
		public string DBTabelle { get; set; }

		[StringLength(2147483647)]
		public string DBFeld { get; set; }

		[StringLength(2147483647)]
		public string Anzeigename { get; set; }

		public int Position { get; set; }

		public long Sichtbar { get; set; }

		public long ausblendbar { get; set; }

		[StringLength(2147483647)]
		public string Variante { get; set; }

		public long aktiv { get; set; }
	}
}
