using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace FreeScoreBoard.Core.DB
{
	[Table("A_Text")]
	public partial class A_Text
	{
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public long ID { get; set; }

		[StringLength(2147483647)]
		public string Area { get; set; }

		[StringLength(2147483647)]
		public string Key { get; set; }

		[StringLength(2147483647)]
		public string Language { get; set; }

		[StringLength(2147483647)]
		public string Text { get; set; }
	}
}
