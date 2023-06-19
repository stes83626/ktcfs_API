using ErrorMsg;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTCFS
{
    internal class clsMySQL
    {

        string MySQL_IP = "127.0.0.1";
        int MySQL_Port = 3306;
        string MySQL_User = "root";
        string MySQL_PWD = "root";
        string MySQL_DB = "aps4";
        string strConnection = "";
        MySqlConnection cn = new MySqlConnection();

        public clsMySQL(string db = "aps4", string pwd = "root", string user = "root", int port = 3306, string ip = "127.0.0.1")
        {
            MySQL_IP=ip;
            MySQL_Port=port;
            MySQL_User=user;
            MySQL_PWD=pwd;
            MySQL_DB = db;

            strConnection = string.Format("server={0};port={1};uid={2};password={3};database={4}", ip, port, user, pwd, db);
            cn.ConnectionString = strConnection;
        }

        public int[] Periodic(string ID)
        {
            int[] perio = new int[26];
            try
            {
                cn.Open();
                string strSQL = string.Format("Select * FROM periodic Where ID='{0}' ", ID);
                MySqlCommand cmd = new MySqlCommand(strSQL, cn);
                MySqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    perio[0] = int.Parse(rd["H0"].ToString());
                    perio[1] = int.Parse(rd["H1"].ToString());
                    perio[2] = int.Parse(rd["H2"].ToString());
                    perio[3] = int.Parse(rd["H3"].ToString());
                    perio[4] = int.Parse(rd["H4"].ToString());
                    perio[5] = int.Parse(rd["H5"].ToString());
                    perio[6] = int.Parse(rd["H6"].ToString());
                    perio[7] = int.Parse(rd["H7"].ToString());
                    perio[8] = int.Parse(rd["H8"].ToString());
                    perio[9] = int.Parse(rd["H9"].ToString());
                    perio[10] = int.Parse(rd["H10"].ToString());
                    perio[11] = int.Parse(rd["H11"].ToString());
                    perio[12] = int.Parse(rd["H12"].ToString());
                    perio[13] = int.Parse(rd["H13"].ToString());
                    perio[14] = int.Parse(rd["H14"].ToString());
                    perio[15] = int.Parse(rd["H15"].ToString());
                    perio[16] = int.Parse(rd["H16"].ToString());
                    perio[17] = int.Parse(rd["H17"].ToString());
                    perio[18] = int.Parse(rd["H18"].ToString());
                    perio[19] = int.Parse(rd["H19"].ToString());
                    perio[20] = int.Parse(rd["H20"].ToString());
                    perio[21] = int.Parse(rd["H21"].ToString());
                    perio[22] = int.Parse(rd["H22"].ToString());
                    perio[23] = int.Parse(rd["H23"].ToString());
                    perio[24] = int.Parse(rd["PERIOD"].ToString());
                    perio[25] = int.Parse(rd["N_MAX"].ToString());
                }
                rd.Close();
            }
            catch (Exception ex)
            {
                Error_log.log(ex.Message, "MySql", "KTCFS");
            }
            finally
            {
                cn.Close();
            }
            return perio;
        }
    }
}
