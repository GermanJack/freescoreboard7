using System.Windows.Forms;
using FreeScoreBoard.Core.Localisation;

namespace FreeScoreBoard.ExportImport
{
    public static class ClsDialog
    {
        public static string[] OpenMulti(string Extention)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = ClsLocalisationFunctions.Keytext("Dateityp", Extention) + " (*." + Extention + ")|*." + Extention + "|"
                + ClsLocalisationFunctions.Keytext("Dateityp", "alles") + " (*.*)|*.*",
                Multiselect = true
            };

            if (ofd.ShowDialog() != DialogResult.Cancel)
            {
                return ofd.FileNames;
            }

            return null;
        }

        public static string Open(string Extention)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = ClsLocalisationFunctions.Keytext("Dateityp", Extention) + " (*." + Extention + ")|*." + Extention + "|"
                + ClsLocalisationFunctions.Keytext("Dateityp", "alles") + " (*.*)|*.*",
                Multiselect = false
            };

            if (ofd.ShowDialog() != DialogResult.Cancel)
            {
                return ofd.FileName;
            }

            return null;
        }

        public static string Save(string Extention)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = ClsLocalisationFunctions.Keytext("Dateityp", Extention) + " (*." + Extention + ")|*." + Extention;
            if (sfd.ShowDialog() != DialogResult.Cancel)
            {
                return sfd.FileName;
            }

            return null;
        }
    }
}
