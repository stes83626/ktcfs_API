
using KTCFS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static winPlatFind.clsSetting;

namespace winPlatFind
{
    public partial class Mainfrm : Form
    {
        public Mainfrm()
        {
            InitializeComponent();

        }

        KTCFSAPI ktcfs = null;
        AppSettings parm;

        private void btn_Key_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Button btn = (System.Windows.Forms.Button)sender;
            string k = btn.Text;
            if (txtPlatNo.Text.Length < 7)
            {
                txtPlatNo.Text += k;

            }
        }

        private void btn_Del_Click(object sender, EventArgs e)
        {
            txtPlatNo.Text = "";
        }

        private void btn_Find_Click(object sender, EventArgs e)
        {
    

            //panel1.Visible = true;
            try
            {

            string plateNo = txtPlatNo.Text;
            KTCFSAPI.CarLocInfoSend send = new KTCFSAPI.CarLocInfoSend();
            KTCFSAPI.CarLocInfoRec ret = new KTCFSAPI.CarLocInfoRec();


            send.data.plateNo = plateNo;

            string responseContent = ktcfs.getCarLocInfo(send, ref ret);
            if (ret.content.Count == 0)
            {
                timer1.Enabled = true;
                
                panel1.Visible = true;
                return;
            }

            string originalUrl = ret.content[0].carImage;

            string newUrl = originalUrl.Replace("http://127.0.0.1:8083", $"http://{parm.TableLamp_IP}:{parm.TableLamp_Port}");

            frmFind find = new frmFind();
            find.PlatNo = ret.content[0].plateNo;
            find.spaceNo = ret.content[0].floorName;
            find.imageUrl = newUrl;
            find.inTime = ret.content[0].inTime;
            find.ParkTime = ret.content[0].parkTime;
            find.frm = this;
            find.Show();

            }
            catch (Exception)
            {

 
            }


        }


        private void Mainfrm_Load(object sender, EventArgs e)
        {
            parm = LoadSettings<AppSettings>("Setting.xml");
            ktcfs = new KTCFSAPI(parm.TableLamp_Secert, parm.TableLamp_IP, parm.TableLamp_Port);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            panel1.Visible = false;
            timer1.Enabled = false;
            txtPlatNo.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string licensePlate = txtPlatNo.Text;
            if (licensePlate.Length > 0)
            {
                licensePlate = licensePlate.Substring(0, licensePlate.Length - 1);
                txtPlatNo.Text = licensePlate;
            }

        }
    }
}
