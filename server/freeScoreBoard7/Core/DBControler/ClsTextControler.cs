using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FreeScoreBoard.Core.DB;
using FreeScoreBoard.Interpreter.Error;

namespace FreeScoreBoard.Core.DBControler
{
	public static class ClsTextControler
	{
		public static string TextByKey(string Key, string Langu = "DE")
		{
			string ret = "";

			using (fsbDB FSBDB = new fsbDB())
			{
				ret = (from s in FSBDB.A_Text
					   where s.Key == Key && s.Language == Langu
					   select s.Text).FirstOrDefault();
			}

			return ret;
		}

		public static string TextByNameAndNumber(string KeyName, string KeyNumber, string Langu = "DE")
		{
			string ret = "";

			using (fsbDB FSBDB = new fsbDB())
			{
				ret = (from s in FSBDB.A_Text
					   where s.Key == KeyName + "_" + KeyNumber && s.Language == Langu
					   select s.Text).FirstOrDefault();
			}

			return ret;
		}

	}
}