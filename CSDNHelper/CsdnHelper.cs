using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net ;
using System.Web;
using HttpCodeLib;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;
using System.Globalization;
namespace xCsdn
{
    /// <summary>
    /// 工具类
    /// </summary>
    public  class CsdnHelper
    {
        public CsdnHelper()
        {
            cc = new CookieContainer();
        }
        #region 变量
        Random ran = new Random();
        Object LockObject = new object();

        private string user;

        /// <summary>
        /// 用户帐号
        /// </summary>
        public string User
        {
            get { return user; }
            set { user = value; }
        }

        private string nickName;
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName
        {
            get { return nickName; }
            set { nickName = value; }
        }

        private string pass;
        /// <summary>
        /// 密码
        /// </summary>
        public string Pass
        {
            get { return pass; }
            set { pass = value; }
        }

        private bool isonline;
        /// <summary>
        /// 登录状态
        /// </summary>
        public bool Isonline
        {
            get { return isonline; }
            set { isonline = value; }
        }

        private int jfCount = -1;

        /// <summary>
        /// 积分数量
        /// </summary>
        public int JfCount
        {
            get { return jfCount; }
            set { jfCount = value; }
        }

        private CookieContainer cc;
        private HttpResults hr_down;
        private HttpResults hr_com;

        private List<CsdnResouce> listConSource = new List<CsdnResouce>();

        private int canComCount = 0;
        /// <summary>
        /// 可评论数量
        /// </summary>
        public int CanComCount
        {
            get { return canComCount; }
            set { canComCount = value; }
        }

        private int preComCount;
        /// <summary>
        /// 不足评论时间条件数量
        /// </summary>
        public int PreComCount
        {
            get { return preComCount; }
            set { preComCount = value; }
        }


        private string[] comMsg;
        /// <summary>
        /// 评论语句库
        /// </summary>
        public string[] ComMsg
        {
            get { return comMsg; }
            set { comMsg = value; }
        }


        /// <summary>
        /// 获取图片验证码
        /// </summary>
        /// <param name="imgbytes"></param>
        /// <returns></returns>
        public delegate string  GetImgVcode(byte [] imgbytes);
        public GetImgVcode getImgVcode;

        /// <summary>
        /// 显示日志
        /// </summary>
        /// <param name="logtext"></param>
        public delegate void ShowLogs(string logtext);
        public ShowLogs showLogs;

        /// <summary>
        /// 返回注册帐号信息
        /// </summary>
        /// <param name="s"></param>
        public delegate void RegeristResult(string[] s,CsdnHelper cs=null );
        public RegeristResult regeristResult;


        public delegate string GetRegVcode(byte[] imgbytes);
        public GetRegVcode getRegVcode;


        private List<CsdnResouce> listPreTDown = new List<CsdnResouce>();

        public List<CsdnResouce> ListPreTDown
        {
            get { return listPreTDown; }
            set { listPreTDown = value; }
        }

        private bool saveFile;

        public bool SaveFile
        {
            get { return saveFile; }
            set { saveFile = value; }
        }
        private string savePath;

        public string SavePath
        {
            get { return savePath; }
            set { savePath = value; }
        }


        private Thread thread_down = null;


        private Thread thread_com = null;

       
        private static  int timeForCom = 1000 * 63;

        public int TimeForCom
        {
            get { return timeForCom; }
            set { timeForCom = value; }
        }

        private static int timeForDown = 1000 * 3 + new Random().Next(1, 1000);

        public int TimeForDown
        {
            get { return timeForDown; }
            set { timeForDown = value; }
        }

        private List<CsdnResouce> uploadedRS = new List<CsdnResouce>();


        private string proxyIp = "";

        #endregion 变量

        #region 上传随机文件
        /// <summary>
        /// 根据规则创建字符串
        /// </summary>
        /// <returns></returns>
        private  string getRandomString(int lenth)
        {
            if (lenth <= 0)
            {
                return "";
            }
            StringBuilder  name =new StringBuilder ();
            string a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string b = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            name.Append (a[ran.Next(a.Length)]);
            //随机文本
            for (int i = 0; i < lenth - 1; i++)
            {
                name.Append(b[ran.Next(b.Length)]);
            }
            //合成
            return name.ToString ();
        }

        private  void upload(string filepath,string proxyip="")
        {
            string filename = Path.GetFileName(filepath);

            //获取APC_UPLOAD_PROGRESS
            HttpResults hr_up;
            string rehtml;
            string progress, param1;
            string cookiess = new XJHTTP().CookieTostring(cc);

            hr_up = new HttpHelpers().GetHtml(new HttpItems()
            {
                URL = @"http://u.download.csdn.net/upload",
                Cookie = cookiess
            }, ref cookiess);
            rehtml = hr_up.Html.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            //id="progress_key"value="fdfa65ddb300eaceb54cd1ef425be4cd"
            Regex reg = new Regex("id=\"progress_key\"value=\"(.*?)\"");
            Match mct = reg.Match(rehtml);
            //string s=mct.Groups[1].Value;
            //progress_key" value="
            progress = mct.Groups[1].Value;
            // cookiess += ";dc_session_id="+new XJHTTP ().GetTimeByJs ()+";tos=36;";
            CHECKVCODE:

            //获取验证码
            hr_up = new HttpHelpers().GetHtml(new HttpItems()
            {
                URL = @"http://u.download.csdn.net/index.php/rest/tools/validcode/uploadvalidcode/10.94257339" + new Random().Next().ToString(),
                Referer = "http://u.download.csdn.net/upload",
                Cookie = cookiess,
                ResultType = ResultType.Byte

            }, ref cookiess);
            byte[] imgbytes = hr_up.ResultByte;
            string code = getImgVcode(imgbytes);
            if (code == "")
            {
                return;
            }

            //检查验证码
            hr_up = new HttpHelpers().GetHtml(new HttpItems()
            {
                URL = "http://u.download.csdn.net/index.php/upload/checkform/txt_validcode=" + code,
                Cookie = cookiess,
                Referer = "http://u.download.csdn.net/upload"
            }, ref cookiess);
            rehtml = hr_up.Html.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            if (rehtml != "{\"succ\":1}")
            {
                rehtml = new XJHTTP().FromUnicodeString(rehtml);
                goto CHECKVCODE;
            }
            //上传
            string bundry = "WebKitFormBoundary" + getRandomString(16);

            StringBuilder strb = new StringBuilder();

            strb.Append("------" + bundry).Append("\r\n");
            strb.Append("Content-Disposition: form-data; name=\"APC_UPLOAD_PROGRESS\"").Append("\r\n");
            strb.Append("").Append("\r\n");
            strb.Append(progress).Append("\r\n");

            strb.Append("------" + bundry).Append("\r\n");
            strb.Append("Content-Disposition: form-data; name=\"txt_userfile\"; filename=\"" + filename + "\"").Append("\r\n");
            strb.Append("Content-Type: application/octet-stream").Append("\r\n");
            strb.Append("").Append("\r\n");

            string headerstr = strb.ToString();
            strb.Clear();


            strb.Append("\r\n------" + bundry).Append("\r\n");
            strb.Append("Content-Disposition: form-data; name=\"txt_title\"").Append("\r\n");
            strb.Append("").Append("\r\n");
            strb.Append("课程资源_"+filename.Substring (1,5) + "_名称").Append("\r\n");

            strb.Append("------" + bundry).Append("\r\n");
            strb.Append("Content-Disposition: form-data; name=\"sel_filetype\"").Append("\r\n");
            strb.Append("").Append("\r\n");
            strb.Append("1").Append("\r\n");//改

            strb.Append("------" + bundry).Append("\r\n");
            strb.Append("Content-Disposition: form-data; name=\"txt_tag\"").Append("\r\n");
            strb.Append("").Append("\r\n");
            strb.Append(filename .Substring (0,filename .Length >5?5:filename .Length )).Append("\r\n");//改

            strb.Append("------" + bundry).Append("\r\n");
            strb.Append("Content-Disposition: form-data; name=\"sel_primary\"").Append("\r\n");
            strb.Append("").Append("\r\n");
            strb.Append("15").Append("\r\n");//改

            strb.Append("------" + bundry).Append("\r\n");
            strb.Append("Content-Disposition: form-data; name=\"sel_subclass\"").Append("\r\n");
            strb.Append("").Append("\r\n");
            strb.Append("15013").Append("\r\n");//改

            strb.Append("------" + bundry).Append("\r\n");
            strb.Append("Content-Disposition: form-data; name=\"sel_score\"").Append("\r\n");
            strb.Append("").Append("\r\n");
            strb.Append("0").Append("\r\n");//改

            strb.Append("------" + bundry).Append("\r\n");
            strb.Append("Content-Disposition: form-data; name=\"txt_desc\"").Append("\r\n");
            strb.Append("").Append("\r\n");
            strb.Append(filename + "资源，需要的下载吧,课程内容进度的保存等" ).Append("\r\n");//改

            strb.Append("------" + bundry).Append("\r\n");
            strb.Append("Content-Disposition: form-data; name=\"txt_validcode\"").Append("\r\n");
            strb.Append("").Append("\r\n");
            strb.Append(code).Append("\r\n");//改

            strb.Append("------" + bundry).Append("\r\n");
            strb.Append("Content-Disposition: form-data; name=\"cb_agree\"").Append("\r\n");
            strb.Append("").Append("\r\n");
            strb.Append("").Append("\r\n");
            strb.Append("------" + bundry).Append("--");
            //string headerstr = strb.ToString();
            string tailstr = strb.ToString();

            byte[] heaerbytes = Encoding.ASCII.GetBytes(headerstr);
            byte[] bodybytes = File.ReadAllBytes(filepath);
            byte[] tailbytes = Encoding.ASCII.GetBytes(tailstr);

            byte[] pstdata = ComposeArrays(ComposeArrays(heaerbytes, bodybytes), tailbytes);


            //items_down.Header.Add("Content-Length:"+pstdata .Length );
            hr_up = new HttpHelpers().GetHtml(new HttpItems()
            {
                URL = "http://u.download.csdn.net/upload/do_upload",
                Cookie = cookiess,
                Referer = "http://u.download.csdn.net/upload",
                Method = "post",
                PostDataType = PostDataType.Byte,
                PostdataByte = pstdata,
                Accept = "Accept-Encoding: gzip, deflate",

                ContentType = "multipart/form-data; boundary=----" + bundry,
                ProxyIp =proxyip 
                
            }, ref cookiess);


            rehtml = hr_up.Html.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");
            if (!rehtml.Contains("uploadkey"))
            {
                //{"succ":0,"errmsg":"\u8bf7\u586b\u5199\u8d44\u6e90\u7684\u6807\u9898!"}
                //{"succ":1,"errmsg":"","uploadkey":"e6a58597b7fd882ff67fe192de62dee8"}
                rehtml = new XJHTTP().FromUnicodeString(rehtml);
                string[] status = { "操作", "成功/失败", "状态信息", "无验证码", "无附加信息" };
                status[0] = "上传文件";
                status[1] = "失败";
                status[2] = filename;
                status[3] = code;
                status[4] = rehtml;
                Logscomsole(status);
            }
            else
            {
                string[] status = { "操作", "成功/失败", "状态信息", "无验证码", "无附加信息" };
                status[0] = "上传文件";
                status[1] = "成功";
                status[2] = filename;
                status[3] = code;
                status[4] = "";
                Logscomsole(status);
            }
        }

