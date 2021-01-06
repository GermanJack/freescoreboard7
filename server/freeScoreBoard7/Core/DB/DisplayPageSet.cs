using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace FreeScoreBoard.Core.DB
{
	[Table("DisplayPageSet")]
	public partial class DisplayPageSet
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long ID { get; set; }

		[StringLength(2147483647)]
		public string PageSetName { get; set; }

		public long Sort { get; set; }
	}
}
