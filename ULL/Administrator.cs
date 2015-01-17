using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;

namespace ULL
{
    public class Administrator
    {
        public Session _session = null;

        public Administrator()
        {
        }

        public bool Login(string username, string password)
        {
            _session = Security.Connect();
            return _session.Login(username, password);
        }


    }
}
