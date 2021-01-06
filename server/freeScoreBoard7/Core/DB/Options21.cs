namespace FreeScoreBoard.Core.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Options21
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        public int Min { get; set; }

        public int Max { get; set; }

        public int WertSeq { get; set; }

        [StringLength(2147483647)]
        public string Wert { get; set; }

        [StringLength(2147483647)]
        public string Type { get; set; }
    }
}
