namespace FreeScoreBoard.Core.DB
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	[Table("Timerevent")]
	public partial class Timerevent
	{
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public long ID { get; set; }

		[StringLength(2147483647)]
		public string Name { get; set; }

		public int Eventtype { get; set; }

		[StringLength(2147483647)]
		public string Soundfile { get; set; }

		public bool Active { get; set; }

		public int TimerNr { get; set; }

		public int Sekunden { get; set; }

		public int AndereTimerNr { get; set; }

		[StringLength(2147483647)]
		public string Layer { get; set; }
	}
}
