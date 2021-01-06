using System.IO;
using System.Linq;
using System.Xml.Serialization;
using FreeScoreBoard.Core.DBControler;

namespace FreeScoreBoard.ExportImport
{
	public static class ClsTurnierImpExp
	{
		private static ClsTurnier LoadFromDB(int TurnierID)
		{
			ClsTurnier Turnier = new ClsTurnier();
			Turnier.TKopf = ClsTurnierControler.Turnier(TurnierID);
			Turnier.TRunden = ClsTurnierControler.Runden(TurnierID);
			Turnier.TGruppen = ClsTurnierControler.Gruppen(TurnierID);
			Turnier.TSpiele = ClsTurnierControler.Spiele(TurnierID);
			Turnier.TTabellen = ClsTurnierControler.Tabellen(TurnierID);
			Turnier.TEreignisse = ClsTurnierControler.Ereignisse(TurnierID);

			return Turnier;
		}

		private static void SaveToDB(ClsTurnier Turnier)
		{
			int TurnierID = ClsTurnierControler.AddTurnierKopf(Turnier.TKopf);

			if (Turnier.TRunden.Any())
			{
				for (int i = 0; i < Turnier.TRunden.Count; i++)
				{
					Turnier.TRunden[i].TurnierID = TurnierID;
					ClsTurnierControler.AddRunden(Turnier.TRunden[i]);
				}
			}

			if (Turnier.TGruppen.Any())
			{
				for (int i = 0; i < Turnier.TGruppen.Count; i++)
				{
					Turnier.TGruppen[i].TurnierID = TurnierID;
					ClsTurnierControler.AddGruppen(Turnier.TGruppen[i]);
				}
			}

			if (Turnier.TSpiele.Any())
			{
				for (int i = 0; i < Turnier.TSpiele.Count; i++)
				{
					Turnier.TSpiele[i].TurnierID = TurnierID;
					ClsTurnierControler.AddSpiele(Turnier.TSpiele[i]);
				}
			}

			if (Turnier.TTabellen.Any())
			{
				for (int i = 0; i < Turnier.TTabellen.Count; i++)
				{
					Turnier.TTabellen[i].TurnierID = TurnierID;
					ClsTurnierControler.AddTabellen(Turnier.TTabellen[i]);
				}
			}

			if (Turnier.TEreignisse.Any())
			{
				for (int i = 0; i < Turnier.TEreignisse.Count; i++)
				{
					Turnier.TEreignisse[i].TurnierID = TurnierID;
					ClsTurnierControler.AddEreignis(Turnier.TEreignisse[i]);
				}
			}
		}

		public static void ExportToFile(string Datei, int TurnierID)
		{
			ClsTurnier Turnier = LoadFromDB(TurnierID);

			XmlSerializer x = new XmlSerializer(Turnier.GetType());
			StreamWriter writer = new StreamWriter(Datei);
			x.Serialize(writer, Turnier);
			writer.Close();
		}

		public static void ImportFromFile(string Datei)
		{
			ClsTurnier Turnier = new ClsTurnier();

			XmlSerializer x = new XmlSerializer(typeof(ClsTurnier));
			StreamReader reader = new StreamReader(Datei);
			Turnier = (ClsTurnier)x.Deserialize(reader);
			reader.Close();

			SaveToDB(Turnier);
		}

	}
}
