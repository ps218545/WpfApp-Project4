using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_Project4.Models
{
    public class Project4
    {
        #region Messages
        public static readonly string UNKNOWN = "Unknown";
        public static readonly string OK = "Ok";
        public static readonly string NOTFOUND = "not found";
        #endregion

        private readonly string connString = ConfigurationManager.ConnectionStrings["project4"].ConnectionString;


    }
}
