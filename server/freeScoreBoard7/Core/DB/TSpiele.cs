namespace FreeScoreBoard.Core.DB
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	[Table("TSpiele")]
	public partial class TSpiele
	{
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public long ID { get; set; }

		public int TurnierNr { get; set; }

		public int Spiel { get; set; }

		public int Runde { get; set; }

		[StringLength(2147483647)]
		public string Gruppe { get; set; }

		public int GruppenSpiel { get; set; }

		[StringLength(2147483647)]
		public string PlanMannA { get; set; }

		[StringLength(2147483647)]
		public string IstMannA { get; set; }

		[StringLength(2147483647)]
		public string PlanMannB { get; set; }

		[StringLength(2147483647)]
		public string IstMannB { get; set; }

		public int ToreA { get; set; }

		public int ToreB { get; set; }

		public int Status { get; set; }

		public int SPlatz { get; set; }

		public int VPlatz { get; set; }

		[StringLength(2147483647)]
		public string Ort { get; set; }

		[StringLength(2147483647)]
		public string Datum { get; set; }

		public int PunkteA { get; set; }

		public int PunkteB { get; set; }

		public int TurnierID { get; set; }

		[StringLength(2147483647)]
		public string Uhrzeit { get; set; }
	}
}
