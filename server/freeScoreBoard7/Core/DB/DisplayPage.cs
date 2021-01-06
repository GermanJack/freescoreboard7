using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace FreeScoreBoard.Core.DB
{
	[Table("DisplayPage")]
	public partial class DisplayPage
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long ID { get; set; }

		public long PageSetNo { get; set; }

		[StringLength(2147483647)]
		public string PageName { get; set; }

		[StringLength(2147483647)]
		public string Style { get; set; }

		[StringLength(2147483647)]
		public string MarkColor { get; set; }

		public long Sort { get; set; }

		public long Grid { get; set; }
	}
}
