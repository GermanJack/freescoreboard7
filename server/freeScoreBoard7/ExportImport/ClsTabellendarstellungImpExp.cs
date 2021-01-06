using System.IO;
using System.Linq;
using System.Xml.Serialization;
using FreeScoreBoard.Core.DBControler;

namespace FreeScoreBoard.ExportImport
{
	public class ClsTabellendarstellungImpExp
	{
		private static ClsTabellendarstellung LoadFromDB()
		{
			ClsTabellendarstellung Tabellendarstellung = new ClsTabellendarstellung();
			Tabellendarstellung.Anzeigetabelle = ClsOptionsControler.AnzeigetabelleAll();

			return Tabellendarstellung;
		}

		private static void SaveToDB(ClsTabellendarstellung Tabellendarstellung)
		{
			if (Tabellendarstellung.Anzeigetabelle.Any())
			{
				for (int i = 0; i < Tabellendarstellung.Anzeigetabelle.Count; i++)
				{
					ClsOptionsControler.AddAnzeigetabelle(Tabellendarstellung.Anzeigetabelle[i]);
				}
			}
		}

		public static void ExportToFile(string Datei)
		{
			ClsTabellendarstellung Tabellendarstellung = LoadFromDB();

			XmlSerializer x = new XmlSerializer(Tabellendarstellung.GetType());
			StreamWriter writer = new StreamWriter(Datei);
			x.Serialize(writer, Tabellendarstellung);
			writer.Close();
		}

		public static void ImportFromFile(string Datei)
		{
			ClsTabellendarstellung Tabellendarstellung = new ClsTabellendarstellung();

			XmlSerializer x = new XmlSerializer(typeof(ClsTabellendarstellung));
			StreamReader reader = new StreamReader(Datei);
			Tabellendarstellung = (ClsTabellendarstellung)x.Deserialize(reader);
			reader.Close();

			SaveToDB(Tabellendarstellung);
		}

	}
}
