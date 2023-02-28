using System;
using System.Text;

namespace Services.Utils
{
    public static class EncodingUtils
    {
        public static string EncodeData(string data)
        {
            var textBytes = Encoding.UTF8.GetBytes(data);
            return Convert.ToBase64String(textBytes);
        }

        public static string DecodeData(string data)
        {
            var base64EncodedBytes = Convert.FromBase64String(data);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
