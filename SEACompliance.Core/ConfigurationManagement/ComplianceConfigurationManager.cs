using SEACompliance.Core.Encryption;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEACompliance.Core.ConfigurationManagement
{
    public class ComplianceConfigurationManager
    {
        public static string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ToString();
        }

        public static string GetString(string name)
        {
            if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[name]))
            {
                return string.Empty;
            }

            return ConfigurationManager.AppSettings[name].ToString();
        }

        public static int GetInt(string name)
        {
            return Int32.Parse(GetString(name));
        }

        public static bool GetBool(string name)
        {
            bool ret;

            if (Boolean.TryParse(GetString(name), out ret))
            {
                return ret;
            }

            return false;
        }

        public static string GetEncryptString(string name)
        {
            var value = ConfigurationManager.AppSettings[name].ToString();
            return Encryption.Encryption.Decrypt(value);
        }
    }
}
