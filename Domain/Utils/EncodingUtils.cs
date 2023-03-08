using System;
using System.Security.Cryptography;
using System.Text;

namespace Domain.Utils
{
    public static class EncodingUtils
    {
        private static Encoding dataEncoding = Encoding.UTF8;

        public static Encoding DataEncoding => dataEncoding;

        public static Encoding AltDataEncoding => Encoding.ASCII;

        public static string EncodeData(string? data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return string.Empty;
            }

            var textBytes = dataEncoding.GetBytes(data);
            return Convert.ToBase64String(textBytes);
        }

        public static string DecodeData(string? data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return string.Empty;
            }

            var base64EncodedBytes = Convert.FromBase64String(data);
            return dataEncoding.GetString(base64EncodedBytes);
        }

        public static string GetHashCode(string? data)
        {
            var stringBuilder = new StringBuilder();

            using (SHA256 hashManager = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                var result = hashManager.ComputeHash(enc.GetBytes(data ?? ""));

                foreach (var item in result)
                    stringBuilder.Append(item.ToString("x2"));
            }

            return stringBuilder.ToString();
        }
    }
}
