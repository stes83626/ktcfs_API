using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace winPlatFind
{
    public partial class frmFind : Form
    {
        public frmFind()
        {
            InitializeComponent();
        }

        public Mainfrm frm = null;
        public string PlatNo = "";
        public string imageUrl = "";
        public string inTime = "";
        public string spaceNo = "";
        public string ParkTime = "";

        private void frmFind_Load(object sender, EventArgs e)
        {
            int a = int.Parse(ParkTime);
            int h = a / 60;
            int m = a % 60;
            string s = string.Format("{0}小時 {1}分鐘 ", h , m);

            lblPlateNo.Text = PlatNo;
            lblParkLoc.Text = spaceNo;
            lblEnter.Text = inTime;
            lblDuration.Text = s;
            pictureBox1.Load(imageUrl);
            timer1.Enabled = true;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            frm.txtPlatNo.Text = "";

            timer1.Enabled = false;
            this.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            frm.txtPlatNo.Text = "";
            timer1.Enabled = false;

            this.Close();

        }
    }
}
