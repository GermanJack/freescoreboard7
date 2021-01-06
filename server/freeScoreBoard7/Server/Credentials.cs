using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeScoreBoard.Server
{
    [Serializable]
    public class Credentials
    {
        public Credentials() { }

        public Credentials(string UserName, string Password)
        {
            this.username = UserName;
            this.password = Password;
        }

        public string username { get; set; }
        public string password { get; set; }
    }
}
