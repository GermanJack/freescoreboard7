using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using FreeScoreBoard.Core;
using FreeScoreBoard.Core.DB;
using FreeScoreBoard.Core.DBControler;
//using FreeScoreBoard.Core.Localisation;
using FreeScoreBoard.Core.Objekte;
using FreeScoreBoard.Core.Variablen;
using FreeScoreBoard.Forms;
using FreeScoreBoard.Interpreter.Error;

namespace FreeScoreBoard.Core
{
	public static class ClsFunktionen
	{
		private static string Name = MethodBase.GetCurrentMethod().DeclaringType.Name;

		public static string Setspielstand()
		{
			// Spielstand sammelt Mannschaften und Tore in der StringVariable Spielstand
			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append(ClsDBVariablen.Instance.GetTextVariableWert("S05"));
				sb.Append(" ");
				sb.Append(ClsDBVariablen.Instance.GetTextVariableWert("S07"));
				sb.Append(" ");
				sb.Append(ClsDBVariablen.Instance.GetTextVariableWert("S06"));

				return sb.ToString();
				//return ClsVariablen.Instance.S05 + " " + ClsVariablen.Instance.S07 + " " + ClsVariablen.Instance.S06;
			}
			catch (Exception ex)
			{
				ClsError.Error(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return string.Empty;
			}
		}


		/// <summary>
		/// sortiert die tabelle
		/// </summary>
		/// <param name="tabelle"></param>
		/// <returns></returns>
		public static List<TTabellen> STTabellem(List<TTabellen> tabelle)
		{
			// Sortiertabelle lesen
			List<TabellenSort> sortExpressions = ClsOptionsControler.Tabellensortierung();

			IEnumerable<TTabellen> query = from item in tabelle select item;
			IOrderedEnumerable<TTabellen> orderedQuery = null;

			for (int i = 0; i < sortExpressions.Count; i++)
			{
				// We need to keep the loop index, not sure why it is altered by the Linq.
				int index = i;
				object expression(TTabellen item) => item.GetType()
								.GetProperty(sortExpressions[index].Feld)
								.GetValue(item, null);

				if (sortExpressions[index].absteigend == false)
				{
					orderedQuery = (index == 0) ? query.OrderBy(expression)
						: orderedQuery.ThenBy(expression);
				}
				else
				{
					orderedQuery = (index == 0) ? query.OrderByDescending(expression)
							: orderedQuery.ThenByDescending(expression);
				}
			}

			query = orderedQuery;

			return query.ToList();
		}



		//public static string Langu(string xmlfile)
		//{
		//	try
		//	{
		//		string l = "De";

		//		string xml = xmlfile;

		//		if (!File.Exists(xml))
		//		{
		//			string content = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<langu>\n\t<value>De</value>\n</langu>";
		//			Classes.TextDatei.WriteFile(xml, content);
		//		}

		//		XmlDocument doc = new XmlDocument();
		//		doc.Load(xml);
		//		XmlNode node = doc.DocumentElement.SelectSingleNode("/langu/value");
		//		if (node != null)
		//		{
		//			l = node.InnerText;

		//			if (l == string.Empty)
		//			{
		//				l = "De";
		//			}
		//		}

		//		return l;
		//	}
		//	catch (Exception)
		//	{
		//		return "De";
		//	}
		//}

