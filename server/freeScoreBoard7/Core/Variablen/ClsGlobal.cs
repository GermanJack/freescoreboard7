using FreeScoreBoard.Core.DB;
using FreeScoreBoard.Interpreter.Error;
using System;
using System.ComponentModel;
using System.Reflection;

namespace FreeScoreBoard.Core.Variablen
{
    public sealed class ClsGlobal
    {
        private static readonly System.Lazy<ClsGlobal> Lazy =
         new System.Lazy<ClsGlobal>(() => new ClsGlobal());

        public static ClsGlobal Instance
        {
            get { return Lazy.Value; }
        }

        private string klasse = MethodBase.GetCurrentMethod().DeclaringType.Name;

        private FilterE myfilterE = new FilterE();
        private FilterS myfilterS = new FilterS();
        private FilterT myfilterT = new FilterT();

        private int myTurniernr;
        private int myTurnierID;

        private TSpiele myaktivesSpiel;
        private TSpiele mynextSpiel;

        [Description("Ereignisfilter")]
        public FilterE FilterE
        {
            set
            {
                this.myfilterE = value;
            }

            get
            {
                return this.myfilterE;
            }
        }

        [Description("Spielplanfilter")]
        public FilterS FilterS
        {
            set
            {
                this.myfilterS = value;
            }

            get
            {
                return this.myfilterS;
            }
        }

        [Description("Tabellenfilter")]
        public FilterT FilterT
        {
            set
            {
                this.myfilterT = value;
            }

            get
            {
                return this.myfilterT;
            }
        }

        public int TurnierNr
        {
            get
            {
                return this.myTurniernr;
            }

            set
            {
                try
                {
                    //if (this.myTurniernr != value || value == 0)
                    //{
                    //    int ret = 0;
                    //    if (value != 0)
                    //    {
                    //        using (ClsKontrolfunktionen kfunktionen = new ClsKontrolfunktionen())
                    //        {
                    //            ret = kfunktionen.LoadTurnier((int)value);
                    //        }

                    //        if (ret == 0)
                    //        {
                    //            this.myTurniernr = value;
                    //            ClsEventArgs e1 = new ClsEventArgs(this.TurnierNr.ToString());
                    //            this.TurnierwechselEventAusloesen((object)this, e1);
                    //        }
                    //    }

                    //    if (value == 0)
                    //    {
                            this.myTurniernr = value;

                    //        using (ClsKontrolfunktionen kfunktionen = new ClsKontrolfunktionen())
                    //        {
                    //            ret = kfunktionen.LoadTurnier((int)value);
                    //        }

                    //        ClsEventArgs e1 = new ClsEventArgs(this.TurnierNr.ToString());
                    //        this.TurnierwechselEventAusloesen((object)this, e1);
                    //    }
                    //}
                }
                catch (Exception ex)
                {
                    ClsError.Fehler(this.klasse, MethodBase.GetCurrentMethod().ToString(), ex);
                }
            }
        }

        public int TurnierID
        {
            get
            {
                return this.myTurnierID;
            }

            set
            {
                try
                {
                    if (this.myTurnierID != value || value == 0)
                    {
                        int ret = 0;
                        if (value != 0)
                        {
                            using (ClsKontrolfunktionen kfunktionen = new ClsKontrolfunktionen())
                            {
                                ret = kfunktionen.LoadTurnier((int)value);
                            }

                            if (ret == 0)
                            {
                                this.myTurnierID = value;
                                ClsEventArgs e1 = new ClsEventArgs(this.TurnierID.ToString());
                                this.TurnierwechselEventAusloesen((object)this, e1);
                            }
                        }

                        if (value == 0)
                        {
                            this.myTurnierID = value;

                            using (ClsKontrolfunktionen kfunktionen = new ClsKontrolfunktionen())
                            {
                                ret = kfunktionen.LoadTurnier((int)value);
                            }

                            ClsEventArgs e1 = new ClsEventArgs(this.TurnierID.ToString());
                            this.TurnierwechselEventAusloesen((object)this, e1);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ClsError.Fehler(this.klasse, MethodBase.GetCurrentMethod().ToString(), ex);
                }
            }
        }

        public TSpiele AktivesSpiel
        {
            set
            {
                try
                {
                    this.myaktivesSpiel = value;

                    if (this.myaktivesSpiel == null)
                    {
                        ClsDBVariablen.Instance.SetTextVariableWert("S05", "0");
                        ClsDBVariablen.Instance.SetTextVariableWert("S06", "0");
                        ClsDBVariablen.Instance.SetTextVariableWert("S15", " - ");
                        ClsDBVariablen.Instance.SetTextVariableWert("S17", "0");
                        ClsDBVariablen.Instance.SetTextVariableWert("S29", "0");
                        ClsDBVariablen.Instance.SetTextVariableWert("S30", "0");
                    }
                    else
                    {
                        ClsDBVariablen.Instance.SetTextVariableWert("S05", value.ToreA.ToString());
                        ClsDBVariablen.Instance.SetTextVariableWert("S06", value.ToreB.ToString());
                        ClsDBVariablen.Instance.SetTextVariableWert("S15", value.IstMannA + " - " + value.IstMannB);
                        ClsDBVariablen.Instance.SetTextVariableWert("S17", value.Runde.ToString());
                        ClsDBVariablen.Instance.SetTextVariableWert("S29", value.Spiel.ToString());
                        ClsDBVariablen.Instance.SetTextVariableWert("S30", value.Gruppe);
                    }

                    ClsEventArgs e1 = new ClsEventArgs(null);
                    this.SpielwechselEventAusloesen((object)this, e1);
                }
                catch (Exception ex)
                {
                    ClsError.Fehler(this.klasse, MethodBase.GetCurrentMethod().ToString(), ex);
                }
            }

            get
            {
                return this.myaktivesSpiel;
            }
        }

        public TSpiele NextSpiel
        {
            set
            {
                this.mynextSpiel = value;
                ClsEventArgs e1 = new ClsEventArgs(null);
                this.NextSpielwechselEventAusloesen((object)this, e1);
            }

            get
            {
                return this.mynextSpiel;
            }
        }

        //// event Turnierwechsel start-------------------------
        private void TurnierwechselEventAusloesen(object o, ClsEventArgs e)
        {
            if (this.Turnierwechsel != null)
            {
                this.Turnierwechsel(o, e);
            }
        }

        public event OwnEventHandler Turnierwechsel;
        //// event Turnierwechsel stop-------------------------------

        //// event Spielwechsel start-------------------------
        private void SpielwechselEventAusloesen(object o, ClsEventArgs e)
        {
            if (this.Spielwechsel != null)
            {
                this.Spielwechsel(o, e);
            }
        }

        public event OwnEventHandler Spielwechsel;
        //// event Spielwechsel stop-------------------------------

        //// event NextSpielwechsel start-------------------------
        private void NextSpielwechselEventAusloesen(object o, ClsEventArgs e)
        {
            if (this.NextSpielwechsel != null)
            {
                this.NextSpielwechsel(o, e);
            }
        }

        public event OwnEventHandler NextSpielwechsel;
        //// event NextSpielwechsel stop-------------------------------
    }
}
