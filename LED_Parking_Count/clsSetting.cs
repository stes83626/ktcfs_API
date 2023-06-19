using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static LED_Parking_Count.Form1.getSpaceInfoRec;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LED_Parking_Count
{
    public class clsSetting
    {
        public class AppSettings
        {
            public string TableLamp_IP { get; set; }
            public string TableLamp_Port { get; set; }
            public string TableLamp_Secert { get; set; }

            public class floor
            {           
                public string floorName { get; set; }
                public string Handicap { get; set; }
                public string Mch { get; set; }
                public string Electric { get; set; }
            }

            public List<floor> Parking { get; set; }

            public class Led
            {
                public string COM { get; set; }
                public string FontSize { get; set; }
                public string Normal_ID { get; set; }
                public string Handicap_ID { get; set; }
                public string Mch_ID { get; set; }
                public string Electric_ID { get; set; }
                public int Normal_Color { get; set; }
                public int Handicap_Color { get; set; }
                public int Mch_Color { get; set; }
                public int Electric_Color { get; set; }
                public string weekday_ID { get; set; }
                public int weekday_Color { get; set; }
                public string holiday_ID { get; set; }
                public int holiday_Color { get; set; }

            }

            public Led led { get; set; }
            public class MySQL
            {
                public string url { get; set; }
                public string port { get; set; }
                public string user { get; set; }
                public string pass { get; set; }
                public string database { get; set; }
                public string table { get; set; }
            }

            public MySQL db { get; set; }
        }


        public static AppSettings LoadSettings(string filePath)
        {
            var serializer = new XmlSerializer(typeof(AppSettings));
            using (var reader = new StreamReader(filePath))
            {
                return (AppSettings)serializer.Deserialize(reader);
            }
        }

    }
}
