using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USh.Utility
{
    public class StaticData
    {
        public static string ShortUrlTemplate = "Shtnr";

        public static Dictionary<string, int> VerifyDict = new Dictionary<string, int>
        {
            {"1.jpg", 110},
            {"2.jpg", 56},
            {"3.jpg", 50},
            {"4.jpg", 150},
            {"5.jpg", 0},
            {"6.jpg", 0},
            {"7.jpg", 40}
        };
        public const string Role_Admin = "Admin";
        public const string Role_Customer = "Customer";
    }
}
