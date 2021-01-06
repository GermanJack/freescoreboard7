namespace FreeScoreBoard.Core.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TKopf")]
    public partial class TKopf
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        public int TurnierNr { get; set; }

        [StringLength(2147483647)]
        public string Beschreibung { get; set; }

        [StringLength(2147483647)]
        public string Liga { get; set; }

        [StringLength(2147483647)]
        public string Kommentar { get; set; }

        [StringLength(2147483647)]
        public string Matrix { get; set; }

        [StringLength(2147483647)]
        public string Platzierungstyp { get; set; }

        [StringLength(2147483647)]
        public string Vorkommnisse { get; set; }

        public int Mananz { get; set; }

        public int status { get; set; }

        [StringLength(2147483647)]
        public string Version { get; set; }

        [StringLength(2147483647)]
        public string Turniertyp { get; set; }

        public int MatrixID { get; set; }
    }
}
