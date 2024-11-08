using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Domain.Helper
{
    public static class UniqueIdGenerator
    {
        private static readonly object _lock = new object();

        public static string UniqueId
        {
            get
            {
                lock (_lock)
                {
                    return Generate();
                }
            }
        }
        public static string Generate()
        {
            long timestamp = Stopwatch.GetTimestamp(); // Provides a much finer-grained timestamp than DateTime.Now

            // Step 2: Convert timestamp to string and append a GUID to ensure uniqueness
            string dateTimeString = timestamp.ToString() + Guid.NewGuid().ToString("N").Substring(0, 4);


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
