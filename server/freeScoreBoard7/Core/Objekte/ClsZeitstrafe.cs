namespace FreeScoreBoard.Core.Objekte
{
	public class ClsZeitstrafe
	{
		private int myID;
		private int myZeit;
		private string myMinuten;
		private string mySpielerID;
		private string myspieler;
		private string mymannschaft;
		private string myBezeichnung;

		public int ID
		{
			get { return this.myID; }
			set { this.myID = value; }
		}

		public int Zeit
		{
			get { return this.myZeit; }
			set { this.myZeit = value; }
		}

		public string Minuten
		{
			get { return this.myMinuten; }
			set { this.myMinuten = value; }
		}

		public string SpielerID
		{
			get { return this.mySpielerID; }
			set { this.mySpielerID = value; }
		}

		public string Spieler
		{
			get { return this.myspieler; }
			set { this.myspieler = value; }
		}

		public string Mannschaft
		{
			get { return this.mymannschaft; }
			set { this.mymannschaft = value; }
		}

		public string Bezeichnung
		{
			get { return this.myBezeichnung; }
			set { this.myBezeichnung = value; }
		}
	}
}
