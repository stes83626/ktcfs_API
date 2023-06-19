using ErrorMsg;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace KTCFS
{
    public  class PlatXmlURL
    {
        public string webapi = "127.0.0.1:80";
        public PlatXmlURL(String WebAPI ) 
        {
            webapi = string.Format("{0}api/KTCFS/", WebAPI);
        }

        private string GetWebAPI(string Fun, string parm)
        {
            string strValue = "";
            try
            {
                string url = webapi + Fun;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                // 設定超時為10秒 (1000 毫秒)
                request.Timeout = 3000;
                string postData = parm;
                string contentType = "text/plain";
                // 將內容轉換為位元組陣列
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // 建立 HTTP 請求物件
                request.Method = "POST";
                request.ContentType = contentType;
                request.ContentLength = byteArray.Length;
                // 寫入內容到請求流
                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }
                // 發送請求並取得回應
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                // 讀取回應資料
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    strValue = streamReader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Error_log.log(ex.Message, "WebAPI:GetWebAPI", "KTCFS");
            }
            return strValue;
        }
        public int getPlatTotal(string PlatNo)
        {
            int total = 0;
            try
            {
                string responseData = GetWebAPI("getPlatTotal", PlatNo);

                //WebClient client = new WebClient();
                //client.Headers[HttpRequestHeader.ContentType] = "text/plain";
                //var result = client.UploadString(url, "POST", data);
                total = int.Parse(responseData);
            }
            catch (Exception ex)
            {
                Error_log.log(ex.Message, "WebAPI:getPlatTotal", "KTCFS");
            }
            return total;
        }
        public int getFee(string PlatNo )
        {
 
            int total = 0;
            try
            {
                string result = GetWebAPI("PlatFee", PlatNo);
                total = int.Parse(result);
            }catch (Exception ex) 
            {
                Error_log.log(ex.Message, "WebAPI:getFee", "KTCFS");
            }
            return total;

        }
        public bool FinalMove(string PlatNo)
        {

            try
            {
                string result = GetWebAPI("FinalMove", PlatNo);
                return result== "True" ? true:false;
            }
            catch (Exception ex)
            {
                Error_log.log(ex.Message, "WebAPI:FinalMove", "KTCFS");
            }
            return false;
        }
        public string getImage(string PlatNo)
        {
            string img = "";
            try
            {
                string result = GetWebAPI("getIamge", PlatNo);
                img =result;
            }
            catch (Exception ex)
            {
                Error_log.log(ex.Message, "WebAPI:getImage", "KTCFS");
            }
            return img;
        }

        public PlatInf getInf(string PlatNo, int Periodic)
        {
            PlatInf pi = new PlatInf();
            try
            {
                string result = GetWebAPI("getInf", PlatNo + "," + Periodic);
                pi = JsonConvert.DeserializeObject<PlatInf>(result);

            }
            catch (Exception ex)
            {
                Error_log.log(ex.Message, "WebAPI:getPlatTotal", "KTCFS");
            }

            return pi;
        }
    }
}
