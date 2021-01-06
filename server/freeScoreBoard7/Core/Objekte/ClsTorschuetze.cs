namespace FreeScoreBoard.Core.Objekte
{
    public class ClsTorschuetze
    {
            private string myspieler;
            private string mymannschaft;
            private int myTore;

            public string Mannschaft
            {
                get { return this.mymannschaft; }
                set { this.mymannschaft = value; }
            }

            public string Spieler
            {
                get { return this.myspieler; }
                set { this.myspieler = value; }
            }

            public int Tore
            {
                get { return this.myTore; }
                set { this.myTore = value; }
            }
    }
}
