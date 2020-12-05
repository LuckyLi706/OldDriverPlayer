using myplayer.httprequest;
using myplayer.utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace myplayer
{
    public delegate void CallBackDelegate(string message);
    public partial class WaitingForm : Form
    {
        CallBackDelegate callBack = null;
        private AxAPlayer3Lib.AxPlayer player;
        private String videoPath;
        Baidu.Aip.Speech.Asr client;
        Dictionary<String, Object> dic;
        Label label;
        public WaitingForm(Label label,AxAPlayer3Lib.AxPlayer player, String videoPath)
        {
            //Control.CheckForIllegalCrossThreadCalls = false;
            callBack = new CallBackDelegate(updateUI);
            InitializeComponent();
            this.videoPath = videoPath;
            this.player = player;
            this.label = label;
            dic = new Dictionary<string, object>();
            dic.Add("dev_pid", 1737);
            new Thread(work).Start();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }


        public void work() {
           if (!FileUtil.setVideoFile(videoPath, 0 + ".pcm"))
            {
                CommandUtil.StartCmdProcess(FileUtil.ffmepg + " -ss " + 0 + " -t " + 7 + " -i " + videoPath + " -f s16le -acodec pcm_s16le  -ar 16000 -ac 1  " + FileUtil.videoPath + "/" + Path.GetFileName(videoPath).GetHashCode() + "/" + 0 + ".pcm");
                Thread.Sleep(1000);
            }
            var API_KEY = "uz804M95AKkq5OffLCB9GD3q";
            var SECRET_KEY = "IZxCofo7vFRYiPKhXqdmh2Da51MrhDKA";
            client = new Baidu.Aip.Speech.Asr(API_KEY, SECRET_KEY);
            var data = FileUtil.getFileVideo(videoPath, 0 + ".pcm");
            client.Timeout = 120000; // 若语音较长，建议设置更大的超时时间. ms
            var result = client.Recognize(data, "pcm", 16000, dic);
            if (result["err_no"].ToString().Equals("3301")) {
                Close();
            }
            Console.WriteLine(result);
            if (result["result"] != null)
            {
                String value = result["result"][0].ToString();
                String response = PostRequest.sendGet(value);
                Thread.Sleep(300);
                JObject jo = (JObject)JsonConvert.DeserializeObject(response);
                Console.WriteLine(response);

                String tranvalue = jo["trans_result"][0]["dst"].ToString();
                if (tranvalue.Length > 7)
                {
                    String data1 = tranvalue.Substring(0, 7) + "\r\n" + tranvalue.Substring(7);
                    label.BeginInvoke(callBack, new object[] { data1 });
                    //player.SetConfig(511, 0 + 0 + "\r\n "+"\r\n " + "00:00:" + 0 * 7 + ",500" + " --> 00:00:" + (0 + 1) * 7 + ",000" + "\r\n " + data1);
                    //player.SetConfig(508, "微软雅黑;20;16777215;1");   //设置字幕字体
                    //player.SetConfig(507, "1;50;90");    //设置字幕位置
                    FileUtil.writeSrtFile(videoPath,data1);
                    Close();
                }
                else
                {
                    label.BeginInvoke(callBack, new object[] {  tranvalue });
                    //player.SetConfig(511, 0 + " \r\n " + "00:00:" + 0 * 7 + ",500" + " --> 00:00:" + (0 + 1) * 7 + ",000" + " \r\n " + tranvalue);
                    //player.SetConfig(508, "微软雅黑;20;16777215;1");   //设置字幕字体
                    //player.SetConfig(507, "1;50;50");    //设置字幕位置
                    Console.WriteLine(tranvalue);
                    FileUtil.writeSrtFile(videoPath, 1+"\r\n"+"00:00:" + 0 * 7 + ",500" + " --> 00:00:" + (0 + 1) * 7 + ",000" + "\r\n " + tranvalue);
                    Close();
                }
            }
        }


        public void updateUI(String value) {
            label.Text = value;
        }
    }
}
