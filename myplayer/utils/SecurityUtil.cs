using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace myplayer.utils
{
    class SecurityUtil
    {
        public static String md5Encrypt(String orignData)
        {
            StringBuilder tmp = new StringBuilder();
            try
            {
                MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
                byte[] hashedDataBytes;
                hashedDataBytes = md5Hasher.ComputeHash(Encoding.GetEncoding("gb2312").GetBytes(orignData));
                foreach (byte i in hashedDataBytes)
                {
                    tmp.Append(i.ToString("x2"));
                }
            }
            catch (Exception e){
                MessageBox.Show(e.Message);
            }
            return tmp.ToString();
           // return pwd;
        }
    }
}
