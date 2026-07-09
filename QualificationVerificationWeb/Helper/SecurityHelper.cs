using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QualificationVerificationWeb.Helper
{

    

    public class SecurityHelper
    {
        static readonly string Hashkey = "P@@Sw0rd";
        static readonly string SaltKey = "S@LT&KEY";
        static readonly string VIKey = "@1B2c3D4e5F6g7H8";
        //private static readonly int IsActice = 1;


        public static string Encrypt(string strToEncrypt)
        {
            Validator valid = new Validator();


            byte[] strToEncryptBytes = Encoding.UTF8.GetBytes(strToEncrypt);

            byte[] keyBytes = new Rfc2898DeriveBytes(Hashkey, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

            byte[] cipherTextBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(strToEncryptBytes, 0, strToEncryptBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return Convert.ToBase64String(cipherTextBytes);
        }


        public static string Decrypt(string strToDecrypt)
        {
            Validator valid = new Validator();
            string decrptedValue;

            try
            {
                strToDecrypt = strToDecrypt.Replace(" ", "+");
                byte[] cipherTextBytes = Convert.FromBase64String(strToDecrypt);
                byte[] keyBytes = new Rfc2898DeriveBytes(Hashkey, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
                var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

                var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
                var memoryStream = new MemoryStream(cipherTextBytes);
                var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                byte[] strToDecryptBytes = new byte[cipherTextBytes.Length];

                int decryptedByteCount = cryptoStream.Read(strToDecryptBytes, 0, strToDecryptBytes.Length);
                memoryStream.Close();
                cryptoStream.Close();
                decrptedValue = Encoding.UTF8.GetString(strToDecryptBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return decrptedValue;
        }

        public static string Decrypt(string academicRecordLoggedInUser, string v)
        {
            throw new NotImplementedException();
        }
    }
}
