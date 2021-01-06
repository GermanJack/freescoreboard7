namespace FreeScoreBoard.Core.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Mannschaften")]
    public partial class Mannschaften
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        [StringLength(2147483647)]
        public string Name { get; set; }

        [StringLength(2147483647)]
        public string Kurzname { get; set; }

        [StringLength(2147483647)]
        public string Bild1 { get; set; }

        [StringLength(2147483647)]
        public string Bild2 { get; set; }

        [StringLength(2147483647)]
        public string Torton { get; set; }
    }
}
