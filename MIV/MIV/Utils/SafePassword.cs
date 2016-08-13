using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;      

namespace MIV.Utils
{
    public class SafePassword
    {
        public const int StretchCnt = 1000;    

        /// <summary>
        /// salt使ってストレッチしたハッシュ値の取得
        /// </summary>        
        /// <param name="id">
        /// id
        /// </param>             
        /// <param name="psw">
        /// パスワード
        /// </param>
        /// <see cref="https://www.websec-room.com/2013/02/27/239"/>
        public static string GetStretchedPassword(string id, string psw)
        {                             
            string salt = GetSha256(id);
            string hash = "";

            for (int i = 0; i < StretchCnt; i++)
            {
                hash = GetSha256(hash + salt + psw);
            }       
            return hash; 
        }

        /// <summary>
        /// SHA256を取得
        /// </summary>
        /// <param name="target">
        /// ハッシュ化する文字列
        /// </param>
        private static string GetSha256(string target)
        {
            SHA256 mySHA256 = SHA256Managed.Create();
            byte[] byteValue = Encoding.UTF8.GetBytes(target);
            byte[] hash = mySHA256.ComputeHash(byteValue);

            StringBuilder buf = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                buf.AppendFormat("{0:x2}", hash[i]);
            }
 
            return buf.ToString();
        }
    }
}
