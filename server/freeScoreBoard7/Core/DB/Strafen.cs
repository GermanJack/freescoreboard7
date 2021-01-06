namespace FreeScoreBoard.Core.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Strafen")]
    public partial class Strafen
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        [StringLength(2147483647)]
        public string Bezeichnung { get; set; }

        [StringLength(2147483647)]
        public string Art { get; set; }

        public int Sekunden { get; set; }

        [StringLength(2147483647)]
        public string TonCountdown { get; set; }

        public int Countdowndauer { get; set; }

        [StringLength(2147483647)]
        public string EndeTon { get; set; }
    }
}
