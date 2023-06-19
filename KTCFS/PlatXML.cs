using ErrorMsg;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static KTCFS.clsSetting;

namespace KTCFS
{
    public class PlatXML
    {
        public string Dir = "Plat";
        public PlatXML(string dir = "Plat")
        {
            Dir = dir;
        }
        public int getPlatTotal(string PlatNo)
        {
            int total = 0;
            try
            {
                // 讀取 XML 資料
                List<PlatDetail> li = new List<PlatDetail>();
                string filePath = string.Format("{0}\\{1}.xml", Dir, PlatNo);
                if (File.Exists(filePath))
                {
                    DateTime curTT = DateTime.Now;
                    li = LoadSettings<List<PlatDetail>>(filePath);
                    foreach (PlatDetail p in li)
                    {
                        DateTime d1 = DateTime.Parse(p.InPlat.Substring(0, 16));
                        DateTime d2;
                        if (p.OutPlat == "")
                            d2 = curTT;
                        else
                            d2 = DateTime.Parse(p.OutPlat.Substring(0, 16));
                        TimeSpan diff = d2 - d1;
                        total += (int)diff.TotalMinutes;
                    }
                }
            }
            catch (Exception ex)
            {
                Error_log.log(ex.Message, "getPlatTotal", "KTCFS");
            }
            return total;
        }
        public void FinalMove(string PlatNo)
        {
            DateTime tt = DateTime.Now;
            try
            {
                string path = Dir + "\\history";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                path += "\\" + tt.ToString("yyyyMMdd");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                List<PlatDetail> li = new List<PlatDetail>();
                string filePath = string.Format("{0}\\{1}.xml", Dir, PlatNo);
                if (File.Exists(filePath))
                {
                    li = LoadSettings<List<PlatDetail>>(filePath);
                    File.Delete(filePath);
                    foreach (PlatDetail p in li)
                    {
                        p.Change = tt.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    filePath = string.Format("{0}\\{1}.xml", path, PlatNo);
                    if (File.Exists(filePath))
                    {
                        Random random = new Random();
                        int randomNumber = random.Next(1, 101);
                        filePath = string.Format("{0}\\{1}_{2}.xml", path, PlatNo, randomNumber);
                    }
                    SaveSettings<List<PlatDetail>>(li, filePath);
                }
            }
            catch (Exception ex)
            {
                Error_log.log(ex.Message, "FinalMove", "KTCFS");
            }
        }
        public int getFee(string PlatNo, string Periodic = "21", string db = "aps4", string pwd = "root", string user = "root", int port = 3306, string ip = "127.0.0.1")
        {
            int total = 0;
            try
            {
                clsMySQL mysql = new clsMySQL(db, pwd, user, port, ip);
                int[] perio = mysql.Periodic(Periodic);

                if (perio[24] == 0) { return 0; }

                int p = perio[24];

                int m = getPlatTotal(PlatNo);
                int h = m / p;


                for (int i = 0; i <= h; i++)
                {
                    int x = 23;
                    if (i < 24)
                        x = i;
                    total += perio[x];
                }
                if (perio[25] != 0)  //= 0 為無上限累計
                    if (total > perio[25])
                        total = perio[25];
            }
            catch (Exception ex)
            {
                Error_log.log(ex.Message, "getFee", "KTCFS");
            }
            return total;
        }
    
        public string getImage(string PlatNo)
        {
            string img = "";
            try
            {
                // 讀取 XML 資料
                List<PlatDetail> li = new List<PlatDetail>();
                string filePath = string.Format("{0}\\{1}.xml", Dir, PlatNo);
                if (File.Exists(filePath))
                {
                    li = LoadSettings<List<PlatDetail>>(filePath);
                    foreach (PlatDetail p in li)
                    {
                        img = p.ImgUrl;
                    }
                }
            }
            catch (Exception ex)
            {
                Error_log.log(ex.Message, "getPlatTotal", "KTCFS");
            }

            return img;
        }
    }
}
