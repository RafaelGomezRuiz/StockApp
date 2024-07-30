using System.Security.Cryptography;
using System.Text;

namespace StockApp.Core.Application.Helpers
{
    public class PasswordEncryption
    {
        public static string ComputeSha256Hash(string password)
        {
            //Create sha
            using(SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes= sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                //Combert byte array to string
                StringBuilder builder = new();

                for (int i = 0; i< bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
