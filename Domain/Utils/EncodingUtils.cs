using System;
using System.Text;

namespace Domain.Utils
{
    public static class EncodingUtils
    {
        private static Encoding dataEncoding = Encoding.UTF8;

        public static Encoding DataEncoding => dataEncoding;

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
            if (string.IsNullOrEmpty(data))
            {
                return string.Empty;
            }

            return data.GetHashCode().ToString();
        }
    }
}
