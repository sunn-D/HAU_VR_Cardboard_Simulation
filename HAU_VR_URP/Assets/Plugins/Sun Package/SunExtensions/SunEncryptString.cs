using System.Security.Cryptography;
// ReSharper disable CheckNamespace

namespace Sun_Package
{
    public static class SunEncryptString
    {
        //
        private static byte[] _key = { 5, 1, 2, 7, 1, 6, 9, 4 };
        private static byte[] _iv = { 1, 9, 7, 6, 4, 2, 5, 3 };

        //
        public static string Encrypt(string text)
        {
            try
            {
                SymmetricAlgorithm algorithm = DES.Create();
                var transform = algorithm.CreateEncryptor(_key, _iv);
                var inputBuffer = System.Text.Encoding.Unicode.GetBytes(text);
                var outputBuffer = transform.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
                return System.Convert.ToBase64String(outputBuffer);
            }
            catch
            {
                return text;
            }
        }

        //
        public static string Decrypt(string text)
        {
            try
            {
                SymmetricAlgorithm algorithm = DES.Create();
                var transform = algorithm.CreateDecryptor(_key, _iv);
                var inputBuffer = System.Convert.FromBase64String(text);
                var outputBuffer = transform.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
                return System.Text.Encoding.Unicode.GetString(outputBuffer);
            }
            catch
            {
                return "";
            }
        }
    }
}

