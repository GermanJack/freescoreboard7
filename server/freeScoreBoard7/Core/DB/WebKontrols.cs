namespace FreeScoreBoard.Core.DB
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	public partial class WebKontrols
	{
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public long ID { get; set; }

		[StringLength(2147483647)]
		public string Objekt { get; set; }

		[StringLength(2147483647)]
		public string Title { get; set; }

		public int Spalte { get; set; }

		public int Sort { get; set; }

		public int Offen { get; set; }

		public int Reset { get; set; }

		public int Sichtbar { get; set; }
	}
}
