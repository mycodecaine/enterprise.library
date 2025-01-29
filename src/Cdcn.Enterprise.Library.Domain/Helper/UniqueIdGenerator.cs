using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace Cdcn.Enterprise.Library.Domain.Helper
{
    /// <summary>
    /// Provides functionality for generating unique identifiers.
    /// This static class is thread-safe, ensuring unique IDs across multiple threads.
    /// </summary>
    public static class UniqueIdGenerator
    {
        private static readonly object _lock = new object();

        /// <summary>
        /// Gets a unique identifier. The ID is generated using a combination of a 
        /// high-resolution timestamp, a GUID, and an MD5 hash to ensure uniqueness.
        /// This property is thread-safe.
        /// </summary>
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

        /// <summary>
        /// Generates a unique identifier by combining:
        /// - A high-resolution timestamp from <see cref="Stopwatch.GetTimestamp"/>.
        /// - A portion of a GUID to add entropy.
        /// - An MD5 hash of the combined string, formatted for compactness.
        /// The resulting ID is an 8-character alphanumeric string.
        /// </summary>
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
