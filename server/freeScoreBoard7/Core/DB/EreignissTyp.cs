namespace FreeScoreBoard.Core.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EreignissTyp")]
    public partial class EreignissTyp
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        public int Sort { get; set; }

        [StringLength(2147483647)]
        public string Nummer { get; set; }

        public bool Log { get; set; }
    }
}
