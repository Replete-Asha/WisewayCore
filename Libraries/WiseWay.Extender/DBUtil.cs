using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace WiseWay.Extender
{
    public class DBUtil
    {
        #region [Connection String]
        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["DBConnection"].ToString();
            }
        }
        #endregion
    }
}
