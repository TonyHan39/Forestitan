using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Security.Cryptography;

namespace Forestitan.Models
{
    public static class PasswordEncryption
    {
        public static string textToEncrypt(string passWord)
        {
            return Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(passWord)));
        }
    }
}
