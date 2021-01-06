using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace FreeScoreBoard.Core.DB
{
	[Table("User")]

	public partial class User
	{
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public long ID { get; set; }

		[StringLength(40)]
		public string Name { get; set; }

		[StringLength(40)]
		public string Password { get; set; }
	}
}
