using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace PlatLamp
{
    public class clsSetting
    {
        public  class AppSettings
        {
            public string TableLamp_IP { get; set; }
            public string TableLamp_Port { get; set; }
            public string TableLamp_Secert { get; set; }
            public space Parking { get; set; }
            public MySQL db { get; set; }
            public int inplat { get; set; }

        }
        public class space
        {
            public string Handicap { get; set; }
            public string Mch { get; set; }
            public string Electric { get; set; }
        }

        public class MySQL
        {
            public string url { get; set; }
            public int port { get; set; }
            public string user { get; set; }
            public string pass { get; set; }
            public string database { get; set; }

        }

        public static void SaveSettings<T>(T settings, string filePath)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, settings);
            }
        }

        public static T LoadSettings<T>(string filePath)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var reader = new StreamReader(filePath))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

        //public static XDocument LoadSettings(string filePath)
        //{
        //    return XDocument.Load(filePath);
        //}

    }
}