        #endregion 上传随机文件


        public byte[] ComposeArrays(byte[] Array1, byte[] Array2)
        {
            byte[] Temp = new byte[Array1.Length + Array2.Length];
            Array1.CopyTo(Temp, 0);
            Array2.CopyTo(Temp, Array1.Length);
            return Temp;
        }



        #region 公开

        /// <summary>
        /// 帐号是否被锁定
        /// </summary>
        public string  GetStatus()
        {
            //http://u.download.csdn.net/upload
            //您因违反CSDN下载频道规则而被锁定帐户
            hr_down = new HttpHelpers().GetHtml(new HttpItems()
            {
                URL = "http://u.download.csdn.net/upload",
                Container = cc
            });
            string rehtml = hr_down.Html.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");
            if (rehtml.Contains("您因违反CSDN下载频道规则而被锁定帐户"))
            {
                Logscomsole(new string[]  { "检查状态", "失败喽，锁定了", rehtml, "", "" } );
                return "锁定";
            }else
            {
                Logscomsole(new string[] { "检查状态", "成功，尚未", rehtml, "", "" });
                return "正常";
            }
        }

       /// <summary>
       /// 获取通知
       /// </summary>
        public void GetMsg()
        {
            hr_down = new HttpHelpers().GetHtml(new HttpItems() {
                URL = "http://msg.csdn.net/",
                Container =cc 
            });
            string rehtml = hr_down.Html.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");
            if (rehtml != "")
            {
                //很遗憾，您上传的资源hosts修改工具因资源违规没有通过审核
                string restr = "很遗憾，您上传的资源(.*?)没有通过审核";
                Regex reg = new Regex(restr );
                MatchCollection mct = reg.Matches(rehtml);
                if (mct.Count > 0)
                {
                    Logscomsole(new string[] { "检查通知", "失败，有错喽", mct.Count.ToString(), mct[0].Groups[1].Value.ToString(), "" });
                }
            }
        }

        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="dir"></param>
        public void Upload(string dir="")
        {

            string name="";
            string contents="";
            string filename="";
            string pass = "";
            if (dir == "")
            {
                name = getRandomString(6) + DateTime.Now.ToString("ffff");
                contents = getRandomString(600) + "\r\n" + DateTime.Now.ToString("yyyyMMddHHmmssffff");
                filename = Environment.CurrentDirectory + "\\" + name;
                File.WriteAllText(filename+".txt", contents);
                pass = getRandomString(5);
                yq.ZipHelper.ZipFile(filename+".txt" ,filename +"解压密码"+pass +".zip",pass );
                filename = filename + "解压密码" + pass + ".zip";
            }
            else
            {
                filename = dir;
            }
    
            if(File.Exists (filename ))
                upload(filename);
            //upload(filename, "183.95.81.100:80");
            if(File.Exists (filename)&& dir=="")
            {
                File.Delete(filename);
            }
        }


        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public string Login()
        {
            string[] status = { "操作", "成功/失败", "状态信息", "无验证码", "无附加信息" };

            if (User == "" || Pass == "")
            {
                Logscomsole(new string[] { "模拟登录", "失败", "帐号或者密码有误", "", "" });
                return "";
            }
            //Logscomsole(status);

            string reText = "";
            string url = "https://passport.csdn.net/account/login";//请求地址
            string loginUrl = "https://passport.csdn.net/account/login";
            reText = string.Empty;//请求结果,请求类型不是图片时有效   
            hr_down = new HttpHelpers ().GetHtml(new HttpItems() {
                URL=url ,
                Container =cc 
            });//发起请求并得到结果
            reText = hr_down.Html;//得到请求结果
            if (!string.IsNullOrEmpty(reText) && !reText.Equals("请求被中止: 连接被意外关闭。"))
            {

                //内部变量
                string lt = string.Empty, execution = string.Empty, eventId = string.Empty;
                //匹配必须参数

                Regex reg = new Regex("userId\":(.*?),\"isLocked\"");
                MatchCollection mc = reg.Matches(reText);
                if (mc.Count > 0)
                {
                    return "已登录成功";
                }
                 reg = new Regex("name=\"lt\" value=\"(.+?)\"");
                 mc = reg.Matches(reText);
                //(name=\"lt\" value=\"(.+?)\")|
                 if (mc.Count == 0)
                 {
                     return reText;
                 }
                lt = mc[0].Groups[1].Value;
                //-execution-
                MatchCollection mc2 = new Regex ("name=\"execution\" value=\"(.+?)\"").Matches(reText);
                execution = mc2[0].Groups[1].Value;
                //-eventId-
                MatchCollection mc3 = new Regex("name=\"_eventId\" value=\"(.+?)\"").Matches(reText);
                eventId = mc3[0].Groups[1].Value;

                hr_down = new HttpHelpers().GetHtml(new HttpItems()
                {
                    URL = loginUrl,
                    Container = cc,
                    Method = "post",
                    Postdata = string.Format("username={0}&password={1}&lt={2}&execution={3}&_eventId={4}", user, pass, lt, execution, eventId)
                });

                reText = hr_down.Html;
                //检查是否登录成功
                Regex regerro = new Regex("<span id=\"error-message\">(.+?)</span>");
                MatchCollection mcerro = regerro.Matches(reText);
                //错误数大于0
                if (mcerro.Count > 0)
                {
                    reText = mcerro[0].Groups[1].Value;
                    Logscomsole(new string[] { "模拟登录", "失败", reText, "", "" });
                    isonline = false;
                }
                else
                {
                    if (reText.Contains("redirect_back") && reText.Contains("loginapi.js"))
                    {
                        reText = reText.Replace("\r\n", "").Replace("\n", "").Replace("\t", "");
                        NickName = getPater(reText, "var data = {", "};", new char[] { ',' }, new char[] { ':', '\"' })["nickName"];
                        reText = "登录成功";
                        Logscomsole(new string[] { "模拟登录","成功", "昵称：" + NickName ,"",""});
                        isonline = true;
                    }
                    else
                    {
                        reText = "登录失败,账号已失效！";
                        Logscomsole(new string[] { "模拟登录","失败",reText ,"","" });
                        isonline = false;
                    }
                }
            }

            return reText;
        }


        /// <summary>
        /// 评论已下载资源
        /// </summary>
        public void Command()
        {
            if (!Isonline)
            {
                Logscomsole(new string[] { "自动评分","失败","未登录呢","",""});
                return;
            }
            if (comMsg==null||comMsg.Length == 0)
            {
                Logscomsole(new string[] { "自动评分", "失败", "语句库[comMsg]是空的", "", "" });
                return;
            }
            if (thread_com != null && thread_com.IsAlive)
            {
                thread_com.Abort();
            }
            thread_com = new Thread(new ThreadStart(command));
            thread_com.Start();

        }

        /// <summary>
        /// 下载免费资源
        /// </summary>
        /// <param name="list"></param>
        public void DownloadFree(List <CsdnResouce >list=null )
        {
            if (list != null&&list .Count >0)
            {
                ListPreTDown = list;
            }
            if (thread_down != null && thread_down.IsAlive)
            {
                thread_down.Abort();
            }
            thread_down = new Thread(new ThreadStart(download));
            thread_down.Start();
        }

