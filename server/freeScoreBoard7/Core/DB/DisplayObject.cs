using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace FreeScoreBoard.Core.DB
{
	[Table("DisplayObject")]
	public partial class DisplayObject
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long ID { get; set; }

		public long PageSetNo { get; set; }

		public long PageNo { get; set; }

		[StringLength(2147483647)]
		public string style { get; set; }

		[StringLength(2147483647)]
		public string innerText { get; set; }

		[StringLength(2147483647)]
		public string textid { get; set; }

		[StringLength(2147483647)]
		public string bgid { get; set; }


		[StringLength(2147483647)]
		public string tableid { get; set; }

		public long Zindex { get; set; }

		public long Speed { get; set; }

		[StringLength(2147483647)]
		public string Info { get; set; }

		[StringLength(2147483647)]
		public string TableStyle { get; set; }
	}
}
