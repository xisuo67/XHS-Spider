using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_XHS.Model
{
    public class LoginInfoStatus
    {
        public int code_status { get; set; }

        public LoginInfo login_info { get; set; }
    }

    public class LoginInfo
    { 
        public string secure_session { get; set; }

        public string user_id { get; set;}

        public string session { get; set;}
    }
}
