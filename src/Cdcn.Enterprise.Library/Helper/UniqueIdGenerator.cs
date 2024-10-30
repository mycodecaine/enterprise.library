using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Domain.Helper
{
    internal class UniqueIdGenerator
    {
        public static string UniqueId => Generate();
        public static string Generate()
        {
            // Step 1: Get the current date and time
            DateTime now = DateTime.Now;

            // Step 2: Format the date and time to a string
            string dateTimeString = now.ToString("yyyyMMddHHmmssffff");

            // Step 3: Create a hash of the dateTimeString
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(dateTimeString));

                // Convert the hash to a Base64 string and take the first 8 characters
                string base64String = Convert.ToBase64String(hashBytes);

                // Remove any non-alphanumeric characters and take the first 8 characters
                string uniqueId = base64String.Replace("+", "").Replace("/", "").Replace("=", "").Substring(0, 8);

                return uniqueId;
            }
        }
    }
}
