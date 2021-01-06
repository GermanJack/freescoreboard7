using System;
using System.Data;

namespace FreeScoreBoard.Core.Variablen
{
    public delegate void TPropChangedEventHandler(object sender, TPropChanged e);

    public class TPropChanged : EventArgs
    {
        public readonly string Variablename;
        public readonly DataTable Variablenwert;

        public TPropChanged(string name, DataTable wert)
        {
            this.Variablename = name;
            this.Variablenwert = wert;
        }
    }
}
