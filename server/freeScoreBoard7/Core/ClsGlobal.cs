using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using FreeScoreBoard.Classes;
using FreeScoreBoard.Core.DB;
using FreeScoreBoard.Core.Kontrolle;
using FreeScoreBoard.Core.Objekte;
using FreeScoreBoard.Core.Turnier;
using FreeScoreBoard.Core.Variablen;
using FreeScoreBoard.Interpreter.Error;

namespace FreeScoreBoard.Core
{
	public sealed class ClsGlobal
	{
		private static readonly System.Lazy<ClsGlobal> Lazy = new System.Lazy<ClsGlobal>(() => new ClsGlobal());

		public static ClsGlobal Instance
		{
			get { return Lazy.Value; }
		}

		private readonly string Name = MethodBase.GetCurrentMethod().DeclaringType.Name;

		private int myTurnierID;

		private TSpiele myaktivesSpiel;
		private TSpiele mynextSpiel;

		public List<ClsTableFilter> TableFilterList { set; get; }

		public List<ClsZeitstrafe> Strafen1 { set; get; } = new List<ClsZeitstrafe>();
		public List<ClsZeitstrafe> Strafen2 { set; get; } = new List<ClsZeitstrafe>();

		public string MansschaftAID { get; set; }
		public string MansschaftBID { get; set; }
		public string SpielerID { get; set; }

		public int ActivePage { get; set; }

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
					//this.myTurnierID = value;

					// Wenn anders Turnier oder freies Spielen
					if (this.myTurnierID != value || value == 0)
					{
						int ret = 0;
						// Wenn anderes Turnier
						if (value != 0)
						{
							ret = ClsTurnierfunktionen.LoadTurnier(value);

							if (ret == 0)
							{
								this.myTurnierID = value;
							}
						}

						if (value == 0)
						{
							this.myTurnierID = value;

							ret = ClsTurnierfunktionen.LoadTurnier(value);
						}
					}
				}
				catch (Exception ex)
				{
					ClsError.Error(this.Name, MethodBase.GetCurrentMethod().ToString(), ex);
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
						ClsSpielfunktionen.SetManName("A", "");
						ClsSpielfunktionen.SetManName("B", "");
						ClsDBVariablen.Instance.SetTextVariableWert("S05", "0"); // Tore1
						ClsDBVariablen.Instance.SetTextVariableWert("S06", "0"); // Tore2
						ClsDBVariablen.Instance.SetTextVariableWert("S15", " - "); // Begegnung
						ClsDBVariablen.Instance.SetTextVariableWert("S17", "0"); // Runde
						ClsDBVariablen.Instance.SetTextVariableWert("S29", "0"); // SpielNr
						ClsDBVariablen.Instance.SetTextVariableWert("S30", "0"); // Gruppe
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

					ClsStringEventArgs e1 = new ClsStringEventArgs(null);
					this.SpielwechselEventAusloesen((object)this, e1);
				}
				catch (Exception ex)
				{
					ClsError.Error(this.Name, MethodBase.GetCurrentMethod().ToString(), ex);
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

				if (this.mynextSpiel == null)
				{
					ClsDBVariablen.Instance.SetTextVariableWert("S32", ""); // LangnameMannschaft1_next
					ClsDBVariablen.Instance.SetTextVariableWert("S33", ""); // LangnameMannschaft2_next
					ClsDBVariablen.Instance.SetTextVariableWert("S16", " - "); // Begegnung_next
					ClsDBVariablen.Instance.SetTextVariableWert("S48", "0"); // Runde_next
					ClsDBVariablen.Instance.SetTextVariableWert("S50", "0"); // SpielNr_next
					ClsDBVariablen.Instance.SetTextVariableWert("S49", "0"); // Gruppe_next
				}
				else
				{
					ClsDBVariablen.Instance.SetTextVariableWert("S32", value.IstMannA); // LangnameMannschaft1_next
					ClsDBVariablen.Instance.SetTextVariableWert("S33", value.IstMannB); // LangnameMannschaft2_next
					ClsDBVariablen.Instance.SetTextVariableWert("S16", value.IstMannA + " - " + value.IstMannB);
					ClsDBVariablen.Instance.SetTextVariableWert("S48", value.Runde.ToString());
					ClsDBVariablen.Instance.SetTextVariableWert("S50", value.Spiel.ToString());
					ClsDBVariablen.Instance.SetTextVariableWert("S49", value.Gruppe);
				}

				ClsStringEventArgs e1 = new ClsStringEventArgs(null);
				this.NextSpielwechselEventAusloesen((object)this, e1);
			}

			get
			{
				return this.mynextSpiel;
			}
		}

		//// event Spielwechsel start-------------------------
		private void SpielwechselEventAusloesen(object o, ClsStringEventArgs e)
		{
			//this.Spielwechsel?.Invoke(o, e);
		}

		//public event EventHandler<ClsStringEventArgs> Spielwechsel;
		//// event Spielwechsel stop-------------------------------

		//// event NextSpielwechsel start-------------------------
		private void NextSpielwechselEventAusloesen(object o, ClsStringEventArgs e)
		{
			//this.NextSpielwechsel?.Invoke(o, e);
		}

		//public event EventHandler<ClsStringEventArgs> NextSpielwechsel;
		//// event NextSpielwechsel stop-------------------------------

	}
}
