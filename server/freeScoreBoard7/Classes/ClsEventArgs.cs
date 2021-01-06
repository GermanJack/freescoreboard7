using System;

namespace FreeScoreBoard.Classes
{
    public delegate void OwnStringEventHandler(object sender, ClsStringEventArgs e);
    public delegate void OwnObjectEventHandler(object sender, ClsObjectEventArgs e);
    // public delegate void OwnArrayEventHandler(object sender, ClsArrayEventArgs e);

    public class ClsStringEventArgs : EventArgs
    {
        public readonly string Argument;

        public ClsStringEventArgs(string wert)
        {
            this.Argument = wert;
        }
    }

    public class ClsObjectEventArgs : EventArgs
    {
        public readonly object ObjectArgument;

        public ClsObjectEventArgs(object wert)
        {
            this.ObjectArgument = wert;
        }
    }

    //public class ClsArrayEventArgs : EventArgs
    //{
    //    public readonly string[] WertListe;

    //    public ClsArrayEventArgs(string[] WertListe)
    //    {
    //        this.WertListe = WertListe;
    //    }
    //}
}