		public static string Sek2uhr(decimal sekunden)
		{
			string sek2uhr = "99:99";
			try
			{
				string min;
				string sek;
				int minus = 0;

				if (sekunden < 0)
				{
					minus = 1;
					sekunden = System.Math.Abs(sekunden);
				}
				else
				{
					minus = 0;
				}

				min = Math.Truncate(sekunden / 60).ToString();
				if (min.Length == 1)
				{
					min = "0" + min;
				}

				sek = Math.Truncate(sekunden - (Convert.ToDecimal(min) * 60)).ToString();
				if (sek.Length == 1)
				{
					sek = "0" + sek;
				}

				sek2uhr = min + ":" + sek;

				if (minus == 1)
				{
					sek2uhr = "-" + sek2uhr;
				}

				return sek2uhr;
			}
			catch (Exception ex)
			{
				ClsError.Error(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return sek2uhr;
			}
		}

		public static string MakeSpielerName(int SpielerID)
		{
			Spieler s;
			s = ClsMannschaftenControler.Spieler(SpielerID);

			if (s == null)
			{
				return "";
			}

			StringBuilder sb = new StringBuilder();
			if (!string.IsNullOrEmpty(s.SID))
			{
				sb.Append(s.SID);
				sb.Append(" - ");
			}

			sb.Append(s.Vorname);
			sb.Append(" ");
			sb.Append(s.Nachname);

			return sb.ToString();
		}

		public static string MakeSpielerNameOhneSID(string SpielerID)
		{
			int sid = 0;
			try
			{
				sid = Convert.ToInt32(SpielerID);
			}
			catch
			{
				sid = 0;
			}

			Spieler s;
			s = ClsMannschaftenControler.Spieler(sid);

			if (s == null)
			{
				return "";
			}

			StringBuilder sb = new StringBuilder();
			sb.Append(s.Vorname);
			sb.Append(" ");
			sb.Append(s.Nachname);

			return sb.ToString();
		}

		public static int Uhr2sek(string uhr)
		{
			// Rechnet den angezeiget uhrenstring in einen interger wert um
			int uhr2sek = 0;
			try
			{
				int min;
				int sek;
				int minus = 0;

				if (uhr.Substring(0, 1) == "-")
				{
					minus = 1;
					uhr = uhr.Substring(1, uhr.Length - 1);
				}

				string[] uhrarr;
				uhrarr = uhr.Split(':');
				min = Convert.ToInt32(uhrarr[0]);
				sek = Convert.ToInt32(uhrarr[1]);

				uhr2sek = (min * 60) + sek;

				if (minus == 1)
				{
					uhr2sek *= -1;
				}

				return uhr2sek;
			}
			catch (Exception ex)
			{
				ClsError.Error(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return uhr2sek;
			}
		}

		public static void Versioncheck2(bool aktuelhinweis = true)
		{
			// in newVersion variable we will store the version info from xml file
			System.Version newVersion = null;
			string updateUrl = "";

			XmlDocument doc = new XmlDocument();
			try
			{
				// provide the XmlTextReader with the URL of our xml document
				string xmlURL = "http://web.FreeScoreBoard.org/download/version.xml";
				HttpWebRequest rq = (HttpWebRequest)WebRequest.Create(xmlURL);
				rq.Timeout = 5000;
				HttpWebResponse response = rq.GetResponse() as HttpWebResponse;

				using (Stream responseStream = response.GetResponseStream())
				{
					XmlTextReader reader = new XmlTextReader(responseStream);
					doc.Load(reader);
				}

				XmlElement root = doc.DocumentElement;

				foreach (XmlNode node in root.ChildNodes)
				{
					switch (node.Name)
					{
						case "version":
							// thats why we keep the version info
							// in xxx.xxx.xxx.xxx format
							// the Version class does the parsing for us
							newVersion = new System.Version(node.InnerText);
							break;
						case "url":
							updateUrl = node.InnerText;
							break;
					}
				}

				// get the running version
				System.Version curVersion = Assembly.GetExecutingAssembly().GetName().Version;
				// compare the versions
				if (curVersion.CompareTo(newVersion) < 0)
				{
					// ask the user if he would like to download the new version
					////if (ClsTranslateControls.AskMessage("M0011", "Versionscheck", new object[] { "\n", newVersion.ToString() }, MessageBoxButtons.YesNo) == DialogResult.Yes)
					{
						// navigate the default web browser to our app
						// homepage (the url comes from the xml content)
						System.Diagnostics.Process.Start(updateUrl);
					}
				}
				else
				{
					if (aktuelhinweis == true)
					{
						////ClsTranslateControls.ShowMessage("M0012", "Versionscheck", new object[] { "\n" }, MessageBoxButtons.OK);
						// MessageBox.Show("Die Software ist auf dem neusten Stand.", "Versionscheck...");
					}
				}
			}
			catch (Exception)
			{
				if (aktuelhinweis == true)
				{
					////ClsTranslateControls.ShowMessage("M0013", "Versionscheck", new object[] { "\n" }, MessageBoxButtons.OK);
					// MessageBox.Show("Bei der Versionsprüfung ist ein Fehler aufgetreten.\nPrüfen Sie Ihre Internetverbindung.", "Versionscheck...");
				}
			}
		}

		public static string MakeErrorMessage(string Modul, string Function, Exception Ex)
		{
			StringBuilder sb = new StringBuilder();


			string winbit = "32Bit";
			if (Environment.Is64BitOperatingSystem)
			{
				winbit = "64Bit";
			}

			sb.Append(Environment.OSVersion.VersionString).Append(" ").Append(winbit).Append("\r\n");
			sb.Append("netVersion").Append(": ").Append(Environment.Version).Append("\r\n");
			sb.Append("Programmversion").Append(": ").Append(Application.ProductVersion).Append("\r\n\r\n");
			sb.Append("Modul").Append(": ").Append(Modul).Append("\r\n");
			sb.Append("Funktion").Append(": ").Append(Function).Append("\r\n\r\n");
			sb.Append("Systemmeldung").Append(": ").Append(Ex.Message).Append("\r\n");

			if (Ex.InnerException != null)
			{
				sb.Append("\r\n").Append(Ex.InnerException.Message);
			}

			return sb.ToString();
		}


	}
}
