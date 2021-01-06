namespace FreeScoreBoard.Core.Objekte
{
    public class FilterE
    {
        // FilterE = cmbVarianteE.Text + TurnierNr + cmbEreignisart.Text;
        private string myVariante = "std";
        private int myTurnierID;
        private string mySpielNr = string.Empty;
        private string myEreignisart = string.Empty;

        public string Variante
        {
            get { return this.myVariante; }
            set { this.myVariante = value; }
        }

        public int TurnierID
        {
            get { return this.myTurnierID; }
            set { this.myTurnierID = value; }
        }

        public string SpielNr
        {
            get { return this.mySpielNr; }
            set { this.mySpielNr = value; }
        }

        public string Ereignisart
        {
            get { return this.myEreignisart; }
            set { this.myEreignisart = value; }
        }
    }
}
