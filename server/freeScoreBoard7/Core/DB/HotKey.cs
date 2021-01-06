namespace FreeScoreBoard.Core.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotKey")]
    public partial class HotKey
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        public int Sort { get; set; }

        [StringLength(2147483647)]
        public string Befehl { get; set; }

        [Column("Hotkey")]
        [StringLength(2147483647)]
        public string Hotkey1 { get; set; }
    }
}
