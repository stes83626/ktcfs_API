using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTCFS
{
    public  class PlatDetail
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
    public class PlatInf
    {
        public int Fee { get; set; } =0;
        public int Total { get; set; } =0;
        public string Image { get; set; } = "";
        public string InTime { get; set; } = "";
        public string OutTime { get; set; } = "";
        public string spaceType { get; set; } = "";
        public string spaceNo { get; set; } = "";

    }
}
