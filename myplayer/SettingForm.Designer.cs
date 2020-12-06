namespace myplayer
{
    partial class SettingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.closeBtn = new DMSkin.Controls.DMButtonCloseLight();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lb_choose = new DMSkin.Controls.DMLabel();
            this.dmTextBox1 = new DMSkin.Controls.DMTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cb_ordin = new System.Windows.Forms.ComboBox();
            this.cb_goal = new System.Windows.Forms.ComboBox();
            this.lb = new System.Windows.Forms.Label();
            this.cb_storage = new System.Windows.Forms.CheckBox();
            this.btn_save = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // closeBtn
            // 
            this.closeBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.closeBtn.BackColor = System.Drawing.Color.Transparent;
            this.closeBtn.Location = new System.Drawing.Point(406, 1);
            this.closeBtn.Margin = new System.Windows.Forms.Padding(4);
            this.closeBtn.MaximumSize = new System.Drawing.Size(30, 27);
            this.closeBtn.MinimumSize = new System.Drawing.Size(30, 27);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(30, 27);
            this.closeBtn.TabIndex = 1;
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.LightCoral;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(-2, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "设置";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label2.Location = new System.Drawing.Point(46, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 21);
            this.label2.TabIndex = 3;
            this.label2.Text = "图片存储位置";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label3.Location = new System.Drawing.Point(236, 145);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 12);
            this.label3.TabIndex = 6;
            // 
            // lb_choose
            // 
            this.lb_choose.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lb_choose.BackColor = System.Drawing.Color.Transparent;
            this.lb_choose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lb_choose.DM_Color = System.Drawing.Color.White;
            this.lb_choose.DM_Font_Size = 13F;
            this.lb_choose.DM_Key = DMSkin.Controls.DMLabelKey.文件夹_打开;
            this.lb_choose.DM_Text = "";
            this.lb_choose.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_choose.Location = new System.Drawing.Point(360, 85);
            this.lb_choose.Name = "lb_choose";
            this.lb_choose.Size = new System.Drawing.Size(30, 53);
            this.lb_choose.TabIndex = 34;
            this.lb_choose.Text = "dmLabel1";
            this.lb_choose.Click += new System.EventHandler(this.lb_choose_Click);
            this.lb_choose.MouseEnter += new System.EventHandler(this.btns_MouseEnter);
            this.lb_choose.MouseLeave += new System.EventHandler(this.btns_MouseLeave);
            // 
            // dmTextBox1
            // 
            this.dmTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dmTextBox1.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.dmTextBox1.Location = new System.Drawing.Point(50, 85);
            this.dmTextBox1.Name = "dmTextBox1";
            this.dmTextBox1.Size = new System.Drawing.Size(295, 23);
            this.dmTextBox1.TabIndex = 35;
            this.dmTextBox1.WaterText = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label4.Location = new System.Drawing.Point(46, 121);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 21);
            this.label4.TabIndex = 36;
            this.label4.Text = "当前语言";
            // 
            // cb_ordin
            // 
            this.cb_ordin.FormattingEnabled = true;
            this.cb_ordin.Items.AddRange(new object[] {
            "auto",
            "en",
            "zh"});
            this.cb_ordin.Location = new System.Drawing.Point(50, 154);
            this.cb_ordin.Name = "cb_ordin";
            this.cb_ordin.Size = new System.Drawing.Size(121, 20);
            this.cb_ordin.TabIndex = 37;
            // 
            // cb_goal
            // 
            this.cb_goal.FormattingEnabled = true;
            this.cb_goal.Items.AddRange(new object[] {
            "en",
            "zh"});
            this.cb_goal.Location = new System.Drawing.Point(50, 226);
            this.cb_goal.Name = "cb_goal";
            this.cb_goal.Size = new System.Drawing.Size(121, 20);
            this.cb_goal.TabIndex = 39;
            // 
            // lb
            // 
            this.lb.AutoSize = true;
            this.lb.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lb.Location = new System.Drawing.Point(46, 193);
            this.lb.Name = "lb";
            this.lb.Size = new System.Drawing.Size(74, 21);
            this.lb.TabIndex = 38;
            this.lb.Text = "目标语言";
            // 
            // cb_storage
            // 
            this.cb_storage.AutoSize = true;
            this.cb_storage.Location = new System.Drawing.Point(50, 266);
            this.cb_storage.Name = "cb_storage";
            this.cb_storage.Size = new System.Drawing.Size(120, 16);
            this.cb_storage.TabIndex = 40;
            this.cb_storage.Text = "是否缓存字幕数据";
            this.cb_storage.UseVisualStyleBackColor = true;
            // 
            // btn_save
            // 
            this.btn_save.Location = new System.Drawing.Point(172, 294);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(75, 23);
            this.btn_save.TabIndex = 41;
            this.btn_save.Text = "保存";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightCoral;
            this.ClientSize = new System.Drawing.Size(443, 334);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.cb_storage);
            this.Controls.Add(this.cb_goal);
            this.Controls.Add(this.lb);
            this.Controls.Add(this.cb_ordin);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dmTextBox1);
            this.Controls.Add(this.lb_choose);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.closeBtn);
            this.DM_BackToColor = false;
            this.Name = "SettingForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SettingForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DMSkin.Controls.DMButtonCloseLight closeBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private DMSkin.Controls.DMLabel lb_choose;
        private DMSkin.Controls.DMTextBox dmTextBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cb_ordin;
        private System.Windows.Forms.ComboBox cb_goal;
        private System.Windows.Forms.Label lb;
        private System.Windows.Forms.CheckBox cb_storage;
        private System.Windows.Forms.Button btn_save;
    }
}