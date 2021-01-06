using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml.Serialization;
using FreeScoreBoard.Core.DBControler;
using FreeScoreBoard.Core.DB;

namespace FreeScoreBoard.ExportImport
{
	public static class ClsMannschaftenImpExp
	{
		private static readonly string MyFilepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\FreeScoreBoard");

		private static ClsMannschaften LoadFromDB()
		{
			ClsMannschaften Mannschaften = new ClsMannschaften();
			Mannschaften.Mannschaften = ClsMannschaftenControler.Mannschaften();
			Mannschaften.Spieler = ClsMannschaftenControler.Spielers();

			return Mannschaften;
		}

		private static void SaveToDB(ClsMannschaften Mannschaften, bool Append = true)
		{
			if (!Append)
			{
				// alle Mannschften löschen
				List<Mannschaften> s = ClsMannschaftenControler.Mannschaften();
				if (s.Any())
				{
					for (int i = 0; i < s.Count; i++)
					{
						ClsMannschaftenControler.DelMannschaft(s[i].Name);
					}
				}

				// alle Spieler löschen
				List<Spieler> s2 = ClsMannschaftenControler.Spielers();
				if (s2.Any())
				{
					for (int i = 0; i < s2.Count; i++)
					{
						ClsMannschaftenControler.DelSpieler((int)s2[i].ID);
					}
				}

			}

			// Mannschaften speichen
			if (Mannschaften.Mannschaften.Any())
			{
				for (int i = 0; i < Mannschaften.Mannschaften.Count; i++)
				{
					ClsMannschaftenControler.AddMannschaft(Mannschaften.Mannschaften[i]);
				}
			}

			// Spieler speichern
			if (Mannschaften.Spieler.Any())
			{
				for (int i = 0; i < Mannschaften.Spieler.Count; i++)
				{
					ClsMannschaftenControler.AddSpieler(Mannschaften.Spieler[i]);
				}
			}
		}

		public static void ExportToFile(string Datei)
		{
			string tmpFolder = Path.Combine(MyFilepath + @"\tmp");
			if (Directory.Exists(tmpFolder))
			{
				Directory.Delete(tmpFolder, true);
			}

			Directory.CreateDirectory(tmpFolder);

			string tmpXMLfile = Path.Combine(tmpFolder + @"\Mannschaften.xml");

			ClsMannschaften Mannschaften = LoadFromDB();

			XmlSerializer x = new XmlSerializer(Mannschaften.GetType());
			StreamWriter writer = new StreamWriter(tmpXMLfile);
			x.Serialize(writer, Mannschaften);
			writer.Close();

			// Liste alle bildDateien erstellen
			List<string> ExpFiles = BildDateien(Mannschaften);
			ExpFiles.Add(tmpXMLfile);

			ZipArchive zip = ZipFile.Open(Datei, ZipArchiveMode.Create);
			foreach (string file in ExpFiles)
			{
				if (file != null)
				{
					zip.CreateEntryFromFile(file, Path.GetFileName(file), CompressionLevel.Optimal);
				}
			}

			zip.Dispose();

			// tmp xml löschen
			if (File.Exists(tmpXMLfile))
			{
				File.Delete(tmpXMLfile);
			}
		}

		private static List<string> BildDateien(ClsMannschaften Mannschaften)
		{
			List<string> filelist = new List<string>();

			foreach (Mannschaften a in Mannschaften.Mannschaften)
			{
				if (!string.IsNullOrEmpty(a.Bild1))
				{
					if (File.Exists(a.Bild1))
					{
						filelist.Add(a.Bild1);
					}
				}

				if (!string.IsNullOrEmpty(a.Bild2))
				{
					if (File.Exists(a.Bild2))
					{
						filelist.Add(a.Bild2);
					}
				}

				if (!string.IsNullOrEmpty(a.Torton))
				{
					if (File.Exists(a.Torton))
					{
						filelist.Add(a.Torton);
					}
				}
			}

			foreach (Spieler a in Mannschaften.Spieler)
			{
				if (!string.IsNullOrEmpty(a.Bild))
				{
					if (File.Exists(a.Bild))
					{
						filelist.Add(a.Bild);
					}
				}
			}

			List<string> distfilelist = filelist.Distinct().ToList();
			return distfilelist;
		}

