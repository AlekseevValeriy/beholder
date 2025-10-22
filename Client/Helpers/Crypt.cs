using System.Security.Cryptography;
using System.Text;

namespace Beholder.Helpers
{
    public static class Crypt
    {
        public static String StringToSha256Hash(String rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                Byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();

                for (Int32 i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}
