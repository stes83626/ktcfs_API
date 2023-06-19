using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PlatLamp
{
    internal class clsMySQL
    {
        string MySQL_IP = "127.0.0.1";
        int  MySQL_Port = 3306;
        string MySQL_User = "root";
        string MySQL_PWD = "root";
        string MySQL_DB = "aps4";
        string strConnection = "";
        MySqlConnection cn = new MySqlConnection();

        public clsMySQL(string db = "aps4", string pwd ="root", string user = "root", int port = 3306 , string ip = "127.0.0.1") 
        {
            strConnection = string.Format("server={0};port={1};uid={2};password={3};database={4}", ip, port, user, pwd, db);
            cn.ConnectionString = strConnection;
        }

        public class LPR
        {
            public string PlatNo = "";
            public string EN_Datetime = "";
            public string CH_Datetime = "";
            public string EX_Datetime = "";
            public string changed = "0";
            
        }

        public class RENT
        {
            public string PlatNo = "";
            public string ENDDATE = "";
            public string ACPT_IN = "";
        }

        public LPR getPLR(string PlatNo)
        {
            LPR lpr = new LPR();
            try
            {
                cn.Open();
                string strSQL = string.Format("Select LICENSE, CHARGED,EN_DATETIME, CH_DATETIME, EX_DATETIME FROM lpr_log Where LICENSE='{0}' ORDER BY EN_DATETIME ASC", PlatNo);
                MySqlCommand cmd = new MySqlCommand(strSQL, cn);
                MySqlDataReader rd = cmd.ExecuteReader();
                while(rd.Read())
                {
                    lpr.PlatNo = rd["LICENSE"].ToString();
                    lpr.changed = rd["CHARGED"].ToString();
                    lpr.EN_Datetime = rd["EN_DATETIME"].ToString();
                    lpr.CH_Datetime = rd["CH_DATETIME"].ToString();
                    lpr.EX_Datetime = rd["EX_DATETIME"].ToString(); 
                }
                rd.Close();
            }



            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                cn.Close();
            }
            return lpr;

            int aaaa = 0;
        }

        public RENT getRENT(string PlatNo)
        {
            RENT rent = new RENT();
            try
            {
                cn.Open();
                string strSQL = string.Format("Select VEHICLENO,ENDDATE,ACPT_IN FROM rent Where VEHICLENO='{0}' ", PlatNo);
                MySqlCommand cmd = new MySqlCommand(strSQL, cn);
                MySqlDataReader rd = cmd.ExecuteReader();
                while(rd.Read())
                {
                    rent.PlatNo = rd["VEHICLENO"].ToString();
                    rent.ENDDATE = rd["ENDDATE"].ToString();
                    rent.ACPT_IN = rd["ACPT_IN"].ToString();
                }
                rd.Close();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                cn.Close();
            }
            return rent;
        }

        public void RentToLPR(string plateNo,string Path) 
        {

            string datetime = getRENT(plateNo).ACPT_IN;
            string id = "1" + DateTime.Now.ToString("yyyyMMddHHmmss");
            updateRent(plateNo);
            try
            {
                cn.Open();
                string strSQL = "INSERT INTO lpr_log (LICENSE, TYPE, CHARGED, EN_DATETIME, PATH, ID,OtherFee,AISLE_IN,ETAG,Identity,AISLE_OUT,MochiPayID,ParkPayID,Description,EXP_DATETIME,CORRECT) " +
                                             "VALUES (@license, 1, 0, @enDatetime, @path,@id,2,1,0,0,0,'','',0,'','')";
                MySqlCommand cmd = new MySqlCommand(strSQL, cn);
                cmd.Parameters.AddWithValue("@license", plateNo);
                cmd.Parameters.AddWithValue("@enDatetime", datetime);
                cmd.Parameters.AddWithValue("@path", Path);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                cn.Close();

            }
            catch (Exception)
            {

            }

        }

        public void updateRent(string plateNo)
        {
            try
            {
                cn.Open();
                string strSQL = $"update rent set OtherFee = '2' where VEHICLENO = '{plateNo}'";
                MySqlCommand cmd = new MySqlCommand(strSQL, cn);
                cmd.ExecuteNonQuery();

                cn.Close();
            }
            catch (Exception)
            {

            }
        
        
        }

        public void updateLPR(string plateNo,string Fee)
        {

            try
            {
                cn.Open();
                string strSQL = $"update lpr_log set OtherFee = '{Fee}' where LICENSE = '{plateNo}'";
                MySqlCommand cmd = new MySqlCommand(strSQL, cn);
                cmd.ExecuteNonQuery();

                cn.Close();
            }
            catch (Exception)
            {

            }
        
        }


    }
}
