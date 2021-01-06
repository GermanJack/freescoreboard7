using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FreeScoreBoard.Core.Display
{
	public class ClsCommon : IDisposable
	{
		/// <summary>
		/// Returns the list of file names in a specified folder.
		/// </summary>
		/// <param name="Folder">Folder</param>
		/// <param name="WithExt">return the file extentions or not</param>
		/// <returns></returns>
		public static string[] FileNames(string Folder, bool WithExt = true)
		{
			List<string> dl = new List<string>();
			string displayfolder = Path.Combine(ClsMain.AppFolder, ClsMain.WebFolder, Folder);

			foreach (string folder in Directory.GetDirectories(displayfolder))
			{
				foreach (string file in Directory.GetFiles(Path.Combine(displayfolder, folder)))
				{
					dl.Add(Path.GetFileName(folder) + "/" + Path.GetFileName(file));
				}
			}


			foreach (string file in Directory.GetFiles(displayfolder))
			{
				if (WithExt)
				{
					dl.Add(Path.GetFileName(file));
				}
				else
				{
					dl.Add(Path.GetFileNameWithoutExtension(file));
				}
			}

			return dl.ToArray();
		}

		public void Dispose()
		{
			//throw new NotImplementedException();
		}
	}
}
