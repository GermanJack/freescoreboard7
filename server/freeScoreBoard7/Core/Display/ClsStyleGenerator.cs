using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace FreeScoreBoard.Core.Display
{
	public class ClsStyleGenerator : IDisposable
	{
		private readonly Dictionary<string, string> styleDB;

		public ClsStyleGenerator()
		{
			this.styleDB = new Dictionary<string, string>();
		}

		public ClsStyleGenerator(string StyleString)
		{
			this.styleDB = new Dictionary<string, string>();
			this.ParseStyleString(StyleString);
		}

		public void Clear()
		{
			this.styleDB.Clear();
		}

		public bool ContainsStyle(string name)
		{
			return this.styleDB.ContainsKey(name);
		}

		public void Dispose()
		{
			//throw new NotImplementedException();
		}

		public string GetStyle(string name)
		{
			if (!(name.Length > 0))
			{
				throw new ArgumentException("Parameter name cannot be zero-length.");
			}

			if (this.styleDB.ContainsKey(name))
			{
				return this.styleDB[name];
			}
			else
			{
				return string.Empty;
			}
		}

		public string GetStyleString()
		{
			if (this.styleDB.Count > 0)
			{
				StringBuilder styleString = new StringBuilder("");
				foreach (string key in this.styleDB.Keys)
				{
					styleString.Append(string.Format("{0}:{1};", (object)key, (object)this.styleDB[key]));
				}

				return styleString.ToString();
			}
			else
			{
				return string.Empty;
			}
		}

		public string GetStyleStringJson()
		{
			if (this.styleDB.Count > 0)
			{
				return new JavaScriptSerializer().Serialize(this.styleDB);
			}
			else
			{
				return string.Empty;
			}
		}

		public void ParseStyleString(string styles)
		{
			if (styles.Length > 0)
			{
				string[] stylePairs = styles.Split(new char[] { ';' }).Select(prop => prop.Trim()).ToArray();
				foreach (string stylePair in stylePairs)
				{
					if (stylePairs.Length > 0)
					{
						string[] styleNameValue = stylePair.Split(new char[] { ':' }).Select(prop => prop.Trim()).ToArray();
						if (styleNameValue.Length == 2)
						{
							this.styleDB[styleNameValue[0]] = styleNameValue[1];
						}
					}
				}
			}
		}

		public void ParseStyleStringJson(string stylesJson)
		{
			Dictionary<string, string> styleDB1 = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(stylesJson);

			foreach (var stylePair in styleDB1)
			{
				this.styleDB.Add(stylePair.Key, stylePair.Value);
			}
		}

		public void RemoveStyle(string name)
		{
			if (this.styleDB.ContainsKey(name))
			{
				this.styleDB.Remove(name);
			}
		}

		public string SetStyle(string name, string value)
		{
			string oldValue = "";

			if (!(name.Length > 0))
			{
				throw new ArgumentException("Parameter name cannot be zero-length.");
			}

			if (!(value.Length > 0))
			{
				this.styleDB.Remove(name);
				//throw new ArgumentException("Parameter value cannot be zero-length.");
			}

			if (this.styleDB.ContainsKey(name))
			{
				oldValue = this.styleDB[name];
			}

			this.styleDB[name] = value;

			return oldValue;
		}
	}
}
