using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using 富軥888_API;
using static LED_Parking_Count.Form1;
using static LED_Parking_Count.Form1.getSpaceInfoRec;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LED_Parking_Count
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private static string GetMd5(string input)
        {//MD5加密
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }
            return sb.ToString();
        }

        public class getSpaceInfoSend
        {
            public string cmd { get; set; }
            public string ts { get; set; }
            public string sign { get; set; }
            public class dataInf
            {
                public int areaId { get; set; }
                public int floorId { get; set; }
            }
            public dataInf data { get; set; } = new dataInf();

        }

        public class getSpaceInfoRec
        {
            public string success { get; set; }
            public string msg { get; set; }
            public string code { get; set; }

            public class SpaceInfo
            {
                public string spaceNo { get; set; }
                public int status { get; set; }

            }
            public List<SpaceInfo> content { get; set; }

        }


        public class ParkingSpace
        {
            public string SpaceNo { get; set; }
            public int Status { get; set; }
        }


        public async Task getSpaceInfoAsync(string IP, string Port, List<ParkingSpace> parkingSpaces)
        {
            DateTimeOffset utcTime = DateTimeOffset.UtcNow;
            long timestamp = utcTime.ToUnixTimeMilliseconds();

            getSpaceInfoSend send = new getSpaceInfoSend();
            send.cmd = "getSpaceInfo";
            send.ts = timestamp.ToString();
            send.data.areaId = 0;
            send.data.floorId = 0;

            string sign = send.cmd + send.ts + Parm.TableLamp_Secert;
            send.sign = GetMd5(sign);

            var JsonPost = JsonConvert.SerializeObject(send);
            
            string strURL = string.Format("http://{0}:{1}/v1/open/getSpaceInfo", IP, Port);

            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(JsonPost, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(strURL, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    getSpaceInfoRec rc = JsonConvert.DeserializeObject<getSpaceInfoRec>(responseContent);

                    if (rc.content == null)
                    {
                        return;
                    }

                    foreach (SpaceInfo data in rc.content)
                    {
                        parkingSpaces.Add(new ParkingSpace() { SpaceNo = data.spaceNo, Status = data.status });
                    }
                }
                else
                {
                    // Handle error
                }
            }
        }

        List<string> HandIcap = new List<string>();
        List<string> Mch = new List<string>();
        List<string> Electric = new List<string>();

        clsSetting.AppSettings Parm = clsSetting.LoadSettings("Setting.xml");
        private void Form1_Load(object sender, EventArgs e)
        {
            int x = Parm.Parking.Count;

            for (int i = 0; i < Parm.Parking.Count; i++)
            {
                //comboBox1.Items.Add(Parm.Parking[i].floorName);

                HandIcap.AddRange(Parm.Parking[i].Handicap.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
                Mch.AddRange(Parm.Parking[i].Mch.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
                Electric.AddRange(Parm.Parking[i].Electric.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));

            }

            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 1000; // 設定每秒讀取一次資料
            timer1.Start();    

        }

        private bool MySql_Open(MySqlConnection con, string strConnection)
        {
            try
            {
                con.ConnectionString = strConnection;
                con.Open();
            }
            catch
            {
                //ShowInfo("資料連線失敗 ");
                return false;
            }

            return con.State == ConnectionState.Open;
        }
        private void MySql_Close(MySqlConnection con)
        {
            con.Close();
        }

        public int LoadMySQL(){

            int carTotal = 0;
            string strConnection = "server=" + Parm.db.url + ";port=" + Parm.db.port + ";uid=" + Parm.db.user + ";password=" + Parm.db.pass + ";database=" + Parm.db.database;

            MySqlConnection con = new MySqlConnection();
            if (MySql_Open(con, strConnection))
            {
                string query = "SELECT * FROM space";
                MySqlCommand command = new MySqlCommand(query, con);

                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    carTotal = reader.GetInt32("AVAILABLE");
                }

                MySql_Close(con);
            }


            return carTotal;
        }


        int _HandicapTotal = 0;
        int _MchTotal = 0;
        int _ElectricTotal = 0;
        int _CarsTotal = 0;

        bool isFirstTime = true;
        private async void timer1_Tick(object sender, EventArgs e)
        {




            try
            {
                int HandicapTotal = 0;
                int MchTotal = 0;
                int ElectricTotal = 0;
                int CarsTotal = 0;

                List<ParkingSpace> parkingSpaces = new List<ParkingSpace>();
                await getSpaceInfoAsync(Parm.TableLamp_IP, Parm.TableLamp_Port, parkingSpaces);

                CarsTotal = LoadMySQL();

                bool isHandIcapChecked = checkHandicap.Checked; // 判斷 HandIcap 是否被勾選
                bool isMchChecked = checkMch.Checked; // 判斷 Mch 是否被勾選
                bool isElectricChecked = checkElectric.Checked; // 判斷 Electric 是否被勾選



                //label5.Text = CarsTotal.ToString().PadLeft(4,'0');

                // 使用
                // 
                foreach (var item in parkingSpaces)
                {
                    string car = item.SpaceNo;
                    int s = item.Status;

                    if (HandIcap.Contains(car))
                    {
                        // car 在 HandIcap 陣列中
                        // 取得 s 的值
                        if (s == 0)
                        {
                            HandicapTotal += 1;
                            label6.Text = HandicapTotal.ToString().PadLeft(4, '0');
                        }
                    }
                    if (Mch.Contains(car))
                    {
                        // car 在 HandIcap 陣列中
                        // 取得 s 的值
                        if (s == 0)
                        {
                            MchTotal += 1;
                            label7.Text = MchTotal.ToString().PadLeft(4, '0');
                        }

                    }
                    if (Electric.Contains(car))
                    {
                        // car 在 HandIcap 陣列中
                        // 取得 s 的值
                        if (s == 0)
                        {
                            ElectricTotal += 1;
                            label8.Text = ElectricTotal.ToString().PadLeft(4, '0');
                        }

                    }

                }



                if (isHandIcapChecked)
                {
                    CarsTotal -= HandicapTotal; // 如果 HandIcap 被勾選，則從 CarsTotal 中減去 HandicapTotal
                }

                // 將 CarsTotal 顯示在標籤上
                if (isMchChecked)
                {
                    CarsTotal -= MchTotal; // 如果 Mch 被勾選，則從 CarsTotal 中減去 MchTotal
                }

                if (isElectricChecked)
                {
                    CarsTotal -= ElectricTotal; //如果 Electric 被勾選，則從 CarsTotal 中減去 ElectricTotal
                }


                label5.Text = CarsTotal.ToString().PadLeft(4, '0');






                LED led = new LED(Parm.led.COM); //設定COM埠

                LED.LedColor color = (LED.LedColor)(1);




                if (_CarsTotal != CarsTotal || isFirstTime)
                {
                    _CarsTotal = CarsTotal;
                    color = (LED.LedColor)(Parm.led.Normal_Color);
                    led.GettextBox(CarsTotal.ToString().PadLeft(4, '0'), color, Parm.led.FontSize, Parm.led.Normal_ID);

                }

                if (_HandicapTotal != HandicapTotal || isFirstTime)
                {
                    _HandicapTotal = HandicapTotal;
                    color = (LED.LedColor)(Parm.led.Handicap_Color);
                    led.GettextBox(HandicapTotal.ToString().PadLeft(4, '0'), color, Parm.led.FontSize, Parm.led.Handicap_ID);

                }

                if (_MchTotal != MchTotal || isFirstTime)
                {
                    _MchTotal = MchTotal;
                    color = (LED.LedColor)(Parm.led.Mch_Color);
                    led.GettextBox(MchTotal.ToString().PadLeft(4, '0'), color, Parm.led.FontSize, Parm.led.Mch_ID);

                }

                if (_ElectricTotal != ElectricTotal || isFirstTime)
                {
                    _ElectricTotal = ElectricTotal;
                    color = (LED.LedColor)(Parm.led.Electric_Color);
                    led.GettextBox(ElectricTotal.ToString().PadLeft(4, '0'), color, Parm.led.FontSize, Parm.led.Electric_ID);

                }

                if (isFirstTime) { isFirstTime = false; }


            }
            catch (Exception)
            {


            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LED led = new LED(Parm.led.COM);
            LED.LedColor color = (LED.LedColor)(1);

            color = (LED.LedColor)(Parm.led.weekday_Color);
            led.GettextBox(txt_weekday.Text, color, Parm.led.FontSize, Parm.led.weekday_ID);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LED led = new LED(Parm.led.COM);
            LED.LedColor color = (LED.LedColor)(1);

            color = (LED.LedColor)(Parm.led.holiday_Color);
            led.GettextBox(txt_holiday.Text, color, Parm.led.FontSize, Parm.led.holiday_ID);
        }



    }
}
