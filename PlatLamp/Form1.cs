using KTCFS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static PlatLamp.clsMySQL;
using static PlatLamp.clsSetting;


namespace PlatLamp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        AppSettings parm ;
        KTCFSAPI ktcfs = null;
        string[] Electric, Handicap, Mch;
        clsMySQL db = null;
        private void Form1_Load(object sender, EventArgs e)
        {

            if (!Directory.Exists("Plat"))
                Directory.CreateDirectory("Plat");

            parm = LoadSettings<AppSettings>("Setting.xml");
            ktcfs = new KTCFSAPI(parm.TableLamp_Secert, parm.TableLamp_IP, parm.TableLamp_Port);
            string strValue = parm.Parking.Electric;
            Electric = strValue.Split(',');
            foreach (string s in Electric)
            {
                txtElectric.Text += s + "\r\n";
            }
            //strValue = parm.Parking.Handicap;
            //Handicap = strValue.Split(',');
            //foreach (string s in Handicap)
            //{
            //    txtHandicap.Text += s + "\r\n";
            //}
            //strValue = parm.Parking.Mch;
            //Mch = strValue.Split(',');
            //foreach (string s in Mch)
            //{
            //    txtMch.Text += s + "\r\n";
            //}
            db = new clsMySQL(parm.db.database, parm.db.user, parm.db.pass, parm.db.port, parm.db.url);



            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 10000; // 設定每十秒讀取一次資料
            timer1.Start();



        }



        public class PlatDetail
        {
            public string spaceType { get; set; } = "";
            public string spaceNo { get; set; } = "";
            public string plateNo { get; set; } = "";
            public string InPlat { get; set; } = "";
            public string OutPlat { get; set; } = "";
            public string Change { get; set; } = "";
            public string inparking { get; set; } = "";
            public string ImgUrl { get; set; } = "";

        }
        List<PlatDetail> curDetail = new List<PlatDetail>();
        List<PlatDetail> historyDetail = new List<PlatDetail>();


        private KTCFSAPI.getSpaceDetailRec funGetSpaceDetail()
        {
            curDetail.Clear();
            KTCFSAPI.getSpaceDetailSend send = new KTCFSAPI.getSpaceDetailSend();
            KTCFSAPI.getSpaceDetailRec  rec = new KTCFSAPI.getSpaceDetailRec();

            send.data.areaId = -1;
            send.data.floorId = 0;

            string responseContent = ktcfs.getSpaceDetail(send, ref rec);
            foreach (KTCFSAPI.getSpaceDetailRec.SpaceInfo data in rec.content)
            {
                if (data.status == 1 && data.plateNo != "") 
                {
                    PlatDetail d = new PlatDetail();
                    d.spaceType = funGetSpaceType(data.spaceNo);
                    //if (d.spaceType != "一般")
                    //if(true)
                    if (d.spaceType == "電動車")
                    {
                        d.spaceNo = data.spaceNo;
                        d.plateNo = data.plateNo;
                        d.InPlat = funCarLocInfo(d.plateNo);
                        d.ImgUrl = getIMGurl(d.plateNo);
                        curDetail.Add(d);
                    }
                }
            }
            return rec;
        }

        private string funGetSpaceType(string spaceNo)
        {
            foreach ( string s in Electric)
            {
                if (s == spaceNo)
                    return "電動車";
            }
            //foreach (string s in Handicap)
            //{
            //    if (s == spaceNo)
            //        return "身障";
            //}
            //foreach (string s in Mch)
            //{
            //    if (s == spaceNo)
            //        return "婦幼";
            //}
            return "一般";
        }
        private string funCarLocInfo(string plateNo)
        {
            KTCFSAPI.CarLocInfoSend send = new KTCFSAPI.CarLocInfoSend();
            KTCFSAPI.CarLocInfoRec ret = new KTCFSAPI.CarLocInfoRec();
            send.data.plateNo = plateNo;

            string responseContent = ktcfs.getCarLocInfo(send, ref ret);
            return ret.content[0].inTime;
        }
        private void CurrentPlat()
        {

            foreach (PlatDetail d in curDetail)
            {
                string platNo = d.plateNo;

                //讀取 mySql -> Lpr ,比對車牌 , 進場時間, 繳費時間  確認為有效的 車牌 
                bool isRENT = CheckDbRENT(platNo,d);
                bool timer = exceed(platNo);

                if (isRENT && timer)
                {
                    InsertDbLPR(platNo);
                }

                bool isOK = CheckDbLPR(platNo ,d);

                if (isOK && timer)
                {
                    updateLPR_Other(platNo);

                    bool b = true;
                    List<PlatDetail> li = new List<PlatDetail>();
                    string filePath = $"Plat\\{platNo}.xml";
                    string EXDateTime = db.getPLR(platNo).EX_Datetime;//取出可離場時間
                    DateTime originalDateTime = DateTime.Now;
                    if (!string.IsNullOrEmpty(EXDateTime))
                    {
                    originalDateTime = DateTime.ParseExact(EXDateTime, "yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture);//轉換成DateTime
                    }
                    //檔案是否存在
                    if (File.Exists(filePath))
                    {
                        li = LoadSettings<List<PlatDetail>>(filePath);//讀取檔案

                        foreach (PlatDetail p in li)
                        {
                            if ((p.inparking).Trim() == (d.inparking).Trim())//進入停車場時間
                            {
                                if (p.OutPlat == "")//離開車格時間
                                {
                                    b = false;
                                    break;
                                }
                                else if (p.InPlat == d.InPlat)//進入車格時間
                                {
                                    p.OutPlat = "";
                                    SaveSettings<List<PlatDetail>>(li, filePath);
                                    b = false;
                                    break;
                                } else if ((p.InPlat).Trim() == (originalDateTime.AddSeconds(10).ToString("yyyy-MM-dd HH:mm:ss")).Trim())//可離場時間+10秒
                                {
                                    b = false;
                                    break;
                                }
                            }
                            else//進入停車場時間不一樣
                            {
                                PlatXML plat = new PlatXML("Plat");
                                plat.FinalMove(platNo);//檔案移到歷史紀錄
                                b = false;
                                break;
                            }
                        }
                        if (b)//進入停車場時間一樣，有離開過車格，又進來車格一次
                        {
                            li.Add(d);
                            SaveSettings<List<PlatDetail>>(li, filePath);
                        }
                    } 
                    //超過可離場時間的話再產生一筆XML，更改OtherFee狀態
                    else if (!string.IsNullOrEmpty(EXDateTime) && DateTime.Parse(EXDateTime) > DateTime.Now) 
                    {   
                        d.InPlat = originalDateTime.AddSeconds(10).ToString("yyyy-MM-dd HH:mm:ss"); //可離場時間加上10秒鐘
                        li.Add(d);
                        SaveSettings<List<PlatDetail>>(li, filePath);


                        if (isRENT)//是否月租
                        {
                            db.updateRent(platNo);//更改rent的OtherFee狀態
                            db.updateLPR(platNo,"2");//更改lpr_log的Other_Fee狀態
                        }
                        else
                        {
                            db.updateLPR(platNo, "1");//更改lpr_log的Other_Fee狀態
                        }

                    }
                    //檔案不存在的話，產生新的XML
                    else
                    {
                        li.Add(d);
                        FileStream fs = File.Create(filePath);
                        fs.Close();

                        SaveSettings<List<PlatDetail>>(li, filePath);
                    }                   
                }
            }
            //第一次執行
            if (isFirst)
            {
                historyDetail.AddRange(curDetail);
                isFirst = false;
            }

        }

        bool isFirst = true;
        private void ChekoutPlat()
        {
            // 將 curDetail 與 historyDetail 的 plateNo 分別存成 HashSet
            HashSet<string> curPlateNos = new HashSet<string>(curDetail.Select(x => x.plateNo));
            HashSet<string> historyPlateNos = new HashSet<string>(historyDetail.Select(x => x.plateNo));
            //curPlateNos.Remove("AZX3111");
            // 找出 historyDetail 中存在，但 curDetail 中不存在的 plateNo
            IEnumerable<string> missingPlateNos = historyPlateNos.Except(curPlateNos);

            foreach (string missingPlateNo in missingPlateNos)
            {
                string filePath = "Plat\\" + missingPlateNo + ".xml";
                if (File.Exists(filePath))
                {
                    List<PlatDetail> li = LoadSettings<List<PlatDetail>>(filePath);
                    foreach (PlatDetail platDetail in li)
                    {
                        if (string.IsNullOrEmpty(platDetail.OutPlat))
                        {
                            platDetail.OutPlat = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            break;
                        }
                    }

                    SaveSettings<List<PlatDetail>>(li, filePath);
                }
            }

            historyDetail.Clear();
            historyDetail.AddRange(curDetail);

        }


        private bool CheckDbLPR(string PlatNo , PlatDetail d)
        {
            //clsMySQL db = new clsMySQL(parm.db.database, parm.db.user, parm.db.pass, parm.db.port, parm.db.url); 
            LPR lpr = db.getPLR(PlatNo);
            if (lpr.PlatNo == "") return false;
            bool Check = CheckRENT(PlatNo);
            if (Check)
            {
                d.inparking = lpr.EN_Datetime;
            }
            DateTime d1 = DateTime.Parse(lpr.EN_Datetime); //閘口 時間
            DateTime d2 = DateTime.Parse(d.InPlat);
            if (d1 > d2) return false;

            if (lpr.EX_Datetime != "")
            {
                d1 = DateTime.Parse(lpr.EX_Datetime); //己繳費 最後可離場時間
                d2 = DateTime.Now;
                if (d1 > d2) return false;
            }
            return true;
        }

        private bool CheckDbRENT(string PlatNo, PlatDetail d)
        {
            //clsMySQL db = new clsMySQL(parm.db.database, parm.db.user, parm.db.pass, parm.db.port, parm.db.url);
            RENT rent = db.getRENT(PlatNo);
            d.inparking = rent.ACPT_IN;


            if (rent.PlatNo == "") return false;

            string date1Str = rent.ENDDATE;
            string date2Str = d.InPlat;

            DateTime d1 = DateTime.ParseExact(rent.ENDDATE + " 00:00", "yyyyMMdd HH:mm", CultureInfo.InvariantCulture);
            DateTime d2 = DateTime.Parse(d.InPlat, CultureInfo.InvariantCulture);

            if (d1 < d2) return false;

            return true;
        }

        private bool CheckRENT(string PlatNo)
        {
           //clsMySQL db = new clsMySQL(parm.db.database, parm.db.user, parm.db.pass, parm.db.port, parm.db.url);
            RENT rent = db.getRENT(PlatNo);
          
            if (rent.PlatNo == "") return true;

            return false;

        }
        private void InsertDbLPR(string plateNo) 
        {
            //clsMySQL db = new clsMySQL(parm.db.database, parm.db.user, parm.db.pass, parm.db.port, parm.db.url);
            string url = getIMGurl(plateNo);
            LPR lpr = db.getPLR(plateNo);
            RENT rent = db.getRENT(plateNo);


            if (lpr.EN_Datetime != rent.ACPT_IN) 
            {
            db.RentToLPR(plateNo, url);
            }

        }

        private void updateLPR_Other(string plateNo)
        {
            //clsMySQL db = new clsMySQL(parm.db.database, parm.db.user, parm.db.pass, parm.db.port, parm.db.url);
            bool Check = CheckRENT(plateNo);
            if (Check)
            {
            db.updateLPR(plateNo,"1");
            }

        }
        private string getIMGurl(string plateNo) 
        {
            string path = "";
            
            KTCFSAPI.CarLocInfoSend send = new KTCFSAPI.CarLocInfoSend();
            KTCFSAPI.CarLocInfoRec ret = new KTCFSAPI.CarLocInfoRec();

            send.data.plateNo = plateNo;

            _ = ktcfs.getCarLocInfo(send, ref ret);
            string originalUrl = ret.content[0].carImage;
            string newUrl = originalUrl.Replace("http://127.0.0.1:8083/", $"http://{parm.TableLamp_IP}:{parm.TableLamp_Port}");

            path = newUrl;

            return path;
        }

        private bool exceed(string plateNo)
        {
            KTCFSAPI.CarLocInfoSend send = new KTCFSAPI.CarLocInfoSend();
            KTCFSAPI.CarLocInfoRec ret = new KTCFSAPI.CarLocInfoRec();

            send.data.plateNo = plateNo;
            string responseContent = ktcfs.getCarLocInfo(send, ref ret);

            int time = int.Parse(ret.content[0].parkTime);
            if (time < parm.inplat) 
            {
                return false;
            }

            return true;
        }


        private void btn_getSpaceDetail_Click(object sender, EventArgs e)
        {
            _ = funGetSpaceDetail();

            string strMsg = "";
            foreach (PlatDetail p in curDetail)
            {
                //p.InPlat = funCarLocInfo(p.plateNo);
                strMsg += string.Format("類別:[{0}] - 車格:[{1}] - 車牌:[{2}] - 進場時間:[{3}] \r\n", p.spaceType, p.spaceNo, p.plateNo, p.InPlat);
            }
            txtMsg.Text = strMsg;
            int x = 0;
        }
         

        private void button1_Click(object sender, EventArgs e)
        {
            //目前還在車格的 
            CurrentPlat();

            //離場的車格
            ChekoutPlat();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            try
            {
                funGetSpaceDetail();
                string strMsg = "";
                foreach (PlatDetail p in curDetail)
                {
                    //p.InPlat = funCarLocInfo(p.plateNo);
                    strMsg += string.Format("類別:[{0}] - 車格:[{1}] - 車牌:[{2}] - 進場時間:[{3}] \r\n", p.spaceType, p.spaceNo, p.plateNo, p.InPlat);
                }
                txtMsg.Text = strMsg;

                //目前還在車格的 
                CurrentPlat();

                //離場的車格
                ChekoutPlat();
            }
            catch (Exception)
            {

            }

        }

    }
}