        /// <summary>
        /// 终止所有行为
        /// </summary>
        public void Clear()
        {
            if (thread_com != null && thread_com.IsAlive)
            { 
                Logscomsole(new string[] { "自动模拟评分","结束", "被强制终止","","" });
                thread_com.Abort();
                thread_com = null;
            }
            if (thread_down != null && thread_down.IsAlive)
            {
                Logscomsole(new string[] { "自动模拟下载", "结束", "被强制终止", "", "" });
                thread_down.Abort();
                thread_down = null;
            }
            listConSource = new List<CsdnResouce>();
            listPreTDown = new List<CsdnResouce>();
        }

        /// <summary>
        /// 获取已上传资源
        /// </summary>
        /// <returns></returns>
        public List<CsdnResouce> GetUploadRs()
        {
            getUploadRs();
            return uploadedRS;
        }

        private string mima = "";
        private int pointNum = -1;
        private int finishNum = 0;

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="mima"></param>
        /// <param name="regNum">注册个数，间隔时间5分钟</param>
        public void AutoRunTocheck(string mima,int regNum) {
            this.mima = mima;
            this.Pass  = mima;
            this.pointNum = regNum;
            new Thread(new ThreadStart (auto )).Start ();

        }


        #endregion 公开

        #region 获取上传资源
        private void getUploadRs()
        {
            // Logscomsole(status);
            HttpHelpers helper_up = new HttpHelpers();
            HttpItems items_up = new HttpItems();
            HttpResults hr_up = new HttpResults();
            string url;
            string rehtml;
            


            url =string.Format ("http://download.csdn.net/my");
            items_up = new HttpItems() {
                URL = url,
                Container = cc 
            };
            hr_up = helper_up.GetHtml(items_up);
            string DownHtml = hr_up.Html.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");
            string result = Regex.Replace(DownHtml, @"<!--(.+?)-->", "");


            //获取下载资源数量
            Regex regDown = new Regex("上传资源：<spanclass=\"red\">(.+?)</span>");
            MatchCollection mcDown = regDown.Matches(result);
            double upCount = 0;
            if (mcDown.Count > 0)
            {
                upCount = Convert.ToDouble(mcDown[0].Groups[1].Value);
            }
            //Regex reg = new Regex("<dt><divclass=\"icon\"><imgsrc=\"(.+?)\"title=\"(.+?)\"></div><divclass=\"btns\"></div><h3><ahref=\"(.+?)\"(.*?)\">(.+?)</a><spanclass=\"points\">(.+?)</span></h3></dt>");
            Regex reg = new Regex("<dt><divclass=\"icon\"><imgsrc=\"(.+?)\"title=\"(.+?)\"></div><divclass=\"btns\">(<ahref=(.*?)删除</a>)?</div><h3><ahref=\"(.+?)\"(.*?)\">(.+?)</a><spanclass=\"points\">(.+?)</span></h3></dt>");
            MatchCollection mc = reg.Matches(result);
            if (mc.Count > 0)
            {
                foreach (Match item in mc)
                {
                    CsdnResouce cdsr = new CsdnResouce();
                    if (item.Groups.Count == 9)
                    {
                        cdsr.Url = item.Groups[5].Value;
                        cdsr.Name = item.Groups[7].Value;
                        cdsr.Point = item.Groups[8].Value;
                    }
                    else
                    {
                        cdsr.Url = item.Groups[3].Value;
                        cdsr.Name = item.Groups[5].Value;
                        cdsr.Point = item.Groups[6].Value;
                    }
                    //CsdnResouce cdsr = new CsdnResouce(tmp1[6], tmp1[4]);
                    cdsr.Tag = String.Format("http://download.csdn.net{0}", cdsr.Url);
                    uploadedRS.Add(cdsr );
                }      
            }
            Logscomsole(new string[] { "获取上传资源", "结束", "共 " + upCount.ToString() + " 个资源", "", "" });
        }


        #endregion 获取上传资源


        #region 注册


        public void auto()
        {
            try
            {
                int trytimes = 0;
                Start:
                if (string.IsNullOrEmpty(this.User))
                {
                    string[] regre = regForIn();
                    if (regre.Length == 2)
                    {
                        finishNum++;
                        this.User = regre[0];
                        this.Pass = regre[1];
                    }
                    if (regeristResult != null)
                    {
                        regeristResult(regre, this);
                    }
                    //注册失败
                    if (regre.Length != 2)
                    {
                        return;
                    }
                    File.AppendAllText(Environment.CurrentDirectory + @"\users.txt", string.Format("{0}----{1}\r\n", this.User, this.Pass));
                    if (finishNum < pointNum)
                    {
                        Thread.Sleep(1000 * 60 * 10);
                        this.User = "";
                        goto Start;
                    }
                }
            }
            catch (Exception ex)
            {
                yq.LogHelper.Error(ex);
                //throw;
            }


        }


        #region 邮件

        /// <summary>
        /// .net
        /// </summary>
        /// <param name="ccc"></param>
        /// <returns></returns>
        private string getRegEmail(CookieContainer ccc)
        {
            HttpItems item = new HttpItems();
            HttpHelpers heler = new HttpHelpers();
            HttpResults hr = new HttpResults();
            string url;

            url = @"https://10minutemail.net/";
            item = new HttpItems()
            {
                URL = url ,
                Container =ccc
            };
            hr = heler.GetHtml(item);
            Regex regex = new Regex("ata-clipboard-text=\".*?\"");
            MatchCollection mc = regex.Matches(hr.Html.Replace("\r\n", "").Replace("\t", "").Replace("\n", ""));
            if (mc != null && mc.Count > 0)
            {

                string email = mc[0].Groups[0].Value;
                email = email.Split(new char[] { '\\', '\"' })[1];
                return email;
            }

            return "";
        }
        /// <summary>
        /// .net
        /// </summary>
        /// <param name="ccc"></param>
        /// <returns></returns>
        private string activeRegEmai(CookieContainer ccc)
        {
            string reText = "";
            string[] status = { "操作", "成功/失败", "状态信息", "无验证码", "无附加信息" };
           
            HttpItems item = new HttpItems();
            HttpHelpers heler = new HttpHelpers();
            HttpResults hr = new HttpResults();
            Regex regex = null ;
            MatchCollection mc = null;
            int trytimes = 0;

            item = new HttpItems();
            //https://10minutemail.net/
            item.URL = @"https://10minutemail.net/mailbox.ajax.php?_=";

            item.Container = ccc ;
         CHECKEMAIL:
            hr = heler.GetHtml(item);
            string emalurl = hr.Html.Replace("\r\n", "").Replace("\t", "").Replace("\n", "");
            if ("errorerror".Equals(emalurl))
            {
                return "失败：errorerror";
            }
            regex = new Regex("<td>service@register\\.csdn.net</td><td><a href=\".*?\">\\[CSDN\\]");
            mc = regex.Matches(emalurl);

            if (mc.Count == 0)
            {
                if (trytimes <= 10)
                {
                    trytimes++;
                    status[0] = "注册帐号";
                    status[1] = "进行中";
                    status[2] = "等待邮件到达";
                    status[3] = "20秒后重新扫描";
                    status[4] = "";
                    Logscomsole(status);
                    Thread.Sleep(20000);
                    goto CHECKEMAIL;
                }
                else
                {
                    status[0] = "注册帐号";
                    status[1] = "失败";
                    status[2] = "邮件长时间未到";
                    status[3] = "重试申请";
                    status[4] = "";
                    Logscomsole(status);
                    //reg();
                    return "邮件长时间未到";
                }

            }
            if (mc != null && mc.Count >= 1)
            {
                status[0] = "注册帐号";
                status[1] = "进行中";
                status[2] = "扫描到激活邮件";
                status[3] = "即将完成申请";
                status[4] = "";
                Logscomsole(status);

                string urltmp = @"https://10minutemail.net/" + mc[0].Groups[0].Value.Split(new char[] { '\"' })[1];// +"lang=zh-cn";
                //Console.WriteLine(urltmp );
                item = new HttpItems();
                item.URL = urltmp;
                item.Container =ccc ;
                hr  = heler .GetHtml(item);
                emalurl = hr.Html.Replace("\r\n", "").Replace("\t", "").Replace("\n", "");
                // Console.WriteLine(emalurl);
                regex = new Regex("href=\".*?\"");
                mc = regex.Matches(emalurl);
                if (mc != null && mc.Count > 0)
                {
                    string urlacc = mc[103].Groups[0].Value.Split(new char[] { '\"' })[1].Replace("&amp;", "&"); ;
                    item = new HttpItems();
                    item.URL = urlacc;
                    item.Container = new CookieContainer ();
                    hr = heler.GetHtml(item);
                    string htmlss = hr.Html;
                    if (htmlss.Contains("注册成功"))
                    {
                        return "注册成功";
                    }
                    else
                    {
                        return "注册失败"+htmlss;
                    }
                }
            }


            return "注册失败" ;
        }

        private string getRegEmail1(CookieContainer ccc)
        {
            HttpItems item = new HttpItems();
            HttpHelpers heler = new HttpHelpers();
            HttpResults hr = new HttpResults();

            item = new HttpItems();
            //http://10minutemail.com/10MinuteMail/index.html
            item.URL = @"http://10minutemail.com/10MinuteMail/index.html";
            item.Container = ccc;

            hr = heler.GetHtml(item);
            Regex regex = new Regex("ata-clipboard-text=\".*?\"");
            MatchCollection mc = regex.Matches(hr.Html.Replace("\r\n", "").Replace("\t", "").Replace("\n", ""));
            if (mc != null && mc.Count > 0)
            {

                string email = mc[0].Groups[0].Value;
                email = email.Split(new char[] { '\\', '\"' })[1];
                return email;
            }

            return "";
        }

