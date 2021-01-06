namespace FreeScoreBoard.Core.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Options2
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        public int PropertySeq { get; set; }

        [StringLength(2147483647)]
        public string Name { get; set; }

        [StringLength(2147483647)]
        public string Type { get; set; }

        [StringLength(2147483647)]
        public string Default { get; set; }

        public int GroupNr { get; set; }

        public int PropertyNr { get; set; }
    }
}
