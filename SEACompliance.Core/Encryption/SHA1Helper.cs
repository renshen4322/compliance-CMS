using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SEACompliance.Core.Encryption
{
    public class SHA1Helper
    {
        public static string Compute(string input)
        {
            var engine = new SHA1CryptoServiceProvider();
            var byteArray = Encoding.Default.GetBytes(input);
            var hash = engine.ComputeHash(byteArray);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