        private string activeRegEmai1(CookieContainer ccc)
        {
            string reText = "";
            string[] status = { "操作", "成功/失败", "状态信息", "无验证码", "无附加信息" };

            HttpItems item = new HttpItems();
            HttpHelpers heler = new HttpHelpers();
            HttpResults hr = new HttpResults();
            Regex regex = null;
            MatchCollection mc = null;
            int trytimes = 0;

            item = new HttpItems();
            //https://10minutemail.net/
            item.URL = @"http://10minutemail.com/10MinuteMail/index.html";
            item.Method = "Post";
            item.Postdata = @"AJAXREQUEST=j_id3&j_id4=j_id4&javax.faces.ViewState=j_id42162&j_id4%3Apoll=j_id4%3Apoll&";
            item.Container = ccc;
            item.Allowautoredirect = true;
        CHECKEMAIL:
            hr = heler.GetHtml(item);
            string emalurl = hr.Html.Replace("\r\n", "").Replace("\t", "").Replace("\n", "");
            if ("errorerror".Equals(emalurl))
            {
                return "失败：errorerror";
            }
            regex = new Regex("<td>service@register\\.csdn.net</td><td><a href=\".*?\">\\[CSDN\\]");
            mc = regex.Matches(emalurl);

            if (mc.Count == 0)
            {
                if (trytimes <= 10)
                {
                    trytimes++;
                    status[0] = "注册帐号";
                    status[1] = "进行中";
                    status[2] = "等待邮件到达";
                    status[3] = "20秒后重新扫描";
                    status[4] = "";
                    Logscomsole(status);
                    Thread.Sleep(20000);
                    goto CHECKEMAIL;
                }
                else
                {
                    status[0] = "注册帐号";
                    status[1] = "失败";
                    status[2] = "邮件长时间未到";
                    status[3] = "重试申请";
                    status[4] = "";
                    Logscomsole(status);
                    //reg();
                    return "邮件长时间未到";
                }

            }
            if (mc != null && mc.Count >= 1)
            {
                status[0] = "注册帐号";
                status[1] = "进行中";
                status[2] = "扫描到激活邮件";
                status[3] = "即将完成申请";
                status[4] = "";
                Logscomsole(status);

                string urltmp = @"http://10minutemail.com/" + mc[0].Groups[0].Value.Split(new char[] { '\"' })[1].Replace ("&amp;","&");// +"lang=zh-cn";
                //Console.WriteLine(urltmp );
                item = new HttpItems();
                item.URL = urltmp;
                item.Container = ccc;
                item.Allowautoredirect=true;
                hr = heler.GetHtml(item);
                emalurl = hr.Html.Replace("\r\n", "").Replace("\t", "").Replace("\n", "");
                // Console.WriteLine(emalurl);
                //action=userInfoView
                //regex = new Regex("CSDN各项服务。<br/><br/> https(.*?)<br/>");

                regex = new Regex("https://passport.csdn.net(.*?)action=userInfoView");
                mc = regex.Matches(emalurl);
                if (mc != null && mc.Count > 0)
                {
                    string urlacc = mc[0].Groups[0].Value.Replace("&amp;", "&");
                    //string urlacc = mc[0].Groups[0].Value.Split(new char[] { '\"' })[1]; ;
                    item = new HttpItems();
                    item.URL = urlacc;
                    item.Container = new CookieContainer();
                    hr = heler.GetHtml(item);
                    string htmlss = hr.Html;
                    if (htmlss.Contains("注册成功"))
                    {
                        return "注册成功";
                    }
                    else
                    {
                        return "注册失败" + htmlss;
                    }
                }
            }


            return "注册失败";
        }



        private string getRegEmail2(CookieContainer ccc)
        {
            HttpItems item = new HttpItems();
            HttpHelpers heler = new HttpHelpers();
            HttpResults hr = new HttpResults();

            item = new HttpItems();
            //http://10minutemail.com/10MinuteMail/index.html
            item.URL = @"http://mailcatch.com/en/disposable-email";
            item.Container = ccc;

            hr = heler.GetHtml(item);
            Regex regex = new Regex("ata-clipboard-text=\".*?\"");
            MatchCollection mc = regex.Matches(hr.Html.Replace("\r\n", "").Replace("\t", "").Replace("\n", ""));
            if (mc != null && mc.Count > 0)
            {

                string email = mc[0].Groups[0].Value;
                email = email.Split(new char[] { '\\', '\"' })[1];
                return email;
            }

            return "";
        }

        private string activeRegEmai2(string emailname,CookieContainer ccc)
        {
            string reText = "";
            string[] status = { "操作", "成功/失败", "状态信息", "无验证码", "无附加信息" };
            Thread.Sleep(15000);
            HttpItems item = new HttpItems();
            HttpHelpers heler = new HttpHelpers();
            HttpResults hr = new HttpResults();
            Regex regex = null;
            MatchCollection mc = null;
            int trytimes = 0;
            string emalid="";
            emalid = emailname .Substring (0,emailname .IndexOf ('@'));
            item = new HttpItems();
            //https://10minutemail.net/
            item.URL = @"http://mailcatch.com/en/rpc.lua";
            item.Method = "Post";
            item.Postdata = string.Format("mod=ListMailsRPC&fct=List&json=%7B%22box%22%3A%22{0}%3Dmailcatch.com%22%2C%22anim%22%3Atrue%7D",emalid );//@"AJAXREQUEST=j_id3&j_id4=j_id4&javax.faces.ViewState=j_id42162&j_id4%3Apoll=j_id4%3Apoll&";
            item.Container = ccc;
            item.Allowautoredirect = true;
            hr = heler.GetHtml(item);
            string emalurl = hr.Html.Replace("\r\n", "").Replace("\t", "").Replace("\n", "");
            if ("errorerror".Equals(emalurl))
            {
                return "失败：errorerror";
            }
            //?box=$boxid&show=a134fc9d-5412-4cda-ac7e-04b48103f78f\">[CSDN

            regex = new Regex("\\&show=(.*?)\\\"\\>\\[CSDN");
            mc = regex.Matches(emalurl);
            if (mc != null && mc.Count >= 1)
            {
                status[0] = "注册帐号";
                status[1] = "进行中";
                status[2] = "扫描到激活邮件";
                status[3] = "即将完成申请";
                status[4] = "";
                Logscomsole(status);
                string []strtmp=mc[0].Groups[0].Value.Split(new char[] { '\"','\\' });
                string urltmp = string.Format("http://yourinbox.mailcatch.com/en/temporary-inbox?box={0}=mailcatch.com{1}", emalid, strtmp[0]);// +"lang=zh-cn";
                //Console.WriteLine(urltmp );
                item = new HttpItems();
                item.URL = urltmp;
                item.Container = ccc;
                item.Allowautoredirect = true;
                hr = heler.GetHtml(item);
                emalurl = hr.Html.Replace("\r\n", "").Replace("\t", "").Replace("\n", "");

                regex = new Regex("https://passport.csdn.net(.*?)action=userInfoView");
                mc = regex.Matches(emalurl);
                if (mc != null && mc.Count > 0)
                {
                    string urlacc = mc[0].Groups[0].Value.Replace("&amp;", "&");
                    return urlacc;
                    //string urlacc = mc[0].Groups[0].Value.Split(new char[] { '\"' })[1]; ;
                    item = new HttpItems();
                    item.URL = urlacc;
                    item.Container = new CookieContainer();
                    hr = heler.GetHtml(item);
                    string htmlss = hr.Html;
                    if (htmlss.Contains("注册成功"))
                    {
                        return "注册成功";
                    }
                    else
                    {
                        return "注册失败" + htmlss;
                    }
                }
            }


            return "注册失败";
        }

        #endregion 邮件

