using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml.Serialization;
using FreeScoreBoard.Core.DB;
using FreeScoreBoard.Core.DBControler;

namespace FreeScoreBoard.ExportImport
{
    public static class ClsAnzeigeImpExp
    {
        private static readonly string MyFilepath = Path.Combine($@"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\FreeScoreBoard");

        private static ClsAnzeige LoadFromDB(int DisplaySettingsNr)
        {
            ClsAnzeige Anzeige = new ClsAnzeige();
            //Anzeige.DisplaySettings = ClsDisplayControler.DisplaySettings(DisplaySettingsNr);
            //Anzeige.DisplayPage = ClsDisplayControler.DisplayPages(DisplaySettingsNr);
            //Anzeige.DivDB = ClsDisplayControler.Anzeigeobjekte(DisplaySettingsNr);

            return Anzeige;
        }

        private static void SaveToDB(ClsAnzeige Anzeige, int DisplaySettingsNr)
        {
            //Anzeige.DisplaySettings.Set = DisplaySettingsNr;
            //int AnzeigeID = ClsDisplayControler.AddDisplaySettings(Anzeige.DisplaySettings);

            //if (Anzeige.DisplayPage.Any())
            //{
            //	for (int i = 0; i < Anzeige.DisplayPage.Count; i++)
            //	{
            //			Anzeige.DisplayPage[i].Set = DisplaySettingsNr;
            //                 ClsDisplayControler.AddDisplayPage(Anzeige.DisplayPage[i]);
            //	}
            //}

            //if (Anzeige.DivDB.Any())
            //{
            //	for (int i = 0; i < Anzeige.DivDB.Count; i++)
            //	{
            //			Anzeige.DivDB[i].Set = DisplaySettingsNr;
            //                 ClsDisplayControler.AddAnzeigeobjekt(Anzeige.DivDB[i]);
            //	}
            //}

        }

        public static void ExportToFile(string Datei, int DisplaySettingsNr)
        {
            if (File.Exists(Datei))
            {
                File.Delete(Datei);
            }

            string tmpFolder = Path.Combine(MyFilepath + @"\tmp");
            if (Directory.Exists(tmpFolder))
            {
                Directory.Delete(tmpFolder, true);
            }

            Directory.CreateDirectory(tmpFolder);

            string tmpXMLfile = Path.Combine(tmpFolder + @"\Anzeige.xml");
            if (File.Exists(tmpXMLfile))
            {
                File.Delete(tmpXMLfile);
            }

            ClsAnzeige Anzeige = LoadFromDB(DisplaySettingsNr);

            // xml temporer Zwischenspeichern
            XmlSerializer x = new XmlSerializer(Anzeige.GetType());
            StreamWriter writer = new StreamWriter(tmpXMLfile);
            x.Serialize(writer, Anzeige);
            writer.Close();

            // Liste alle bildDateien erstellen
            List<string> ExpFiles = BildDateien(Anzeige);
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

        private static List<string> BildDateien(ClsAnzeige Anzeige)
        {
            List<string> filelist = new List<string>();

            //foreach (DisplayPage a in Anzeige.DisplayPage)
            //{
            //	if (!string.IsNullOrEmpty(a.BackGroundPicture))
            //	{
            //		if (File.Exists(a.BackGroundPicture))
            //		{
            //			filelist.Add(a.BackGroundPicture);
            //		}
            //	}
            //}

            //foreach (DivDB a in Anzeige.DivDB)
            //{
            //	if (!string.IsNullOrEmpty(a.Bild))
            //	{
            //		if (File.Exists(a.Bild))
            //		{
            //			filelist.Add(a.Bild);
            //		}
            //	}
            //}

            List<string> distfilelist = filelist.Distinct().ToList();
            return distfilelist;
        }

        public static int ImportFromFile(string Datei)
        {
            // nexte freie setnummer
            int DisplaySettingsNr = 0;

            //using (fsbDB FSBDB = new fsbDB())
            //{
            //	int max = (from y in FSBDB.DisplaySettings select y.Set).Max();
            //	DisplaySettingsNr = max + 1;
            //}

            //// Pfade und filenamen vorbereiten
            //string tmp = Path.Combine(MyFilepath + @"\tmp");
            //if (Directory.Exists(tmp))
            //{
            //	Directory.Delete(tmp, true);
            //}

            //Directory.CreateDirectory(tmp);

            //string tmpfile = Path.Combine(tmp + @"\Anzeige.xml");

            //string bilderfolder = MyFilepath + @"\Bilder\Set" + DisplaySettingsNr.ToString() + @"\";
            //if (!Directory.Exists(bilderfolder))
            //{
            //	Directory.CreateDirectory(bilderfolder);
            //}
            //else
            //{
            //	List<string> dir = Directory.GetFiles(bilderfolder).ToList();
            //	foreach (string f in dir)
            //	{
            //		File.Delete(f);
            //	}
            //}

            //// zip file entpacken
            //ZipArchive zip = ZipFile.Open(Datei, ZipArchiveMode.Read);
            //foreach (ZipArchiveEntry file in zip.Entries)
            //{
            //	string completeFileName = Path.Combine(tmp, file.FullName);
            //	if (file.Name == "")
            //	{
            //		// Assuming Empty for Directory
            //		if (!Directory.Exists(Path.GetDirectoryName(completeFileName)))
            //		{
            //			Directory.CreateDirectory(Path.GetDirectoryName(completeFileName));
            //			continue;
            //		}
            //	}

            //	file.ExtractToFile(completeFileName, true);
            //}

            //zip.Dispose();

            //// bilder an definierten ort speichern
            //List<string> dirbmp = Directory.GetFiles(tmp).ToList();
            //foreach (string f in dirbmp)
            //{
            //	string ext = Path.GetExtension(f);
            //	if (ext != ".xml")
            //	{
            //		string quelle = f;
            //		string ziel = bilderfolder + Path.GetFileName(f);
            //		File.Move(quelle, ziel);
            //	}
            //}

            //// xml verarbeiten

            //ClsAnzeige Anzeige = new ClsAnzeige();

            //XmlSerializer x = new XmlSerializer(typeof(ClsAnzeige));
            //StreamReader reader = new StreamReader(tmpfile);
            //Anzeige = (ClsAnzeige)x.Deserialize(reader);
            //reader.Close();

            //// Dateifoldere ändern
            //Anzeige = UpdateFileLocations(Anzeige, bilderfolder);

            //SaveToDB(Anzeige, DisplaySettingsNr);

            //// tmp xml löschen
            //if (File.Exists(tmpfile))
            //{
            //	File.Delete(tmpfile);
            //}

            return DisplaySettingsNr;
        }

        private static ClsAnzeige UpdateFileLocations(ClsAnzeige Anzeige, string BildFolder)
        {
            //foreach (DisplayPage a in Anzeige.DisplayPage)
            //{
            //	if (!string.IsNullOrEmpty(a.BackGroundPicture))
            //	{
            //		string datei = Path.GetFileName(a.BackGroundPicture);
            //		a.BackGroundPicture = Path.Combine(BildFolder, datei);
            //	}
            //}

            //foreach (DivDB a in Anzeige.DivDB)
            //{
            //	if (!string.IsNullOrEmpty(a.Bild))
            //	{
            //		string datei = Path.GetFileName(a.Bild);
            //		a.Bild = Path.Combine(BildFolder, datei);
            //	}
            //}

            return Anzeige;
        }
    }
}
