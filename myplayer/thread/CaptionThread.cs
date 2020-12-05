using DMSkin;
using myplayer.httprequest;
using myplayer.utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace myplayer.thread
{
    public delegate void Update(String value);
    public delegate int Time();
    class CaptionThread
    {
        //Update update = null;
        //AxAPlayer3Lib.AxPlayer player;
        //public static int position;
        String videoPath;
        //Baidu.Aip.Speech.Asr client;
        //PlayerForm form;
        //Dictionary<String, Object> dic;
        int duration;
        public CaptionThread(int duration,String videoPath, PlayerForm form)
        {
            this.duration = duration;
            this.videoPath = videoPath;
        }
        //int oldtime = 0;
        //public void startCaption()
        //{
        //    update = new Update(updateUI);
        //    var API_KEY = "uz804M95AKkq5OffLCB9GD3q";
        //    var SECRET_KEY = "IZxCofo7vFRYiPKhXqdmh2Da51MrhDKA";
        //    client = new Baidu.Aip.Speech.Asr(API_KEY, SECRET_KEY);
        //    Time timer = new Time(getTime);
        //    Thread.Sleep(500);
        //    while (true)
        //    {
        //        //Console.WriteLine(duration);
        //        //int position =player.GetPosition();
        //        if (position / 1000 % 7 == 2)
        //        {
        //            int newtime = (position / 1000 / 7) + 1;
        //            //Console.WriteLine(newtime);
        //            if (newtime != oldtime)
        //            {
        //                //Console.WriteLine(oldtime);
        //                var data = FileUtil.getFileVideo(videoPath, newtime + ".pcm");
        //                client.Timeout = 5000; // 若语音较长，建议设置更大的超时时间. ms
        //                var result = client.Recognize(data, "pcm", 16000, dic);
        //                Console.WriteLine(result);
        //                if (result["result"] != null)
        //                {
        //                    String value = result["result"][0].ToString();
        //                    String response = PostRequest.sendGet(value);
        //                    Thread.Sleep(200);
        //                    JObject jo = (JObject)JsonConvert.DeserializeObject(response);
        //                    String tranvalue = jo["trans_result"][0]["dst"].ToString();
        //                    Console.WriteLine(tranvalue);
        //                    if (tranvalue.Length > 16)
        //                    {
        //                        String data1 = tranvalue.Substring(0, 16) + "\r\n" + tranvalue.Substring(16);
        //                        //player.BeginInvoke(update, new object[] { newtime + " \r\n" + "00:00:" + newtime * 7 + ",500" + " --> 00:00:" + (newtime + 1) * 7 + ",000" + " \r\n" + data1 });
        //                        //player.SetConfig(511, newtime + " \r\n" + "00:00:" + newtime * 7 + ",500" + " --> 00:00:" + (newtime + 1) * 7 + ",000" + " \r\n" + data1);
        //                        //player.SetConfig(508, "微软雅黑;20;16777215;1");   //设置字幕字体
        //                        //player.SetConfig(507, "1;50;99");    //设置字幕位置
        //                        //Thread.Sleep(200);
        //                        FileUtil.writeSrtFile(videoPath, "\r\n" + (newtime + 1) + "\r\n" + "00:00:" + newtime * 7 + ",500" + " --> 00:00:" + (newtime + 1) * 7 + ",000" + "\r\n" + data1);
        //                    }
        //                    else
        //                    {
        //                        //Thread.Sleep(100);
        //                        //player.BeginInvoke(update, new object[] { newtime + " \r\n" + "00:00:" + newtime * 7 + ",500" + " --> 00:00:" + (newtime + 1) * 7 + ",000" + " \r\n" + tranvalue });
        //                        //player.SetConfig(508, "微软雅黑;20;16777215;1");   //设置字幕字体
        //                        //player.SetConfig(507, "1;50;99");    //设置字幕位置
        //                        //Thread.Sleep(200);
        //                        FileUtil.writeSrtFile(videoPath, "\r\n" + (newtime + 1) + "\r\n" + "00:00:" + newtime * 7 + ",500" + " --> 00:00:" + (newtime + 1) * 7 + ",000" + "\r\n" + tranvalue);
        //                    }
        //                }
        //                oldtime = newtime;
        //            }

        //            Thread.Sleep(50);
        //        }
        //    }
        //}

        public void startVideo()
        {
            //int duration = player.GetDuration();
            for (int i = 1; i <= duration + 1; i++)
            {
                if (!FileUtil.setVideoFile(videoPath, i + ".pcm"))
                {
                    CommandUtil.StartCmdProcess(FileUtil.ffmepg + " -ss " + i * 7 + " -t " + 7 + " -i " + videoPath + " -f s16le -acodec pcm_s16le  -ar 16000 -ac 1  " + FileUtil.videoPath + "/" + Path.GetFileName(videoPath).GetHashCode() + "/" + i + ".pcm");
                }
            }
        }

        private void updateUI(String value)
        {
           // player.SetConfig(511, value);
           // Console.WriteLine("thread:----" + Thread.CurrentThread.ManagedThreadId);
        }

        //private int getTime()
        //{
            //return player.GetPosition();
        //}
    }
}
