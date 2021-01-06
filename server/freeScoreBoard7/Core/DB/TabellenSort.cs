namespace FreeScoreBoard.Core.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TabellenSort")]
    public partial class TabellenSort
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        public int Prio { get; set; }

        [StringLength(2147483647)]
        public string Feld { get; set; }

        public bool absteigend { get; set; }
    }
}
