using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FreeScoreBoard.Core.DB;
using FreeScoreBoard.Core.DBControler;
using FreeScoreBoard.Core.Objekte;
using FreeScoreBoard.Core.Variablen;
using FreeScoreBoard.Interpreter.Error;

namespace FreeScoreBoard.Core.Turnier
{
	public static class ClsTurnierverwaltung
	{
		private readonly static string Name = MethodBase.GetCurrentMethod().DeclaringType.Name;

		public static void TurnierSpeichern(ClsTurnier Turnier)
		{
			try
			{
				int turnierID = ClsTurnierControler.AddTurnierKopf(Turnier.Kopf);

				for (int i = 0; i < Turnier.Runden.Count; i++)
				{
					TRunden t = Turnier.Runden[i];
					t.TurnierID = turnierID;
					if (t.Runde == 1)
					{
						t.status = 1;
					}
					ClsTurnierControler.AddRunden(t);
				}

				for (int i = 0; i < Turnier.Tabellen.Count; i++)
				{
					TTabellen et = Turnier.Tabellen[i];
					et.TurnierID = turnierID;
					ClsTurnierControler.AddTabellen(et);
				}

				for (int i = 0; i < Turnier.Spiele.Count; i++)
				{
					TSpiele er = Turnier.Spiele[i];
					er.TurnierID = turnierID;
					er.Datum = "";
					er.Uhrzeit = "";
					ClsTurnierControler.AddSpiele(er);
				}

				List<string> grp = (from x in Turnier.Tabellen select x.Gruppe).Distinct().ToList();
				for (int i = 0; i < grp.Count; i++)
				{
					TGruppen gr = new TGruppen();
					gr.TurnierID = turnierID;
					gr.TurnierNr = 0;
					gr.Gruppe = grp[i];
					gr.status = 0;
					gr.Runde = Convert.ToInt32(grp[i].Substring(0, grp[i].Length - 1));
					ClsTurnierControler.AddGruppen(gr);
				}
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		public static void TurnierLoeschen(int TurnierID)
		{
			try
			{
				ClsTurnierControler.DelTabellen(TurnierID);
				ClsTurnierControler.DelSpiele(TurnierID);
				ClsTurnierControler.DelGruppen(TurnierID);
				ClsTurnierControler.DelRunden(TurnierID);
				ClsEreignisControler.DelEreignisse(TurnierID);
				ClsTurnierControler.DelTurnierKopf(TurnierID);
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		public static void EreignisLoeschen(int[] IDs)
		{
			for (int i = 0; i < IDs.Count(); i += 1)
			{
				DBControler.ClsEreignisControler.DelEvent(IDs[i]);
			}

			ClsDBVariablen.Instance.SetTabellenVariableWert("T03", ClsTabellenfunktionen.EreignisseToVariable_Json(ClsGlobal.Instance.TurnierID));
		}


	}
}
