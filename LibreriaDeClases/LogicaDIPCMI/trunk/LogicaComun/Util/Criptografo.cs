using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace DipCmiGT.LogicaComun.Util
{
    public static class Criptografo
    {
        private static byte[] key = { };
        private static byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xab, 0xcd, 0xef };

        public static string Decrypt(string stringToDecrypt, string sEncryptionKey)
        {
            byte[] inputByteArray = new byte[stringToDecrypt.Length + 1];
            try
            {
                key = System.Text.Encoding.UTF8.GetBytes(sEncryptionKey);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(stringToDecrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms,
                  des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static string Encrypt(string stringToEncrypt, string SEncryptionKey)
        {
            try
            {
                key = System.Text.Encoding.UTF8.GetBytes(SEncryptionKey);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        private static byte[] HexStringToByteArray(String hexString)
        {
            int NumberChars = hexString.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }
            return bytes;
        }

        /// <summary>
        /// cifra una cadena de caracteres.
        /// </summary>
        /// <param name="cadenaPorCifrar">string con la cadena de caracteres a cifrar</param>
        /// <param name="llave">string hexadecimal de 64 posiciones que representa la llave de encripcion</param>
        /// <param name="vectorInicializacion">string hexadecimal de 32 posiciones que representa el vector de inicializacion.</param>
        /// <returns>La cadena de caracteres cifrada.</returns>
        public static string Cifrar(string cadenaPorCifrar, string llave, string vectorInicializacion)
        {
            byte[] bytesCifrados;
            RijndaelManaged rm = new RijndaelManaged();
            byte[] key = HexStringToByteArray(llave);
            byte[] IV = HexStringToByteArray(vectorInicializacion);
            ICryptoTransform encryptor = rm.CreateEncryptor(key, IV);
            UTF8Encoding encoder = new UTF8Encoding();
            byte[] valorBytes = encoder.GetBytes(cadenaPorCifrar);

            using (MemoryStream ms = new MemoryStream())
            {

                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(valorBytes, 0, valorBytes.Length);
                    cs.FlushFinalBlock();

                    ms.Position = 0;
                    bytesCifrados = new byte[ms.Length];
                    ms.Read(bytesCifrados, 0, bytesCifrados.Length);
                }
            }

            StringBuilder sb = new StringBuilder(bytesCifrados.Length * 2);
            for (int i = 0; i < bytesCifrados.Length; i++)
                sb.Append(string.Format("{0:X2}", bytesCifrados[i]));

            return sb.ToString();
        }

        /// <summary>
        /// Decifra una cadena de caracteres cifrada.
        /// </summary>
        /// <param name="cadenaPorDecifrar">string hexadecimal con la cadena de caracteres por decifrar.</param>
        /// <param name="llave">string hexadecimal de 64 posiciones que representa la llave de encripcion</param>
        /// <param name="vectorInicializacion">string hexadecimal de 32 posiciones que representa el vector de inicializacion.</param>
        /// <returns>La cadena de caracteres decifrada.</returns>
        public static string Decifrar(string cadenaPorDecifrar, string llave, string vectorInicializacion)
        {
            byte[] bytesDecifrados;
            RijndaelManaged rm = new RijndaelManaged();
            byte[] key = HexStringToByteArray(llave);
            byte[] IV = HexStringToByteArray(vectorInicializacion);
            ICryptoTransform decryptor = rm.CreateDecryptor(key, IV);
            UTF8Encoding encoder = new UTF8Encoding();
            byte[] valorBytes = HexStringToByteArray(cadenaPorDecifrar);

            using (MemoryStream ms = new MemoryStream())
            {

                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                {
                    cs.Write(valorBytes, 0, valorBytes.Length);
                    cs.FlushFinalBlock();

                    ms.Position = 0;
                    bytesDecifrados = new byte[ms.Length];
                    ms.Read(bytesDecifrados, 0, bytesDecifrados.Length);
                }
            }

            return encoder.GetString(bytesDecifrados);
        }

        public static string GetSHA1String(string str)
        {
            SHA1 sha1 = SHA1Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha1.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++)
                sb.AppendFormat("{0:x2}", stream[i]);

            return sb.ToString();
        }

    }
}
