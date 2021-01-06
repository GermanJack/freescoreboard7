namespace FreeScoreBoard.Core.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Registrierung")]
    public partial class Registrierung
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        [StringLength(2147483647)]
        public string extID { get; set; }

        [StringLength(2147483647)]
        public string Nachname { get; set; }

        [StringLength(2147483647)]
        public string Vorname { get; set; }

        [StringLength(2147483647)]
        public string Verein { get; set; }

        [StringLength(2147483647)]
        public string Sportart { get; set; }

        [StringLength(2147483647)]
        public string Land { get; set; }

        [StringLength(2147483647)]
        public string PLZ { get; set; }

        [StringLength(2147483647)]
        public string Ort { get; set; }

        [StringLength(2147483647)]
        public string Mail { get; set; }

        public bool statistic { get; set; }

        public bool entered { get; set; }

        public bool send { get; set; }
    }
}
