namespace FreeScoreBoard.Core.Objekte
{
    public class FilterT
    {
        // FilterT = cmbVarianteT.Text + TurnierNr + cmbRundeT.Text + cmbGruppeT.Text;
        private string myVariante = "std";
        private int myTurnierID;
        private string myRunde = "";
        private string myGruppe = "";

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

        public string Runde
        {
            get { return this.myRunde; }
            set { this.myRunde = value; }
        }

        public string Gruppe
        {
            get { return this.myGruppe; }
            set { this.myGruppe = value; }
        }
    }
}
