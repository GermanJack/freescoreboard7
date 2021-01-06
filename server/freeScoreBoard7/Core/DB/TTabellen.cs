namespace FreeScoreBoard.Core.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TTabellen")]
    public partial class TTabellen
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        public int TurnierNr { get; set; }

        public int Runde { get; set; }

        [StringLength(2147483647)]
        public string Gruppe { get; set; }

        [StringLength(2147483647)]
        public string Mannschaft { get; set; }

        public int Punkte { get; set; }

        public int Tore { get; set; }

        public int Gegentore { get; set; }

        public int Torverhaeltnis { get; set; }

        public int Spiele { get; set; }

        public int Platz { get; set; }

        public int fixPlatz { get; set; }

        public int Auszeiten { get; set; }

        public int Rundenplatz { get; set; }

        public int Direkterpunktvergleich { get; set; }

        public int Direktererweitertervergleich { get; set; }

        public int TurnierID { get; set; }

        [StringLength(2147483647)]
        public string Quelltyp { get; set; }

        public int Quellrunde { get; set; }

        [StringLength(2147483647)]
        public string QuellGruppe { get; set; }

        public int QuellGruppenplatz { get; set; }

        public int QuellRundenplatz { get; set; }
    }
}
