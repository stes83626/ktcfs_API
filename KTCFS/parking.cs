using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KTCFS
{
    public class PlatAPI
    {
        string IP = "127.0.0.1";
        string Port = "80";
        string secret = "";
        public PlatAPI(string Secret,string mIP = "127.0.0.1",string mPort = "80")
        {
            secret = Secret;
            IP = mIP;
            Port = mPort;
        }

        public class AreaListSend
        {
            public class dataInf
            {
                public int floorId { get; set; }
            }
            public dataInf data { get; set; } = new dataInf();
            public string cmd { get; set; }
            public string ts { get; set; }
            public string sign { get; set; }
        }
        public class AreaListRec
        {
            public class contentList
            {
                public int floorId { get; set; }
                public string floorName { get; set; }
                public int areaID { get; set; }
                public string areaName { get; set; }
            }
            public List<contentList> content { get; set; }
            public string success { get; set; }
            public string msg { get; set; }
            public string code { get; set; }

        }
        public string getAreaList(  AreaListSend send, ref AreaListRec ret)
        {
            string result = "";
            DateTimeOffset utcTime = DateTimeOffset.UtcNow;
            long timestamp = utcTime.ToUnixTimeMilliseconds();
            send.ts = timestamp.ToString();
            send.cmd = "getAreaList";
            string sign = send.cmd + send.ts + secret;
            send.sign = GetMd5(sign);

            var JsonPost = JsonConvert.SerializeObject(send);

            string strURL = string.Format("http://{0}:{1}/v1/open/getAreaList", IP, Port);
            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                byte[] response = client.UploadData(strURL, "POST", Encoding.UTF8.GetBytes(JsonPost));
                result = Encoding.UTF8.GetString(response);
                ret = JsonConvert.DeserializeObject<AreaListRec>(result);
            }

            return result;
        }


        public class FloorListSend
        {
            public string cmd { get; set; }
            public string ts { get; set; }
            public string sign { get; set; }
        }
        public class FloorListRec
        {
            public class FloortList
            {
                public int floorId { get; set; }
                public string floorName { get; set; }
                public string floorMap { get; set; }

            }
            public List<FloortList> content { get; set; }
            public string success { get; set; }
            public string msg { get; set; }
            public string code { get; set; }

        }
        public string getFloorList( FloorListSend send, ref FloorListRec ret)
        {
            string result = "";
            DateTimeOffset utcTime = DateTimeOffset.UtcNow;
            long timestamp = utcTime.ToUnixTimeMilliseconds();
            send.ts = timestamp.ToString();
            send.cmd = "getFloorList";
            string sign = send.cmd + send.ts + secret;
            send.sign = GetMd5(sign);

            var JsonPost = JsonConvert.SerializeObject(send);

            string strURL = string.Format("http://{0}:{1}/v1/open/getFloorList", IP, Port);
            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                byte[] response = client.UploadData(strURL, "POST", Encoding.UTF8.GetBytes(JsonPost));
                result = Encoding.UTF8.GetString(response);
                ret = JsonConvert.DeserializeObject<FloorListRec>(result);
            }

            return result;

        }


        public class CarLocInfo
        {
            public string floorId { get; set; }
            public string spaceNo { get; set; }
            public string floorName { get; set; }
            public string carImage { get; set; }
            public string inTime { get; set; }
            public string parkTime { get; set; }
            public string plateNo { get; set; }

        }
        public class CarLocInfoSend
        {
            public string cmd { get; set; }
            public string ts { get; set; }
            public string sign { get; set; }
            public class dataInf
            {
                public string plateNo { get; set; }
            }
            public dataInf data { get; set; } = new dataInf();

        }
        public class CarLocInfoRec
        {

            public string success { get; set; }
            public string msg { get; set; }
            public string code { get; set; }


            public List<CarLocInfo> content { get; set; }

        }
        public string getCarLocInfo( CarLocInfoSend send, ref CarLocInfoRec ret)
        {
            //車牌號碼(全牌號)
            string result = "";
            DateTimeOffset utcTime = DateTimeOffset.UtcNow;
            long timestamp = utcTime.ToUnixTimeMilliseconds();
            send.ts = timestamp.ToString();
            send.cmd = "getCarLocInfo";
            string sign = send.cmd + send.ts + secret;
            send.sign = GetMd5(sign);
            var JsonPost = JsonConvert.SerializeObject(send);

            string strURL = string.Format("http://{0}:{1}/v1/open/getCarLocInfo", IP, Port);
            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                byte[] response = client.UploadData(strURL, "POST", Encoding.UTF8.GetBytes(JsonPost));
                result = Encoding.UTF8.GetString(response);
                ret = JsonConvert.DeserializeObject<CarLocInfoRec>(result);
            }

            return result;
        }

        public class CarLocListSend
        {
            public string cmd { get; set; }
            public string ts { get; set; }
            public string sign { get; set; }
            public class dataInf
            {
                public string plateNo { get; set; }
                public int count { get; set; }
            }
            public dataInf data { get; set; } = new dataInf();

        }
        public class CarLocListRec
        {
            public string success { get; set; }
            public string msg { get; set; }
            public string code { get; set; }

            public List<CarLocInfo> content { get; set; }

        }
        public string getCarLocList( CarLocListSend send, ref CarLocListRec ret)
        {
            //車牌號碼(全牌號?)
            //count
            //數量(返回列表最大數.建議不超過10)
            string result = "";
            DateTimeOffset utcTime = DateTimeOffset.UtcNow;
            long timestamp = utcTime.ToUnixTimeMilliseconds();
            send.ts = timestamp.ToString();
            send.cmd = "getCarLocList";
            string sign = send.cmd + send.ts + secret;
            send.sign = GetMd5(sign);
            var JsonPost = JsonConvert.SerializeObject(send);

            string strURL = string.Format("http://{0}:{1}/v1/open/getCarLocList", IP, Port);
            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                byte[] response = client.UploadData(strURL, "POST", Encoding.UTF8.GetBytes(JsonPost));
                result = Encoding.UTF8.GetString(response);
                ret = JsonConvert.DeserializeObject<CarLocListRec>(result);
            }

            return result;
        }


        public class CarLocList2Send
        {
            public string cmd { get; set; }
            public string ts { get; set; }
            public string sign { get; set; }
            public class dataInf
            {
                public string plateNo { get; set; }
                public int pageIndex { get; set; }
                public int pageSize { get; set; }
            }
            public dataInf data { get; set; } = new dataInf();

        }
        public class CarLocList2Rec
        {
            public string success { get; set; }
            public string msg { get; set; }
            public string code { get; set; }

            public List<CarLocInfo> content { get; set; }

        }
        public string getCarLocList2( CarLocList2Send send, ref CarLocList2Rec ret)
        {
            //車牌號碼
            //pageIndex
            //分頁頁數(1開始)
            //pageSize
            //每頁數量(>0)
            string result = "";
            DateTimeOffset utcTime = DateTimeOffset.UtcNow;
            long timestamp = utcTime.ToUnixTimeMilliseconds();
            send.ts = timestamp.ToString();
            send.cmd = "getCarLocList2";
            string sign = send.cmd + send.ts + secret;
            send.sign = GetMd5(sign);
            var JsonPost = JsonConvert.SerializeObject(send);

            string strURL = string.Format("http://{0}:{1}/v1/open/getCarLocList2", IP, Port);
            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                byte[] response = client.UploadData(strURL, "POST", Encoding.UTF8.GetBytes(JsonPost));
                result = Encoding.UTF8.GetString(response);
                ret = JsonConvert.DeserializeObject<CarLocList2Rec>(result);
            }
            return result;
        }

        public class CarLocRouteSend
        {
            public string cmd { get; set; }
            public string ts { get; set; }
            public string sign { get; set; }
            public class dataInf
            {
                public string beginNo { get; set; }
                public string endNo { get; set; }
            }
            public dataInf data { get; set; } = new dataInf();

        }
        public class CarLocRouteRec
        {
            public string success { get; set; }
            public string msg { get; set; }
            public string code { get; set; }

            public class CarLocRoute
            {
                public string url { get; set; }

            }
            public CarLocRoute content { get; set; }

        }
        public string getCarLocRoute( CarLocRouteSend send, ref CarLocRouteRec ret)
        {
            //endNo 終點車位編號
            string result = "";
            DateTimeOffset utcTime = DateTimeOffset.UtcNow;
            long timestamp = utcTime.ToUnixTimeMilliseconds();
            send.ts = timestamp.ToString();
            send.cmd = "getCarLocRoute";
            string sign = send.cmd + send.ts + secret;
            send.sign = GetMd5(sign);
            var JsonPost = JsonConvert.SerializeObject(send);

            string strURL = string.Format("http://{0}:{1}/v1/open/getCarLocRoute", IP, Port);
            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                byte[] response = client.UploadData(strURL, "POST", Encoding.UTF8.GetBytes(JsonPost));
                result = Encoding.UTF8.GetString(response);
                ret = JsonConvert.DeserializeObject<CarLocRouteRec>(result);
            }
            return result;
        }

        public class FreeSpaceNumSend
        {
            public string cmd { get; set; }
            public string ts { get; set; }
            public string sign { get; set; }
            public dataInf data { get; set; } = new dataInf();
            public class dataInf
            {
                public int areaId { get; set; }
                public int floorId { get; set; }
            }
        }
        public class FreeSpaceNumRec
        {
            public string success { get; set; }
            public string msg { get; set; }
            public string code { get; set; }

            public class FreeSpaceNum
            {
                public string floorId { get; set; }
                public string floorName { get; set; }
                public string areaId { get; set; }
                public string areaName { get; set; }
                public string freeSpaceNum { get; set; }
                public string totalSpaceNum { get; set; }

            }
            public List<FreeSpaceNum> content { get; set; }

        }
        public string getFreeSpaceNum( FreeSpaceNumSend send, ref FreeSpaceNumRec ret)
        {
            //分區ID > 0：查詢指定區域的空餘車位數
            // 0：查詢所有區域的空於車位數(分組)
            //-1：不使用區域作為條件來分組顯示
            //floorId
            //樓層ID > 0 : 查詢指定樓層的空餘車位數
            //  0：查詢所有樓層的空餘車位數(分組)
            //- 1：不使用樓層作為條件来分組顯示
            string result = "";
            DateTimeOffset utcTime = DateTimeOffset.UtcNow;
            long timestamp = utcTime.ToUnixTimeMilliseconds();
            send.ts = timestamp.ToString();
            send.cmd = "getFreeSpaceNum";
            string sign = send.cmd + send.ts + secret;
            send.sign = GetMd5(sign);
            var JsonPost = JsonConvert.SerializeObject(send);

            string strURL = string.Format("http://{0}:{1}/v1/open/getFreeSpaceNum", IP, Port);
            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                byte[] response = client.UploadData(strURL, "POST", Encoding.UTF8.GetBytes(JsonPost));
                result = Encoding.UTF8.GetString(response);
                ret = JsonConvert.DeserializeObject<FreeSpaceNumRec>(result);

            }
            return result;
        }


        public class ParkingLotInfoSend
        {
            public string cmd { get; set; }
            public string ts { get; set; }
            public string sign { get; set; }


        }
        public class ParkingLotInfoRec
        {
            public string success { get; set; }
            public string msg { get; set; }
            public string code { get; set; }

            public class ParkingLotInfo
            {
                public string parkId { get; set; }
                public string parkName { get; set; }
                public string totalSpace { get; set; }
                public string address { get; set; }
                public string tel { get; set; }

            }
            public ParkingLotInfo content { get; set; }

        }
        public string getParkingLotInfo(ParkingLotInfoSend send, ref ParkingLotInfoRec ret)
        {
            string result = "";
            DateTimeOffset utcTime = DateTimeOffset.UtcNow;
            long timestamp = utcTime.ToUnixTimeMilliseconds();
            send.ts = timestamp.ToString();
            send.cmd = "getParkingLotInfo";
            string sign = send.cmd + send.ts + secret;
            send.sign = GetMd5(sign);
            var JsonPost = JsonConvert.SerializeObject(send);

            string strURL = string.Format("http://{0}:{1}/v1/open/getParkingLotInfo", IP, Port);
            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                byte[] response = client.UploadData(strURL, "POST", Encoding.UTF8.GetBytes(JsonPost));
                result = Encoding.UTF8.GetString(response);
                ret = JsonConvert.DeserializeObject<ParkingLotInfoRec>(result);
            }

            return result;
        }


        public class SpaceInfoSend
        {
            public string cmd { get; set; }
            public string ts { get; set; }
            public string sign { get; set; }

            public dataInf data { get; set; } = new dataInf();
            public class dataInf
            {
                public int areaId { get; set; }
                public int floorId { get; set; }
            }
        }
        public class SpaceInfoRec
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
        public string getSpaceInfo( SpaceInfoSend send, ref SpaceInfoRec ret)
        {
            //區域ID > 0：查詢指定區域
            //-1：不指定區域查詢
            //floorId
            //樓層ID > 0 : 查詢指定樓層
            string result = "";
            DateTimeOffset utcTime = DateTimeOffset.UtcNow;
            long timestamp = utcTime.ToUnixTimeMilliseconds();
            send.ts = timestamp.ToString();
            send.cmd = "getSpaceInfo";
            string sign = send.cmd + send.ts + secret;
            send.sign = GetMd5(sign);
            var JsonPost = JsonConvert.SerializeObject(send);

            string strURL = string.Format("http://{0}:{1}/v1/open/getSpaceInfo", IP, Port);
            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                byte[] response = client.UploadData(strURL, "POST", Encoding.UTF8.GetBytes(JsonPost));
                result = Encoding.UTF8.GetString(response);
                ret = JsonConvert.DeserializeObject<SpaceInfoRec>(result);

            }
            return result;
        }

        public class getSpaceDetailSend
        {
            public string cmd { get; set; }
            public string ts { get; set; }
            public string sign { get; set; }

            public dataInf data { get; set; } = new dataInf();
            public class dataInf
            {
                public int areaId { get; set; }
                public int floorId { get; set; }
            }
        }
        public class getSpaceDetailRec
        {
            public string success { get; set; }
            public string msg { get; set; }
            public string code { get; set; }

            public class SpaceInfo
            {
                public string spaceNo { get; set; }
                public int status { get; set; }
                public int areaId { get; set; }
                public string areaName { get; set; }
                public int floorId { get; set; }
                public string floorName { get; set; }
                public string plateNo { get; set; }
            }
            public List<SpaceInfo> content { get; set; }

        }
        public string getSpaceDetail(getSpaceDetailSend send, ref getSpaceDetailRec ret)
        {
            string result = "";
            DateTimeOffset utcTime = DateTimeOffset.UtcNow;
            long timestamp = utcTime.ToUnixTimeMilliseconds();
            send.ts = timestamp.ToString();
            send.cmd = "getSpaceDetail";
            string sign = send.cmd + send.ts + secret;
            send.sign = GetMd5(sign);
            var JsonPost = JsonConvert.SerializeObject(send);

            string strURL = string.Format("http://{0}:{1}/v1/open/getSpaceDetail", IP, Port);
            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                byte[] response = client.UploadData(strURL, "POST", Encoding.UTF8.GetBytes(JsonPost));
                result = Encoding.UTF8.GetString(response);
                ret = JsonConvert.DeserializeObject<getSpaceDetailRec>(result);

            }
            return result;
        }


        private string GetMd5(string input)
        {
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
    }
}
