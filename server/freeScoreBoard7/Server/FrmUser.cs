using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace FreeScoreBoard.Server
{
	public partial class FrmUser : Form
	{
		private Credentials c;
		string filename = "";

		private string myAppFolder;

		public FrmUser()
		{
			this.InitializeComponent();
		}

		public string AppFolder
		{
			get
			{
				return this.myAppFolder;
			}
			set
			{
				this.myAppFolder = value;
				this.filename = Path.Combine(this.myAppFolder, "websecurity.xml");
			}
		}

		private void BtnShow_Click(object sender, EventArgs e)
		{
			this.TxtPW1.PasswordChar = '\0';
			this.TxtPW2.PasswordChar = '\0';
		}

		private void BtnOK_Click(object sender, EventArgs e)
		{
			if (this.TxtPW1.Text != this.TxtPW2.Text)
			{
				MessageBox.Show("Passwörter sind unterschiedlich");
				return;
			}

			this.c.username = this.TxtName.Text;
			this.c.password = this.TxtPW1.Text;

			XmlSerializer x = new XmlSerializer(typeof(Credentials));
			FileStream file = File.Create(this.filename);
			x.Serialize(file, this.c);
			file.Close();
			this.Close();
		}

		private void FrmUser_Load(object sender, EventArgs e)
		{
			XmlSerializer x = new XmlSerializer(typeof(Credentials));
			StreamReader reader = new StreamReader(this.filename);
			this.c = (Credentials)x.Deserialize(reader);
			reader.Close();

			this.TxtName.Text = this.c.username;
			this.TxtPW1.Text = this.c.password;
			this.TxtPW2.Text = this.c.password;
		}
	}
}
