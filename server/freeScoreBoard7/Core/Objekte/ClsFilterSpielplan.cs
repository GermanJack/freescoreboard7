namespace FreeScoreBoard.Core.Objekte
{
    public class FilterS
    {
        // FilterS = cmbVarianteS.Text + TurnierNr + cmbRundeS.Text + cmbGruppeS.Text + cmbStatus.Text;
        private string myVariante = "std";
        private int myTurnierID;
        private string myRunde = string.Empty;
        private string myGruppe = string.Empty;
        private string myStatus = string.Empty;
        private string myDate = string.Empty;

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

        public string Status
        {
            get { return this.myStatus; }
            set { this.myStatus = value; }
        }

        public string Date
        {
            get { return this.myDate; }
            set { this.myDate = value; }
        }
    }
}
