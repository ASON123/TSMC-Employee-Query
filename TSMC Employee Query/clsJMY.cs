using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Timers;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;

namespace TSMC_Employee_Query

{


    class clsJMY
    {
        bool ConnSuccess = false;
        
        string _connStr = "";
        string sConn = "";
        /// <summary>
        /// 回傳系統所設定的 資料庫連接字串
        /// </summary>
        public string connStr
        {
            //set { _connStr = value; }
            get
            {
                _connStr = "Persist Security Info=False;";
                _connStr = _connStr + "User ID= " + TSMC_Employee_Query.Properties.Settings.Default.connUser + ";";
                _connStr = _connStr + "Password=" + Properties.Settings.Default.connPass + ";";
                _connStr = _connStr + "Initial Catalog=" + Properties.Settings.Default.DB_Name + ";";
                _connStr = _connStr + "Data Source=" + Properties.Settings.Default.DB_IP + ";";
                _connStr = _connStr + "Max pool size = 300;";


                return _connStr;
            }
        }
        /// <summary>
        /// 回傳系統所設定的 資料庫資料表連接字串
        /// </summary>
        public string connDStr(string mytable)
        {
            //set { _connStr = value; }

            _connStr = "Data Source=" + Properties.Settings.Default.DB_IP + ";";
            _connStr = _connStr + "Database=" + mytable + ";";
            _connStr = _connStr + "User ID= " + Properties.Settings.Default.connUser + ";";
            _connStr = _connStr + "Password=" + Properties.Settings.Default.connPass + ";";
            _connStr = _connStr + "Max pool size = 300;";

            return _connStr;

        }
        /// <summary>
        /// 回傳Sql連接狀態
        /// </summary>
        public void ConnState()
        {
            using (SqlConnection cn = new SqlConnection(sConn))
            {
                cn.Open();
                MessageBox.Show(cn.State.ToString());
            }

        }

        /// <summary>
        /// 將SQLCommand所選取資料,回傳字串陣列
        /// </summary>
        /// <param name="sCmd">SQL Command</param>
        /// <returns></returns>
        public string[,] getDBtoDR(string sCmd)
        {
            sConn = connStr;
            return getDBtoDR(sCmd, sConn);
        }

