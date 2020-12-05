using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace myplayer.utils
{
    class FFmpegUtil
    {
        //切割识别   将视频转化为pcm格式
        public void cutVideo(int time,String videoPath) {
            for (int i = 1; i <= time; i++)
            {
                if (!FileUtil.setVideoFile(videoPath, i + ".pcm"))
                {
                    CommandUtil.StartCmdProcess(FileUtil.ffmepg + " -ss " + i * 7 + " -t " + 7 + " -i " + videoPath + " -f s16le -acodec pcm_s16le  -ar 16000 -ac 1  " + FileUtil.videoPath + "/" + Path.GetFileName(videoPath).GetHashCode() + "/" + i + ".pcm");
                }
            }
        }
    }
}
