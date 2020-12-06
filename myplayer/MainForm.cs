using DMPlay;
using DMSkin.Controls;
using myplayer.helpers;
using myplayer.model;
using myplayer.Properties;
using myplayer.utils;
using myplayer.thread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using myplayer.api;
using System.Net.NetworkInformation;

namespace myplayer
{
    public partial class PlayerForm : DMSkin.Main
    {
        private SettingForm settingForm; //设置窗体
        private List<VideoItem> videoList = new List<VideoItem>();//视频列表
        private VideoItem playingItem = null; //正在播放的item
        private VideoItem currItem = null; //当前鼠标选中的item
        private bool IsMouseDown = false;//是否按住音量调整条
        private bool Full = false;//是否全屏播放
        private bool isMaxSize = false;
        private Rectangle Nor = new Rectangle(0, 0, 0, 0);//当前窗口的矩形
        public System.Windows.Forms.Timer timer;//计时器，重新绘制界面

        private long duration = 0;//视频总共时间
        private long position = 0;//视频播放位置
        public int volume = 0;//音量
        public int LastCursorX, LastCursorY;
        public int LastHeight, LastWidth;
        public int LastLeft, LastTop;
        public int CursorStayMaxTime = 10;
        public int CursorStayTime = 0;
        private bool isTran = true;

        private Dictionary<int, String> captionDic;


        public PlayerForm()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            this.timer = new System.Windows.Forms.Timer();
            this.timer.Tick += new EventHandler(this.time_Tick);
            this.timer.Interval = 50;
            this.timer.Enabled = true;
            this.player.Focus();
            captionDic = new Dictionary<int, string>();
            //metroContextMenu1.Visible = false;
        }

        /// <summary>
        /// 初始化窗体
        /// </summary>
        private void mainFrom_Load(object sender, EventArgs e)
        {

            this.InitPlayListView(null);
            this.player.Focus();
            initView();
            initAplayerConfig();
        }

        private void initAplayerConfig()
        {
            //设置播放器默认展示
            //this.player.SetCustomLogo(Properties.Resources.logo.GetHbitmap().ToInt32());
            //从配置文件读取上一次用户的设置的音量，配置文件默认在C:/Users/用户名/AppData/Local/项目名/
            this.volume = Settings.Default.volume;
            //更新播放器音量和音量的UI
            this.UpdateVol();
            this.player.SetConfig(119, "2");//获取或者设置循环播放, 0-自动, 1-循环, 2-不循环, 默认0 (自动模式中, GIF 会自动循环, 其他格式默认不循环)
            this.player.SetConfig(2, FileUtil.codecsPath);    //设置解码器的位置

        }

        //初始化控件
        private void initView()
        {

            //初始化菜单项
            udItem.Click += new EventHandler(MenuStrip_Click);
            vdItem.Click += new EventHandler(MenuStrip_Click);
        }

        //标题菜单方法
        private void MenuStrip_Click(object sender, EventArgs e)
        {
            Ping ping = new System.Net.NetworkInformation.Ping();
            try
            {
                PingReply res = ping.Send("www.baidu.com");  //
                if (res.Status != System.Net.NetworkInformation.IPStatus.Success)
                {
                    //没有联上Internet哦，做下自己想做个事情咯
                    MessageBox.Show("当前没接入互联网，请连接网络");
                    return;
                }
            }
            catch (Exception er)
            {
                //里你，仲系当你断网咯，做下自己想做个事情咯
                MessageBox.Show("当前没接入互联网，请连接网络");
                return;
            }

            ToolStripMenuItem mItem = (ToolStripMenuItem)sender;
            //打开URL视频
            if (mItem.Name.Equals(udItem.Name))
            {
                isTran = false;
                player.Open("http://data.vod.itc.cn/?rb=1&key=jbZhEJhlqlUN-Wj_HEI8BjaVqKNFvDrn&prod=flash&pt=1&new=/121/185/tcURwYNJRQ6Y4SDz0kTfkD.mp4");
                player.Play();
            }
            //打开本地视频
            else if (mItem.Name.Equals(vdItem.Name))
            {

                OpenAndPlay();//打开和播放视频
                //isTran = true;
                //this.player.Focus();
            }
        }

