namespace FreeScoreBoard.Core.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TEreignisse")]
    public partial class TEreignisse
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        public int TurnierNr { get; set; }

        public int Spiel { get; set; }

        [StringLength(2147483647)]
        public string Mannschaft { get; set; }

        [StringLength(2147483647)]
        public string Spielzeit { get; set; }

        [StringLength(2147483647)]
        public string Spielzeitrichtung { get; set; }

        public DateTime CPUZeit { get; set; }

        [StringLength(2147483647)]
        public string Ereignistyp { get; set; }

        [StringLength(2147483647)]
        public string Spieler { get; set; }

        [StringLength(2147483647)]
        public string Details { get; set; }

        [StringLength(2147483647)]
        public string Spielabschnitt { get; set; }

        public int TurnierID { get; set; }
    }
}
