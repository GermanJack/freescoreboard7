using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using FreeScoreBoard.Core.DB;
using FreeScoreBoard.Interpreter.Error;

namespace FreeScoreBoard.Core.DBControler
{
	public static class ClsControler
	{
		private static string Name = MethodBase.GetCurrentMethod().DeclaringType.Name;
		private static string myFilenameDB = "";
		private static string myFilepath = Path.Combine($@"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\freescoreboard7");

		public static void CheckDB()
		{
			myFilenameDB = Path.Combine($@"{myFilepath}\freescoreboard.sqlite");

			if (!File.Exists(myFilenameDB))
			{
				File.Copy(Path.Combine($@"{myFilepath}\master\FreeScoreBoard.sqlite"), myFilenameDB);
			}
		}

		public static void DBimport(string filter)
		{
			using (OpenFileDialog ofd = new OpenFileDialog
			{
				// ofd.Multiselect = true;
				Filter = filter,
				Multiselect = false
			})
			{
				if (ofd.ShowDialog() == DialogResult.OK)
				{
					foreach (string datei in ofd.FileNames)
					{
						Executesqlspript(datei);
					}
				}
			}
		}

		public static void Executesqlspript(string filename)
		{
			try
			{
				string script = string.Empty;
				if (File.Exists(filename))
				{
					script = File.ReadAllText(filename);

					// split script on GO command
					IEnumerable<string> commandStrings = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

					using (fsbDB FSBDB = new fsbDB())
					{
						foreach (string commandString in commandStrings)
						{
							if (commandString.Trim() != string.Empty)
							{
								int a = commandString.IndexOf("Anzeigeobjekt", StringComparison.Ordinal);
								FSBDB.Database.ExecuteSqlCommand(commandString);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		public static string Makesqlscript<T>() where T : class
		{
			StringBuilder sql = new StringBuilder();

			// sql.Append("SET IDENTITY_INSERT [Options1] ON;\r\n;\r\nGO\r\n");
			// Feldeigenschaften lesen
			Type myclass = typeof(T);
			List<PropertyInfo> myprop = myclass.GetProperties().ToList();

			// Key Felder ermitteln
			ObjectContext objectContext;
			using (fsbDB FSBDB = new fsbDB())
			{
				objectContext = ((IObjectContextAdapter)FSBDB).ObjectContext;
			}

			ObjectSet<T> set = objectContext.CreateObjectSet<T>();
			IEnumerable<string> keyNames = set.EntitySet.ElementType.KeyMembers.Select(k => k.Name);

			// erster Teildes scripts erstellen
			StringBuilder sql1 = new StringBuilder();

			sql1.Append("INSERT INTO [");
			sql1.Append(myprop[0].ReflectedType.Name);
			sql1.Append("] (");

			// feldnames hinzufügen
			foreach (PropertyInfo prop in myprop)
			{
				foreach (string s in keyNames)
				{
					if (s != prop.Name)
					{
						sql1.Append("[" + prop.Name + "],");
					}
				}
			}

			sql1.Remove(sql1.Length - 1, 1);  // letztes Komma entfernen
			sql1.Append(") VALUES (");

			// zweiter Teil (Werte) des scripts erstellen
			DbSet<T> table;
			using (fsbDB FSBDB = new fsbDB())
			{
				table = FSBDB.Set<T>();

				var lm = from m in table select m;
				foreach (var m in lm)
				{
					sql.Append(sql1.ToString());

					// werte hinzufügen
					foreach (var prop in myprop)
					{
						foreach (string s in keyNames)
						{
							if (s != prop.Name)
							{
								string fieldValue = "NULL";

								if (prop.GetValue(m, null) != null)
								{
									fieldValue = prop.GetValue(m, null).ToString();
									switch (prop.PropertyType.Name)
									{
										case "Int32":
											{
												sql.Append(fieldValue);
												break;
											}

										case "Decimal":
											{
												sql.Append("CAST(" + fieldValue.Replace(',', '.') + " AS Decimal(18, 2))");
												break;
											}

										default:
											{
												sql.Append("N'" + fieldValue.Replace("'", "''") + "'");
												break;
											}
									}
								}
								else
								{
									sql.Append(fieldValue); // if NULL
								}

								sql.Append(",");
							}
						}
					}

					sql.Remove(sql.Length - 1, 1);  // letztes Komma entfernen
					sql.Append(");\r\n");
				}
			}

			// System.Windows.Forms.Clipboard.SetText(sql.ToString());
			return sql.ToString();
		}

		public static string Makesqlscript(string Table)
		{
			StringBuilder sql = new StringBuilder();

			Type myclass = Assembly.GetExecutingAssembly()
						.GetTypes()
						.FirstOrDefault(t => t.Name == Table);

			// sql.Append("SET IDENTITY_INSERT [Options1] ON;\r\n;\r\nGO\r\n");
			// Feldeigenschaften lesen
			///Type myclass = typeof(T);
			List<PropertyInfo> myprop = myclass.GetProperties().ToList();

			// erster Teildes scripts erstellen
			StringBuilder sql1 = new StringBuilder();

			sql1.Append("INSERT INTO [");
			sql1.Append(myprop[0].ReflectedType.Name);
			sql1.Append("] (");

			// feldnames hinzufügen
			foreach (PropertyInfo prop in myprop)
			{
				if (!string.Equals(prop.Name, "ID", StringComparison.OrdinalIgnoreCase))
				{
					sql1.Append("[" + prop.Name + "],");
				}
			}

			sql1.Remove(sql1.Length - 1, 1);  // letztes Komma entfernen
			sql1.Append(") VALUES (");

			// zweiter Teil (Werte) des scripts erstellen
			DbSet table;
			PropertyInfo[] properties = typeof(fsbDB).GetProperties();
			var prope = properties.FirstOrDefault(p => p.Name == Table);

			using (fsbDB FSBDB = new fsbDB())
			{
				table = FSBDB.Set(Type.GetType(Table)); //FSBDB.Set<>();

				var lm = table.SqlQuery("select * from " + Table); // from m in table select m;
				var lm1 = prope?.GetValue(FSBDB);
				foreach (var m in lm)
				{
					sql.Append(sql1.ToString());

					// werte hinzufügen
					foreach (var prop in myprop)
					{
						if (!string.Equals(prop.Name, "ID", StringComparison.OrdinalIgnoreCase))
						{
							string fieldValue = "NULL";

							if (prop.GetValue(m, null) != null)
							{
								fieldValue = prop.GetValue(m, null).ToString();
								switch (prop.PropertyType.Name)
								{
									case "Int32":
										{
											sql.Append(fieldValue);
											break;
										}

									case "Decimal":
										{
											sql.Append("CAST(" + fieldValue.Replace(',', '.') + " AS Decimal(18, 2))");
											break;
										}

									default:
										{
											sql.Append("N'" + fieldValue.Replace("'", "''") + "'");
											break;
										}
								}
							}
							else
							{
								sql.Append(fieldValue); // if NULL
							}

							sql.Append(",");
						}
					}

					sql.Remove(sql.Length - 1, 1);  // letztes Komma entfernen
					sql.Append(");\r\n");
				}
			}

			// System.Windows.Forms.Clipboard.SetText(sql.ToString());
			return sql.ToString();
		}
	}
}