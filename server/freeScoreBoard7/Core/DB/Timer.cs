namespace FreeScoreBoard.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Timer")]
    public partial class Timer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        public int Nr { get; set; }

        [StringLength(2147483647)]
        public string Name { get; set; }

        public int StartSekunden { get; set; }

        public int AbhängigeTimerNr { get; set; }

        public bool Countdown { get; set; }

        public bool MinutenDarstellung { get; set; }

        [StringLength(2147483647)]
        public string Variable { get; set; }

        public bool Kontrolanzeige { get; set; }

        public bool DisplayDynamisch { get; set; }

        public bool AutoReset { get; set; }

        public int AbhaengigeTimerStatus { get; set; }

        public bool AbhängigeTimerStatus { get; set; }
    }
}
