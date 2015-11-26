namespace xCsdn
{
    partial class FormSetting
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
            this.label1 = new System.Windows.Forms.Label();
            this.txt_USER = new System.Windows.Forms.TextBox();
            this.btn_Cancle = new System.Windows.Forms.Button();
            this.txt_pass = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_downSleep = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_comSleep = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_savePath = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btn_OK = new System.Windows.Forms.Button();
            this.btn_brw = new System.Windows.Forms.Button();
            this.checkbox_savefile = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "帐    号：";
            // 
            // txt_USER
            // 
            this.txt_USER.Location = new System.Drawing.Point(70, 17);
            this.txt_USER.Name = "txt_USER";
            this.txt_USER.Size = new System.Drawing.Size(100, 21);
            this.txt_USER.TabIndex = 1;
            // 
            // btn_Cancle
            // 
            this.btn_Cancle.Location = new System.Drawing.Point(260, 185);
            this.btn_Cancle.Name = "btn_Cancle";
            this.btn_Cancle.Size = new System.Drawing.Size(78, 22);
            this.btn_Cancle.TabIndex = 2;
            this.btn_Cancle.Text = "撤销修改";
            this.btn_Cancle.UseVisualStyleBackColor = true;
            this.btn_Cancle.Click += new System.EventHandler(this.btn_Cancle_Click);
            // 
            // txt_pass
            // 
            this.txt_pass.Location = new System.Drawing.Point(260, 17);
            this.txt_pass.Name = "txt_pass";
            this.txt_pass.Size = new System.Drawing.Size(100, 21);
            this.txt_pass.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(202, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "密    码：";
            // 
            // txt_downSleep
            // 
            this.txt_downSleep.Location = new System.Drawing.Point(70, 65);
            this.txt_downSleep.Name = "txt_downSleep";
            this.txt_downSleep.Size = new System.Drawing.Size(100, 21);
            this.txt_downSleep.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "下载延时:";
            // 
            // txt_comSleep
            // 
            this.txt_comSleep.Location = new System.Drawing.Point(260, 68);
            this.txt_comSleep.Name = "txt_comSleep";
            this.txt_comSleep.Size = new System.Drawing.Size(100, 21);
            this.txt_comSleep.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(202, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "评分延时：";
            // 
            // txt_savePath
            // 
            this.txt_savePath.Location = new System.Drawing.Point(70, 145);
            this.txt_savePath.Name = "txt_savePath";
            this.txt_savePath.Size = new System.Drawing.Size(290, 21);
            this.txt_savePath.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 148);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "保存目录";
            // 
            // btn_OK
            // 
            this.btn_OK.Location = new System.Drawing.Point(70, 185);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(78, 22);
            this.btn_OK.TabIndex = 13;
            this.btn_OK.Text = "保存修改";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // btn_brw
            // 
            this.btn_brw.Location = new System.Drawing.Point(366, 145);
            this.btn_brw.Name = "btn_brw";
            this.btn_brw.Size = new System.Drawing.Size(78, 22);
            this.btn_brw.TabIndex = 14;
            this.btn_brw.Text = "浏览";
            this.btn_brw.UseVisualStyleBackColor = true;
            this.btn_brw.Click += new System.EventHandler(this.btn_brw_Click);
            // 
            // checkbox_savefile
            // 
            this.checkbox_savefile.AutoSize = true;
            this.checkbox_savefile.Location = new System.Drawing.Point(14, 113);
            this.checkbox_savefile.Name = "checkbox_savefile";
            this.checkbox_savefile.Size = new System.Drawing.Size(96, 16);
            this.checkbox_savefile.TabIndex = 15;
            this.checkbox_savefile.Text = "保存下载文件";
            this.checkbox_savefile.UseVisualStyleBackColor = true;
            // 
            // FormSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 213);
            this.Controls.Add(this.checkbox_savefile);
            this.Controls.Add(this.btn_brw);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.txt_savePath);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txt_comSleep);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txt_downSleep);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txt_pass);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_Cancle);
            this.Controls.Add(this.txt_USER);
            this.Controls.Add(this.label1);
            this.Name = "FormSetting";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.FormSetting_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_USER;
        private System.Windows.Forms.Button btn_Cancle;
        private System.Windows.Forms.TextBox txt_pass;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_downSleep;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_comSleep;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_savePath;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Button btn_brw;
        private System.Windows.Forms.CheckBox checkbox_savefile;

    }
}