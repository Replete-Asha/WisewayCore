using System;
using System.Collections.Generic;
using System.Text;

namespace WiseWay.Core
{
   public class AppSettings
    {
        public string DbConn { get; set; }
        public int ExpiryDay { get; set; }
        public string Secret { get; set; }
    }
}