        public void open()
        {
            //OpenAndPlay();//打开和播放视频
            int SH = Screen.PrimaryScreen.Bounds.Height;
            int SW = Screen.PrimaryScreen.Bounds.Width;
            this.player.Open(playingItem.Url);//播放视频
            this.lb_play.DM_Key = DMSkin.Controls.DMLabelKey.暂停;//播放->暂停
            this.player.Focus();
        }



        /// <summary>
        /// 重新绘制窗体
        /// </summary>
        SolidBrush sb = new SolidBrush(Color.LightCoral);
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = InterpolationMode.Bilinear;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            e.Graphics.FillRectangle(sb, -1, -1, Width + 30, Height + 30);
        }


        /// <summary>
        /// 重绘播放列表
        /// </summary>
        public void InitPlayListView(VideoItem playingItem)
        {
            //清除列表
            this.list_play.Items.Clear();

            if (videoList != null)
            {
                int indexY = 0;
                for (int i = 0; i < videoList.Count; i++)
                {
                    Item item = new Item();
                    item.Bounds = new Rectangle(0, indexY, 1920, 28);
                    item.Text = videoList[i].Name;
                    item.Url = videoList[i].Url;
                    item.OnLine = videoList[i].IsPlaying;
                    list_play.Items.Add(item);
                    indexY += 28;
                }
            }
        }

        Point pt;
        private void canMove_Panel_MouseDown(object sender, MouseEventArgs e)
        {
            pt = Cursor.Position;
        }

        private void canMove_Panel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int px = Cursor.Position.X - pt.X;
                int py = Cursor.Position.Y - pt.Y;
                this.Location = new Point(this.Location.X + px, this.Location.Y + py);
                pt = Cursor.Position;
            }
        }


        /// <summary>
        /// 双击视频全屏
        /// </summary>>
        private void player_OnMessage(object sender, AxAPlayer3Lib._IPlayerEvents_OnMessageEvent e)
        {
            Constant.N_MESSAGE nMessage = (Constant.N_MESSAGE)e.nMessage;
            switch (nMessage)
            {
                case Constant.N_MESSAGE.DOUBLE_LEFT_CLICK:
                    this.FullScreen();//全屏
                    break;
            }
        }

        private bool isTest = true;

        /// <summary>
        /// 主计时器，每50ms自动根据播放器的状态来控制播放器的动作和重绘视图
        /// </summary>>
        private void time_Tick(Object sender, EventArgs e)
        {
            //获取player状态
            Constant.PLAY_STATE state = (Constant.PLAY_STATE)this.player.GetState();
            switch (state)
            {
                case Constant.PLAY_STATE.PS_PLAY:
                case Constant.PLAY_STATE.PS_PAUSED: //暂停状态
                    this.position = this.player.GetPosition();
                    this.duration = this.player.GetDuration();
                    //设置视频进度条值
                    this.pgb_time.DM_Value = this.position / Convert.ToDouble(this.duration) * 100.0;
                    if (position > 7000 && position / 1000 % 7 == 0)
                    {
                        
                        int time = Convert.ToInt32((position / 1000 / 7));
                        if (captionDic.ContainsKey(time))
                        {
                            //player.SetConfig(511, time + " \r\n" + "00:00:" + time * 7 + ",500" + " --> 00:00:" + (time + 1) * 7 + ",000" + " \r\n" + captionDic[time]);
                            label2.Text = captionDic[time];
                            //Console.WriteLine(time + " \r\n" + "00:00:" + time * 7 + ",500" + " --> 00:00:" + (time + 1) * 7 + ",000" + " \r\n" + captionDic[time]);
                        }
                        else {
                            label2.Text = "";
                        }
                        //isTest = true;
                    }
                    //    CaptionThread.position = Convert.ToInt32(position);
                    //设置 当前播放时间/视频总共时间
                    this.lb_time.Text = PlayHelper.GetTime(this.position) + " / " + PlayHelper.GetTime(this.duration);
                    this.pgb_time.Invalidate();
                    UpdateVol();
                    break;
            }

            if (this.Full)
            {
                if ((this.LastCursorX == Cursor.Position.X) && (this.LastCursorY == Cursor.Position.Y))
                {
                    this.CursorStayTime++;
                }
                else
                {
                    this.CursorStayTime = 0;
                }
                if (Cursor.Position.Y >= (Screen.PrimaryScreen.Bounds.Height - this.menuPanel.Height))
                {
                    this.menuPanel.BringToFront();
                }
                else if (this.CursorStayTime >= this.CursorStayMaxTime)
                {
                    this.player.BringToFront();
                }
                this.LastCursorX = Cursor.Position.X;
                this.LastCursorY = Cursor.Position.Y;
            }

        }


        //------------------------------------------------------------

        /// <summary>
        /// 按钮或标签停留效果
        /// </summary>
        private void btns_MouseEnter(object sender, EventArgs e)
        {
            DMLabel label = (DMLabel)sender;
            label.DM_Color = Color.LightPink;
        }

        /// <summary>
        /// 按钮或标签停留后移开的效果
        /// </summary>
        private void btns_MouseLeave(object sender, EventArgs e)
        {
            DMLabel label = (DMLabel)sender;
            label.DM_Color = Color.White;
        }

        /// <summary>
        /// 打开设置窗体
        /// </summary>
        private void lb_set_Click(object sender, EventArgs e)
        {
            if (settingForm == null || settingForm.IsDisposed)
            {
                settingForm = new SettingForm();
            }
            settingForm.Show();
        }

        /// <summary>
        /// 打开视频文件
        /// </summary>
        private void lb_openFile_Click(object sender, EventArgs e)
        {
            OpenAndPlay();//打开和播放视频
            this.player.Focus();
        }


        /// <summary>
        /// 打开和播放视频
        /// </summary>
        private void OpenAndPlay()
        {
            //返回选中的视频实体
            VideoItem myitem = PlayUtils.OpenFile();
            //没有，退出
            if (myitem == null)
                return;
            myitem.IsPlaying = true;//设置播放标志位为true
            this.playingItem = myitem;//设置播放item
            bool isEqual = false;//不存在一样的
            //搜索一遍视频列表
            foreach (var item in this.videoList)
            {
                //如果在列表中找到重复的
                if (item.Name == myitem.Name)
                {
                    isEqual = true;//存在一样的
                    item.IsPlaying = true;
                }
                else
                {
                    item.IsPlaying = false;
                }
            }
            //不存在一样的
            if (!isEqual)
            {
                this.videoList.Add(myitem);//加入视频列表
            }
            this.InitPlayListView(playingItem);//重绘播放列表
            video = playingItem.Url;
            if (player.GetState() == 5)
            {
                player.Close();
            }
            Thread.Sleep(100);
            if (FileUtil.readSrtFile(video)&&Constant.isCache)
            {
                label2.Visible = false;
                isTran = false;
                player.SetConfig(503, FileUtil.getSrtPath(video));
                player.SetConfig(504, "1");
                
            }
            else
            {
                label2.Visible = true;
                isTran = true;
                WaitingForm waitingForm = new WaitingForm(label2,player, video);
                waitingForm.StartPosition = FormStartPosition.CenterParent;
                waitingForm.ShowDialog();
            }
            captionDic.Clear();
            open();
        }

        /// <summary>
        /// 停止播放
        /// </summary>
        private void lb_stop_Click(object sender, EventArgs e)
        {
            this.player.Close();//关闭视频
            this.lb_play.DM_Key = DMSkin.Controls.DMLabelKey.播放;//暂停->播放
            this.pgb_time.DM_Value = 0;//播放进度条置0
        }

        /// <summary>
        /// 开始播放
        /// </summary>
        private void lb_play_Click(object sender, EventArgs e)
        {
            //获取player状态
            Constant.PLAY_STATE state = (Constant.PLAY_STATE)this.player.GetState();
            //如果为播放
            if (state == Constant.PLAY_STATE.PS_PLAY)
            {
                this.player.SetConfig(602, "1");//激活视频叠图加功能, 1-激活, 0-不激活
                this.player.SetConfig(621, "1");//获取或设置 EVRCP 是否使用线形插值叠图，0-不是用，1-使用，默认0
                this.player.SetConfig(612, "暂停");//设置一段文本作为叠加图像，值为文本内容
                ClearTip.Clear(player, 1000);
                this.player.Pause();
                this.lb_play.DM_Key = DMSkin.Controls.DMLabelKey.播放;

            }
            //如果为暂停
            else if (state == Constant.PLAY_STATE.PS_PAUSED)
            {
                this.player.SetConfig(602, "1");
                this.player.SetConfig(621, "1");
                this.player.SetConfig(612, "播放");
                ClearTip.Clear(player, 1000);
                this.player.Play();
                this.lb_play.DM_Key = DMSkin.Controls.DMLabelKey.暂停;
            }
        }



        /// <summary>
        /// 播放列表的显示和隐藏
        /// </summary>
        private void lb_list_Click(object sender, EventArgs e)
        {
            if (!this.Full)//如果不是全屏
            {
                if (rightPanel.Visible)//如果显示
                {
                    rightPanel.Hide();//隐藏列表
                    player.Dock = DockStyle.Fill;
                }
                else
                {
                    rightPanel.Show();//显示列表
                }
            }
        }



        /// <summary>
        /// 设置全屏
        /// </summary>
        private void lb_max_Click(object sender, EventArgs e)
        {
            this.FullScreen();
        }



        /// <summary>
        /// 全屏操作(参考博客)
        /// </summary>
        public void FullScreen()
        {
            if (this.Full)//全屏->默认化
            {
                base.WindowState = FormWindowState.Normal;//设置为默认窗口
                this.masterPanel.Height = this.Nor.Height;
                this.player.Parent = this.masterPanel;
                this.player.Dock = DockStyle.Fill;
                this.player.BringToFront();
                this.Full = false;
            }
            else//不是全屏->全屏化
            {
                this.Nor = new Rectangle(this.player.Location, this.player.Size);
                this.player.Parent = this;
                this.player.Dock = DockStyle.None;
                this.WindowState = FormWindowState.Maximized;//最大化窗口
                this.player.BringToFront();
                this.player.Location = new System.Drawing.Point(0, 0);
                this.player.Size = this.Size;
                this.Full = true;
            }
            this.player.Focus();
        }



        /// <summary>
        /// 双击面板顶部铺满屏幕
        /// </summary>
        private void topPanel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.isMaxSize)
            {//最大->还原 
                base.Width = this.LastWidth;
                base.Height = this.LastHeight;
                base.Top = this.LastTop;
                base.Left = this.LastLeft;
                this.isMaxSize = false;
                base.CanMove = true;
                base.DM_CanResize = true;
            }
            else//铺满屏幕，与全屏有差异
            {
                this.LastWidth = base.Width;
                this.LastHeight = base.Height;
                this.LastTop = base.Top;
                this.LastLeft = base.Left;
                this.isMaxSize = true;
                base.Top = 0;
                base.Left = 0;
                base.Width = Screen.PrimaryScreen.WorkingArea.Width;
                base.Height = Screen.PrimaryScreen.WorkingArea.Height;
                base.DM_CanResize = false;
                base.CanMove = false;
            }
        }



        /// <summary>
        /// 截屏操作
        /// </summary>
        private void lb_jietu_Click(object sender, EventArgs e)
        {
            //if (this.player.GetConfig(701) == "1")//查询截图功能是否可用。
            //{
            //    this.player.SetConfig(707, "2");//截图的输出格式，1-bmp, 2-jpg, 3-png, 4-gif, 默认为 1。
            //    string dir = Settings.Default.picDir;
            //    string date = DateTime.Now.ToString("HHMMddhhmmssfff");
            //    string path = dir + "\\我的视频截图" + date + ".jpg";
            //    this.player.SetConfig(702, path);//截取当前视频图像，值为文件路径，例如："C:\snapshot.bmp"
            //    this.player.SetConfig(602, "1");//激活视频叠加图功能, 1-激活, 0-不激活
            //    this.player.SetConfig(621, "1");//获取或设置 EVRCP 是否使用线形插值叠图，0-不是用，1-使用，默认0
            //    this.player.SetConfig(612, "视频截图成功");//设置一段文本作为叠加图像，值为文本内容。
            //    ClearTip.Clear(player, 2000);
            //}
        }

        /// <summary>
        /// 最小化窗口
        /// </summary>
        private void btn_min_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        private void btn_close_Click(object sender, EventArgs e)
        {
            Settings.Default.volume = this.volume;
            Settings.Default.Save();
            if (this.timer.Enabled)
            {
                this.timer.Enabled = false;
            }
            Close();
        }

        /// <summary>
        /// 关闭程序前的工作
        /// </summary>
        private void PlayerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.volume = this.volume;
            Settings.Default.Save();
            if (this.timer.Enabled)
            {
                this.timer.Enabled = false;
            }
        }


        /// <summary>
        /// 打开视频
        /// </summary>
        private void 打开视频ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenAndPlay();
            this.player.Focus();
        }

        /// <summary>
        /// 打开文件夹
        /// </summary>
        private void 打开文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                bool isEqual = false;
                //使用委托，指定规则
                PlayHelper.LoopFolder(dialog.SelectedPath, delegate (FileInfo file)
                {
                    //新建视频实体
                    VideoItem myitem = new VideoItem(file.Name, file.FullName);
                    //如果支持此格式，并且不重复，加入视频列表
                    if (PlayHelper.isRightExt(myitem))
                    {
                        isEqual = false;
                        foreach (var item in this.videoList)
                        {
                            if (item.Name == myitem.Name)
                            {
                                isEqual = true;
                                break;
                            }
                        }
                        if (!isEqual)
                        {
                            this.videoList.Add(myitem);
                        }
                    }
                });
                //重绘播放列表
                if (this.player.GetState() == 0)
                {
                    this.InitPlayListView(null);
                }
                else
                {
                    this.InitPlayListView(this.playingItem);
                }
            }
        }

        /// <summary>
        /// 删除当前视频
        /// </summary>
        private void 删除当前视频ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var item in this.videoList)
            {
                if (item == currItem)
                {
                    if (currItem.IsPlaying)
                    {
                        this.closePlayer();//停止播放
                    }
                    this.videoList.Remove(currItem);
                    this.InitPlayListView(null);
                    break;
                }
            }
            this.player.Focus();
        }

        /// <summary>
        /// 清空视频列表
        /// </summary>
        private void 清空视频列表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.videoList = new List<VideoItem>();
            this.InitPlayListView(null);
            this.closePlayer();
            this.player.Focus();
        }

        /// <summary>
        /// 关闭player
        /// </summary>
        private void closePlayer()
        {
            this.player.Close();
            this.pgb_time.DM_Value = 0;
            this.lb_play.DM_Key = DMSkin.Controls.DMLabelKey.播放;
            this.player.Focus();
        }


        private String video = "";
        /// <summary>
        /// 点击当前播放列表的视频, 播放该视频
        /// </summary>
        private void list_play_ItemClick(object sender, EventArgs e)
        {
            Item clickedItem = (Item)sender;
            foreach (var item in this.videoList)
            {
                if (item.Name == clickedItem.Text)
                {
                    this.playingItem = item;//设置正在播放item
                    item.IsPlaying = true;
                }
                else
                {
                    item.IsPlaying = false;
                }
            }
            this.InitPlayListView(playingItem);
            video = playingItem.Url;
            this.player.Open(playingItem.Url);
            this.lb_play.DM_Key = DMSkin.Controls.DMLabelKey.暂停;
            this.player.Focus();
        }

        /// <summary>
        /// 右键视频列表显示菜单
        /// </summary>
        private void list_play_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)//右键
            {
                //var x = this.PointToClient(Control.MousePosition).X;
                //var y = this.PointToClient(Control.MousePosition).Y - 40;
                //this.metroContextMenu1.Show(player, new Point(x, y));
            }
        }

        private void list_play_rightItemClick(object sender, EventArgs e)
        {
            MouseEventArgs mouse_e = (MouseEventArgs)e;
            if (mouse_e.Button == MouseButtons.Right)//右键
            {
                //Item clickedItem = (Item)sender;
                //foreach (var item in this.videoList)//遍历视频列表
                //{
                //    if (item.Name == clickedItem.Text)
                //    {
                //        this.currItem = item;//设置被选中视频
                //        if (clickedItem.OnLine == true)
                //        {
                //            this.currItem.IsPlaying = true;
                //        }
                //        break;
                //    }
                //}
                //var x = this.PointToClient(Control.MousePosition).X;
                //var y = this.PointToClient(Control.MousePosition).Y - 40;
                //this.删除当前视频ToolStripMenuItem.Enabled = true;
                //this.metroContextMenu1.Show(player, new Point(x, y));
            }
        }

        /// <summary>
        /// 鼠标：音量抬起
        /// </summary>
        private void pgb_vol_MouseUp(object sender, MouseEventArgs e)
        {
            this.IsMouseDown = false;
            this.player.Focus();
        }

        /// <summary>
        /// 鼠标：改变音量
        /// </summary>
        private void pgb_vol_MouseMove(object sender, MouseEventArgs e)
        {
            int num = 1;
            if (this.IsMouseDown)
            {
                this.volume = ((int)this.pgb_vol.DM_Value) * num;
                this.player.SetConfig(602, "1");//激活视频叠加图功能, 1-激活, 0-不激活
                this.player.SetConfig(621, "1");//获取或设置 EVRCP 是否使用线形插值叠图，0-不是用，1-使用，默认0
                this.player.SetConfig(612, "音量 (" + this.volume + "%)");//设置一段文本作为叠加图像，值为文本内容，支持回车换行符主动换行和自动换行
                ClearTip.Clear(player, 1000);
                UpdateVol();//更新喇叭音量视图效果
            }
        }

        /// <summary>
        /// 更新喇叭音量视图效果
        /// </summary>
        public void UpdateVol()
        {
            this.player.SetVolume(this.volume);
            this.pgb_vol.DM_Value = this.volume;
            if (this.volume == 0)
            {
                lb_vol.DM_Key = DMSkin.Controls.DMLabelKey.音量_无;
            }
            else if (this.volume > 0 && this.volume <= 30)
            {
                lb_vol.DM_Key = DMSkin.Controls.DMLabelKey.音量_小;

            }
            else if (this.volume > 30 && this.volume <= 50)
            {
                lb_vol.DM_Key = DMSkin.Controls.DMLabelKey.音量_小;
            }
            else
            {
                lb_vol.DM_Key = DMSkin.Controls.DMLabelKey.音量_大;
            }
        }

        /// <summary>
        /// 鼠标：按住音量
        /// </summary>
        private void pgb_vol_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.pgb_vol.Cursor == Cursors.Hand)
            {
                this.IsMouseDown = true;
            }
        }

        private void player_OnDownloadCodec(object sender, AxAPlayer3Lib._IPlayerEvents_OnDownloadCodecEvent e)
        {
            MessageBox.Show(e.strCodecPath);

        }

        private void player_OnStateChanged(object sender, AxAPlayer3Lib._IPlayerEvents_OnStateChangedEvent e)
        {
            if (e.nNewState == 4)
            {
                Console.WriteLine("PS_PLAYING");
            }
            if (e.nNewState == 5)
            {
                int SH = Screen.PrimaryScreen.Bounds.Height;
                int SW = Screen.PrimaryScreen.Bounds.Width;
                Height = SH / 3 * 2;
                Width = SW / 2;
                player.SetConfig(204, 16 + ";" + 9);
            }
            if (e.nNewState == 6)
            {
                Console.WriteLine("PS_CLOSING");
            }
        }

        private void player_OnOpenSucceeded(object sender, EventArgs e)
        {
            if (isTran)
            {
                int times = Convert.ToInt32(player.GetDuration() / 1000 / 7);
                //切视频
                CaptionThread captionThread = new CaptionThread(times, video, null);
                //Thread thread = new Thread(captionThread.startCaption);
                Thread thread = new Thread(captionThread.startVideo);
                thread.IsBackground = true;
                thread.Start();

                //百度api翻译
                BaiduApi api = BaiduApi.getInstance(video, captionDic, times);
                Thread thread1 = new Thread(api.recoVideo);
                //Thread thread1 = new Thread(captionThread.startVideo);
                //thread1.Start();
                thread1.IsBackground = true;
                thread1.Start();
            }
        }

        private void player_OnBuffer(object sender, AxAPlayer3Lib._IPlayerEvents_OnBufferEvent e)
        {
            Console.WriteLine(e.nPercent);
        }

        private void player_OnVideoSizeChanged(object sender, EventArgs e)
        {
            int SH = Screen.PrimaryScreen.Bounds.Height;
            int SW = Screen.PrimaryScreen.Bounds.Width;
            player.SetConfig(204, SW + ";" + SH);
        }

       

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void TopPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        /// <summary>
        /// 点击视频进度条, 根据当前光标位置计算进度条的百分比, 修改播放进度
        /// </summary>
        private void pgb_time_Click(object sender, EventArgs e)
        {
            var leftSpace = 10;
            var rightSpace = 10;
            var width = this.pgb_time.Size.Width;
            //光标指定位置变为点
            Point p = pgb_time.PointToClient(MousePosition);
            // 先判断一下p点的y坐标，在进度条间距内
            bool validY = p.Y <= 20 && p.Y >= 8;
            bool validX = p.X >= leftSpace && p.X <= width - rightSpace;
            if (validX && validY)
            {
                var offset = p.X - leftSpace;
                var realLength = width - leftSpace - rightSpace;
                var dur = this.player.GetDuration();
                var pos = (int)(1.0 * dur * offset / realLength);
                timer.Stop();
                this.player.SetPosition(pos);//设置当前位置
                timer.Start();
            }
        }

        /// <summary>
        /// 抬起视频进度条
        /// </summary>
        private void pgb_time_MouseUp(object sender, MouseEventArgs e)
        {
            this.IsMouseDown = false;
            switch (((Constant.PLAY_STATE)this.player.GetState()))
            {
                case Constant.PLAY_STATE.PS_PLAY:
                case Constant.PLAY_STATE.PS_PAUSED:

                    if (!(this.timer.Enabled || (this.player.GetPosition() == 0)))
                    {
                        double num = this.pgb_time.DM_Value;
                        double num2 = num / (100.0) * this.duration;
                        this.player.SetPosition(Convert.ToInt32(num2));
                        this.timer.Start();
                    }
                    break;
            }
        }

        /// <summary>
        /// 按下视频进度条
        /// </summary>
        private void pgb_time_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.pgb_time.Cursor == Cursors.Hand)
            {
                this.IsMouseDown = true;
                this.timer.Stop();
            }
        }

    }
}
