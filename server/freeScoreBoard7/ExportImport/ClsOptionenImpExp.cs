using FreeScoreBoard.Core.DBControler;
using FreeScoreBoard.Core.DB;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace FreeScoreBoard.ExportImport
{
    public static class ClsOptionenImpExp
    {

        private static ClsOptionen LoadFromDB()
        {
            ClsOptionen Optionen = new ClsOptionen();
            Optionen.HotKey = ClsHotkeyControler.hotkeys();
            Optionen.Kontrols = ClsKontrolControler.Kontrols();
            Optionen.Options3 = ClsOptionsControler.Options3();
            Optionen.Strafen = ClsOptionsControler.Strafen();
            Optionen.TabellenSort = ClsOptionsControler.Tabellensortierung();
            Optionen.Timer = ClsTimerControler.Timers();
            Optionen.Timerevent = ClsTimerControler.TimerEvents();

            return Optionen;
        }

        private static void SaveToDB(ClsOptionen Optionen)
        {
            List<HotKey> o1 = ClsHotkeyControler.hotkeys();
            for (int i = 0; i < Optionen.HotKey.Count; i++)
            {
                HotKey h = (from x in o1 where x.Befehl == Optionen.HotKey[i].Befehl select x).FirstOrDefault();
                if (h != null)
                {
                    o1[i].Hotkey1 = Optionen.HotKey[i].Hotkey1;
                    ClsHotkeyControler.SaveHotKey(o1[i]);
                }
            }

            List<Kontrols> o2 = ClsKontrolControler.Kontrols();
            for (int i = 0; i < Optionen.Kontrols.Count; i++)
            {
                Kontrols h = (from x in o2 where x.Name == Optionen.Kontrols[i].Name select x).FirstOrDefault();
                if (h != null)
                {
                    o2[i].Ort = Optionen.Kontrols[i].Ort;
                    o2[i].Sort = Optionen.Kontrols[i].Sort;
                    ClsKontrolControler.SaveKontrol(o2[i]);
                }
            }

            List<Options3> o3 = ClsOptionsControler.Options3();
            for (int i = 0; i < Optionen.Options3.Count; i++)
            {
                Options3 h = (from x in o3 where x.Prop == Optionen.Options3[i].Prop select x).FirstOrDefault();
                if (h != null)
                {
                    o3[i].Value = Optionen.Options3[i].Value;
                    ClsOptionsControler.SaveOptions3(o3[i]);
                }
            }

            List<Strafen> s = ClsOptionsControler.Strafen();
            if (s.Any())
            {
                for(int i = 0; i < s.Count; i++)
                {
                    ClsOptionsControler.DelStrafe(s[i].Bezeichnung);
                }
            }

            if (Optionen.Strafen.Any())
            {
                for (int i = 0; i < Optionen.Strafen.Count; i++)
                {
                    ClsOptionsControler.AddStrafe(Optionen.Strafen[i]);
                }
            }

            List<TabellenSort> o4 = ClsOptionsControler.Tabellensortierung();
            for (int i = 0; i < Optionen.TabellenSort.Count; i++)
            {
                TabellenSort h = (from x in o4 where x.Feld == Optionen.TabellenSort[i].Feld select x).FirstOrDefault();
                if (h != null)
                {
                    o4[i].Prio = Optionen.TabellenSort[i].Prio;
                    o4[i].absteigend = Optionen.TabellenSort[i].absteigend;
                    ClsOptionsControler.SaveTabellensortierung(o4[i]);
                }
            }

            List<Timer> o5 = ClsTimerControler.Timers();
            for (int i = 0; i < Optionen.Timer.Count; i++)
            {
                Timer h = (from x in o5 where x.Nr == Optionen.Timer[i].Nr select x).FirstOrDefault();
                if (h != null)
                {
                    bool ats1 = o5[i].AbhaengigeTimerStatus == 0 ? false : true;
                    bool ats2 = Optionen.Timer[i].AbhaengigeTimerStatus == 0 ? false : true;
                    o5[i].AbhängigeTimerNr = Optionen.Timer[i].AbhängigeTimerNr;
                    ats1 = ats2;
                    o5[i].AutoReset = Optionen.Timer[i].AutoReset;
                    o5[i].Countdown = Optionen.Timer[i].Countdown;
                    o5[i].DisplayDynamisch = Optionen.Timer[i].DisplayDynamisch;
                    o5[i].Kontrolanzeige = Optionen.Timer[i].Kontrolanzeige;
                    o5[i].MinutenDarstellung = Optionen.Timer[i].MinutenDarstellung;
                    o5[i].StartSekunden = Optionen.Timer[i].StartSekunden;
                    ClsOptionsControler.SaveTabellensortierung(o4[i]);
                }
            }

            List<Timerevent> s2 = ClsTimerControler.TimerEvents();
            if (s2.Any())
            {
                for (int i = 0; i < s2.Count; i++)
                {
                    ClsTimerControler.DelTimerEvent((int)s2[i].ID);
                }
            }

            if (Optionen.Timerevent.Any())
            {
                for (int i = 0; i < Optionen.Timerevent.Count; i++)
                {
                    ClsTimerControler.AddTimerEvent(Optionen.Timerevent[i]);
                }
            }
        }

        public static void ExportToFile(string Datei)
        {
            ClsOptionen Optionen = LoadFromDB();

            XmlSerializer x = new XmlSerializer(Optionen.GetType());
            StreamWriter writer = new StreamWriter(Datei);
            x.Serialize(writer, Optionen);
            writer.Close();
        }

        public static void ImportFromFile(string Datei)
        {
            ClsOptionen Optionen = new ClsOptionen();

            XmlSerializer x = new XmlSerializer(typeof(ClsOptionen));
            StreamReader reader = new StreamReader(Datei);
            Optionen = (ClsOptionen)x.Deserialize(reader);
            reader.Close();

            SaveToDB(Optionen);
        }


    }
}
