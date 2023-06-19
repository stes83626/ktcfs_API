using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace 富軥888_API
{
    public class LED
    {


        public string COM { get; set; }

        public LED(string name)
        {
            COM = name;
        }

        public enum LedColor : byte
        {
            Red = 0x01,
            Green = 0x02,
            Yellow = 0x03,
            Blue = 0x04,
            Pink = 0x05,
            Cyan = 0x06,
            White = 0x07
        }


        Dictionary<string, byte> sizeMap = new Dictionary<string, byte>()
        {
            { "16",0x10},
            { "24",0x18},
            { "32",0x20},

        };

        private byte XOR8250(byte[] Db)
        {
            byte xorResult = Db[0];
            // 求xor校验和。注意：XOR运算从第二元素开始
            for (int i = 1; i < Db.Length; i++)
            {
                xorResult ^= Db[i];
            }
            return xorResult;


        }

        public byte[] ReplaceBytes(byte[] src, byte[] search, byte[] repl)
        {

            byte[] F5 = new byte[] { 0xFA, 0x05 };
            byte[] FA = new byte[] { 0xFA, 0x0A };
            byte[] dst = null;
            //int index = FindBytes(src, search);
            byte[] DATAQ = new byte[] { };
            byte[] DQ = new byte[] { };
            int index = -1;
            int matchIndex = 0;
            DQ = new byte[1] { src[0] };
            DATAQ = DATAQ.Concat(DQ).ToArray();
            // handle the complete source array
            for (int i2 = 1; i2 < src.Length; i2++)
            {
                if (src[i2] == search[matchIndex])
                {

                    DATAQ = DATAQ.Concat(repl).ToArray();
                    if (matchIndex == (search.Length - 1))
                    {
                        index = i2 - matchIndex;

                    }

                }
                else
                {
                    DQ = new byte[1] { src[i2] };
                    DATAQ = DATAQ.Concat(DQ).ToArray();
                }

            }


            return DATAQ;
        }

        private void COMPORT(byte[] BYT)
        {
            SerialPort comportCounter = new SerialPort();

            comportCounter.PortName = COM;
            comportCounter.BaudRate = 9600;
            comportCounter.Parity = Parity.None;
            comportCounter.DataBits = 8;
            comportCounter.StopBits = StopBits.One;
            try
            {
                comportCounter.Close();
            }
            catch
            {
            }
            try
            {
                comportCounter.Open();
                comportCounter.DiscardInBuffer();
            }
            catch (Exception EX)
            {
                //MessageBox.Show("Port open error!!" + EX.ToString());
            }
            finally
            {
                if (comportCounter.IsOpen)
                {
                    try
                    {

                        comportCounter.Write(BYT, 0, BYT.Length);
                    }
                    catch (Exception ds)
                    {
                        //MessageBox.Show(ds.ToString());
                        //  WriteTextLog(ds.ToString());
                    }
                }
                try
                {
                    comportCounter.DiscardInBuffer();
                    comportCounter.Close();
                }
                catch
                {

                }
                // MessageBox.Show("Port open OK!!");
            }
        }


        public void GettextBox(string showtext, LedColor color, string fontsize, string address)
        {

            if (showtext == "" || fontsize == "" || address == "") 
            {
                return;
            }

            string msg = showtext;

            byte[] DbyteD = new byte[] { };//
            byte[] DbyteD2 = new byte[] { };//

            byte[] X2 = new byte[] { 0xF5, 0x01 };
            DbyteD2 = DbyteD2.Concat(X2).ToArray();

            byte addr = Convert.ToByte(address);

            byte[] XID = new byte[] { addr };//屏地址 范围:0x00-0xff;
            DbyteD = DbyteD.Concat(XID).ToArray();

            byte[] XCOM = new byte[] { 0x02 };//命令;
                                              //0x01	固定文字信息（永久保存）
                                              //0x02    临时文字信息（不保存）
                                              //0x03    清除行临时信息
                                              //0x04    清除全部临时信息
                                              //0x05    亮度调节
                                              //0x06    校正时间
                                              //0x07    控制红绿灯
                                              //0x08    播放语音
                                              //0x09    车位引导（不保存）
                                              //0x0A    发送图片的长度(bmp文件)（永久保存）
                                              //0x0B    发送图片的内容（永久保存)
                                              //0x0C    显示固定图片（永久保存)
                                              //0x0E    开机显示
                                              //0x0F    控制继电器
            DbyteD = DbyteD.Concat(XCOM).ToArray();
            byte[] XASK = new byte[] { 0x00 };//0X00=发送，不需要应答，0X01=发送，需要应答
            DbyteD = DbyteD.Concat(XASK).ToArray();
            byte[] XWAIT = new byte[] { 0x00, 0x00 };//停留时间 0x0005表示停留5秒,0xFFFF表示停留65535秒 
            DbyteD = DbyteD.Concat(XWAIT).ToArray();
            byte[] XLINE = new byte[] { 0x01 };//第一行
            DbyteD = DbyteD.Concat(XLINE).ToArray();
            byte[] XMODE = new byte[] { 0x0B };//显示方式(00左移入) 0x01 上移入
                                               //0x00 左移入
                                               //0x01 上移入
                                               //0x02 下移入
                                               //0x03 左展入
                                               //0x04 右展入
                                               //0x05 上展入
                                               //0x06 下展入
                                               //0x07 横向展开
                                               //0x08 横向闭合
                                               //0x09 纵向展开
                                               //0x0A 纵向闭合
                                               // 0x0B 同时显示
            DbyteD = DbyteD.Concat(XMODE).ToArray();
            //byte[] XDATE = new byte[] { 0x00 };//0x00 固定信息
            //0x01日期 + 固定信息
            //0x02 日期
            // DbyteD = DbyteD.Concat(XDATE).ToArray();
            byte[] XSPACE = new byte[] { 0x00 };//速度(最快)备注：0x00最快，0x02最慢
            DbyteD = DbyteD.Concat(XSPACE).ToArray();


            //string selectedColor = showcolor;
            //int x = int.Parse(color.ToString());


            string strvalue = color.ToString();
            int x = (int)color;
            byte selectedValue = (byte)(x);

            byte[] XBACKCOOL = new byte[] { selectedValue };///1=红色
            DbyteD = DbyteD.Concat(XBACKCOOL).ToArray();
            //2 = 绿色
            //3 = 黄色
            //4 = 蓝色
            //5 = 粉色
            //6 = 青色
            //7 = 白色


            string selectedSize = fontsize;
            byte SizeValue = sizeMap[selectedSize];

            byte[] XFORNTSIZE = new byte[] { SizeValue }; //0x10-16点字体,0x18-24点字体,0x20-32点字体
            DbyteD = DbyteD.Concat(XFORNTSIZE).ToArray();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            byte[] XDATA = Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding(950), Encoding.UTF8.GetBytes(msg)); //System.Text.Encoding.Convert(gb2312, big5, bGb2312);


            var checklinte = (byte)XDATA.Length;
            byte[] XDATALIHT = new byte[] { checklinte };
            DbyteD = DbyteD.Concat(XDATALIHT).ToArray(); //显示内容的长度
            DbyteD = DbyteD.Concat(XDATA).ToArray();//显示内容(ascII或汉字内码GB2312

           

            var checksum = (byte)DbyteD.Length;
            byte[] X2L = new byte[] { 0x00, checksum }; //长度 2Byte，从[屏地址]到[数据]最后字节。高字节在前，低字节在后。()// byte[] XDATA = new byte[] { };//N Byte，显示内容(ascII或汉字内码GB2312） 可以为空 
            DbyteD2 = DbyteD2.Concat(X2L).ToArray();
            DbyteD2 = DbyteD2.Concat(DbyteD).ToArray();

            byte check_sum = XOR8250(DbyteD2);//总包校验
            byte[] check = new byte[] { check_sum };
            DbyteD2 = DbyteD2.Concat(check).ToArray();
            byte[] F5 = new byte[] { 0xFA, 0x05 };
            byte[] FA = new byte[] { 0xFA, 0x0A };

            DbyteD2 = ReplaceBytes(DbyteD2, new byte[] { 0xFA }, FA);
            DbyteD2 = ReplaceBytes(DbyteD2, new byte[] { 0xF5 }, F5);

            COMPORT(DbyteD2);

        }

    }

}
