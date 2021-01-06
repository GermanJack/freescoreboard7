using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using FreeScoreBoard.Core.DBControler;
using FreeScoreBoard.Forms.Dialoge;

namespace FreeScoreBoard
{
    class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>

        [STAThread]
        static void Main()
        {

            //ClsFunktionen funktionen = new ClsFunktionen();

            //string l = Core.ClsFunktionen.Langu(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\FreeScoreBoard\langu.xml"));

            //switch (l)
            //{
            //    case "En":
            //        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");
            //        break;
            //    case "De":
            //        Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
            //        break;
            //}

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            FrmSplash splash = new FrmSplash();
            splash.Show();
            Application.DoEvents();

            ClsControler.CheckDB();
            Application.Run(new FrmFSBMain(splash));
        }
    }
}
