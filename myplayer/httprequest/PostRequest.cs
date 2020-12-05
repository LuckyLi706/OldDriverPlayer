using Mono.Web;
using myplayer.utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace myplayer.httprequest
{
    class PostRequest
    {
        private const String URL = "http://api.fanyi.baidu.com/api/trans/vip/translate";
        private const String APP_ID = "20190109000255608";
        private const String FROM_LAN = "auto";
        private const String TO_LAN = "en";

        //密钥
        private const String SIGNATURE = "eeYyhcD_LWjyDLoTyq_D";

        //Post请求方式
        public static String sendPost(String tranText)
        {
            string result = string.Empty;
            string salt = DateTime.UtcNow.ToLongDateString();
            Dictionary<String, String> headerDic = new Dictionary<string, string>();
            //headerDic.Add("q", tranText);
            //headerDic.Add("appid", APP_ID);
            //headerDic.Add("from", FROM_LAN);
            //headerDic.Add("to", TO_LAN);
            //headerDic.Add("salt", salt);
            //headerDic.Add("sign", SecurityUtil.md5Encrypt(APP_ID+tranText+salt+SIGNATURE));
            string paraUrlCoded = "q:" + tranText + "\r\nappid:" + APP_ID + "\r\nfrom:" + FROM_LAN + "\r\nsalt:" + salt + "\r\nto:" + TO_LAN + "\r\nsign:" + SecurityUtil.md5Encrypt(APP_ID + tranText + salt + SIGNATURE);
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Method = "POST";
                request.ContentType = "multipart/form-data";
                //string paraUrlCoded = json;
                byte[] payload;
                payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
                request.ContentLength = payload.Length;
                Stream writer = request.GetRequestStream();
                writer.Write(payload, 0, payload.Length);
                writer.Close();
                System.Net.HttpWebResponse response;
                response = (System.Net.HttpWebResponse)request.GetResponse();
                System.IO.Stream s;
                s = response.GetResponseStream();
                string StrDate = "";
                string strValue = "";
                StreamReader Reader = new StreamReader(s, Encoding.UTF8);
                while ((StrDate = Reader.ReadLine()) != null)
                {
                    strValue += StrDate + "\r\n";
                }
                return strValue;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "";
        }

        public static string sendGet(String tranText)
        {
            String jsonResult;
            string randomNum = System.DateTime.Now.Millisecond.ToString();
            string oldsign = APP_ID + tranText.Trim() + randomNum + SIGNATURE;
            string url = String.Format("http://api.fanyi.baidu.com/api/trans/vip/translate?q={0}&from={1}&to={2}&appid={3}&salt={4}&sign={5}",
            tranText.Trim(), FROM_LAN, TO_LAN, APP_ID, randomNum, SecurityUtil.md5Encrypt(oldsign));
            WebClient wc = new WebClient();
            try
            {
                jsonResult = wc.DownloadString(url);
            }
            catch
            {
                jsonResult = string.Empty;
            }
            return jsonResult;
        }

    }
}