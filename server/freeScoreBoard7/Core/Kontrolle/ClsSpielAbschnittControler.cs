using System;
using System.Reflection;
using FreeScoreBoard.Core.DBControler;
using FreeScoreBoard.Core.Variablen;
using FreeScoreBoard.Interpreter.Error;

namespace FreeScoreBoard.Core.Kontrolle
{
    public class ClsSpielAbschnittControler
    {
        private static readonly System.Lazy<ClsSpielAbschnittControler> Lazy = new System.Lazy<ClsSpielAbschnittControler>(() => new ClsSpielAbschnittControler());

        public static ClsSpielAbschnittControler Instance
        {
            get { return Lazy.Value; }
        }

        private int mySpielabschnitt = 1;
        private int mymaxSpielabschnitte = 1;

        private string Name = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public int Spielabschnitt
        {
            get
            {
                return this.mySpielabschnitt;
            }

            set
            {
                if (value <= this.MaxSpielabschnitte && value >= 1)
                {
                    this.mySpielabschnitt = value;
                    ClsDBVariablen.Instance.SetTextVariableWert("S09", this.mySpielabschnitt.ToString());
                }
            }
        }

        public int MaxSpielabschnitte
        {
            get
            {
                return this.mymaxSpielabschnitte;
            }

            set
            {
                this.mymaxSpielabschnitte = value;
                ClsDBVariablen.Instance.SetTextVariableWert("S31", this.mymaxSpielabschnitte.ToString());
            }
        }

        public ClsSpielAbschnittControler()
        {
        }

        public void Ini()
        {
            try
            {
                // Abschnitte setzten
                this.MaxSpielabschnitte = Convert.ToInt32(ClsOptionsControler.Options3("Spielabschnitte").Value);
            }
            catch (Exception ex)
            {
                ClsError.Error(this.Name, MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
    }
}
