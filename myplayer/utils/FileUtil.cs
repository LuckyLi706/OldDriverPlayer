using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace myplayer.utils
{
    class FileUtil
    {
        public static String codecsPath = System.Environment.CurrentDirectory + "/codecs";
        public static String videoPath = System.Environment.CurrentDirectory + "/videos";
        public static String ffmepg = System.Environment.CurrentDirectory + "/ffmpeg/ffmpeg.exe";
        public static String srtName = "test.srt";

        public static bool setVideoFile(string video, string fileName)
        {
            if (!Directory.Exists(videoPath + "/" + Path.GetFileName(video).GetHashCode()))
            {
                Directory.CreateDirectory(videoPath + "/" + Path.GetFileName(video).GetHashCode());
            }
            bool exsit = File.Exists(videoPath + "/" + Path.GetFileName(video).GetHashCode() + "/" + fileName);
            return exsit;
        }

        public static byte[] getFileVideo(string video, string fileName)
        {

            //System.IO.FileStream fs = new System.IO.FileStream(videoPath + "/" + Path.GetFileName(video).GetHashCode()+"/"+fileName,System.IO.FileMode.Open, System.IO.FileAccess.Read, FileShare.ReadWrite);

            return File.ReadAllBytes(videoPath + "/" + Path.GetFileName(video).GetHashCode() + "/" + fileName);
        }

        public static void writeSrtFile(string video, string file)
        {
            String path = videoPath + "/" + Path.GetFileName(video).GetHashCode() + "/" + srtName;
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }
            File.AppendAllText(path, file,Encoding.UTF8);

        }

        public static bool readSrtFile(String video) {
            String path = videoPath + "/" + Path.GetFileName(video).GetHashCode() + "/" + srtName;
            if (File.Exists(path)) {
                return true;
            }
            return false;
        }

        public static String getSrtPath(String video) {
            return videoPath + "/" + Path.GetFileName(video).GetHashCode() + "/" + srtName; 
        }
    }
}
