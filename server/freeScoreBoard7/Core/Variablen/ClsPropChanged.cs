using System;

namespace FreeScoreBoard.Core.Variablen
{
    public delegate void PropChangedEventHandler(object sender, VariableEventArgs e);
    
    public class VariableEventArgs : EventArgs
    {
        public readonly string Variablename;
        public readonly string Variablenwert;

        public VariableEventArgs(string name, string wert)
        {
            this.Variablename = name;
            this.Variablenwert = wert;
        }
    }
}
