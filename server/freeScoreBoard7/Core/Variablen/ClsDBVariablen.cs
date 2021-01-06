using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using FreeScoreBoard.Classes;
using FreeScoreBoard.Core.DB;
using FreeScoreBoard.Core.DBControler;
using FreeScoreBoard.Interpreter;
using FreeScoreBoard.Interpreter.Error;
using FreeScoreBoard.Server;

namespace FreeScoreBoard.Core.Variablen
{
	public sealed class ClsDBVariablen
	{
		private static readonly System.Lazy<ClsDBVariablen> Lazy = new System.Lazy<ClsDBVariablen>(() => new ClsDBVariablen());

		public static ClsDBVariablen Instance
		{
			get { return Lazy.Value; }
		}

		private readonly string Name = MethodBase.GetCurrentMethod().DeclaringType.Name;

		private List<ClsTextVariabeln> TextVariabelListe;
		private List<ClsBildVariabeln> BildVariabelListe;
		private List<ClsTabellenVariabeln> TabelleVariabelListe;

		//public event PropChangedEventHandler TextPropChanged;
		//public event PropChangedEventHandler BildPropChanged;
		//public event TPropChangedEventHandler TabellenPropChanged;

		public void Ini()
		{
			// Debugmodus
			string modus = System.Configuration.ConfigurationManager.AppSettings["Modus"];
			List<string> filter = new List<string>
			{
				"True"
			};
			if (modus != "")
			{
				filter.Add("Test");
			}

			// TextVariablen
			this.TextVariabelListe = new List<ClsTextVariabeln>();

			List<DB.Variablen> varlst1 = ClsVariablenControler.Variablen("S");

			foreach (DB.Variablen v in varlst1)
			{
				// suchen ob schon da
				int x = (from y in this.TextVariabelListe where y.ID == v.Name select y).Count();
				if (x > 0)
				{
					break;
				}

				// debugmode prüfen
				if (filter.Contains(v.Aktiv))
				{
					ClsTextVariabeln tv = new ClsTextVariabeln();
					//tv.ID = v.ID;
					tv.ID = v.Name;
					tv.Wert = v.Default;
					tv.Default = v.Default;

					this.TextVariabelListe.Add(tv);
				}
			}

			// Bildvariablen
			this.BildVariabelListe = new List<ClsBildVariabeln>();

			List<DB.Variablen> varlst2 = ClsVariablenControler.Variablen("B");

			foreach (DB.Variablen v in varlst2)
			{
				// suchen ob schon da
				int x = (from y in this.BildVariabelListe where y.ID == v.Name select y).Count();
				if (x > 0)
				{
					break;
				}

				// debugmode prüfen
				if (filter.Contains(v.Aktiv))
				{
					ClsBildVariabeln bv = new ClsBildVariabeln();
					bv.ID = v.Name;
					bv.Wert = v.Default;

					this.BildVariabelListe.Add(bv);
				}
			}

			this.SetPaintballPictures();
			string prosession = ClsOptionsControler.Options3("PossessionBild").Value;
			ClsDBVariablen.Instance.SetBildVariableWert("B15", prosession);

			// TabellenVariablen
			this.TabelleVariabelListe = new List<ClsTabellenVariabeln>();

			List<DB.Variablen> varlst3 = ClsVariablenControler.Variablen("T");

			foreach (DB.Variablen v in varlst3)
			{
				// suchen ob schon da
				int x = (from y in this.TabelleVariabelListe where y.ID == v.Name select y).Count();
				if (x > 0)
				{
					break;
				}

				// debugmode prüfen
				if (filter.Contains(v.Aktiv))
				{
					ClsTabellenVariabeln bv = new ClsTabellenVariabeln();
					bv.ID = v.Name;
					bv.RecPerPage = Convert.ToInt32(v.Default);

					this.TabelleVariabelListe.Add(bv);
				}
			}

			// Anzeige Texte eintragen
			this.SetText();
		}

