using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace winPlatFind
{
    public class clsSetting
    {

        public class AppSettings
        {
            public string TableLamp_IP { get; set; }
            public string TableLamp_Port { get; set; }
            public string TableLamp_Secert { get; set; }

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
    }
}
