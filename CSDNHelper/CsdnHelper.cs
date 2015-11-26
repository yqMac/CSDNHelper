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
        private HttpHelpers helper_down;
        private HttpItems items_down;
        private HttpResults hr_down;

       // private CookieContainer cc;
        private HttpHelpers helper_com;
        private HttpItems items_com;
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

        bool conload = true;

        bool isInAuto = false;
        #endregion 变量


        #region 公开



        public CsdnHelper()
        {
            cc = new CookieContainer();
            helper_down = new HttpHelpers();
            helper_com = new HttpHelpers();
        }

        public string Login()
        {
            string[] status = { "操作", "成功/失败", "状态信息", "无验证码", "无附加信息" };

            if (User == "" || Pass == "")
            {
                status[0] = "模拟登录";
                status[1] = "失败";
                status[2] = "帐号或者密码有误";
                status[3] = "";
                status[4] = "";
                Logscomsole(status);
                return "";
            }
            status[0] = "模拟登录";
            status[1] = "开始";
            status[2] = "";
            status[3] = "";
            status[4] = "";
            //Logscomsole(status);

            string reText = "";
            string url = "https://passport.csdn.net/account/login";//请求地址
            string loginUrl = "https://passport.csdn.net/account/login";
            reText = string.Empty;//请求结果,请求类型不是图片时有效   
            items_down = new HttpItems();
            items_down.URL = url;//设置请求地址
            items_down.Container = cc;//自动处理Cookie时,每次提交时对cc赋值即可
            //helper_down = new HttpHelpers();
            hr_down = helper_down.GetHtml(items_down);//发起请求并得到结果
            reText = hr_down.Html;//得到请求结果
            if (!string.IsNullOrEmpty(reText) && !reText.Equals("请求被中止: 连接被意外关闭。"))
            {

                //内部变量
                string lt = string.Empty, execution = string.Empty, eventId = string.Empty;
                //匹配必须参数
                //-lt-
               

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
                Regex reg2 = new Regex("name=\"execution\" value=\"(.+?)\"");
                MatchCollection mc2 = reg2.Matches(reText);
                execution = mc2[0].Groups[1].Value;
                //-eventId-
                Regex reg3 = new Regex("name=\"_eventId\" value=\"(.+?)\"");
                MatchCollection mc3 = reg3.Matches(reText);
                eventId = mc3[0].Groups[1].Value;

                items_down = new HttpItems();
                items_down.URL = loginUrl;
                items_down.Container = cc;
                items_down.Method = "POST";
                items_down.Postdata = string.Format("username={0}&password={1}&lt={2}&execution={3}&_eventId={4}", user, pass, lt, execution, eventId);
                hr_down = helper_down.GetHtml(items_down);
                reText = hr_down.Html;
                //检查是否登录成功
                Regex regerro = new Regex("<span id=\"error-message\">(.+?)</span>");
                MatchCollection mcerro = regerro.Matches(reText);
                //错误数大于0
                if (mcerro.Count > 0)
                {
                    reText = mcerro[0].Groups[1].Value;
                    status[0] = "模拟登录";
                    status[1] = "失败";
                    status[2] = reText;
                    status[3] = "";
                    status[4] = "";
                    //Logscomsole(status);
                    Logscomsole(status);
                    isonline = false;
                }
                else
                {
                    if (reText.Contains("redirect_back") && reText.Contains("loginapi.js"))
                    {
                        reText = reText.Replace("\r\n", "").Replace("\n", "").Replace("\t", "");
                        NickName = getPater(reText, "var data = {", "};", new char[] { ',' }, new char[] { ':', '\"' })["nickName"];


                        // NickName =loginInfos [95];
                        reText = "登录成功";

                        status[0] = "模拟登录";
                        status[1] = "成功";
                        status[2] = "昵称：" + NickName;
                        status[3] = "";
                        status[4] = "";
                        Logscomsole(status);

                        isonline = true;
                    }
                    else
                    {
                        reText = "登录失败,账号已失效！";
                        status[0] = "模拟登录";
                        status[1] = "失败";
                        status[2] = reText;
                        status[3] = "";
                        status[4] = "";
                        Logscomsole(status);
                        isonline = false;
                    }
                }
            }

            return reText;
        }


        public void Command()
        {
            string[] status = { "操作", "成功/失败", "状态信息", "无验证码", "无附加信息" };
            status[0] = "自动评分";
            status[1] = "开始";
            status[2] = "";
            status[3] = "";
            status[4] = "";
            //Logscomsole(status);


            if (!Isonline)
            {
                status[0] = "自动评分";
                status[1] = "失败";
                status[2] = "未登录呢";
                status[3] = "";
                status[4] = "";
                Logscomsole(status);
                //Logscomsole("自动评分", "失败", "未登录呢");
                return;
            }
            if (comMsg==null||comMsg.Length == 0)
            {
                status[0] = "自动评分";
                status[1] = "失败";
                status[2] = "语句库[comMsg]是空的";
                status[3] = "";
                status[4] = "";
                Logscomsole(status);
                //Console.WriteLine("语句库[comMsg]是空的");
                //Logscomsole("自动评分", "失败", "语句库[comMsg]是空的");
                return;
            }
            if (thread_com != null && thread_com.IsAlive)
            {
                thread_com.Abort();
            }
            thread_com = new Thread(new ThreadStart(command));
            thread_com.Start();

        }


        public void DownloadFree(List <CsdnResouce >list=null )
        {
            if (list != null&&list .Count >0)
            {
                ListPreTDown = list;
                conload = false;
            }
            else
            {
                conload = true;
            }
            if (thread_down != null && thread_down.IsAlive)
            {
                thread_down.Abort();
            }
            thread_down = new Thread(new ThreadStart(download));
            thread_down.Start();
        }


        public void Clear()
        {
            string[] status = { "操作", "成功/失败", "状态信息", "无验证码", "无附加信息" };
            status[0] = "自动评分";
            status[1] = "开始";
            status[2] = "";
            status[3] = "";
            status[4] = "";
            //Logscomsole(status);
            if (thread_com != null && thread_com.IsAlive)
            {
                status[0] = "自动模拟评分";
                status[1] = "结束";
                status[2] = "被强制终止";
                status[3] = "";
                status[4] = "";
                Logscomsole(status);
                //Logscomsole("自动模拟评分", "结束", "被强制终止");
                thread_com.Abort();
                thread_com = null;
            }
            if (thread_down != null && thread_down.IsAlive)
            {
                status[0] = "自动模拟下载";
                status[1] = "结束";
                status[2] = "被强制终止";
                status[3] = "";
                status[4] = "";
                Logscomsole(status);
                //Logscomsole("自动模拟下载", "结束", "被强制终止");
                thread_down.Abort();
                thread_down = null;
            }
            listConSource = new List<CsdnResouce>();
            listPreTDown = new List<CsdnResouce>();
        }


        public List<CsdnResouce> GetUploadRs()
        {
            getUploadRs();
            return uploadedRS;
        }



        private string mima = "";
        private int pointNum = -1;
        private int finishNum = 0;

        public void AutoRunTocheck(string mima,int pointnum) {
            isInAuto = true;
            this.mima = mima;
            this.Pass  = mima;
            this.pointNum = pointnum;
            new Thread(new ThreadStart (auto )).Start ();

        }


        #endregion 公开

        #region 自动化程序
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
                        Thread.Sleep(1000 * 60 * 5);
                        this.User = "";
                        goto Start;
                    }
                }
            }
            catch (Exception ex)
            {
                yq.LogHelper.Error(ex );
                //throw;
            }
            
            
            //if (!isonline &&!this.Login().Contains("成功"))
            //{
            //    if (trytimes < 3)
            //    {
            //        trytimes++;
            //        goto Start;
            //    }
            //    else
            //    {
            //        this.User = string.Empty;
            //        trytimes = 0;
            //        goto  Start;

            //    }
            //}
            //getInfo();
            //pointNum =pointNum == -1 ? 200 : pointNum;
            //if (JfCount >=pointNum )
            //{
            //    File.AppendAllText(Environment.CurrentDirectory + @"\usersFinish.txt", string.Format("{0}----{1}----{2}\r\n", this.User, this.Pass,JfCount .ToString ()));
            //    return;
            //}
            //if (DowCount < pointNum)
            //{
            //    getdownlist();

            //    DownloadFree();
            //}
            //Command();
        }


        #endregion 自动化程序





        #region 获取上传资源
        private void getUploadRs()
        {
            string[] status = { "操作", "成功/失败", "无验证码", "状态", "无附加信息" };
            status[0] = "获取上传资源";
            status[2] = "";
            status[3] = "";
            status[4] = "";
            // Logscomsole(status);

            HttpHelpers helper_up = new HttpHelpers();
            HttpItems items_up = new HttpItems();
            HttpResults hr_up = new HttpResults();

            items_up = new HttpItems();
            items_up.URL = "http://download.csdn.net/my";
            items_up.Container = cc;
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

            status[0] = "获取上传资源";
            status[1] = "结束";
            status[2] = "共 "+upCount .ToString ()+" 个资源";
            status[3] = "";
            Logscomsole(status);
        }


        #endregion 获取上传资源


        #region 注册

        public void Reger()
        {
            new Thread(new ThreadStart(reg)).Start();
        }


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

            item = new HttpItems();
            //https://10minutemail.net/
            item.URL = @"https://10minutemail.net/";
            item.Container = ccc ;

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
        CHECKEMAIL:
            hr = heler.GetHtml(item);
            string emalurl = hr.Html.Replace("\r\n", "").Replace("\t", "").Replace("\n", "");
            if ("errorerror".Equals(emalurl))
            {
                return "失败：errorerror";
            }
            //?box=$boxid&show=a134fc9d-5412-4cda-ac7e-04b48103f78f\">[CSDN

            regex = new Regex("\\&show=(.*?)\\\"\\>\\[CSDN");
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
                string []strtmp=mc[0].Groups[0].Value.Split(new char[] { '\"','\\' });
                string urltmp = string.Format("http://yourinbox.mailcatch.com/en/temporary-inbox?box={0}=mailcatch.com{1}", emalid, strtmp[0]);// +"lang=zh-cn";
                //Console.WriteLine(urltmp );
                item = new HttpItems();
                item.URL = urltmp;
                item.Container = ccc;
                item.Allowautoredirect = true;
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




        private string[] regForIn()
        {
        Start:
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

            items_reg = new HttpItems();
            //https://passport.csdn.net/ajax/verifyhandler.ashx//验证码
            items_reg.URL = @"https://passport.csdn.net/ajax/verifyhandler.ashx";
            items_reg.ResultType = ResultType.Byte;
            items_reg.Container = cc_reg;
            hr_reg = heler_reg.GetHtml(items_reg);
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
            items_reg = new HttpItems();
            items_reg.URL = "http://passport.csdn.net/account/register?action=validateCode&validateCode="+regCode ;
            items_reg.Container = cc_reg;
            hr_reg = heler_reg.GetHtml(items_reg);
           if(hr_reg .Html.ToLower () !="true")
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
         GetMail:

            email = getRegEmail2(cc_em );
           //检查验证码
           //http://passport.csdn.net/account/register?action=validateCode&validateCode=
           items_reg = new HttpItems();
           items_reg.URL = "http://passport.csdn.net/account/register?action=validateEmail&email="+email ;
           items_reg.Container = cc_reg;
           hr_reg = heler_reg.GetHtml(items_reg);
           if (hr_reg.Html.ToLower() != "true")
           {
               trytimes++;
               if (trytimes< 5)
               {
                   status[0] = "注册帐号";
                   status[1] = "失败";
                   status[2] = "邮箱验证失败";
                   status[3] = hr_reg.Html;
                   status[4] = "";
                   Logscomsole(status);
                   goto GetMail;
               }
               else
               {
                   status[0] = "注册帐号";
                   status[1] = "失败";
                   status[2] = "邮箱验证失败次数过多";
                   status[3] = hr_reg.Html;
                   status[4] = "";
                   Logscomsole(status);
                   return new string[] { };
               }
           }
           trytimes = 0;
        GetregName:
            name=GetName();
           //检查名字
           //http://passport.csdn.net/account/register?action=validateCode&validateCode=
           items_reg = new HttpItems();
           items_reg.URL = "http://passport.csdn.net/account/register?action=validateUsername&username=" + name ;
           items_reg.Container = cc_reg;
           hr_reg = heler_reg.GetHtml(items_reg);
           if (hr_reg.Html.ToLower() != "true")
           {
      
               trytimes++;
               if (trytimes < 5)
               {
                   status[0] = "注册帐号";
                   status[1] = "失败";
                   status[2] = "用户名验证失败";
                   status[3] = hr_reg.Html;
                   status[4] = "";
                   Logscomsole(status);
                   goto GetregName;
               }
               else
               {
                   status[0] = "注册帐号";
                   status[1] = "失败";
                   status[2] = "用户名验证失败次数过多";
                   status[3] = hr_reg.Html;
                   status[4] = "";
                   Logscomsole(status);
                   return new string[] { };
               }
           }
                //string email = string.Format("{0}@qq.com", name);
                string pwd = string .IsNullOrEmpty (mima )?"aa13655312932bb":mima ;

                status[0] = "注册帐号";
                status[1] = "进行中";
                status[2] = "验证成功";
                status[3] = "用户名："+name ;
                status[4] = "邮箱：" + email;
                Logscomsole(status);
           PostRequest:

                #region 提交注册请求
                items_reg = new HttpItems();
                string regurl = "https://passport.csdn.net/account/register?action=saveUser&isFrom=False";
                string postdata = string.Format("fromUrl={0}&userName={1}&email={2}&password={3}&confirmpassword={4}&validateCode={5}&agree=on",
                    string.Empty, name, email, pwd, pwd, regCode);
                items_reg.Container = cc_reg;
                items_reg.URL = regurl;
                items_reg.Postdata = postdata;
                items_reg.Method = "POST";
                hr_reg = heler_reg.GetHtml(items_reg);

                string html = hr_reg.Html.Replace("\r\n", "").Replace("\t", "").Replace("\n", "");

                if (html.Contains("邮件已发送到邮箱"))
                {
                    status[0] = "注册帐号";
                    status[1] = "进行中";
                    status[2] = "激活邮件已发送";
                    status[3] = "等待邮件到达";
                    status[4] = "";
                    Logscomsole(status);
                    string str="";
                    if ((str = activeRegEmai2(email ,cc_em )).Contains("成功"))
                    {
                        status[0] = "注册帐号";
                        status[1] = "成功";
                        status[2] = "帐号：" + name;
                        status[3] = "密码：" + pwd;
                        status[4] = "";
                        Logscomsole(status);
                        return new string[] { name ,pwd };
                    }
                    else
                    {
                        status[0] = "注册帐号";
                        status[1] = "失败";
                        status[2] = "激活邮箱问题" ;
                        status[3] = str;
                        status[4] = "";
                        Logscomsole(status);
                        if (str.Contains("errorerror"))
                        {
                            return regForIn();
                        }
                        return new string[] { str  };
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

        private void  reg( )
        {
            if (regeristResult != null)
            {
                regeristResult(regForIn ());
            }
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



        double DowCount;
        private bool getInfo()
        {
            items_com = new HttpItems();
            items_com.URL = "http://download.csdn.net/my/downloads";
            items_com.Container = cc;
            hr_com = helper_com.GetHtml(items_com);
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
            string[] status = { "操作", "成功/失败", "状态信息", "无验证码", "无附加信息" };
            status[0] = "批量模拟下载";
            status[1] = "开始";
            status[2] = "";
            status[3] = "";
            status[4] = "";

            if (ListPreTDown == null || ListPreTDown.Count == 0)
            {
                //Logscomsole("获取免费资源", "开始", "随机扫描数据");
               // getdownlist();
                Logscomsole(getdownlist ());

            }
            status[0] = "自动模拟下载";
            status[1] = "开始";
            status[2] = "开始执行";
            status[3] = "";
            status[4] = "";
            Logscomsole(status );
            foreach (CsdnResouce item in listPreTDown)
            {
                item.Method = "模拟下载";
                Logscomsole(downloadOne(item));
                //Logscomsole("模拟下载暂停", "延时等待", (timeForDown / 1000).ToString() + "秒");
                Thread.Sleep(TimeForDown);
            }
            status[0] = "自动模拟下载";
            status[1] = "结束";
            status[2] = "执行完毕";
            status[3] = "";
            status[4] = "";
            Logscomsole(status);
            //Logscomsole("自动模拟下载", "结束", "执行完毕");
            listPreTDown = new List<CsdnResouce>();
            if (conload)
            {
                thread_down = new Thread(new ThreadStart(download));
                thread_down.Start();
            }
            
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

            string url = ("http://download.csdn.net/");

            items_down = new HttpItems();
            items_down.URL = url;
            string result = helper_down.GetHtml(items_down).Html;
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
            url = string.Format("http://download.csdn.net{0}", strList[a]);

            items_down = new HttpItems();
            items_down.URL = url;
            result = helper_down.GetHtml(items_down).Html;
            result = result.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");
            reg = new Regex(szReg_res_url_pagecount);
            Match mat = reg.Match(result);
            string durl = reg.Match(result).Groups[0].Value;
            string[] ss = durl.Split(new char[] { '/', '\"' });
            int pageCount;
            int.TryParse(ss[6], out pageCount);

            ListPreTDown = new List<CsdnResouce>();

            for (int j = 0; j < pageCount; j++)
            {
                url = String.Format("http://download.csdn.net{0}/{1}", strList[a], j + 1);
                items_down = new HttpItems();
                items_down.URL = url;
                result = helper_down.GetHtml(items_down).Html;
                result = result.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");
                reg = new Regex(szReg_res_down_url);
                mc = reg.Matches(result);
                // List<string> downstr = new List<string>();
                foreach (Match m in mc)
                {
                    string[] tmp1 = m.Value.Split(new char[] { '\"', '>', '<' });
                    if (tmp1[12] == "0")
                    {
                        CsdnResouce cdsr = new CsdnResouce(tmp1[6], tmp1[4]);
                        cdsr.Tag = String.Format("http://download.csdn.net{0}", cdsr.Url);
                        ListPreTDown.Add(cdsr);
                        if (pointNum != -1 && ListPreTDown.Count + DowCount >= pointNum)
                        {
                            conload = false;
                            goto End;
                        }
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

            items_down = new HttpItems();
            string url = string.Empty;
            url = cdrs.Tag.ToString().Replace("detail", "download");
            items_down.URL = url;
            items_down.Container = cc;
            string res = helper_down.GetHtml(items_down).Html;
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
                items_down = new HttpItems();
                items_down.URL = durl;
                items_down.Referer = url;
                items_down.Method = "Post";
                items_down.Container = cc;
                items_down.Allowautoredirect = true;
                items_down.Postdata = "ds=&validate_code=&basic%5Breal_name%5D=&basic%5Bmobile%5D=&basic%5Bemail%5D=&basic%5Bjob%5D=&basic%5Bcompany%5D=&basic%5Bprovince%5D=&basic%5Bcity%5D=&basic%5Bindustry%5D=";
                hr_down = helper_down.GetHtml(items_down);
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
                        url = res.Substring(res.IndexOf("http"), res.LastIndexOf('\'') - res.IndexOf("http"));
                        items_down = new HttpItems();
                        items_down.URL = url;
                        items_down.Referer = durl;
                        items_down.Container = cc;
                        items_down.ResultType = ResultType.Byte;
                        items_down.Allowautoredirect = true;
                        try
                        {
                            hr_down = helper_down.GetHtml(items_down);
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
                items_down = new HttpItems();
                items_down.URL = @"http://download.csdn.net/index.php/rest/tools/validcode/source_ip_validate/10.1345321661792" + new Random().Next().ToString();
                items_down.Referer = referurl;
                items_down.Container = this.cc;
                items_down.ResultType = ResultType.Byte;
                HttpResults hr = helper_down.GetHtml(items_down);
                imgbytes = hr.ResultByte;
                code = getImgVcode(imgbytes);
                if (code == "")
                {
                    continue;
                }
                items_down = new HttpItems();
                items_down.URL = url;
                items_down.Referer = referurl;
                items_down.Method = "Post";
                items_down.Container = cc;
                items_down.Allowautoredirect = true;
                items_down.Postdata = "ds=&validate_code=" + code + "&basic%5Breal_name%5D=&basic%5Bmobile%5D=&basic%5Bemail%5D=&basic%5Bjob%5D=&basic%5Bcompany%5D=&basic%5Bprovince%5D=&basic%5Bcity%5D=&basic%5Bindustry%5D=";
                res = helper_down.GetHtml(items_down).Html;
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
            string[] status = { "批量评分", "成功/失败", "状态信息", "无验证码", "无附加信息" };

            //string[] msg = new string[4];
            if (listConSource == null || listConSource.Count == 0)
            {
                status[0] = "获取评分列表";
                status[1] = "开始";
                status[2] = "开始获取";
                status[3] = "耐心等待";
                status[4] = "默认5线程/账户 进行操作";
                Logscomsole(status);
                listConSource = new List<CsdnResouce>();

                Logscomsole(getCommandList());
                if (listConSource.Count == 0)
                {
                    status[0] = "批量评分";
                    status[1] = "失败";
                    status[2] = "已无可评分资源";
                    status[3] = "";
                    status[4] = "";
                    Logscomsole(status);
                    getInfo();
                    if (pointNum !=-1 &&JfCount >= pointNum)
                    {
                        auto();
                        return;

                    }
                    else if(isInAuto )
                    {
                        status[0] = "批量评分";
                        status[1] = "失败";
                        status[2] = "已无可评分资源";
                        status[3] = "任务未完成状态";
                        status[4] = "5分钟后自动重试";
                        Logscomsole(status);
                        Thread.Sleep(1000*60*5);
                        goto StartPoint;
                    }
                    return;
                }
            }
            status[0] = "批量评分";
            status[1] = "开始";
            status[2] = "开始执行";
            status[3] = "";
            status[4] = "";
            //status = { "批量评分", "开始", "开始执行", "无验证码", "无附加信息" };
            Logscomsole(status);

            foreach (CsdnResouce item in listConSource)
            {
                string[] msg = commandOne(item);
                Logscomsole(msg);
    
                if (msg[4].Contains("间隔60秒"))
                {
                    Thread.Sleep(TimeForCom);
                    msg = commandOne(item);
                   // Console.WriteLine("Com:\t" + item.Name + ":\t" + msg[4]);
                }
                if ("成功".Equals(msg[1]))
                {
                    Thread.Sleep(TimeForCom);
                }
            }
            status[1] = "结束";
            status[2] = "执行完毕";
            status[3] = "";
            status[4] = "";
            Logscomsole(status);

            listConSource = new List<CsdnResouce>();
            thread_com = new Thread(new ThreadStart(command));
            thread_com.Start();
        }
        public string Decode(string s)
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

            items_com = new HttpItems();
            items_com.URL = "http://download.csdn.net/index.php/comment/get_comment_data/" + item.Id + @"/1?jsonpcallback=jsonp1448000002880&&t=1448000436379";
            //http://download.csdn.net/index.php/comment/get_comment_data/5342735/1?jsonpcallback=jsonp1448000002880&&t=1448000436379
            items_com.Container = cc;
            items_com .Referer =item.Url ;
            hr_com = helper_com.GetHtml(items_com );

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

            //rehtml =mccont[0].Groups[2].Value;

            //getCommandList();
            long epochS = (DateTime.Now.AddSeconds(-5).ToUniversalTime().Ticks - 621355968000000000) / 10000;
            long epochE = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
            //Get评论
            items_com = new HttpItems();

            int index = ran.Next(comMsg.Length);
            string commentcon=comMsg [index ]+"，"+rehtml ;
            if ("".Equals(code))
            {
                items_com.URL = string.Format("http://download.csdn.net/index.php/comment/post_comment?jsonpcallback=jsonp{0}&sourceid={1}&content={2}&rating={3}&t={4}", epochS, item.Id, new XJHTTP().UrlEncoding(item.Name + commentcon ), new Random().Next(3, 6), epochE);
            }
            else
            {
                items_com.URL = string.Format("http://download.csdn.net/index.php/comment/post_comment?jsonpcallback=jsonp{0}&sourceid={1}&content={2}&txt_validcode={3}&rating={4}&t={5}", epochS, item.Id, new XJHTTP().UrlEncoding(item.Name + commentcon), code, new Random().Next(3, 6), epochE);
            }

            items_com.Container = cc;
            string html = helper_com.GetHtml(items_com).Html;

            //评论成功完成
            if (html.Contains("\"succ\":1"))
            {
                JfCount++;
                status[1] = "成功";
                status[3] = "当前积分：" + JfCount.ToString();
                status[4] = item.Name + comMsg[index];
                //item.Rel = "成功";
                //item.Msg = item.Name + comMsg[index] +(code == "" ? "" : "\t验证码：" + code);
                //item.Log = "当前积分：" + JfCount.ToString();
                //Logscomsole("", "", "", item);
                //return "1";
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

            int tryTime = 0;
            string code = "";
            string res = "";
            byte [] imgbyte=null ;
            while (tryTime < 5)
            {
                tryTime++;
                items_com = new HttpItems();
                items_com.URL = @"http://download.csdn.net/index.php/rest/tools/validcode/comment_validate/10.1749821768607" + new Random().Next().ToString();
                items_com.Referer = referurl;
                items_com.Container = this.cc;
                items_com.ResultType = ResultType.Byte;
                hr_com = helper_com.GetHtml(items_com);
                imgbyte =hr_com.ResultByte;
                code = getImgVcode(imgbyte );
                if (code == "")
                {
                    continue;
                }
                items_com = new HttpItems();
                items_com.URL = @"http://download.csdn.net/index.php/comment/check_validcode/" + code;
                items_com.Referer = referurl;
                items_com.Container = cc;
                //items.Allowautoredirect = true;
                //items.Postdata = "ds=&validate_code=" + code + "&basic%5Breal_name%5D=&basic%5Bmobile%5D=&basic%5Bemail%5D=&basic%5Bjob%5D=&basic%5Bcompany%5D=&basic%5Bprovince%5D=&basic%5Bcity%5D=&basic%5Bindustry%5D=";
                res = helper_com.GetHtml(items_com).Html;
                if (res != "验证码错误")
                {
                    return code;
                }
                //File.WriteAllBytes(System.Environment.CurrentDirectory + @"\Codes\" + imgbyte.Length.ToString() + "_" + code + ".bmp", imgbyte);
            }
            status[0] = "识别评分验证码";
            status[1] = "失败";
            status[2] = "次数:"+tryTime .ToString ();
            status[3] = "";
            status[4] = "";
            Logscomsole(status);
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

            //int index = 1;
            //我的下载资源页的资源列表  
            items_com = new HttpItems();
            items_com.URL = "http://download.csdn.net/my/downloads";
            items_com.Container = cc;
            hr_com = helper_com.GetHtml(items_com);
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

            //Logscomsole("获取下载数", "成功", "当前下载为 " + DowCount.ToString() + " 次");
            //获取剩余积分
            Regex regJf = new Regex("<em>积分：(.+?)</em>");
            MatchCollection mcJf = regJf.Matches(result);
            if (mcJf.Count > 0)
            {
                int.TryParse(mcJf[0].Groups[1].Value, out jfCount);
                //tslMyCount.Text = mcJf[0].Groups[1].Value;
            }

            status[3] = "积分数："+jfCount .ToString ();
           // Logscomsole("获取积分数", "成功", "当前积分为 " + jfCount.ToString() + " 分");
  
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
                    items_com = new HttpItems();
                    items_com.URL = "http://download.csdn.net/my/downloads/" + i;
                    items_com.Container = cc;
                    hr_com = helper_com.GetHtml(items_com);
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
           // Console.WriteLine("线程chuli：" + s.ToString());
            HttpHelpers tmpHelpers = new HttpHelpers();
            HttpItems tmpItems = new HttpItems();
            HttpResults tmpHr = new HttpResults();
            List<CsdnResouce> tmplist = new List<CsdnResouce>();


            tmpItems.URL = "http://download.csdn.net/my/downloads/" + s;
            tmpItems.Container = cc;
            tmpHr = tmpHelpers.GetHtml(tmpItems);
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
                    //commandOne(cdsr );
                    
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
            //if (cdsr == null)
            //{
            //    log = string.Format("{4}\t用户:{0}\t{1}\t{2}\t{3}", NickName == "" ? "UNKNOWN" : NickName, method, res, msg, DateTime.Now.ToString("hh:mm:ss.ffff"));
            //}
            //else
            //{
            //    log = string.Format("{4}\t用户:{0}\t{1}\t{2}\t{3}\t{5}", NickName == "" ? "UNKNOWN" : NickName, cdsr.Method, cdsr.Rel, cdsr.Msg, DateTime.Now.ToString("hh:mm:ss.ffff"), cdsr.Log);
            //}
            if(log !="" && showLogs !=null )
            showLogs (log );
        }

        public CsdnHelper(string user,string pass)
        {
            cc = new CookieContainer();
            helper_down = new HttpHelpers();
            helper_com = new HttpHelpers();
            this.user = user;
            this.pass = pass;
            this.SavePath = System.Environment.CurrentDirectory+@"\"+this.user ;
        }
    }


    public class methodMsg
    {
        private string method;

        public string Method
        {
            get { return method; }
            set { method = value; }
        }
        private string result;

        public string Result
        {
            get { return result; }
            set { result = value; }
        }
        private string status;

        public string Status
        {
            get { return status; }
            set { status = value; }
        }
        private string vcode;

        public string Vcode
        {
            get { return vcode; }
            set { vcode = value; }
        }
        private string logs;

        public string Logs
        {
            get { return logs; }
            set { logs = value; }
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