        /// <summary>
        /// 將SQLCommand所選取資料,回傳字串陣列
        /// </summary>
        /// <param name="sCmd">SQL Command</param>
        /// <param name="sConn">資料庫連接字串</param>
        /// <returns></returns>
        public string[,] getDBtoDR(string sCmd, string sConn)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(sConn))
                {
                    cn.Open();
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.Connection = cn;
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.CommandText = sCmd;
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(sqlCmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0) //如果包含多個DataRow
                    {
                        string[,] sDr = new string[dt.Rows.Count, dt.Columns.Count];

                        for (int nRow = 0; nRow < dt.Rows.Count; nRow++)
                        {
                            for (int nCol = 0; nCol < dt.Columns.Count; nCol++)
                            {
                                sDr[nRow, nCol] = Convert.ToString(dt.Rows[nRow][nCol]);
                            }
                        }
                        return sDr;
                    }
                    else
                        return null;
                }
            }
            catch (SqlException sqlEx)
            {
                //log.Error(sqlEx.Message);
            }
            return null;
        }

        /// <summary>
        /// 將SQLCommand所選取資料,放入ComboBox中,第一位放入空白
        /// </summary>
        /// <param name="sSql">SQLCommand</param>
        /// <param name="cmb">ComboBox</param>
        public void getDBtoCmbHaveSpace(string sSql, ref ComboBox cmb)
        {
            getDBtoCmb(sSql, ref cmb, 0);
            cmb.Items.Insert(0, "");
        }

        /// <summary>
        /// 將SQLCommand所選取資料,放入ComboBox中
        /// </summary>
        /// <param name="sSql">SQLCommand</param>
        /// <param name="cmb">ComboBox</param>
        public void getDBtoCmb(string sSql, ref ComboBox cmb)
        {
            getDBtoCmb(sSql, ref cmb, 0);
        }


        /// <summary>
        /// 將SQLCommand所選取資料,放入ComboBox中
        /// </summary>
        /// <param name="sSql">SQLCommand</param>
        /// <param name="cmb">ComboBox</param>
        /// <param name="n">欲加入第幾個欄位(起始為0)</param>
        public void getDBtoCmb(string sSql, ref ComboBox cmb, int n)
        {
            sConn = connStr;
            getDBtoCmb(sSql, ref cmb, n, sConn);
        }


        /// <summary>
        /// 將SQLCommand所選取資料,放入ComboBox中
        /// </summary>
        /// <param name="sSql">SQLCommand</param>
        /// <param name="cmb">ComboBox</param>
        /// <param name="n">欲加入第幾個欄位(起始為0)</param>
        /// <param name="sConn">資料庫連接字串</param>
        public void getDBtoCmb(string sSql, ref ComboBox cmb, int n, string sConn)
        {
            cmb.Items.Clear();
            cmb.Text = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(sConn))
                {
                    cn.Open();
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.Connection = cn;
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.CommandText = sSql;


                    SqlDataReader dr = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        cmb.Items.Add(dr[n]);
                    }
                    dr.Close();
                    dr.Dispose();


                }
            }
            catch (SqlException sqlEx)
            {
                cmb.Items.Add("主機資料庫連接錯誤 : " + sqlEx.Message);
                //log.Error(sqlEx.Message);
                return;
            }
        }

        /// <summary>
        /// 將SQLCommand所選取資料,放入CheckedListBox中
        /// </summary>
        /// <param name="sSql">SQLCommand</param>
        /// <param name="cmb">ref CheckedListBox</param>
        public void getDBtoCkLb(string sSql, ref CheckedListBox cmb)
        {
            getDBtoCkLb(sSql, ref cmb, 0);
        }

        /// <summary>
        /// 將SQLCommand所選取資料,放入CheckedListBox中
        /// </summary>
        /// <param name="sSql">SQLCommand</param>
        /// <param name="cmb">ref CheckedListBox</param>
        /// <param name="n">欲加入第幾個欄位(起始為0)</param>
        public void getDBtoCkLb(string sSql, ref CheckedListBox cmb, int n)
        {
            sConn = connStr;
            getDBtoCkLb(sSql, ref cmb, n, sConn);
        }

        /// <summary>
        /// 將SQLCommand所選取資料,放入CheckedListBox中
        /// </summary>
        /// <param name="sSql">SQLCommand</param>
        /// <param name="cmb">ref CheckedListBox</param>
        /// <param name="n">欲加入第幾個欄位(起始為0)</param>
        /// <param name="sConn">資料庫連接字串</param>
        public void getDBtoCkLb(string sSql, ref CheckedListBox cmb, int n, string sConn)
        {
            cmb.Items.Clear();
            cmb.Text = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(sConn))
                {
                    cn.Open();
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.Connection = cn;
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.CommandText = sSql;

                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        cmb.Items.Add(dr[n]);
                    }
                    dr.Close();
                    dr.Dispose();
                }
            }
            catch (SqlException sqlEx)
            {
                cmb.Items.Add("主機資料庫連接錯誤 : " + sqlEx.Message);
                //log.Error(sqlEx.Message);
                return;
            }
        }


        public int ExecutSQLID(string sSql)
        {
            sConn = connStr;
            return ExecutSQLID(sSql, sConn);
        }

        public int ExecutSQLID(string sSql, string sConn)
        {
            int nRow = 0;
            try
            {
                using (SqlConnection cn = new SqlConnection(sConn))
                {
                    cn.Open();
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.Connection = cn;
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.CommandText = sSql;

                    nRow = int.Parse(sqlCmd.ExecuteScalar().ToString());
                }
            }
            catch (SqlException sqlEx)
            {
                string sErr = sqlEx.Message;
                //log.Error(sqlEx.Message);
                if (sqlEx.Number == 2627)
                    nRow = -1;
                //log.Error(sqlEx.Message);
            }
            return nRow;
        }

        /// <summary>
        /// 執行SQL指令,傳回成功筆數
        /// </summary>
        /// <param name="sSql">SQL指令</param>
        /// <returns></returns>
        public int ExecutSQL(string sSql)
        {
            sConn = connStr;
            return ExecutSQL(sSql, sConn);
        }

        /// <summary>
        /// 執行SQL指令,傳回成功筆數
        /// </summary>
        /// <param name="sSql">SQL指令</param>
        /// <param name="sConn">資料庫連接字串</param>
        /// <returns></returns>
        public int ExecutSQL(string sSql, string sConn)
        {
            int nRow = 0;
            try
            {
                using (SqlConnection cn = new SqlConnection(sConn))
                {
                    cn.Open();
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.Connection = cn;
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.CommandText = sSql;

                    nRow = sqlCmd.ExecuteNonQuery();
                }
            }
            catch (SqlException sqlEx)
            {
                string sErr = sqlEx.Message;
                //log.Error(sqlEx.Message);
                if (sqlEx.Number == 2627)
                    nRow = -1;
                //log.Error(sqlEx.Message);
            }
            return nRow;
        }

        /// <summary>
        /// 執行SQL指令,傳回成功筆數
        /// </summary>
        /// <param name="sSqlCmd">SQL指令</param>
        /// <returns></returns>
        public int ExecutSQL(SqlCommand sqlCmd)
        {
            int nRow = 0;
            try
            {
                using (SqlConnection cn = new SqlConnection(sConn))
                {
                    cn.Open();
                    sqlCmd.Connection = cn;

                    nRow = sqlCmd.ExecuteNonQuery();
                }
            }
            catch (SqlException sqlEx)
            {
                //log.Error(sqlEx.Message);
            }
            return nRow;
        }


        /// <summary>
        /// 取得DataTable
        /// </summary>
        /// <param name="CommandText">SQL Command</param>
        /// <returns>DataTable</returns>
        public DataTable GetTable(string CommandText)
        {
            Application.DoEvents();
            sConn = connStr;
            DataTable table = new DataTable();
            using (SqlConnection cn = new SqlConnection(sConn))
            {
                //重複連接
                cn.Open();
                SqlDataAdapter da = new SqlDataAdapter(CommandText, cn);
                da.Fill(table);
                da.Dispose();
            }
            return table;
        }

        
        /// <summary>
        /// 取得DataTable
        /// </summary>
        /// <param name="cmd">SQL Command</param>
        /// <returns>DataTable</returns>
        public DataTable GetTable(SqlCommand cmd)
        {
            sConn = connStr;
            DataTable table = new DataTable();
            using (SqlConnection cn = new SqlConnection(sConn))
            {
                cn.Open();
                cmd.Connection = cn;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(table);
            }
            return table;
        }
        /// <summary>
        /// Update資料行
        /// </summary>
        /// <param name="CommandText">SQL Command</param>
        /// <returns>DataUpsate</returns>
        public SqlCommand getSqlCommand(String CommandText)
        {
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand SCD = new SqlCommand(CommandText, conn);
            return SCD;
        }
        /// <summary>
        /// 取得DataRow
        /// </summary>
        /// <param name="cmd">SQL Command</param>
        /// <returns>DataRow</returns>
        public string GetRow(string cmd)
        {
            DataTable table = GetTable(cmd);
            if (table.Rows.Count != 0)
            {
                return table.Rows[0][0].ToString();
            }

            else
            {
                return "";
            }
        }          
        /// <summary>
        /// 取得DataRow
        /// </summary>
        /// <param name="cmd">SQL Command</param>
        /// <returns>DataRow</returns>
        public DataRow GetDataRow(string cmd)
        {
            DataTable table = GetTable(cmd);

            return table.Rows[0];
        }
        /// <summary>
        /// 取得預設資料庫連線
        /// </summary>
        /// <returns></returns>
        public SqlConnection getConnect()
        {
            SqlConnection cn = new SqlConnection(connStr);
            return cn;
        }

        public DataTable GetDataTable(string selectCommand)
        {
            SqlDataAdapter da = new SqlDataAdapter(selectCommand, connStr);
            DataTable dt = new DataTable();
            try
            {

                da.Fill(dt);

                return dt;
            }
            catch (Exception)
            {
                return dt;
            }
        }

        public bool execSQL(string sql)
        {
            //SqlConnection cn = getConnect();
            using (SqlConnection cn = getConnect())
            {
                cn.Open();
                SqlCommand com = new SqlCommand(sql, cn);

                try
                {
                    //執行SQL語句
                    com.ExecuteNonQuery();

                }
                catch (SqlException sqlEx)
                {
                    //log.Error(sqlEx.Message);
                    MessageBox.Show(sqlEx.Message);
                    return false;//返回布林值False
                }
                finally
                {
                    //關閉資料庫連接
                    cn.Close();
                }
            }

            //返回布林值True
            return true;
        }

        public string getItem(string ReName, string Table, string IDName, string IDVaule)
        {
            string sSql = "SELECT " + ReName + " FROM " + Table + " WHERE " + IDName + "='" + IDVaule + "'";
            string[,] sTmp = getDBtoDR(sSql);
            if (sTmp == null)
                return IDVaule;
            else
                return sTmp[0, 0].Trim();
        }

        public string CountRow(string What, string DB, string Name, string Value)
        {
            string sSql = "SELECT Count(" + What + ") FROM " + DB + " WHERE " + Name + "='" + Value + "'";
            string[,] sTmp = getDBtoDR(sSql);
            if (sTmp == null)
                return "0";
            else
                return sTmp[0, 0].Trim();
        }

        internal string execSQLS(string sql)
        {
            //SqlConnection cn = getConnect();
            using (SqlConnection cn = getConnect())
            {
                cn.Open();
                SqlCommand com = new SqlCommand(sql, cn);
                try
                {
                    //執行SQL語句
                    com.ExecuteNonQuery();
                    cn.Close();
                    return "";
                }
                catch (SqlException sqlEx)
                {
                    cn.Close();
                    //log.Error(sqlEx.Message);
                    //MessageBox.Show(sqlEx.Message);
                    return sqlEx.Message;
                }
            }
        }
        public void delete (string sql)
        {
            using (SqlConnection cn = getConnect())
            {
                cn.Open();
                SqlCommand com = new SqlCommand(sql, cn);
                try
                {
                    
                    //執行SQL語句
                    com.ExecuteNonQuery();
                    cn.Close();
                   
                }
                catch (SqlException sqlEx)
                {
                    cn.Close();
                    //log.Error(sqlEx.Message);
                    //MessageBox.Show(sqlEx.Message);
                    
                }
            }
        }

       
    }
}
