namespace xCsdn
{
    partial class FormPub
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_auto = new System.Windows.Forms.Button();
            this.txt_regNum = new System.Windows.Forms.TextBox();
            this.txt_regPass = new System.Windows.Forms.TextBox();
            this.btn_export = new System.Windows.Forms.Button();
            this.btn_reg = new System.Windows.Forms.Button();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.登录选中ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.换随机色ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.选中模拟评分ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.选中模拟下载ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.终止选中行为ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.更改配置toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除用户ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.获取上传的资源ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.下载上传的资源ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.自动刷分ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.全部登录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.已登录评分ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.已登录下载ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.终止所有用户行为ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.清空用户ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listView2 = new System.Windows.Forms.ListView();
            this.日期 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.用户 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.操作 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.结果 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.验证码 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.附加 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.log = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.btn_auto);
            this.groupBox1.Controls.Add(this.txt_regNum);
            this.groupBox1.Controls.Add(this.txt_regPass);
            this.groupBox1.Controls.Add(this.btn_export);
            this.groupBox1.Controls.Add(this.btn_reg);
            this.groupBox1.Controls.Add(this.txtUser);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtPass);
            this.groupBox1.Controls.Add(this.btnLogin);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(743, 65);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "账号登录";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(245, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 12;
            this.label5.Text = "注册个数：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(2, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "注册密码：";
            // 
            // btn_auto
            // 
            this.btn_auto.Location = new System.Drawing.Point(603, 42);
            this.btn_auto.Name = "btn_auto";
            this.btn_auto.Size = new System.Drawing.Size(113, 23);
            this.btn_auto.TabIndex = 10;
            this.btn_auto.Text = "自动注册刷分";
            this.btn_auto.UseVisualStyleBackColor = true;
            this.btn_auto.Click += new System.EventHandler(this.btn_auto_Click);
            // 
            // txt_regNum
            // 
            this.txt_regNum.Location = new System.Drawing.Point(316, 42);
            this.txt_regNum.Name = "txt_regNum";
            this.txt_regNum.Size = new System.Drawing.Size(173, 21);
            this.txt_regNum.TabIndex = 9;
            this.txt_regNum.Text = "2";
            this.txt_regNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txt_regPass
            // 
            this.txt_regPass.Location = new System.Drawing.Point(69, 44);
            this.txt_regPass.Name = "txt_regPass";
            this.txt_regPass.Size = new System.Drawing.Size(175, 21);
            this.txt_regPass.TabIndex = 8;
            this.txt_regPass.Text = "yqmacCSDN";
            // 
            // btn_export
            // 
            this.btn_export.Location = new System.Drawing.Point(618, 12);
            this.btn_export.Name = "btn_export";
            this.btn_export.Size = new System.Drawing.Size(75, 23);
            this.btn_export.TabIndex = 6;
            this.btn_export.Text = "导出";
            this.btn_export.UseVisualStyleBackColor = true;
            this.btn_export.Click += new System.EventHandler(this.btn_export_Click);
            // 
            // btn_reg
            // 
            this.btn_reg.Location = new System.Drawing.Point(522, 42);
            this.btn_reg.Name = "btn_reg";
            this.btn_reg.Size = new System.Drawing.Size(75, 23);
            this.btn_reg.TabIndex = 5;
            this.btn_reg.Text = "注  册";
            this.btn_reg.UseVisualStyleBackColor = true;
            this.btn_reg.Click += new System.EventHandler(this.btn_reg_Click);
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(69, 14);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(175, 21);
            this.txtUser.TabIndex = 1;
            this.txtUser.Text = "wumeijmh@163.com";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "用户名：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(257, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "密  码：";
            // 
            // txtPass
            // 
            this.txtPass.Location = new System.Drawing.Point(316, 14);
            this.txtPass.Name = "txtPass";
            this.txtPass.PasswordChar = '*';
            this.txtPass.Size = new System.Drawing.Size(175, 21);
            this.txtPass.TabIndex = 1;
            this.txtPass.Text = "5885920";
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(522, 12);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 2;
            this.btnLogin.Text = "添  加";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // listView1
            // 
            this.listView1.AllowDrop = true;
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(0, 71);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(743, 183);
            this.listView1.TabIndex = 8;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.listView1_DragDrop);
            this.listView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.listView1_DragEnter);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "序号";
            this.columnHeader1.Width = 40;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "帐号";
            this.columnHeader2.Width = 150;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "密码";
            this.columnHeader3.Width = 150;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "状态";
            this.columnHeader4.Width = 290;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.登录选中ToolStripMenuItem,
            this.换随机色ToolStripMenuItem,
            this.选中模拟评分ToolStripMenuItem,
            this.选中模拟下载ToolStripMenuItem,
            this.终止选中行为ToolStripMenuItem,
            this.更改配置toolStripMenuItem,
            this.删除用户ToolStripMenuItem,
            this.获取上传的资源ToolStripMenuItem,
            this.下载上传的资源ToolStripMenuItem,
            this.自动刷分ToolStripMenuItem,
            this.toolStripSeparator1,
            this.全部登录ToolStripMenuItem,
            this.已登录评分ToolStripMenuItem,
            this.已登录下载ToolStripMenuItem,
            this.终止所有用户行为ToolStripMenuItem,
            this.清空用户ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(173, 340);
            // 
            // 登录选中ToolStripMenuItem
            // 
            this.登录选中ToolStripMenuItem.Name = "登录选中ToolStripMenuItem";
            this.登录选中ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.登录选中ToolStripMenuItem.Text = "登录";
            this.登录选中ToolStripMenuItem.Click += new System.EventHandler(this.登录选中ToolStripMenuItem_Click);
            // 
            // 换随机色ToolStripMenuItem
            // 
            this.换随机色ToolStripMenuItem.Name = "换随机色ToolStripMenuItem";
            this.换随机色ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.换随机色ToolStripMenuItem.Text = "换随机色";
            this.换随机色ToolStripMenuItem.Click += new System.EventHandler(this.换随机色ToolStripMenuItem_Click);
            // 
            // 选中模拟评分ToolStripMenuItem
            // 
            this.选中模拟评分ToolStripMenuItem.Name = "选中模拟评分ToolStripMenuItem";
            this.选中模拟评分ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.选中模拟评分ToolStripMenuItem.Text = "模拟评分";
            this.选中模拟评分ToolStripMenuItem.Click += new System.EventHandler(this.自动评分ToolStripMenuItem_Click);
            // 
            // 选中模拟下载ToolStripMenuItem
            // 
            this.选中模拟下载ToolStripMenuItem.Name = "选中模拟下载ToolStripMenuItem";
            this.选中模拟下载ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.选中模拟下载ToolStripMenuItem.Text = "模拟下载";
            this.选中模拟下载ToolStripMenuItem.Click += new System.EventHandler(this.自动模拟下载ToolStripMenuItem_Click);
            // 
            // 终止选中行为ToolStripMenuItem
            // 
            this.终止选中行为ToolStripMenuItem.Name = "终止选中行为ToolStripMenuItem";
            this.终止选中行为ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.终止选中行为ToolStripMenuItem.Text = "终止行为";
            this.终止选中行为ToolStripMenuItem.Click += new System.EventHandler(this.终止账户行为ToolStripMenuItem_Click);
            // 
            // 更改配置toolStripMenuItem
            // 
            this.更改配置toolStripMenuItem.Name = "更改配置toolStripMenuItem";
            this.更改配置toolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.更改配置toolStripMenuItem.Text = "更改配置";
            this.更改配置toolStripMenuItem.Click += new System.EventHandler(this.更改配置toolStripMenuItem_Click);
            // 
            // 删除用户ToolStripMenuItem
            // 
            this.删除用户ToolStripMenuItem.Name = "删除用户ToolStripMenuItem";
            this.删除用户ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.删除用户ToolStripMenuItem.Text = "删除用户";
            this.删除用户ToolStripMenuItem.Click += new System.EventHandler(this.删除用户ToolStripMenuItem_Click);
            // 
            // 获取上传的资源ToolStripMenuItem
            // 
            this.获取上传的资源ToolStripMenuItem.Name = "获取上传的资源ToolStripMenuItem";
            this.获取上传的资源ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.获取上传的资源ToolStripMenuItem.Text = "获取上传资源";
            this.获取上传的资源ToolStripMenuItem.Click += new System.EventHandler(this.获取上传的资源ToolStripMenuItem_Click);
            // 
            // 下载上传的资源ToolStripMenuItem
            // 
            this.下载上传的资源ToolStripMenuItem.Name = "下载上传的资源ToolStripMenuItem";
            this.下载上传的资源ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.下载上传的资源ToolStripMenuItem.Text = "下载上传资源";
            this.下载上传的资源ToolStripMenuItem.Click += new System.EventHandler(this.下载上传的资源ToolStripMenuItem_Click);
            // 
            // 自动刷分ToolStripMenuItem
            // 
            this.自动刷分ToolStripMenuItem.Name = "自动刷分ToolStripMenuItem";
            this.自动刷分ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.自动刷分ToolStripMenuItem.Text = "自动刷分";
            this.自动刷分ToolStripMenuItem.Click += new System.EventHandler(this.自动刷分ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(169, 6);
            // 
            // 全部登录ToolStripMenuItem
            // 
            this.全部登录ToolStripMenuItem.Name = "全部登录ToolStripMenuItem";
            this.全部登录ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.全部登录ToolStripMenuItem.Text = "全部登录";
            this.全部登录ToolStripMenuItem.Click += new System.EventHandler(this.全部登录ToolStripMenuItem_Click);
            // 
            // 已登录评分ToolStripMenuItem
            // 
            this.已登录评分ToolStripMenuItem.Name = "已登录评分ToolStripMenuItem";
            this.已登录评分ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.已登录评分ToolStripMenuItem.Text = "批量评分";
            this.已登录评分ToolStripMenuItem.Click += new System.EventHandler(this.已登录评分ToolStripMenuItem_Click);
            // 
            // 已登录下载ToolStripMenuItem
            // 
            this.已登录下载ToolStripMenuItem.Name = "已登录下载ToolStripMenuItem";
            this.已登录下载ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.已登录下载ToolStripMenuItem.Text = "批量下载";
            this.已登录下载ToolStripMenuItem.Click += new System.EventHandler(this.已登录下载ToolStripMenuItem_Click);
            // 
            // 终止所有用户行为ToolStripMenuItem
            // 
            this.终止所有用户行为ToolStripMenuItem.Name = "终止所有用户行为ToolStripMenuItem";
            this.终止所有用户行为ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.终止所有用户行为ToolStripMenuItem.Text = "终止所有用户行为";
            this.终止所有用户行为ToolStripMenuItem.Click += new System.EventHandler(this.终止所有用户行为ToolStripMenuItem_Click);
            // 
            // 清空用户ToolStripMenuItem
            // 
            this.清空用户ToolStripMenuItem.Name = "清空用户ToolStripMenuItem";
            this.清空用户ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.清空用户ToolStripMenuItem.Text = "清空用户";
            this.清空用户ToolStripMenuItem.Click += new System.EventHandler(this.清空用户ToolStripMenuItem_Click);
            // 
            // listView2
            // 
            this.listView2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.日期,
            this.用户,
            this.操作,
            this.结果,
            this.验证码,
            this.附加,
            this.log});
            this.listView2.FullRowSelect = true;
            this.listView2.GridLines = true;
            this.listView2.Location = new System.Drawing.Point(0, 260);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(743, 272);
            this.listView2.TabIndex = 10;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            // 
            // 日期
            // 
            this.日期.Text = "日期";
            this.日期.Width = 100;
            // 
            // 用户
            // 
            this.用户.Text = "用户";
            this.用户.Width = 130;
            // 
            // 操作
            // 
            this.操作.Text = "";
            this.操作.Width = 100;
            // 
            // 结果
            // 
            this.结果.Text = "";
            this.结果.Width = 100;
            // 
            // 验证码
            // 
            this.验证码.Text = "";
            this.验证码.Width = 130;
            // 
            // 附加
            // 
            this.附加.Text = "";
            this.附加.Width = 100;
            // 
            // log
            // 
            this.log.Text = "";
            this.log.Width = 100;
            // 
            // FormPub
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(743, 530);
            this.Controls.Add(this.listView2);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormPub";
            this.Text = "FormPub";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormPub_FormClosing);
            this.Load += new System.EventHandler(this.FormPub_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 选中模拟评分ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 选中模拟下载ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 终止选中行为ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 登录选中ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 全部登录ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 已登录评分ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 已登录下载ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 终止所有用户行为ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 更改配置toolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 删除用户ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 清空用户ToolStripMenuItem;
        private System.Windows.Forms.Button btn_reg;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.ColumnHeader 日期;
        private System.Windows.Forms.ColumnHeader 用户;
        private System.Windows.Forms.ColumnHeader 操作;
        private System.Windows.Forms.ColumnHeader 结果;
        private System.Windows.Forms.ColumnHeader 验证码;
        private System.Windows.Forms.ColumnHeader 附加;
        private System.Windows.Forms.ColumnHeader log;
        private System.Windows.Forms.ToolStripMenuItem 获取上传的资源ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 下载上传的资源ToolStripMenuItem;
        private System.Windows.Forms.Button btn_export;
        private System.Windows.Forms.Button btn_auto;
        private System.Windows.Forms.TextBox txt_regNum;
        private System.Windows.Forms.TextBox txt_regPass;
        private System.Windows.Forms.ToolStripMenuItem 自动刷分ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 换随机色ToolStripMenuItem;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
    }
}