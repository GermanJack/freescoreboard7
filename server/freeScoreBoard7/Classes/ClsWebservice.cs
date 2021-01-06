using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web.Script.Serialization;
using FreeScoreBoard.Core.DB;

namespace FreeScoreBoard.Classes
{
	public class ClsWebservice
	{
		private readonly string Name = MethodBase.GetCurrentMethod().DeclaringType.Name;
		private Registrierung myRegisterData = new Registrierung();

		public static void Senden(Registrierung registerData)
		{
			try
			{
				var json = new JavaScriptSerializer().Serialize(registerData);

				// senden
				// Create a request using a URL that can receive a post. 
				WebRequest request = WebRequest.Create("http://FreeScoreBoard.org/webservice/create.php");

				// Set the Method property of the request to POST.
				request.Method = "POST";

				// Create POST data and convert it to a byte array.
				byte[] byteArray = Encoding.UTF8.GetBytes(json);

				// Set the ContentType property of the WebRequest.
				request.ContentType = "application/x-www-form-urlencoded";

				// Set the ContentLength property of the WebRequest.
				request.ContentLength = byteArray.Length;

				// Get the request stream.
				Stream dataStream = request.GetRequestStream();

				// Write the data to the request stream.
				dataStream.Write(byteArray, 0, byteArray.Length);

				// Close the Stream object.
				dataStream.Close();

				// Get the response.
				WebResponse response = request.GetResponse();

				// Display the status.
				// Console.WriteLine(((HttpWebResponse)response).StatusDescription);

				// Get the stream containing content returned by the server.
				dataStream = response.GetResponseStream();

				// Open the stream using a StreamReader for easy access.
				using (StreamReader reader = new StreamReader(dataStream))
				{
					// Read the content.
					string responseFromServer = reader.ReadToEnd();

					// Display the content.
					// Console.WriteLine(responseFromServer);
					// System.Windows.Forms.Clipboard.SetText(json + "\r\n\r\n" + responseFromServer);

					// Clean up the streams.
					reader.Close();
					dataStream.Close();
					response.Close();
				}
			}
			catch (Exception)
			{
				// ClsError.Error(this.Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}
	}
}