		public void SetText()
		{
			try
			{
				// TextVariablen
				foreach (ClsTextVariabeln v in this.TextVariabelListe)
				{
					v.Variable = ClsTextControler.TextByNameAndNumber("TextVariable", v.ID);
				}

				this.TextVariabelListe.Sort((s1, s2) => string.Compare(s1.Variable, s2.Variable));

				// BildVariablen
				foreach (ClsBildVariabeln v in this.BildVariabelListe)
				{
					v.Variable = ClsTextControler.TextByNameAndNumber("PictureVariable", v.ID);
				}

				this.BildVariabelListe.Sort((s1, s2) => string.Compare(s1.Variable, s2.Variable));

				// TabellenVariablen
				foreach (ClsTabellenVariabeln v in this.TabelleVariabelListe)
				{
					v.Variable = ClsTextControler.TextByNameAndNumber("TableVariable", v.ID);
				}

				this.TabelleVariabelListe.Sort((s1, s2) => string.Compare(s1.Variable, s2.Variable));
			}
			catch (Exception ex)
			{
				ClsError.Error(this.Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		public void SetTextVariableWert(string Variable, string Text)
		{
			try
			{
				ClsTextVariabeln v = (from x in this.TextVariabelListe where x.ID == Variable select x).FirstOrDefault();
				if (v != null) // && v.Wert != Text)
				{
					v.Wert = Text;

					// send to HTML page
					GlobalServerEvents.SendMessage("server", new ClsStringEventArgs(ClsVarCom.UpdateDivContentStr(Variable, v.Wert)));
				}
			}
			catch (Exception ex)
			{
				ClsError.Error(this.Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		public void SetBildVariableWert(string Variable, string BildPfad)
		{
			try
			{
				ClsBildVariabeln v = (from x in this.BildVariabelListe where x.ID == Variable select x).FirstOrDefault();
				if (v != null)
				{
					v.Wert = BildPfad;

					// send to HTML page
					GlobalServerEvents.SendMessage("server", new ClsStringEventArgs(ClsVarCom.UpdateDivContentPic(Variable, v.Wert)));
				}
			}
			catch (Exception ex)
			{
				ClsError.Error(this.Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		public void SetTabellenVariableWert(string Variable, string Tabelle)
		{
			try
			{
				ClsTabellenVariabeln v = (from x in this.TabelleVariabelListe where x.ID == Variable select x).FirstOrDefault();
				if (v != null)
				{
					v.Wert = Tabelle;

					// send to HTML page
					GlobalServerEvents.SendMessage("server", new ClsStringEventArgs(ClsVarCom.UpdateDivContentTab(Variable, v.Wert)));
				}
			}
			catch (Exception ex)
			{
				ClsError.Error(this.Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		public void SetTabellenVariableRecPerPage(string Variable, int RecPerPage)
		{
			try
			{
				ClsVariablenControler.SaveDefault(Variable, RecPerPage.ToString());
				ClsTabellenVariabeln v = (from x in this.TabelleVariabelListe where x.ID == Variable select x).FirstOrDefault();
				if (v != null)
				{
					v.RecPerPage = RecPerPage;
				}
			}
			catch (Exception ex)
			{
				ClsError.Error(this.Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		public void SetTabellenVariableAmountOfPages(string Variable, decimal AmountOfPages)
		{
			try
			{
				ClsTabellenVariabeln v = (from x in this.TabelleVariabelListe where x.ID == Variable select x).FirstOrDefault();
				if (v != null)
				{
					v.AmountOfPages = (int)AmountOfPages;
				}
			}
			catch (Exception ex)
			{
				ClsError.Error(this.Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		public string GetTextVariableWert(string ID)
		{
			try
			{
				if (!string.IsNullOrEmpty(ID))
				{
					ClsTextVariabeln v = (from x in this.TextVariabelListe where x.ID == ID select x).FirstOrDefault();
					if (v != null)
					{
						return v.Wert;
					}
				}

				return "";
			}
			catch (Exception ex)
			{
				ClsError.Error(this.Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return "";
			}
		}

		public ClsTextVariabeln GetTextVariable(string ID)
		{
			try
			{
				if (!string.IsNullOrEmpty(ID))
				{
					ClsTextVariabeln v = (from x in this.TextVariabelListe where x.ID == ID select x).FirstOrDefault();
					if (v != null)
					{
						return v;
					}
				}

				return new ClsTextVariabeln();
			}
			catch (Exception ex)
			{
				ClsError.Error(this.Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new ClsTextVariabeln();
			}
		}

		public string GetBildVariableWert(string ID)
		{
			try
			{
				if (!string.IsNullOrEmpty(ID))
				{
					ClsBildVariabeln v = (from x in this.BildVariabelListe where x.ID == ID select x).FirstOrDefault();
					if (v != null)
					{
						return v.Wert;
					}
				}

				return "";
			}
			catch (Exception ex)
			{
				ClsError.Error(this.Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return "";
			}
		}

		public ClsBildVariabeln GetBildVariable(string ID)
		{
			try
			{
				if (!string.IsNullOrEmpty(ID))
				{
					ClsBildVariabeln v = (from x in this.BildVariabelListe where x.ID == ID select x).FirstOrDefault();
					if (v != null)
					{
						return v;
					}
				}

				return new ClsBildVariabeln();
			}
			catch (Exception ex)
			{
				ClsError.Error(this.Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new ClsBildVariabeln();
			}
		}

		public string GetTabellenVariableWert(string ID)
		{
			try
			{
				if (!string.IsNullOrEmpty(ID))
				{
					ClsTabellenVariabeln v = (from x in this.TabelleVariabelListe where x.ID == ID select x).Single();
					if (v != null)
					{
						return v.Wert;
					}
				}

				return null;
			}
			catch (Exception ex)
			{
				ClsError.Error(this.Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return null;
			}
		}

		public ClsTabellenVariabeln GetTabellenVariable(string ID)
		{
			try
			{
				if (!string.IsNullOrEmpty(ID))
				{
					ClsTabellenVariabeln v = (from x in this.TabelleVariabelListe where x.ID == ID select x).Single();
					if (v != null)
					{
						return v;
					}
				}

				return new ClsTabellenVariabeln();
			}
			catch (Exception ex)
			{
				ClsError.Error(this.Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return new ClsTabellenVariabeln();
			}
		}

		public void SetPaintballPictures()
		{
			string b1 = ClsOptionsControler.Options3("Paintball Bild1").Value;
			string b2 = ClsOptionsControler.Options3("Paintball Bild2").Value;

			// Team1
			int numwert = Convert.ToInt32(this.GetTextVariableWert("S46"));
			int vn1 = 17;
			for (int i = 4; i >= 0; i--)
			{
				string v = "B" + (vn1 + i);
				if (i + 1 > numwert)
				{
					this.SetBildVariableWert(v, b2);
				}
				else
				{
					this.SetBildVariableWert(v, b1);
				}
			}

			// Team2
			numwert = Convert.ToInt32(this.GetTextVariableWert("S47"));
			int vn2 = 22;
			for (int i = 4; i >= 0; i--)
			{
				string v = "B" + (vn2 + i);
				if (i + 1 > numwert)
				{
					this.SetBildVariableWert(v, b2);
				}
				else
				{
					this.SetBildVariableWert(v, b1);
				}
			}
		}

		public void SetSpielrichtung(string r)
		{
			string bl = ClsOptionsControler.Options3("SpielrichtungLinks").Value;
			string br = ClsOptionsControler.Options3("SpielrichtungRechts").Value;

			if (r == "l")
			{
				ClsDBVariablen.Instance.SetBildVariableWert("B27", bl);
				ClsDBVariablen.Instance.SetBildVariableWert("B28", "");
			}

			if (r == "x")
			{
				ClsDBVariablen.Instance.SetBildVariableWert("B27", "");
				ClsDBVariablen.Instance.SetBildVariableWert("B28", "");
			}

			if (r == "r")
			{
				ClsDBVariablen.Instance.SetBildVariableWert("B27", "");
				ClsDBVariablen.Instance.SetBildVariableWert("B28", br);
			}
		}

		public List<ClsTextVariabeln> GetAllTextVariablen()
		{
			return this.TextVariabelListe.OrderBy(o => o.ID).ToList();
		}

		public List<ClsBildVariabeln> GetAllBildVariablen()
		{
			return this.BildVariabelListe.OrderBy(o => o.ID).ToList();
		}

		public List<ClsTabellenVariabeln> GetAllTabellenVariablen()
		{
			return this.TabelleVariabelListe.OrderBy(o => o.ID).ToList();
		}

		public void SetVariablenDefault(string Variable, string Wert)
		{
			// in DB speichern
			ClsVariablenControler.SaveDefault(Variable, Wert);

			// liste updaten
			ClsTextVariabeln v = (from x in this.TextVariabelListe where x.ID == Variable select x).FirstOrDefault();
			v.Default = Wert;
		}
	}
}
