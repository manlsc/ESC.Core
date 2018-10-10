using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace ESC.Core
{
    /// <summary>
    /// 加密帮助类
    /// </summary>
    public class ESCSecurity
    {
        #region AES

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <param name="key">32位字符串</param>
        /// <param name="iv">16位字符串</param>
        /// <returns>结果集base64转码</returns>
        public static string EncryptAES(string plainText, string key, string iv)
        {
            byte[] Key = new byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(Key.Length)), Key, Key.Length);
            byte[] IV = new byte[16];
            Array.Copy(Encoding.UTF8.GetBytes(iv.PadRight(IV.Length)), IV, IV.Length);

            byte[] encrypted;

            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(encrypted, 0, encrypted.Length);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="cipherText">密文</param>
        /// <param name="key">32位字符串</param>
        /// <param name="iv">16位字符串</param>
        /// <returns></returns>
        public static string DecryptAES(string cipherText, string key, string iv)
        {
            Byte[] encryptedBytes = Convert.FromBase64String(cipherText);

            Byte[] Key = new Byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(Key.Length)), Key, Key.Length);
            Byte[] IV = new Byte[16];
            Array.Copy(Encoding.UTF8.GetBytes(iv.PadRight(IV.Length)), IV, IV.Length);

            string plaintext = null;

            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(encryptedBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;
        }

        #endregion

        #region MD5

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string MD5Hash(string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }

        /// <summary>
        /// MD5验证
        /// </summary>
        /// <param name="input"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static bool VerifyMd5Hash(string input, string hash)
        {
            string hashOfInput = MD5Hash(input);

            using (MD5 md5Hash = MD5.Create())
            {
                StringComparer comparer = StringComparer.OrdinalIgnoreCase;

                if (0 == comparer.Compare(hashOfInput, hash))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        #endregion

        #region RSA

        /// <summary>
        /// 导出密钥
        /// 公钥私钥同时导出(dic["private","publice"])
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> ExportRSAKey()
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("private", rsa.ToXmlString(true));
                dic.Add("public", rsa.ToXmlString(false));
                return dic;
            }
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="dataToEncrypt"></param>
        /// <param name="publicKey">公钥</param>
        /// <returns></returns>
        public static byte[] EncryptRSA(byte[] dataToEncrypt, string publicKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(publicKey);
                return rsa.Encrypt(dataToEncrypt, true);
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="encryptedData"></param>
        /// <param name="privateKey">私钥</param>
        /// <returns></returns>
        public static byte[] DecryptRSA(byte[] encryptedData, string privateKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(privateKey);
                return rsa.Decrypt(encryptedData, true);
            }
        }

        #endregion
    }
}
