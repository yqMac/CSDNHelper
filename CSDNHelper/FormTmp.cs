using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace xCsdn
{
    public partial class FormTmp : Form
    {


        byte[] imgbytes = null;
        public FormTmp(byte[] byts)
        {
            InitializeComponent();
            this.imgbytes = byts;
        }

        private Image getimg(byte[] imgs)
        {
            using (MemoryStream ms = new MemoryStream(imgs ))
            {
                Image img = Image.FromStream(ms);
                return img;
            }
        }

        private void FormTmp_Load(object sender, EventArgs e)
        {
            if (this.imgbytes != null)
            {
                pictureBox1.Image = getimg(this.imgbytes );
            }
        }

        public string code = "";

        private void button1_Click(object sender, EventArgs e)
        {
            code = textBox1.Text.Trim();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
