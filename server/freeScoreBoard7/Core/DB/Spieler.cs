namespace FreeScoreBoard.Core.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Spieler")]
    public partial class Spieler
    {
        public long ID { get; set; }

        public long? MannschaftsID { get; set; }

        [StringLength(2147483647)]
        public string Nachname { get; set; }

        [StringLength(2147483647)]
        public string Vorname { get; set; }

        [StringLength(2147483647)]
        public string SID { get; set; }

        [StringLength(2147483647)]
        public string NickName { get; set; }

        [StringLength(2147483647)]
        public string Kurzname { get; set; }

        [StringLength(2147483647)]
        public string Bild { get; set; }
    }
}