		public static void ImportFromFile(string Datei, bool Append = true)
		{
			// Pfade und filenamen vorbereiten
			string tmp = Path.Combine(MyFilepath + @"\tmp");
			if (Directory.Exists(tmp))
			{
				Directory.Delete(tmp, true);
			}

			Directory.CreateDirectory(tmp);

			string tmpfile = Path.Combine(tmp + @"\Mannschaften.xml");

			string bilderfolder = MyFilepath + @"\Bilder\Mannschaften" + @"\";
			if (!Directory.Exists(bilderfolder))
			{
				Directory.CreateDirectory(bilderfolder);
			}

			// zip file entpacken
			ZipArchive zip = ZipFile.Open(Datei, ZipArchiveMode.Read);
			foreach (ZipArchiveEntry file in zip.Entries)
			{
				string completeFileName = Path.Combine(tmp, file.FullName);
				if (file.Name == "")
				{
					// Assuming Empty for Directory
					if (!Directory.Exists(Path.GetDirectoryName(completeFileName)))
					{
						Directory.CreateDirectory(Path.GetDirectoryName(completeFileName));
						continue;
					}
				}

				file.ExtractToFile(completeFileName, true);
			}

			zip.Dispose();

			// bilder an definierten ort speichern
			List<string> dirbmp = Directory.GetFiles(tmp).ToList();
			foreach (string f in dirbmp)
			{
				string ext = Path.GetExtension(f);
				if (ext != ".xml")
				{
					string quelle = f;
					string ziel = bilderfolder + Path.GetFileName(f);
					File.Move(quelle, ziel);
				}
			}

			// xml verarbeiten
			ClsMannschaften Mannschaften = new ClsMannschaften();

			XmlSerializer x = new XmlSerializer(typeof(ClsMannschaften));
			StreamReader reader = new StreamReader(tmpfile);
			Mannschaften = (ClsMannschaften)x.Deserialize(reader);
			reader.Close();

			// Dateifoldere ändern
			Mannschaften = UpdateFileLocations(Mannschaften, bilderfolder);

			// Fragen ob Anfügen oder überschreiben
			DlgAppend d = new DlgAppend();
			d.Header = "Mannschaften...";

			if (d.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
			{
				// überschreiben
				SaveToDB(Mannschaften, false);
			}

			if (d.ShowDialog() == System.Windows.Forms.DialogResult.No)
			{
				// anhängen
				SaveToDB(Mannschaften);
			}
		}

		private static ClsMannschaften UpdateFileLocations(ClsMannschaften Mannschaften, string BildFolder)
		{
			foreach(Mannschaften a in Mannschaften.Mannschaften)
			{
				if(!string.IsNullOrEmpty(a.Bild1))
				{
					string datei = Path.GetFileName(a.Bild1);
					a.Bild1 = Path.Combine(BildFolder, datei);
				}

				if (!string.IsNullOrEmpty(a.Bild2))
				{
					string datei = Path.GetFileName(a.Bild2);
					a.Bild2 = Path.Combine(BildFolder, datei);
				}

				if (!string.IsNullOrEmpty(a.Torton))
				{
					string datei = Path.GetFileName(a.Torton);
					a.Torton = Path.Combine(BildFolder, datei);
				}
			}

			foreach (Spieler a in Mannschaften.Spieler)
			{
				if (!string.IsNullOrEmpty(a.Bild))
				{
					string datei = Path.GetFileName(a.Bild);
					a.Bild = Path.Combine(BildFolder, datei);
				}
			}

			return Mannschaften;
		}
	}
}
