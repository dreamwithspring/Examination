using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;

namespace word_company.Extensions
{
    /// <summary>
    /// 加密工具
    /// </summary>
    public static class HashPass
    {
        public static string HashString(string istr,string hashname)
        {
            HashAlgorithm hash = HashAlgorithm.Create(hashname);
            if(istr == null)
            {
                throw new ArgumentException("Unrecognized hash name", "hashName");
            }
            byte[] re = hash.ComputeHash(Encoding.UTF8.GetBytes(istr));

            return Convert.ToBase64String(re);

        }
    }
}
