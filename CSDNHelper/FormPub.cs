using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace xCsdn
{
    public partial class FormPub : Form
    {
        public FormPub()
        {
            InitializeComponent();
            new yq.FormHelper().formMinIco(this ,null,null,true );
        }
        List<CsdnHelper> listCsdnH = new List<CsdnHelper>();
        string[] msgs = new string[] {
            "参考学习一下，谢，感觉还可以",
            "不错，感觉还可以，有借鉴的地方", 
            "还行，可以借鉴一下，就是下载太慢了，网速不给力，刚才也是",
            "还不错，可以参考参考", 
            "谢 谢分  享，帮助了我的开发学习", 
            "谢 谢分享,可以 收 藏，菜鸟一个，拿走学习一下", 
            "不错的 资源，可以作为参考", 
            "还可 以吧，可以参 考  参 考", 
            "谢分享，这些资料很全面,太有用了比我在网上找的好多了",
            "很好，可 以好好研 究一下",
            "很全面,很好用,谢 分 享",
            "挺不错的资料，感谢慷慨，我就拿走了", 
            "比相关书 籍介绍的详细,顶一个",
            "还行,适合于初  级入门的学习",
            "很好 的资料,很齐全,谢 谢",
            "还可以,就是感觉有 点乱", 
            "感 謝LZ收集,用起 來挺方便",
            "感觉还 行,只 是感觉用着不是特别顺手",
            "很有学习价值的文 档,感 谢", 
            "内容很丰富,最可 贵的是资源不需要很多积分", 
            "这个真的非 常好,借鉴意义蛮大",
            "有不少例子 可以 参考,目前正需要",
            "下载后不能 正常使用，麻烦看一下哈",
            "例子简单 实用,但如果再全面些就更好了" };
        int CdsIndex_n=-1;
        int CdsIndex_wn=-1;
        int CdsIndex_reg = -1;
        Dictionary <string,string > users = new Dictionary<string,string> ();
        Dictionary<string, Color> userColor = new Dictionary<string, Color>();
        Random ran = new Random();

        #region dlls


        [DllImport("AntiVC.dll")]
        public static extern int LoadCdsFromFile(string FilePath);

        [DllImport("AntiVC.dll")]
        public static extern int LoadCdsFromBuffer(byte[] FileBuffer, int FileBufLen);

        [DllImport("AntiVC.dll")]
        public static extern bool GetVcodeFromFile(int CdsFileIndex, string FilePath, StringBuilder Vcode);

        [DllImport("AntiVC.dll")]
        public static extern bool GetVcodeFromBuffer(int CdsFileIndex, byte[] FileBuffer, int ImgBufLen, StringBuilder Vcode);

        [DllImport("AntiVC.dll")]
        public static extern bool GetVcodeFromURL(int CdsFileIndex, string ImgURL, StringBuilder Vcode);

        #endregion dlls

        public Color  GetRandomColor()
        {
            //Random RandomNum_First = new Random((int)DateTime.Now.Ticks);
            //  对于C#的随机数，没什么好说的
           // System.Threading.Thread.Sleep(RandomNum_First.Next(50));
            //Random RandomNum_Sencond = new Random((int)DateTime.Now.Ticks);

            //  为了在白色背景上显示，尽量生成深色
            int int_Red = ran .Next(256);
            int int_Green = ran.Next(256);
            int int_Blue = (int_Red + int_Green > 400) ? 0 : 400 - int_Red - int_Green;
            int_Blue = (int_Blue > 255) ? 255 : int_Blue;

            Color color = Color.FromArgb(int_Red, int_Green, int_Blue);
            if (userColor.ContainsValue(color))
            {
                return GetRandomColor();
            }
            return color;
        }
        public string getRegVcode(byte[] byts)
        {
            StringBuilder strb = new StringBuilder();
            if (CdsIndex_reg != -1 && GetVcodeFromBuffer(CdsIndex_reg, byts, byts.Length, strb))
                {
                    int a;
                    if (strb.Length == 5)
                    {
                        return strb.ToString();
                    }
                }
            return strb.ToString();
        }

        public string getVcode(byte[] byts)
        {
            //FormTmp fm = new FormTmp(byts);
            //fm.ShowDialog();

            StringBuilder strb = new StringBuilder();
            if (byts.Length < 1100)
            {
              
                if (CdsIndex_n != -1 && GetVcodeFromBuffer(CdsIndex_n, byts, byts.Length, strb))
                {
                    int a;
                    if (strb.Length == 4 && int.TryParse(strb.ToString(), out a))
                    {
                        return strb.ToString();
                    }
                }
            }
            else
            {
                if (CdsIndex_wn != -1 && GetVcodeFromBuffer(CdsIndex_wn, byts, byts.Length, strb))
                {
                    int a;
                    if (strb.Length == 4)
                    {
                        return strb.ToString();
                    }
                }
            }

            return "";

        }
        public void showlogs(string a)
        {
            bool succ = false;
            ListViewItem lvi = new ListViewItem();
            lvi.UseItemStyleForSubItems = false;
            Color clrO;
            Color clrs;

            string[] msgs = a.Split('\t');
            if(msgs .Length >3&&msgs [3].Contains ("失败")){
                yq.LogHelper.Debug(a );
                clrs  = Color.Red ;
            }
            else
            {
                clrs = Color.Green ;
            }
            if (userColor.ContainsKey(msgs[1]))
            {
               clrO  = userColor[msgs[1]];
            }
            else
            {
                clrO = Color.Red;
            }

            lvi.Text = msgs[0];
            for (int i = 1; i < 7; i++)
            {
                string mm = "";
                if (i < msgs.Length)
                {
                    mm = msgs[i];
                }
                else
                {
                    mm = "";
                }
                lvi.SubItems.Add(mm);
                lvi.SubItems[i].ForeColor = clrO;
            } 
            lvi.SubItems[3].ForeColor =clrs ;

            this.Invoke(new Action(() => { 
                    listView2.Items.Insert(0, lvi); 
            }));
        }

        private void btn_Login(CsdnHelper cs=null )
        {
            string user = "";
            string pass = "";
            if (cs!=null &&cs.User  != null)
            {
                user = cs.User;
                pass = cs.Pass;
            }else {
                user = txtUser.Text.Trim();
                pass = txtPass.Text;
            }

            if (!users.ContainsKey(user ))
            {
                users.Add(user,pass  );
                //userColor.Add(txtUser.Text.Trim(), GetRandomColor());
                ListViewItem lvi = new ListViewItem();
                lvi.Text = listView1.Items.Count.ToString();
                lvi.SubItems.Add(user);
                lvi.SubItems.Add(pass);
                lvi.SubItems.Add("未登录");
                if (cs != null)
                {
                    lvi.Tag = cs;
                }
                //lvi .ForeColor =userColor [txtUser.Text .Trim ()];
                //lvi.Tag = cdh;
                int index = listView1.Items.Add(lvi).Index ;
                if(cs !=null )
                {
                    LoginTest(user , pass, index);
                }
                //CsdnHelper cdh = new CsdnHelper(txtUser.Text.Trim(), txtPass.Text);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            btn_Login();

        }

        public void LoginTest(string user, string pass,int index)
        {
            for (int i = 0; i < listCsdnH.Count; i++)
            {
                if (listCsdnH[i].User == user)
                {
                    return;
                }
            }

            CsdnHelper cdh = null;
            bool tag = false;
            this.Invoke(new Action(() => {
                if (listView1.Items[index].Tag != null)
                {
                    tag = true;
                    cdh = (CsdnHelper)listView1.Items[index].Tag;
                }
            }));
            if (!tag)
            {

                cdh = new CsdnHelper(user, pass);
                cdh.getImgVcode += new CsdnHelper.GetImgVcode(getVcode);
                cdh.showLogs += new CsdnHelper.ShowLogs(showlogs);
                cdh.regeristResult += new CsdnHelper.RegeristResult(dealRegcsdn);
                cdh.getRegVcode += new CsdnHelper.GetRegVcode(getRegVcode);
                cdh.ComMsg = msgs;
            }
            string sta = "";
            if ((sta=cdh.Login()).Contains("成功"))
            {
                if (!userColor.ContainsKey(cdh.NickName))
                {
                    userColor.Add(cdh.NickName, GetRandomColor());
                    listCsdnH.Add(cdh);
                }
                //cdh.Command();
                this.Invoke(new Action(() => {
                
                    this.listView1.Items[index].SubItems[3].Text = "登录成功："+cdh .NickName ;
                    this.listView1.Items[index].Tag = cdh;
                    this.listView1.Items[index].ForeColor =userColor [cdh .NickName ];
                
                }));
            }else
            {
                cdh = null;

                this.Invoke(new Action(() =>
                {
                 
                    this.listView1.Items[index].SubItems[3].Text = sta ;
                    //this.listView1.Items[index].Tag = cdh;

                }));

            }
        }

        private void FormPub_Load(object sender, EventArgs e)
        {
            if (File.Exists(Application.StartupPath + @"\AVcode\csdn_n.cds"))
            {
                CdsIndex_n = LoadCdsFromFile(Application.StartupPath + @"\AVcode\csdn_n.cds");
                CdsIndex_wn = LoadCdsFromFile(Application.StartupPath + @"\AVcode\csdn_wn.cds");
                CdsIndex_reg = LoadCdsFromFile(Application.StartupPath + @"\AVcode\csdn_reg.cds");
            }
            if (File.Exists(Application.StartupPath + @"\userinfos.cfg"))
            {
                Dictionary<string,string> tmp = null ;
                BinaryFormatter bf = new BinaryFormatter();
                using (FileStream fs = new FileStream(Application.StartupPath + @"\userinfos.cfg",FileMode.Open ))
                {
                   tmp=(Dictionary <string ,string >)bf.Deserialize(fs);
                }
                if(tmp !=null && tmp .Count >0)
                {
                    foreach (KeyValuePair <string ,string > item in tmp )
                    {
                        users.Add(item.Key, item.Value);
                        //userColor.Add(item.Key ,GetRandomColor ());

                        ListViewItem lvi = new ListViewItem();
                        lvi.Text = listView1.Items.Count.ToString();
                        lvi.SubItems.Add(item.Key );
                        lvi.SubItems.Add(item.Value );
                        lvi.SubItems.Add("未登录");
                        //lvi .ForeColor =userColor [item.Key ];
                        //lvi.Tag = cdh;
                        listView1.Items.Add(lvi);
                        //CsdnHelper cdh = new CsdnHelper(txtUser.Text.Trim(), txtPass.Text);
                        
                    }   
                }
            }
        }
        private void FormPub_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                return;
            }
            foreach (CsdnHelper item in listCsdnH)
            {
                item.Clear();
            }
            if (users != null && users.Count > 0)
            {
                BinaryFormatter bf = new BinaryFormatter();
                using (FileStream fs = new FileStream(Application.StartupPath + @"\userinfos.cfg", FileMode.Create, FileAccess.Write))
                {
                    bf.Serialize(fs, users);
                }
            }
        }


        #region 工具bar


        private void 自动评分ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                return;
            }
            List<int> indexs = new List<int>();

            for (int j = 0; j < listView1.SelectedItems.Count; j++)
            {
                indexs.Add(listView1.SelectedItems[j].Index);
            }
            foreach (int index in indexs)
            {
                CsdnHelper item = (CsdnHelper)listView1.Items[index].Tag;
                //CsdnHelper item = (CsdnHelper)listView1.Items[i].Tag;
                if (item == null)
                {
                    showlogs(DateTime.Now.ToString("HHmmssffff") + "\t操作出错\t该账户尚未登录");
                    continue;
                }
                item.Command();

                Tsleep(800);
            }
        }

        private void 自动模拟下载ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                return;
            }

            List<int> indexs = new List<int>();
            for (int j = 0; j < listView1.SelectedItems.Count; j++)
            {
                indexs.Add(listView1.SelectedItems[j].Index);
            }
            foreach (int index in indexs)
            {
                CsdnHelper item = (CsdnHelper)listView1.Items[index].Tag;
                //CsdnHelper item = (CsdnHelper)listView1.Items[i].Tag;
                if (item == null)
                {
                    showlogs(DateTime.Now.ToString("HHmmssffff") + "\t操作出错\t该账户尚未登录");
                    continue;
                }
                item.DownloadFree();

                Tsleep(800);
            }
        }

        private void 终止账户行为ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                return;
            }
            List<int> indexs = new List<int>();
            for (int j = 0; j < listView1.SelectedItems.Count; j++)
            {
                indexs.Add(listView1.SelectedItems[j].Index);
            }
            foreach (int index in indexs)
            {
                CsdnHelper item = (CsdnHelper)listView1.Items[index].Tag;
                //CsdnHelper item = (CsdnHelper)listView1.Items[i].Tag;
                if (item == null)
                {
                    showlogs(DateTime.Now.ToString("HHmmssffff") + "\t操作出错\t该账户尚未登录");
                    continue;
                }
                item.Clear();
                Tsleep(800);
            }
        }
        private void 更改配置toolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                return;
            }
            CsdnHelper item = (CsdnHelper)listView1.SelectedItems[0].Tag;
            if (item != null)
            {
                FormSetting fset = new FormSetting(item);
                fset.ShowDialog();
            }
        }
        private void 登录选中ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                return;
            }
            List<int> indexs = new List<int>();      
            for (int j = 0; j < listView1.SelectedItems.Count; j++)
            {
                indexs.Add(listView1.SelectedItems[j].Index);
            }
            foreach (int index in indexs)
            {
                string name = listView1.Items[index].SubItems[1].Text;
                string pass = listView1.Items[index].SubItems[2].Text;

                ThreadPool.QueueUserWorkItem(o =>
                {
                    LoginTest(name, pass, index);
                });
                Tsleep(800);
            }
        }
        private void 终止所有用户行为ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (CsdnHelper item in listCsdnH)
            {
                item.Clear ();
            }
        }

        private void 删除用户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                return;
            }
            List<int> indexs = new List<int>();

            for (int j = 0; j < listView1.SelectedItems.Count; j++)
            {
                  if (indexs.Count == 0)
                {
                    indexs.Add(listView1.SelectedItems[j].Index);
                }
                else
                {
                    indexs.Insert(0, listView1.SelectedItems[j].Index);
             
                }
            }
            foreach (int index in indexs)
            {
                string name = listView1.Items [index ].SubItems[1].Text;
                string pass = listView1.Items[index].SubItems[2].Text;
                string status = listView1.Items [index ].SubItems[3].Text;

                if (status.Contains("成功"))
                {
                    for (int s = 0; s < listCsdnH.Count; s++)
                    {
                        if (listCsdnH[s].User == name)
                        {
                            listCsdnH.RemoveAt(s);
                            break;
                        }
                    }
                }
                if (users.ContainsKey(name.Trim()))
                {
                    users.Remove(name.Trim());
                }
                listView1.Items.RemoveAt(index );
            }  
        }

        private void 清空用户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (CsdnHelper  item in listCsdnH )
            {
                item.Clear();
            }

            listCsdnH.Clear();
            users.Clear();
            userColor.Clear();
            listView1.Items .Clear();
            
        }

        private void 获取上传的资源ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            list_uped = new List<CsdnResouce>();
            if (listView1.SelectedItems.Count == 0)
            {
                return;
            }
            List<int> indexs = new List<int>();

            for (int j = 0; j < listView1.SelectedItems.Count; j++)
            {
                indexs.Add(listView1.SelectedItems[j].Index);
            }
            foreach (int index in indexs)
            {
                CsdnHelper item = (CsdnHelper)listView1.Items[index].Tag;
                if (item == null)
                {
                    showlogs(DateTime.Now.ToString("HHmmssffff") + "\t操作出错\t该账户尚未登录");
                    continue;
                }
                list_uped.AddRange(item.GetUploadRs());
            }
        }

        private void 下载上传的资源ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (list_uped == null || list_uped.Count == 0)
            {
                showlogs(DateTime.Now.ToString("HHmmssffff") + "\t操作出错\t无已上传资源");

                return;
            }
            if (listView1.SelectedItems.Count == 0)
            {
                return;
            }

            List<int> indexs = new List<int>();
            for (int j = 0; j < listView1.SelectedItems.Count; j++)
            {
                indexs.Add(listView1.SelectedItems[j].Index);
            }
            foreach (int index in indexs)
            {
                CsdnHelper item = (CsdnHelper)listView1.Items[index].Tag;
                //CsdnHelper item = (CsdnHelper)listView1.Items[i].Tag;
                if (item == null)
                {
                    showlogs(DateTime.Now.ToString("HHmmssffff") + "\t操作出错\t该账户尚未登录");
                    continue;
                }
                item.DownloadFree(list_uped);

                Tsleep(80);
            }
        }


        #endregion 工具bar


        public void Tsleep(int time)
        {
            Thread t = new Thread(o => Thread.Sleep(time));
            t.Start();
            while (t.IsAlive)
            {
                Thread.Sleep(1);
                Application.DoEvents();
            }
        }

        public void dealRegcsdn(string[] str,CsdnHelper cs=null )
        {
            if (str != null)
            {
                if (str.Length == 1)
                {
                    showlogs(DateTime.Now.ToString("HHmmssffff") + "\t操作出错\t"+str[0]);
                    //showlogs("注册失败：" + str[0]);
                }
                else if(str .Length ==2)
                {
                    this.Invoke(new Action(() => {

                        txtUser.Text = str[0];
                        txtPass.Text = str[1];
                        btn_Login(cs );
                       // btnLogin_Click(null, null);
                    
                    }));                
                }
            }
        }


        private void btn_reg_Click(object sender, EventArgs e)
        {
            CsdnHelper ctmp=new CsdnHelper();
            ctmp.getImgVcode += new CsdnHelper.GetImgVcode(getVcode);
            ctmp.showLogs += new CsdnHelper.ShowLogs(showlogs);
            ctmp.regeristResult += new CsdnHelper.RegeristResult(dealRegcsdn);
            ctmp.getRegVcode += new CsdnHelper.GetRegVcode(getRegVcode);
            string regPass = "";
            regPass = txt_regPass.Text.Trim() == "" ? "yqmacCSDN" : txt_regPass.Text.Trim();
            ctmp.ComMsg = msgs;
            ctmp.AutoRunTocheck(regPass, 1);
            //ctmp = null;
        }


        List<CsdnResouce> list_uped = new List<CsdnResouce>();
      
        private void exportUsers()
        {
            StringBuilder strBuf = new StringBuilder();
            foreach (KeyValuePair<string, string> item in users)
            {
                strBuf.Append(string.Format("{0}----{1}\r\n", item.Key, item.Value));
            }
            File.AppendAllText(Application.StartupPath + @"\Users" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + @".txt", strBuf.ToString());
        }


        private void importUsers(string file)
        {

            if (File.Exists(file))
            {
                string content = string.Empty;
                using (StreamReader sr = new StreamReader(file))
                {
                    content = sr.ReadToEnd();//一次性读入内存
                    sr.Dispose();
                }
                if (!string.IsNullOrEmpty(content))
                {
                    //string[] listup = content.Split('\n');
                    // for (int i = 0; i < listup.Length; i++)
                    {
                        string user = "";
                        string pass = "";
                        // if (listup[i].Length > 0)
                        // {
                        Regex reg = new Regex("(.*?)----(.*?)(----(.*?))?\\r\\n");
                        MatchCollection mc = reg.Matches(content);
                        if (mc.Count > 0)
                        {
                            foreach (Match item in mc)
                            {
                                user = item.Groups[1].Value;
                                pass = item.Groups[2].Value;
                                if (!users.ContainsKey(user))
                                {
                                    users.Add(user, pass);
                                    ListViewItem lvi = new ListViewItem();
                                    lvi.Text = listView1.Items.Count.ToString();
                                    lvi.SubItems.Add(user);
                                    lvi.SubItems.Add(pass);
                                    lvi.SubItems.Add("未登录");
                                    listView1.Items.Add(lvi);
                                }
                            }
                        }
                    }
                }

            }

        }


        private void btn_export_Click(object sender, EventArgs e)
        {
            exportUsers();
        }

        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            int i;
            for (i = 0; i < s.Length; i++)
            {
                if (s[i].Trim() != "")
                {
                    importUsers(s[i]);
                }
            }
        }

        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void btn_auto_Click(object sender, EventArgs e)
        {
           // CsdnResouce csdn = new CsdnResouce();
            CsdnHelper csdn = new CsdnHelper();
            csdn.getImgVcode += new CsdnHelper.GetImgVcode (getVcode );
            csdn.showLogs += new CsdnHelper.ShowLogs (showlogs);
            csdn.regeristResult +=new CsdnHelper.RegeristResult ( dealRegcsdn);
            csdn.getRegVcode  += new CsdnHelper.GetRegVcode (getRegVcode );           
            csdn.ComMsg = msgs;

            string regPass="";
            int regNum = 1;
            regPass = txt_regPass.Text.Trim() == "" ? "yqmacCSDN" : txt_regPass.Text.Trim();
            int.TryParse(txt_regNum .Text .Trim (),out regNum );
            regNum=regNum == 0 ? 1 : regNum;
            csdn.AutoRunTocheck(regPass ,regNum );
        }

        private void 换随机色ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                return;
            }
            List<int> indexs = new List<int>();
            for (int j = 0; j < listView1.SelectedItems.Count; j++)
            {
                indexs.Add(listView1.SelectedItems[j].Index);
            }
            foreach (int index in indexs)
            {
                string name = listView1.Items[index].SubItems[1].Text;
                if (userColor.ContainsKey(name))
                {
                    userColor[name] = GetRandomColor();
                    this.Invoke(new Action(() =>
                    {
                        listView1.Items[index].ForeColor =userColor [name];

                    }));
                }
               
                //ThreadPool.QueueUserWorkItem(o =>
                //{
                //    LoginTest(name, pass, index);
                //});
                Tsleep(800);
            }
        }

        private void 上传ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                return;
            }

            List<int> indexs = new List<int>();
            for (int j = 0; j < listView1.SelectedItems.Count; j++)
            {
                indexs.Add(listView1.SelectedItems[j].Index);
            }
            foreach (int index in indexs)
            {
                CsdnHelper item = (CsdnHelper)listView1.Items[index].Tag;
                //CsdnHelper item = (CsdnHelper)listView1.Items[i].Tag;
                if (item == null)
                {
                    showlogs(DateTime.Now.ToString("HHmmssffff") + "\t操作出错\t该账户尚未登录");
                    continue;
                }
                item.Upload();
                //item.DownloadFree(list_uped);
                //item.Clear();
                //item.AutoRunTocheck("yqmacCSDN", 200);
                Tsleep(80);
            }
        }

        private void 全部通知ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                return;
            }

            List<int> indexs = new List<int>();
            for (int j = 0; j < listView1.SelectedItems.Count; j++)
            {
                indexs.Add(listView1.SelectedItems[j].Index);
            }
            foreach (int index in indexs)
            {
                CsdnHelper item = (CsdnHelper)listView1.Items[index].Tag;
                //CsdnHelper item = (CsdnHelper)listView1.Items[i].Tag;
                if (item == null)
                {
                    showlogs(DateTime.Now.ToString("HHmmssffff") + "\t操作出错\t该账户尚未登录");
                    continue;
                }
                item.GetMsg ();
                //item.DownloadFree(list_uped);
                //item.Clear();
                //item.AutoRunTocheck("yqmacCSDN", 200);
                Tsleep(80);
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                return;
            }

            List<int> indexs = new List<int>();
            for (int j = 0; j < listView1.SelectedItems.Count; j++)
            {
                indexs.Add(listView1.SelectedItems[j].Index);
            }

            string user = listView1.SelectedItems[0].SubItems[1].Text;
            Clipboard.SetText(user );
            MessageBox.Show(user );
            return;
        }

        private void 检查用户状态ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                return;
            }

            List<int> indexs = new List<int>();
            for (int j = 0; j < listView1.SelectedItems.Count; j++)
            {
                indexs.Add(listView1.SelectedItems[j].Index);
            }
            foreach (int index in indexs)
            {
                CsdnHelper item = (CsdnHelper)listView1.Items[index].Tag;
                //CsdnHelper item = (CsdnHelper)listView1.Items[i].Tag;
                if (item == null)
                {
                    showlogs(DateTime.Now.ToString("HHmmssffff") + "\t操作出错\t该账户尚未登录");
                    continue;
                }
                string status=item.GetStatus();
                if (listView1 .Items [index ].SubItems .Count == 4)
                {
                    listView1.Items[index].SubItems.Add(status );
                }
                else
                {
                    listView1.Items[index].SubItems[4].Text =status;
                }
                //item.DownloadFree(list_uped);
                //item.Clear();
                //item.AutoRunTocheck("yqmacCSDN", 200);
                Tsleep(80);
            }

        }

 
    }
}