        private string[] regForIn(string proxyip="")
        {
            Start:      
            //string proxyip = "";
            int trytimes = 0;
            string[] status = { "操作", "成功/失败", "状态信息", "无验证码", "无附加信息" };
            status[0] = "注册帐号";
            status[1] = "开始";
            status[2] = "";
            status[3] = "";
            status[4] = "";
            Logscomsole(status);

            HttpItems items_reg = new HttpItems();
            HttpHelpers heler_reg = new HttpHelpers();
            HttpResults hr_reg = new HttpResults();

            CookieContainer cc_em = new CookieContainer();
            CookieContainer cc_reg = new CookieContainer();
            Regex regex = null;
            MatchCollection mc = null;

            byte[] regCodebytes = null;

            string email = "";
            string name = "";
            string regCode = "";

          
            //https://passport.csdn.net/ajax/verifyhandler.ashx//验证码
            hr_reg = heler_reg.GetHtml(new HttpItems()
            {
                URL = @"https://passport.csdn.net/ajax/verifyhandler.ashx",
                ResultType = ResultType.Byte,
                Container = cc_reg
            });

            regCodebytes = hr_reg.ResultByte;

            if (getRegVcode != null)
            {
                regCode = getRegVcode(regCodebytes);
            }
            if (regCode == "")
            {
                status[0] = "注册帐号";
                status[1] = "失败";
                status[2] = "识别验证码失败";
                status[3] = "重试申请";
                status[4] = "";
                Logscomsole(status);
                //reg();
                return regForIn();
            }
            //检查验证码
            //http://passport.csdn.net/account/register?action=validateCode&validateCode=
            items_reg = new HttpItems() {
                URL= "http://passport.csdn.net/account/register?action=validateCode&validateCode=" + regCode,
                Container =cc_reg 
            };
            hr_reg = heler_reg.GetHtml(items_reg);
            if (hr_reg.Html.ToLower() != "true")
            {
                status[0] = "注册帐号";
                status[1] = "失败";
                status[2] = "识别验证码失败";
                status[3] = "重试申请";
                status[4] = "";
                Logscomsole(status);
                goto Start;
            }
            trytimes = 0;
            //检查名字
            //http://passport.csdn.net/account/register?action=validateUsername&username=
            //检查邮箱
            //http://passport.csdn.net/account/register?action=validateEmail&email=323423424@11.com
            //检查验证码
            //http://passport.csdn.net/account/register?action=validateCode&validateCode=
            //重发激活右键
            //http://passport.csdn.net/account/register?action=resendActiveEmail&username=
            GetMail:

            email = getRegEmail2(cc_em);
            //检查验证码
            //http://passport.csdn.net/account/register?action=validateCode&validateCode=

            hr_reg = heler_reg.GetHtml(new HttpItems() {
                URL = "http://passport.csdn.net/account/register?action=validateEmail&email=" + email,
                Container =cc_reg 
            });
            if (hr_reg.Html.ToLower() != "true")
            {
                trytimes++;
                if (trytimes < 5)
                {
                    Logscomsole(new string[] { "注册帐号", "失败", "邮箱验证失败", hr_reg.Html, "" });
                    goto GetMail;
                }
                else
                {
                    Logscomsole(new string[] { "注册帐号","失败", "邮箱验证失败次数过多",hr_reg .Html ,"" });
                    return new string[] { };
                }
            }
            trytimes = 0;
            GetregName:
            name = GetName();
            //检查名字
            hr_reg = heler_reg.GetHtml(new HttpItems()
            {
                URL = "http://passport.csdn.net/account/register?action=validateUsername&username=" + name,
                Container = cc_reg
            });
            if (hr_reg.Html.ToLower() != "true")
            {
                trytimes++;
                if (trytimes < 5)
                {
                    Logscomsole(new string[] { "注册帐号", "失败", "用户名验证失败", hr_reg.Html, "" });
                    goto GetregName;
                }
                else
                {
                    Logscomsole(new string[] { "注册帐号", "失败", "用户名验证失败次数过多", hr_reg.Html, "" });
                    return new string[] { };
                }
            }
            //string email = string.Format("{0}@qq.com", name);
            string pwd = string.IsNullOrEmpty(mima) ? "aa13655312932bb" : mima;
            Logscomsole(new string[] { "注册帐号", "进行中", "验证成功", "用户名：" + name, "邮箱：" + email });
            PostRequest:

            #region 提交注册请求
            
            hr_reg = heler_reg.GetHtml(new HttpItems()
            {
                Container = cc_reg,
                URL = "http://passport.csdn.net/account/register?action=saveUser&isFrom=False",
                Postdata = string.Format("fromUrl={0}&userName={1}&email={2}&password={3}&confirmpassword={4}&validateCode={5}&agree=on",
                string.Empty, name, email, pwd, pwd, regCode),         
                Method = "POST",

                ProxyIp = proxyip,
            });
            int trycount = 0;
          WaitForEmail:
            trycount++;
            string html = hr_reg.Html.Replace("\r\n", "").Replace("\t", "").Replace("\n", "");

            if (html.Contains("邮件已发送到邮箱")||html .Contains ("不允许在一分钟内重复发送激活邮件，请稍后"))
            {
                Logscomsole(new string[] {"注册帐号","进行中","激活邮件已发送", "等待邮件到达","" });
                string str = "";
                if (!(str = activeRegEmai2(email, cc_em)).Contains("失败"))
                {
                    str = "http" + str.Substring(5);
                    hr_reg = new HttpHelpers().GetHtml(new HttpItems() {
                        URL =str.Replace ("https","http") ,
                        ProxyIp =proxyip ,
                        //Allowautoredirect =true 
                    });

                    hr_reg = new HttpHelpers().GetHtml(new HttpItems()
                    {
                        URL = str.Replace("https", "http"),
                        ProxyIp = proxyip,
                        //Allowautoredirect =true 
                    });
                    string htmlss = hr_reg.Html;
                    if (htmlss.Contains("注册成功")||htmlss .Contains ("账户已经激活"))
                    {
                        status[0] = "注册帐号";
                        status[1] = "成功";
                        status[2] = "帐号：" + name;
                        status[3] = "密码：" + pwd;
                        status[4] = "";
                        Logscomsole(status);
                        return new string[] { name, pwd };
                    }
                    else
                    {
                        status[0] = "注册帐号";
                        status[1] = "失败";
                        status[2] = htmlss;
                        status[3] = "";
                        status[4] = "";
                        Logscomsole(status);
                        return new string[] { };
                    }
                }
                else
                {

                     if(trycount <7)
                    {

                        if (trycount % 5 == 0)
                        {
                            status[0] = "注册帐号";
                            status[1] = "进行中";
                            status[2] = "重发邮件" + name;
                            status[3] = "15秒后重新检测";
                            status[4] = "";
                            Logscomsole(status);
                            hr_reg = new HttpHelpers().GetHtml(new HttpItems()
                            {
                                URL = string.Format("http://passport.csdn.net/account/register?action=resendActiveEmail&username={0}", name),
                                ProxyIp =proxyip 
                            });
                        }
                        else
                        {
                            status[0] = "注册帐号";
                            status[1] = "进行中";
                            status[2] = "等待激活邮件" + name;
                            status[3] = "15秒后重新检测";
                            status[4] = "";
                            Logscomsole(status);
                        }
                        goto WaitForEmail;
                    }


                    status[0] = "注册帐号";
                    status[1] = "失败";
                    status[2] = "激活邮箱问题";
                    status[3] = str;
                    status[4] = "";
                    Logscomsole(status);
                    if (str.Contains("errorerror"))
                    {
                        return regForIn();
                    }
                    return new string[] { str };
                }
            }
            else
            {
                status[0] = "注册帐号";
                status[1] = "失败";


                if (html.Contains("此ip单位时间内注册个数已超过限定值"))
                {
                    //if (regeristResult != null)
                    //{
                    //    regeristResult();
                    //}
                    status[2] = "此ip单位时间内注册个数已超过限定值";
                    status[3] = "";
                    status[4] = "";
                    Logscomsole(status);
                    return new string[] { "单位时间内注册个数已超过限定值" };
                }
                else
                {
                    status[2] = "验证码错误";
                    status[3] = "重试申请";
                    status[4] = html;
                    Logscomsole(status);
                    goto Start;
                    // return;
                }
            }
            #endregion

            status[0] = "注册帐号";
            status[1] = "失败";
            status[2] = "未知原因";
            status[3] = "";
            status[4] = "";
            Logscomsole(status);

            return new string[] { };
        }


        public string GetName()
        {
            string name = string.Empty;
            string a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string b = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_";
            name += a[ran.Next(a.Length)];
            int z = ran.Next(6, 10);
            for (int i = 0; i < z; i++)
            {
                name += b[ran.Next(b.Length)];
            }
            return name;
        }


        #endregion 注册



        double DowCount=0;

        private bool getInfo()
        {

            hr_com = new HttpHelpers().GetHtml(new HttpItems()
            {
                URL = "http://download.csdn.net/my/downloads",
                Container = cc
            });

            string DownHtml = hr_com.Html.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");
            string result = Regex.Replace(DownHtml, @"<!--(.+?)-->", "");
            //获取下载资源数量
            Regex regDown = new Regex("下载资源：<spanclass=\"red\">(.+?)</span>");
            MatchCollection mcDown = regDown.Matches(result);
             DowCount = 0;
            if (mcDown.Count > 0)
            {
                DowCount = Convert.ToDouble(mcDown[0].Groups[1].Value);
            }
            Regex regJf = new Regex("<em>积分：(.+?)</em>");
            MatchCollection mcJf = regJf.Matches(result);
            if (mcJf.Count > 0)
            {
               return  int.TryParse(mcJf[0].Groups[1].Value, out jfCount);
            }
            return false;
        }

        #region 下载

        private void download()
        {
            if (ListPreTDown == null || ListPreTDown.Count == 0)
            {
                Logscomsole(getdownlist ());
            }
            Logscomsole(new string[] { "批量模拟下载", "开始" , "开始执行","","" } );
            foreach (CsdnResouce item in listPreTDown)
            {
                item.Method = "模拟下载";
                Logscomsole(downloadOne(item));
                Thread.Sleep(TimeForDown);
            }
            Logscomsole(new string[] { "自动模拟下载", "结束", "执行完毕", "", "" });
            //listPreTDown = new List<CsdnResouce>();
            //if (conload)
            //{
            //    thread_down = new Thread(new ThreadStart(download));
            //    thread_down.Start();
            //} 
        }

