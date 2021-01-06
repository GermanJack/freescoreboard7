namespace FreeScoreBoard.Core.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TGruppen")]
    public partial class TGruppen
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        public int TurnierNr { get; set; }

        [StringLength(2147483647)]
        public string Gruppe { get; set; }

        public int status { get; set; }

        public int Runde { get; set; }

        public int TurnierID { get; set; }
    }
}
