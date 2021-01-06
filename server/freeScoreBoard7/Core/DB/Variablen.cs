namespace FreeScoreBoard.Core.DB
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	[Table("Variablen")]
	public partial class Variablen
	{
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public long ID { get; set; }

		[StringLength(2147483647)]
		public string Name { get; set; }

		[StringLength(2147483647)]
		public string Typ { get; set; }

		/// <summary>
		/// Default Value for String Variables ; amount of Records per Page for Table Variables
		/// </summary>
		[StringLength(2147483647)]
		public string Default { get; set; }

		[StringLength(2147483647)]
		public string Kommentar { get; set; }

		[StringLength(2147483647)]
		public string Aktiv { get; set; }

		public bool LogChange { get; set; }
	}
}