        private string [] getdownlist()
        {
            getInfo();
              string[] status = { "操作", "成功/失败", "状态信息", "无验证码", "无附加信息" };
              status[0] = "获取下载列表";
              status[1] = "开始";
              status[2] = "随机扫描数据";
              status[3] = "";
              status[4] = "";
            Logscomsole (status );

            String szReg_res_url = ("<li><atitle=\"共(.+?)个资源\"href=\"(.+?)\">(.+?)</a></li>");
            String szReg_res_url_pagecount = ("下一页</a>&nbsp;&nbsp;<aclass=\"pageliststy\"href=\"(.+?)\">尾页</a>");
            String szReg_res_down_url = ("<dt><ahref=\"(.+?)\">(.+?)</a><spanclass=\"marks\">(.+?)</span></dt>");
           // String szReg_res_down_forfree_url = ("<dt><ahref=\"(.+?)\">(.+?)</a><spanclass=\"marks\">0</span></dt>");
            //String szReg_res_down_per_url = ("action=\"(http://download.csdn.net/index.php/source/do_download/(.*?))\"");

            string result = new HttpHelpers().GetHtml(new HttpItems() { URL = "http://download.csdn.net/" }).Html;
            result = result.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");


            Regex reg = new Regex(szReg_res_url);
            MatchCollection mc = reg.Matches(result);
            List<string> strList = new List<string>();

            foreach (Match item in mc)
            {
                //<li><atitle="共685219个资源"href="/c-16006">Java</a></li>
                string tmp = item.Value;
                string[] tmps = tmp.Split(new char[] { '/' });
                tmp = tmp.Substring(tmp.LastIndexOf("=") + 2, tmp.LastIndexOf(@""">") - tmp.LastIndexOf("=") - 2);
                strList.Add(tmp);
            }
            int a = new Random().Next(strList.Count);
    
            result = new HttpHelpers().GetHtml(new HttpItems() { URL = string.Format("http://download.csdn.net{0}", strList[a]) }).Html;
            result = result.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            reg = new Regex(szReg_res_url_pagecount);
            Match mat = reg.Match(result);
            string durl = mat .Groups[1].Value;
            string[] ss = durl.Split(new char[] { '/', '\"' }).Where <string >((o)=>{ return !string.IsNullOrEmpty(o); }).ToArray ();
            int pageCount;
            int.TryParse(ss[1], out pageCount);

            ListPreTDown = new List<CsdnResouce>();

            for (int j = 0; j < pageCount; j++)
            {
                result = new HttpHelpers().GetHtml(new HttpItems()
                {
                    URL = String.Format("http://download.csdn.net{0}/{1}", strList[a], j + 1)

                }).Html;
                result = result.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");
                reg = new Regex(szReg_res_down_url);
                mc = reg.Matches(result);
                // List<string> downstr = new List<string>();
                foreach (Match m in mc)
                {
                    //<dt><ahref="/detail/u014554531/9313813"target="_blank">TakeColor优秀屏幕取色工具</a><spanclass="marks">1</span></dt>
                    string regforhref = "<dt><ahref=\"(.*?)\"target=\"_blank\">(.*?)</a><spanclass=\"marks\">(.*?)</span></dt>";
                    Match mcforhref = new Regex(regforhref).Match(m.Value);
                    string href =mcforhref.Groups[1].Value;
                    string namefor = mcforhref.Groups[2].Value;
                    string co = mcforhref.Groups[3].Value;
                    //string[] tmp1 = m.Value.Split(new char[] { '\"', '>', '<' });
                    if (co  == "0")
                    {
                        CsdnResouce cdsr = new CsdnResouce(namefor ,href );
                        cdsr.Tag = String.Format("http://download.csdn.net{0}", cdsr.Url);
                        ListPreTDown.Add(cdsr);
                    }
                }
            }
            End:
            //string[] status = { "操作", "成功/失败", "状态信息", "无验证码", "无附加信息" };
            //status[0] = "获取下载列表";
            status[1] = "结束";
            status[2] = "可下载数据：" + ListPreTDown.Count.ToString() + " 条";
            //status[3] = "";
            //status[4] = "";
            //Logscomsole();
            return status;
        }
        private string [] downloadOne(CsdnResouce cdrs)
        {
            string[] status = { "模拟下载", "成功/失败", "无验证码", "状态信息", "无附加信息" };
            //if (code != "")
            //{
            //    status[3] = "验证码:" + code;
            //}
            status[0] = "模拟下载";
            status[1] = "";
            status[2] = "";
            status[3] = "";
            status[4] = "";
            //Logscomsole(status);
            string url = cdrs.Tag.ToString().Replace("detail", "download");
            string res = new HttpHelpers().GetHtml(new HttpItems()
            {
                URL = url ,
                Container = cc
            }).Html;

            string taskName = "";
            string regexCode = "<meta name=\"description\" content=\"(.*?)\" />";
            Regex reg = new Regex(regexCode);
            taskName = reg.Match(res).Groups[1].Value;
            regexCode = "action=\"(http://download.csdn.net/index.php/source/do_download/(.*?))\"";
            reg = new Regex(regexCode);
            if (reg.IsMatch(res))
            {
                string durl = reg.Match(res).Groups[1].Value;
                // cdrs .Durl = durl;//下载地址
                bool first = true;
                hr_down = new HttpHelpers().GetHtml(new HttpItems()
                {
                    URL = durl,
                    Referer = url,
                    Method = "Post",
                    Container = cc,
                    Allowautoredirect = true,
                    Postdata = "ds=&validate_code=&basic%5Breal_name%5D=&basic%5Bmobile%5D=&basic%5Bemail%5D=&basic%5Bjob%5D=&basic%5Bcompany%5D=&basic%5Bprovince%5D=&basic%5Bcity%5D=&basic%5Bindustry%5D="
                });
                res = hr_down.Html;
            TRUEDOWNLOAD:
                if (res.Contains("您因违反"))
                {
                    status[0] = "模拟下载";
                    status[1] = "失败";
                    status[2] = res ;
                    status[3] = "";
                    status[4] = "";
                    Logscomsole(status );
                    this.Clear();
                    return status;
                }
                if (res != "<script>document.domain='csdn.net';parent.show_validate_pop();</script>")
                {           //模拟点击下载
                    if (saveFile && !string.IsNullOrEmpty(savePath))
                    {

                        if (!Directory.Exists(SavePath))
                        {
                            Directory.CreateDirectory(SavePath);
                        }
                        //href='http://dldx.csdn.net/fd.php?i=391492108198497&s=c936639a4ad2171ef2bee9ae9e706700';
  
                        try
                        {
                            hr_down = new HttpHelpers().GetHtml(new HttpItems()
                            {
                                URL = res.Substring(res.IndexOf("http"), res.LastIndexOf('\'') - res.IndexOf("http")),
                                Referer = durl,
                                Container = cc,
                                ResultType = ResultType.Byte,
                                Allowautoredirect = true
                            });
                            //hr.ResultByte;  真实文件内容
                            byte[] bty = hr_down.ResultByte;
                            string a = hr_down.Header.Get("Content-Disposition");
                            a = a.Substring(a.LastIndexOf('\'') + 1);
                            a = HttpUtility.UrlDecode(a, Encoding.UTF8);
                            //a = Encoding.Default.GetString(Encoding.UTF8.GetBytes (a));
                            //a = Encoding.UTF8.GetString(Encoding .UTF8 .GetBytes (a) );
                            cdrs.Log = a;
                            cdrs.Msg = "已保存";
                            File.WriteAllBytes(SavePath + @"\" + a, bty);
                        }
                        catch (Exception ex)
                        {

                            cdrs.Msg = "保存失败：" + ex.Message;
                        }

                    }
                    status[0] = "模拟下载";
                    status[1] = "成功";
                    status[3] = string.IsNullOrEmpty (cdrs.Name )?"":cdrs .Name ;
                    status[2] = first ?"无验证码":"已智能识别验证码";
                    status[4] = "";
                    //cdrs.Rel = "成功";
                    //cdrs.Msg = res==""?"":"验证码："+res ;
                    //Logscomsole("", "", "", cdrs);
                    return status ;
                }
                else
                {
                    if (first && getImgVcode != null)
                    {
                        res = getdownVcodeUrl(durl, url);
                        first = false;
                        goto TRUEDOWNLOAD;
                    }
                    status[0] = "模拟下载";
                    status[1] = "失败";
                    status[2] = "多次识别错误或者验证码识别接口出错";
                    status[3] = "";
                    status[4] = "";
                }

            }
            else
            {
                status[0] = "模拟下载";
                status[1] = "失败";
                status[2] = "获取下载地址失败";
                status[3] = "";
                status[4] = "";
                //Logscomsole(status);
                //cdrs.Rel = "失败";
                //cdrs.Msg = "获取下载抵制失败";
            }

            //Logscomsole("", "", "", cdrs);
            return status ;
        }

        private string getdownVcodeUrl(string url, string referurl)
        {
            string[] status = { "模拟下载", "成功/失败", "状态信息", "无验证码", "无附加信息" };
                
            int tryTime = 0;
            string code = "";
            string res = "";
            byte[] imgbytes = null;
            while (tryTime < 5)
            {
                tryTime++;
                HttpResults hr = new HttpHelpers().GetHtml(new HttpItems()
                {
                    URL = @"http://download.csdn.net/index.php/rest/tools/validcode/source_ip_validate/10.1345321661792" + new Random().Next().ToString(),
                    Referer = referurl,
                    Container = this.cc,
                    ResultType = ResultType.Byte

                });
                imgbytes = hr.ResultByte;
                code = getImgVcode(imgbytes);
                if (code == "")
                {
                    continue;
                }
                res = new HttpHelpers().GetHtml(new HttpItems()
                {
                    URL = url,
                    Referer = referurl,
                    Method = "Post",
                    Container = cc,
                    Allowautoredirect = true,
                    Postdata = "ds=&validate_code=" + code + "&basic%5Breal_name%5D=&basic%5Bmobile%5D=&basic%5Bemail%5D=&basic%5Bjob%5D=&basic%5Bcompany%5D=&basic%5Bprovince%5D=&basic%5Bcity%5D=&basic%5Bindustry%5D="
                }).Html;
                if (!"<script>document.domain='csdn.net';parent.show_validate_pop();</script>".Equals(res))
                {
                        //if (code != "")
                    //{
                    //    status[3] = "验证码:" + code;
                    //}
                    //status[0] = "";
                    status[1] = "成功";
                    status[2] = "验证码："+code ;
                    status[3] = "";
                    status[4] = "";
                   // Logscomsole(status );
                    return res;
                }
                //File.WriteAllBytes(System.Environment.CurrentDirectory + @"\Codes\" + imgbytes .Length .ToString ()+"_"+ code + ".bmp", imgbytes);
           
            }
            status[0] = "智能识别下载验证码";
            status[1] = "失败";
            status[2] = "超过5次" ;
            status[3] = "";
            status[4] = "";
            Logscomsole(status);
            //Logscomsole("识别下载验证码", "失败", "次数：" + tryTime.ToString());
            return "";
        }

        #endregion 下载

        #region 评分

        private void command()
        {
          StartPoint:
            //string[] msg = new string[4];
            if (listConSource == null || listConSource.Count == 0)
            {
                Logscomsole(new string[] { "获取评分列表", "开始", "开始获取", "耐心等待", "默认5线程/账户 进行操作" });
                listConSource = new List<CsdnResouce>();
                Logscomsole(getCommandList());
                //if (listConSource.Count == 0)
                //{
                //    Logscomsole(new string[] { "批量评分", "失败", "已无可评分资源", "", "" });
                //    getInfo();
                //    if (pointNum !=-1 &&JfCount >= pointNum)
                //    {
                //        auto();
                //        return;
                //    }
                //    else if(isInAuto )
                //    {
                //        Logscomsole(new string[] { "批量评分", "失败", "已无可评分资源", "任务未完成状态", "5分钟后自动重试" });
                //        Thread.Sleep(1000*60*5);
                //        goto StartPoint;
                //    }
                //    return;
                //}
            }
            Logscomsole(new string[] { "批量评分", "开始", "开始执行", "", "" });

            foreach (CsdnResouce item in listConSource)
            {
                string[] msg = commandOne(item);
                Logscomsole(msg);
                if (msg[4].Contains("间隔60秒"))
                {
                    Thread.Sleep(TimeForCom);
                    msg = commandOne(item);
                }
                if ("成功".Equals(msg[1]))
                {
                    Thread.Sleep(TimeForCom);
                }
            }
            Logscomsole(new string[] { "批量评分", "结束", "执行完毕", "", "" });
        }

        private  string Decode(string s)
        {
            return new Regex(@"\\u([0-9a-fA-F]{4})").Replace(s, m =>
            {
                short c;
                if (short.TryParse(m.Groups[1].Value, System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture, out c))
                {
                    return "" + (char)c;
                }
                return m.Value;
            });
        }

        private string[] commandOne(CsdnResouce item, string code = "")
        {

            string[] status = { "模拟评分", "成功/失败", "无验证码", "状态信息", "无附加信息" };
            if (code != "")
            {
                status[2] = "验证码:"+code ;
            }
            hr_com = new HttpHelpers().GetHtml(new HttpItems() {
                URL = "http://download.csdn.net/index.php/comment/get_comment_data/" + item.Id + @"/1?jsonpcallback=jsonp1448000002880&&t=1448000436379",
                Container = cc,
                Referer = item.Url 
            } );

            string rehtml = hr_com.Html.Replace("\\r\\n", "").Replace("\\n", "").Replace("\\t", "").Replace(" ", "");
            rehtml = Decode(rehtml);
            Regex regcont=new Regex (@"user_name\\"">(.*?)<\\\/a><\\\/dt><dd>(.*?)<");
            MatchCollection mccont = regcont.Matches(rehtml);
            rehtml = "";
            foreach (Match  m in mccont )
            {
                if (rehtml.Length < m.Groups[2].Value.Length)
                {
                    rehtml = m.Groups[2].Value;
                }
            }
            //getCommandList();
            long epochS = (DateTime.Now.AddSeconds(-5).ToUniversalTime().Ticks - 621355968000000000) / 10000;
            long epochE = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
            //Get评论
            int index = ran.Next(comMsg.Length);
            string commentcon=comMsg [index ]+"，"+rehtml ;
            string url;

            if ("".Equals(code))
            {
                url = string.Format("http://download.csdn.net/index.php/comment/post_comment?jsonpcallback=jsonp{0}&sourceid={1}&content={2}&rating={3}&t={4}", epochS, item.Id, new XJHTTP().UrlEncoding(item.Name + commentcon ), new Random().Next(3, 6), epochE);
            }
            else
            {
                url = string.Format("http://download.csdn.net/index.php/comment/post_comment?jsonpcallback=jsonp{0}&sourceid={1}&content={2}&txt_validcode={3}&rating={4}&t={5}", epochS, item.Id, new XJHTTP().UrlEncoding(item.Name + commentcon), code, new Random().Next(3, 6), epochE);
            }

            string html = new HttpHelpers().GetHtml(new HttpItems() {
                URL =url ,
                Container =cc 
            }).Html;

            //评论成功完成
            if (html.Contains("\"succ\":1"))
            {
                JfCount++;
                status[1] = "成功";
                status[3] = "当前积分：" + JfCount.ToString();
                status[4] = item.Name + comMsg[index];
            }
            else
            {
                status[1] = "失败";
                status[3] = "当前积分：" + JfCount.ToString();
                
                Regex reg = new Regex("msg\":\"(.+?)\"");
                string msg = new XJHTTP().FromUnicodeString(html);
                MatchCollection mc = reg.Matches(msg);
                if (mc.Count > 0)
                {
                    status[4] = mc[0].Groups[1].Value;
                    //item.Msg = mc[0].Groups[1].Value;
                }
                else
                {
                    status[4] = msg;
                    //item.Msg = msg;
                }

                if ((status[4].Contains("请填写验证码") || status[4].Contains("验证码错误")) && getImgVcode != null)
                {
                    string vcode = getcomVcode("http://download.csdn.net/index.php");
                    if (vcode != "")
                    {
                        return commandOne(item, vcode);
                    }
                }
                else if(msg .Contains ("您因违反"))
                {
                    status[4] = msg;
                    Logscomsole(status );
                    this.Clear();
                    return status;
                }

            }
            return status; 
        }

        private string getcomVcode(string referurl)
        {
            string[] status = { "模拟下载", "成功/失败", "状态信息", "无验证码", "无附加信息" };
            status[0] = "模拟下载";
            status[1] = "";
            status[2] = "";
            status[3] = "";
            status[4] = "";
            //Logscomsole(status);

            int tryTime = 0;
            string code = "";
            string res = "";
            byte [] imgbyte=null ;
            while (tryTime < 5)
            {
                tryTime++;

                hr_com = new HttpHelpers().GetHtml(new HttpItems()
                {

                    URL = @"http://download.csdn.net/index.php/rest/tools/validcode/comment_validate/10.1749821768607" + new Random().Next().ToString(),
                    Referer = referurl,
                    Container = this.cc,
                    ResultType = ResultType.Byte,
                     
                });
                imgbyte =hr_com.ResultByte;
                code = getImgVcode(imgbyte );
                if (code == "")
                {
                    continue;
                }
                res = new HttpHelpers().GetHtml(new HttpItems()
                {
                    URL = @"http://download.csdn.net/index.php/comment/check_validcode/" + code,
                    Referer = referurl,
                    Container = cc
                }).Html;
                if (res != "验证码错误")
                {
                    return code;
                }
                //File.WriteAllBytes(System.Environment.CurrentDirectory + @"\Codes\" + imgbyte.Length.ToString() + "_" + code + ".bmp", imgbyte);
            }
            Logscomsole(new string[] { "识别评分验证码", "失败", "次数:" + tryTime.ToString(),"","" });
            //Logscomsole("识别评分验证码", "失败", "次数：" + tryTime.ToString());
            return "";
        }

        private string[] getCommandList()
        {
            listConSource = new List<CsdnResouce>();
            string[] status = { "获取评分列表", "成功/失败", "状态信息", "无验证码", "无附加信息" };
            //status[1] = "结束";
            status[2] = "执行完毕";
            status[3] = "";
            status[4] = "";
           // Logscomsole(status);

            PreComCount = 0;
            CanComCount = 0;

            //我的下载资源页的资源列表  
            hr_com = new HttpHelpers().GetHtml(new HttpItems() {
                URL = "http://download.csdn.net/my/downloads",
                Container = cc
            });
            string DownHtml = hr_com.Html.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");
            string result = Regex.Replace(DownHtml, @"<!--(.+?)-->", "");
            //获取下载资源数量
            Regex regDown = new Regex("下载资源：<spanclass=\"red\">(.+?)</span>");
            MatchCollection mcDown = regDown.Matches(result);
             DowCount = 0;
            if (mcDown.Count > 0)
            {
                DowCount = Convert.ToDouble(mcDown[0].Groups[1].Value);
            }
            status[1] = "成功";
            status[2] = "下载数："+DowCount .ToString ();
            //获取剩余积分
            Regex regJf = new Regex("<em>积分：(.+?)</em>");
            MatchCollection mcJf = regJf.Matches(result);
            if (mcJf.Count > 0)
            {
                int.TryParse(mcJf[0].Groups[1].Value, out jfCount);
            }

            status[3] = "积分数："+jfCount .ToString ();
  
            //加载数据，Csdn是6个一页
            double Ys = Math.Ceiling(DowCount / 6);

            //10页以上用多线程
            if (Ys > 50)
            {
                //LoadList(DowCount, Ys);
                int threadCount = 5;
                int cnum = (int)Ys / (threadCount - 1);
                //int ynum = (int)Ys % (threadCount - 1);

                var waits = new List<EventWaitHandle>();
                for (int i = 0; i < threadCount-1; i++)
                {
                    int s = 1 + i * cnum;
                    int e = cnum + cnum * i;
                    if (e > Ys)
                    {
                        e = (int)Ys;
                    }
                    //int e=(((cnum +cnum *i)> DowCount )? (int )DowCount:(cnum +cnum *i)) ;

                    var handler = new ManualResetEvent(false);
                    waits.Add(handler);
                    new Thread(new ParameterizedThreadStart(threadForScan))
                        .Start(new Tuple<int[], EventWaitHandle>(new int[] { s, e }, handler));
                }
                WaitHandle.WaitAll(waits.ToArray());
                status[2] = "下载数：" + DowCount.ToString();
                status[3] = "积分数：" + jfCount.ToString();
                status[4] = "共 " + CanComCount.ToString() + " 条可评分记录，" + PreComCount.ToString() + " 条10分钟以内记录";
                return status;
            }
            else
            {
                for (int i = 1; i <= Ys; i++)
                {
                    hr_com = new HttpHelpers().GetHtml(new HttpItems() {

                        URL = "http://download.csdn.net/my/downloads/" + i,
                        Container = cc
                    });
                    string Down2Html = hr_com.Html.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");
                    string result2 = Regex.Replace(Down2Html, @"<!--(.+?)-->", "");
                    //SetList(result2);
                    //获取资源列表
                    Regex reg = new Regex("<dt><divclass=\"icon\"><imgsrc=\"(.+?)\"title=\"(.+?)\"></div><divclass=\"btns\">(.+?)</div><h3><ahref=\"(.+?)\">(.+?)</a><spanclass=\"points\">(.+?)</span></h3></dt>");
                    MatchCollection mc = reg.Matches(result2);
                    if (mc.Count > 0)
                    {
                        foreach (Match item in mc)
                        {
                            CsdnResouce cdsr = new CsdnResouce();

                            Regex regCState = new Regex("<(.+?)>(.+?)</(.+?)>");
                            MatchCollection mcCState = regCState.Matches(item.Groups[3].Value);
                            string sta = mcCState[0].Groups[2].Value;
                            if (sta.Contains("已评价") || sta.Contains("资源已删除"))
                            {
                                continue;
                            }
                            if (!sta.Contains("立即评价"))
                            {
                                PreComCount++;
                                continue;
                            }
                            CanComCount++;
                            String strt = item.Groups[4].Value;
                            cdsr.Url = strt.Substring(0, strt.IndexOf("\""));
                            cdsr.Name = item.Groups[5].Value;
                            cdsr.Point = item.Groups[6].Value;
                            string[] ids = cdsr.Url.Split('/');
                            cdsr.Id = ids[ids.Length - 1];
                            listConSource.Add(cdsr);
                        }
                    }
                    //status[1] = "结束";
                    //status[2] = "执行完毕";
                    //status[3] = "";
                    status[2] = "已完成扫描 " + i.ToString() + "/" + Ys.ToString();
                    status[3] = "";
                    //Logscomsole(status);
                }
                status[2] = "下载数：" + DowCount.ToString();
                status[3] = "积分数：" + jfCount.ToString();
                status[4] = "共 " + CanComCount.ToString() + " 条可评分记录，" + PreComCount.ToString() + " 条10分钟以内记录";
                return status;
            }
            //Logscomsole("获取可评分列表", "结束", "获取到 " + CanComCount.ToString() + " 条可评分记录, " + PreComCount.ToString() + " 条10分钟以内记录");
        }

        private void threadForScan(object param)
        {
            int s, e;
            var p = (Tuple<int[], EventWaitHandle>)param;
            s = p.Item1[0];
            e = p.Item1[1];
            if (s > e)
            {
                Console.WriteLine("线程结束在："+e.ToString ());
                p.Item2.Set();
                return;
            }
            List<CsdnResouce> tmplist = new List<CsdnResouce>();
            HttpResults    tmpHr = new HttpHelpers ().GetHtml(new HttpItems() {
                URL = "http://download.csdn.net/my/downloads/" + s,
                Container = cc
            });
            string Down2Html = tmpHr.Html.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");
            string result2 = Regex.Replace(Down2Html, @"<!--(.+?)-->", "");
            //SetList(result2);
            //获取资源列表
            Regex reg = new Regex("<dt><divclass=\"icon\"><imgsrc=\"(.+?)\"title=\"(.+?)\"></div><divclass=\"btns\">(.+?)</div><h3><ahref=\"(.+?)\">(.+?)</a><spanclass=\"points\">(.+?)</span></h3></dt>");
            MatchCollection mc = reg.Matches(result2);
            if (mc.Count > 0)
            {
                foreach (Match item in mc)
                {
                    CsdnResouce cdsr = new CsdnResouce();

                    Regex regCState = new Regex("<(.+?)>(.+?)</(.+?)>");
                    MatchCollection mcCState = regCState.Matches(item.Groups[3].Value);
                    string sta = mcCState[0].Groups[2].Value;
                    if (sta.Contains("已评价") || sta.Contains("资源已删除"))
                    {
                        continue;
                    }
                    if (!sta.Contains("立即评价"))
                    {
                        PreComCount++;
                        continue;
                    }
                    CanComCount++;
                    String strt = item.Groups[4].Value;
                    cdsr.Url = "http://download.csdn.net" + strt.Substring(0, strt.IndexOf("\""));
                    cdsr.Name = item.Groups[5].Value;
                    cdsr.Point = item.Groups[6].Value;
                    string[] ids = cdsr.Url.Split('/');
                    cdsr.Id = ids[ids.Length - 1];
                    tmplist.Add(cdsr);
                }
            }
            if (tmplist.Count > 0)
            {
                lock (LockObject)
                {
                    listConSource.AddRange(tmplist );
                }
            }
            s++;
            new Thread(new ParameterizedThreadStart(threadForScan))
                    .Start(new Tuple<int[], EventWaitHandle>(new int[] { s, e }, p .Item2 ));
        }

        #endregion 评分

        private Dictionary<string, string> getPater(string allText, string head, string end, char[] zusplit, char[] kvsplit)
        {
            Dictionary<string, string> re = new Dictionary<string, string>();
            if (head != "" && allText.IndexOf(head) != -1)
            {
                allText = allText.Substring(allText.IndexOf(head) + head.Length);
            }
            if (end != "" && allText.IndexOf(end) != -1)
            {
                allText = allText.Substring(0, allText.IndexOf(end));
            }
            string[] tmps = allText.Split(zusplit).Where(s => !string.IsNullOrEmpty(s)).ToArray();
            //tmps = tmps;
            foreach (string item in tmps)
            {
                string[] tmp2 = item.Split(kvsplit).Where(s => !string.IsNullOrEmpty(s)).ToArray();
                if (tmp2.Length == 2 && !re.ContainsKey(tmp2[0]))
                {
                    re.Add(tmp2[0], tmp2[1]);
                }
            }


            return re;
        }

        private void Logscomsole(string[] msg )
        {
            string log="";
            if (msg != null && msg.Length == 5)
            {
                log = string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}",DateTime.Now.ToString("hh:mm:ss.ffff"), string.IsNullOrEmpty (NickName ) ? "UNKNOWN" : NickName, msg [0], msg[1], msg[2],msg [3] ,msg[4]);
          
            }
            if(log !="" && showLogs !=null )
            showLogs (log );
        }

        public CsdnHelper(string user,string pass)
        {
            cc = new CookieContainer();
            this.user = user;
            this.pass = pass;
            this.SavePath = System.Environment.CurrentDirectory+@"\"+this.user ;
        }
    }


    /// <summary>
    /// 资源信息
    /// </summary>
    public class CsdnResouce
    {
        public CsdnResouce()
        {

        }
        public CsdnResouce(string name, string url)
        {
            this.Name = name;
            this.url = url;
        }
        private string method;
        /// <summary>
        /// 当前操作
        /// </summary>
        public string Method
        {
            get { return method; }
            set { method = value; }
        }
        private string rel;

        public string Rel
        {
            get { return rel; }
            set { rel = value; }
        }

        private string msg;

        public string Msg
        {
            get { return msg; }
            set { msg = value; }
        }

        private string log;

        public string Log
        {
            get { return log; }
            set { log = value; }
        }

        private string state;
        /// <summary>
        /// 状态
        /// </summary>
        public string State
        {
            get { return state; }
            set { state = value; }
        }
        private string durl;

        public string Durl
        {
            get { return durl; }
            set { durl = value; }
        }


        private string tag;


        public string Tag
        {
            get { return tag; }
            set { tag = value; }
        }
        private string id;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string url;

        public string Url
        {
            get { return url; }
            set { url = value; }
        }
        private bool down = false;

        public bool Down
        {
            get { return down; }
            set { down = value; }
        }
        private bool comment = false;

        public bool Comment
        {
            get { return comment; }
            set { comment = value; }
        }
        private string point;

        public string Point
        {
            get { return point; }
            set { point = value; }
        }

    }
}
