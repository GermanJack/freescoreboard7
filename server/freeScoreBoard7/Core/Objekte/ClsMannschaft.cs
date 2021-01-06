using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeScoreBoard.Core.DB;

namespace FreeScoreBoard.Core.Objekte
{
    public class ClsMannschaft
    {
        public FreeScoreBoard.Core.DB.Mannschaften Manschaft { get; set; }

        public List<Spieler> Spieler { get; set; }
    }
}
