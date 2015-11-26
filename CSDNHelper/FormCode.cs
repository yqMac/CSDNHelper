using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace xCsdn
{
    public partial class FormSetting : Form
    {
        public CsdnHelper cdhp = null;

        public FormSetting(CsdnHelper cdhp)
        {
            InitializeComponent();
            this.cdhp = cdhp;
        }

        private void FormSetting_Load(object sender, EventArgs e)
        {
            this.txt_USER.Text = cdhp.User;
            this.txt_pass.Text = cdhp.Pass;
            this.txt_comSleep .Text = cdhp.TimeForCom.ToString();
            this.txt_downSleep.Text  = cdhp.TimeForDown.ToString();
            this.txt_savePath .Text = cdhp.SavePath;
            this.checkbox_savefile.Checked = cdhp.SaveFile;
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            cdhp.User= this.txt_USER.Text;
            cdhp.Pass= this.txt_pass.Text;
            cdhp.TimeForCom = int.Parse(this.txt_comSleep .Text );
            cdhp.TimeForDown = int.Parse(this.txt_downSleep.Text);

           cdhp.SavePath=  this.txt_savePath.Text;

            cdhp.SaveFile=this.checkbox_savefile.Checked ;
            this.Close();
        }

        private void btn_Cancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_brw_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txt_savePath.Text = folderBrowserDialog1.SelectedPath;
            }
        }

    
    }
}
