using System;
using System.Windows.Forms;
using FreeScoreBoard.Forms.Dialoge;
using FreeScoreBoard.Forms.Dialoge.Forms.Dialoge;

namespace FreeScoreBoard.Forms.Objekte
{

    public static class ClsError
    {
        public static void Error(string modul, string function, Exception ex)
        {
            DlgError dlgError = new DlgError();
            dlgError.Ex = ex;
            dlgError.Function = function;
            dlgError.Modul = modul;

            if (dlgError.InvokeRequired)
            {
                dlgError.Invoke(new MethodInvoker(() => { Error(modul, function, ex); }));
            }
            else
            {
                DialogResult e = dlgError.ShowDialog();

                // DialogResult r = MessageBox.Show(stb.ToString(),"Fehler...",MessageBoxButtons.OKCancel);
                if (e == DialogResult.Cancel)
                {
                    Application.Exit();
                }
            }
        }

        //if (this.PGStyle.InvokeRequired)
        //    {
        //    this.PGStyle.Invoke(new MethodInvoker(() => { Divaenderung(); }));
        //}
        //    else
        //    {
        //    this.ini();
        //}
    }
}
