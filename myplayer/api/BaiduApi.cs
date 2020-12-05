using Baidu.Aip.Speech;
using myplayer.httprequest;
using myplayer.utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace myplayer.api
{
    class BaiduApi
    {
        private static BaiduApi api;
        private static Dictionary<String, Object> dic;   //识别的语言
        Asr client;
        private static String videoPath;
        private static Dictionary<int, String> captionDic;
        private static int number;

        public static BaiduApi getInstance(String path, Dictionary<int, String> Dic,int num)
        {
            videoPath = path;
            number = num;
            captionDic = Dic;
            if (api == null)
            {
                api = new BaiduApi();
                dic = new Dictionary<string, object>();
                dic.Add("dev_pid", 1737);
            }
            return api;
        }

        //语音识别
        public void recoVideo()
        {
            Thread.Sleep(2000);
            if (client == null)
            {
                var API_KEY = "uz804M95AKkq5OffLCB9GD3q";
                var SECRET_KEY = "IZxCofo7vFRYiPKhXqdmh2Da51MrhDKA";
                client = new Baidu.Aip.Speech.Asr(API_KEY, SECRET_KEY);
            }
            for (int i = 1; i < number; i++)
            {
                try {
                    var data = FileUtil.getFileVideo(videoPath, i + ".pcm");
                    client.Timeout = 12000; // 若语音较长，建议设置更大的超时时间. ms
                    var result = client.Recognize(data, "pcm", 16000, dic);
                    if (result["result"] != null)
                    {
                        String value = result["result"][0].ToString();
                        tranText(value, i, captionDic);
                    }
                }
                catch (Exception e) {
                    Console.WriteLine(e.Message);
                }
            }
        }

        //翻译操作
        private void tranText(String value, int time, Dictionary<int, String> dic)
        {
            String response = PostRequest.sendGet(value);
            JObject jo = (JObject)JsonConvert.DeserializeObject(response);
            String tranvalue = jo["trans_result"][0]["dst"].ToString();
            Console.WriteLine(tranvalue);
            if (tranvalue.Length > 16)
            {
                String data1 = tranvalue.Substring(0, 16) + "\r\n" + tranvalue.Substring(16);
                if (!dic.ContainsKey(time))
                {
                    dic.Add(time, data1);
                }
                FileUtil.writeSrtFile(videoPath, "\r\n" + (time + 1) + "\r\n" + "00:00:" + time * 7 + ",500" + " --> 00:00:" + (time + 1) * 7 + ",000" + "\r\n" + data1);
            }
            else
            {
                dic.Add(time, tranvalue);
                FileUtil.writeSrtFile(videoPath, "\r\n" + (time + 1) + "\r\n" + "00:00:" + time * 7 + ",500" + " --> 00:00:" + (time + 1) * 7 + ",000" + "\r\n" + tranvalue);
            }
        }

    }
}
