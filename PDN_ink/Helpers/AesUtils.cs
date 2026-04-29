using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Pdnink_Coremvc.Helpers
{
    public static class AesUtils
    {
        private const string KeyStr = "Para cuando se dio cuenta de lo ocurrido ya era tarde";

        // Frase tomada del libro: El Developer Humilde   ISBN: 978-0-306-42818-4
        private const string sKey = "los Dioses nos convertimos en humanos y entre ellos nos dicen Developers";

        private static RijndaelManaged GenerateKey()
        {
            var aes = new RijndaelManaged
            {
                BlockSize = 128,
                KeySize = 256,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };

            byte[] ivArr = { 1, 2, 3, 4, 5, 6, 6, 5, 4, 3, 2, 1, 7, 7, 7, 7 };
            var keyArrBytes32Value = new byte[32];
            var ivBytes16Value = new byte[16];

            var keyArr = Convert.FromBase64String(Base64Encode(KeyStr));
            Array.Copy(keyArr, keyArrBytes32Value, 32);
            Array.Copy(ivArr, ivBytes16Value, 16);

            aes.Key = keyArrBytes32Value;
            aes.IV = ivBytes16Value;

            return aes;
        }

        internal static string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText)) return plainText;

            var cipher = GenerateKey();

            var encrypt = cipher.CreateEncryptor();

            var plainTextByte = ASCIIEncoding.UTF8.GetBytes(plainText);
            var cipherText = encrypt.TransformFinalBlock(plainTextByte, 0, plainTextByte.Length);
            return Convert.ToBase64String(cipherText);
        }

        internal static string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText)) return cipherText;
            try
            {
                var cipher = GenerateKey();
                var decrypt = cipher.CreateDecryptor();
                var encryptedBytes = Convert.FromBase64CharArray(cipherText.ToCharArray(), 0, cipherText.Length);
                var decryptedData = decrypt.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                return ASCIIEncoding.UTF8.GetString(decryptedData);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }


        public static string Encriptar(string plainText)
        {
            if (string.IsNullOrEmpty(plainText)) return plainText;

            var cipher = Generate_Key();

            var encrypt = cipher.CreateEncryptor();

            var plainTextByte = ASCIIEncoding.UTF8.GetBytes(plainText);
            var cipherText = encrypt.TransformFinalBlock(plainTextByte, 0, plainTextByte.Length);

            string sEncrip = Convert.ToBase64String(cipherText);

            sEncrip = sEncrip.Replace("=", "pd.n");
            sEncrip = sEncrip.Replace("?", "ah.m");
            sEncrip = HttpUtility.UrlEncode(sEncrip);
            return sEncrip;


        }

        public static string Desencriptar(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText)) return cipherText;
            try
            {
                cipherText = HttpUtility.UrlDecode(cipherText);
                cipherText = cipherText.Replace("pd.n", "=");
                cipherText = cipherText.Replace("ah.m", "?");
                var cipher = Generate_Key();
                var decrypt = cipher.CreateDecryptor();
                var encryptedBytes = Convert.FromBase64CharArray(cipherText.ToCharArray(), 0, cipherText.Length);
                var decryptedData = decrypt.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                return ASCIIEncoding.UTF8.GetString(decryptedData);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private static RijndaelManaged Generate_Key()
        {
            var aes = new RijndaelManaged
            {
                BlockSize = 128,
                KeySize = 256,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };

            byte[] ivArr = { 1, 2, 3, 4, 5, 6, 6, 5, 4, 3, 2, 1, 7, 7, 7, 7 };
            var keyArrBytes32Value = new byte[32];
            var ivBytes16Value = new byte[16];

            var keyArr = Convert.FromBase64String(Base64Encode(sKey));
            Array.Copy(keyArr, keyArrBytes32Value, 32);
            Array.Copy(ivArr, ivBytes16Value, 16);

            aes.Key = keyArrBytes32Value;
            aes.IV = ivBytes16Value;

            return aes;
        }


    }
}
