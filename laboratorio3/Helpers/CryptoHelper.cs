using System.Security.Cryptography;
using System.Text;

namespace laboratorio3.Helpers
{
    public static class CryptoHelper
    {
        public static string ComputeSha256(string input)
        {
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
            var sb = new StringBuilder();
            foreach (byte b in bytes)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }
}
