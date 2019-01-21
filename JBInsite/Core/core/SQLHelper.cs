using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.core
{
    [Serializable]
    public sealed class IData
    {
        /// <summary>
        /// Author:		Bien.vo
        /// Create date: 7/2/2018
        /// Description: Connect to DB PGSQL using Dapper
        /// </summary>
        private string connectionString;
        private SqlConnection idbConn = null; // Đối tượng connection
        private SqlCommand cmdIDbCommand; //Đối tượng thực thi
        private SqlTransaction tsnTransaction = null;//Đối tượng Transaction truyền từ ngoài vào
        private string strTableName;
        private int ConnectionTimeout { get; set; }
        private int GetTimeout(int? commandTimeout = null)
        {
            if (commandTimeout.HasValue)
                return commandTimeout.Value;

            return ConnectionTimeout;
        }
        private IData()
        {
            try
            {
                this.connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                if (string.IsNullOrEmpty(this.connectionString))
                {
                    this.connectionString = ConfigurationManager.AppSettings["ConnectionString"];
                }
            }
            catch
            {

            }
        }
        private IData(string strConnect)
        {
            this.connectionString = strConnect;
        }
        public static IData CreateData()
        {
            return new IData();
        }
        public static IData CreateData(string strConnect)
        {
            return new IData(strConnect);
        }
        ~IData()
        {
            // Nếu còn connect thì ngắt connect
            if (IsConnected())
                Disconnect();
        }
        public void Connect()
        {
            Disconnect();
            idbConn = new SqlConnection(this.connectionString);
            idbConn.Open();
        }
        /// <summary>
        /// Đối tượng Transaction truyền từ ngoài vào
        /// </summary>
        public void Disconnect()
        {
            try
            {
                if (cmdIDbCommand != null)
                    cmdIDbCommand.Dispose();

                if (idbConn != null)
                {
                    idbConn.Close();
                    SqlConnection.ClearPool((SqlConnection)this.idbConn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Return to Datatable by connectionString Init
        /// </summary>
        /// <param name="storedProcedure"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public DataTable ExecStoreToDataTable(string storedProcedure, SortedList param = null, int? commandTimeout = null)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(storedProcedure, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = storedProcedure;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = commandTimeout ?? 500;

                connection.Open();

                for (int i = 0; i < param.Count; i++)
                {
                    AddParameter(cmd, this.FormatParameter(param.GetKey(i).ToString()), Ultility.COnull(param.GetByIndex(i)));
                }

                adapter.SelectCommand = cmd;
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                adapter.Dispose();

                cmd.Cancel();
                cmd.Dispose();

                connection.Close();
                connection.Dispose();
            }
            return dt;
        }

        /// <summary>
        /// Return to Datatable by connectionString Input
        /// </summary>
        /// <param name="storedProcedure"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        #region Dapper
        #region Query
        public List<T> ExecQueryToList<T>(string query, object[] iparam = null,
           dynamic outParam = null, SqlTransaction transaction = null,
           bool buffered = true, int? commandTimeout = null) where T : class
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                var param = Ultility.Converts(iparam);
                var output = connection.Query<T>(query, param: (object)param, transaction: transaction, buffered: buffered, commandTimeout: commandTimeout ?? 60, commandType: CommandType.Text).ToList();
                return output;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }

        }
        public T ExecQueryToObject<T>(string storedProcedure, object[] iparam = null,
           dynamic outParam = null, SqlTransaction transaction = null,
           bool buffered = true, int? commandTimeout = null) where T : class
        {
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                var param = Ultility.Converts(iparam);
                var output = connection.Query<T>(storedProcedure, param: (object)param, transaction: transaction, buffered: buffered, commandTimeout: commandTimeout ?? 60, commandType: CommandType.StoredProcedure).FirstOrDefault();

                return output;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }
        #endregion
        #region StoreProceduce
        public List<T> ExecStoreToList<T>(string storedProcedure, object[] iparam = null,
           dynamic outParam = null, SqlTransaction transaction = null,
           bool buffered = true, int? commandTimeout = null) where T : class
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                if (tsnTransaction != null)
                {
                    transaction = tsnTransaction;
                }
                var param = Ultility.Converts(iparam);
                var output = connection.Query<T>(storedProcedure, param: (object)param, transaction: transaction, buffered: buffered, commandTimeout: commandTimeout ?? 60, commandType: CommandType.StoredProcedure);
                return output.ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }

        }
        public object ExecStoreToPaging<T>(string storedProcedure, object[] iparam = null,
           dynamic outParam = null, SqlTransaction transaction = null,
           bool buffered = true, int? commandTimeout = null) where T : class, new()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                if (tsnTransaction != null)
                {
                    transaction = tsnTransaction;
                }
                var param = Ultility.Converts(iparam);
                var lst = connection.Query<T>(storedProcedure, param: (object)param, transaction: transaction, buffered: buffered, commandTimeout: commandTimeout ?? 60, commandType: CommandType.StoredProcedure).ToList();
                int totalRow = 0;
                if (lst != null)
                {
                    List<T> lstT = (List<T>)lst;
                    if (lstT.Count > 0)
                    {
                        T obj = lstT[0];
                        if (obj.GetType().GetProperty("rowTotal") != null)
                        {
                            totalRow = (int)obj.GetType().GetProperty("rowTotal").GetValue(obj, null);
                        }
                        else
                        {
                            totalRow = lstT.Count;
                        }
                    }
                }
                return new { totalRow = totalRow, lst = lst };
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }

        }
        public List<T> ExecStoreToPaging<T>(string storedProcedure, ref int totalRow, object[] iparam = null,
           dynamic outParam = null, SqlTransaction transaction = null,
           bool buffered = true, int? commandTimeout = null) where T : class, new()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                if (tsnTransaction != null)
                {
                    transaction = tsnTransaction;
                }
                var param = Ultility.Converts(iparam);
                var lst = connection.Query<T>(storedProcedure, param: (object)param, transaction: transaction, buffered: buffered, commandTimeout: commandTimeout ?? 60, commandType: CommandType.StoredProcedure).ToList();
                if (lst != null)
                {
                    List<T> lstT = (List<T>)lst;
                    if (lstT.Count > 0)
                    {
                        T obj = lstT[0];
                        if (obj.GetType().GetProperty("rowTotal") != null)
                        {
                            totalRow = (int)obj.GetType().GetProperty("rowTotal").GetValue(obj, null);
                        }
                        else
                        {
                            totalRow = lstT.Count;
                        }
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }

        }

        public T ExecStoreToObject<T>(string storedProcedure, object[] iparam = null,
           dynamic outParam = null, SqlTransaction transaction = null,
           bool buffered = true, int? commandTimeout = null) where T : class
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                var param = Ultility.Converts(iparam);
                var output = connection.Query<T>(storedProcedure, param: (object)param, transaction: transaction, buffered: buffered, commandTimeout: commandTimeout ?? 60, commandType: CommandType.StoredProcedure).FirstOrDefault();
                return output;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }

        }
        #endregion
        #endregion

        public void CreateNewStoredProcedure(String strStoreName)
        {
            strTableName = strStoreName;
            cmdIDbCommand = SetCommand(strStoreName);
            cmdIDbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            cmdIDbCommand.CommandTimeout = 10;
        }
        public void CreateNewStoredProcedure(String strStoreName, int intTimeout)
        {
            strTableName = strStoreName;
            cmdIDbCommand = SetCommand(strStoreName);
            cmdIDbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            cmdIDbCommand.CommandTimeout = intTimeout;
        }
        public void AddParameter(String strParameterName, object objValue)
        {
            if (cmdIDbCommand != null)
            {
                ((SqlCommand)cmdIDbCommand).Parameters.AddWithValue(strParameterName, objValue);
            }
        }
        public void AddParameter(object[] objArrParam)
        {
            for (int i = 0; i < objArrParam.Length / 2; i++)
                AddParameter(objArrParam[i * 2].ToString().Trim(), objArrParam[i * 2 + 1]);
        }
        public void BeginTransaction()
        {
            if (!IsConnected())
                Connect();

            this.tsnTransaction = this.idbConn.BeginTransaction();
        }
        public void CommitTransaction()
        {
            if (tsnTransaction != null)
                tsnTransaction.Commit();
        }
        public void RollBackTransaction()
        {
            if (tsnTransaction != null)
            {
                tsnTransaction.Rollback();
                tsnTransaction = null;
            }
        }

        #region Query
        public String ExecQueryToString(String strSQL)
        {
            try
            {
                Object objTemp = SetCommand(strSQL).ExecuteScalar();

                if (objTemp == null || objTemp.Equals("null"))
                    return "";

                return objTemp.ToString().Trim();
            }
            catch (SqlException objExce)
            {
                throw (objExce);
            }
        }
        public String ExecQueryToString(String strSQL, IDbTransaction objTrans)
        {
            IDbCommand iCmd = SetCommand(strSQL);
            iCmd.Transaction = objTrans;

            try
            {
                Object objTemp = iCmd.ExecuteScalar();

                if (objTemp == null || objTemp.Equals("null"))
                    return "";

                return objTemp.ToString().Trim();
            }
            catch (SqlException objExec)
            {
                throw (objExec);
            }
        }
        public void ExecNonQuery(String strSQL)
        {
            try
            {
                SetCommand(strSQL).ExecuteNonQuery();
            }
            catch (SqlException objExec)
            {
                throw (objExec);
            }
        }
        public void ExecNonQuery(String strSQL, ArrayList arrParameters)
        {
            try
            {
                IDbCommand iCmd = SetCommand(strSQL);

                foreach (SqlParameter objPara in arrParameters)
                    iCmd.Parameters.Add(objPara);

                iCmd.ExecuteNonQuery();
            }
            catch (SqlException objExec)
            {
                throw (objExec);
            }
        }
        public DataTable ExecQueryToDataTable(String strSQL)
        {
            try
            {
                DataSet dsResult = new DataSet();
                SetDataAdapter(strSQL).Fill(dsResult);
                return dsResult.Tables[0];
            }
            catch (Exception objEx)
            {
                throw new Exception(objEx.ToString());
            }
        }
        #endregion
        #region StoreProceduce
        public String ExecStoreToString()
        {

            try
            {
                if (tsnTransaction != null)
                {
                    cmdIDbCommand.Transaction = tsnTransaction;
                }
                Object objTemp = cmdIDbCommand.ExecuteScalar();
                if (objTemp == null || objTemp.Equals("null"))
                    return "";
                return objTemp.ToString().Trim();
            }
            catch (SqlException objExce)
            {
                throw (objExce);
            }
        }
        public int ExecStoreNonQuery()
        {
            int intNumber = 0;

            try
            {
                if (tsnTransaction != null)
                {
                    cmdIDbCommand.Transaction = tsnTransaction;
                }
                intNumber = cmdIDbCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.ToString());
            }

            return intNumber;
        }
        public DataTable ExecStoreToDataTable()
        {
            try
            {
                if (tsnTransaction != null)
                {
                    cmdIDbCommand.Transaction = tsnTransaction;
                }
                DataSet dsResult = new DataSet();
                SetDataAdapter(cmdIDbCommand).Fill(dsResult);

                dsResult.Tables[0].TableName = "Data";

                return dsResult.Tables[0];
            }
            catch (Exception objEx)
            {
                throw new Exception(objEx.ToString());
            }
        }
        public DataSet ExecStoreToDataSet(String strOutParameter)
        {
            try
            {
                if (tsnTransaction != null)
                {
                    cmdIDbCommand.Transaction = tsnTransaction;
                }
                DataSet dsResult = new DataSet();
                SetDataAdapter(cmdIDbCommand).Fill(dsResult);
                return dsResult;
            }
            catch (Exception objEx)
            {
                throw new Exception(objEx.ToString());
            }
        }
        #endregion
        private String FormatParameter(String name)
        {
            if (name.IndexOf("@").Equals(0))
                return name;
            return "@" + name;
        }

        private void AddParameter(SqlCommand cmd, string name, object value)
        {
            SqlParameter parameter = new SqlParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            cmd.Parameters.Add(parameter);
        }
        private SqlDataAdapter SetDataAdapter(SqlCommand objCommand)
        {
            return new SqlDataAdapter(objCommand);
        }
        private SqlDataAdapter SetDataAdapter(String strSQL)
        {
            return new SqlDataAdapter(strSQL, (SqlConnection)idbConn);
        }
        private SqlCommand SetCommand(String strSQL)
        {
            cmdIDbCommand = new SqlCommand(strSQL, this.idbConn);
            if (tsnTransaction != null)
                cmdIDbCommand.Transaction = tsnTransaction;
            return cmdIDbCommand;
        }
        private bool IsConnected()
        {
            if (idbConn == null || idbConn.State != System.Data.ConnectionState.Open)
                return false;
            return true;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }


    }
}
