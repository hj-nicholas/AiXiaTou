using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace DAL
{
    /// <summary>
    /// 数据访问抽象基础类
    /// </summary>
    public class SQLHelper
    {
        private static SQLHelper sqlHelpInstance = null;

        public const string RETURN_VALUE_PARAMETER_NAME = "@ReturnValue";

        /// <summary>
        /// 私有化类的实现方法；实现单件模式
        /// </summary>
        private SQLHelper()
        {
        }

        /// <summary>
        /// 实例化对象，并返回该对象
        /// </summary>
        /// <returns>SQLHelp对象</returns>
        public static SQLHelper Instance()
        {
            if (sqlHelpInstance == null)
            {
                sqlHelpInstance = new SQLHelper();
            }

            return sqlHelpInstance;
        }

        /// <summary>
        /// 私有方法，以实例化存储过程对应的参数
        /// </summary>
        /// <param name="strConn">数据库连接字符串</param>
        /// <param name="cmdProc">cmd对象名称</param>
        private void CreateCmdParameters(string strConn, SqlCommand cmdProc)
        {
            using (SqlConnection cn = new SqlConnection(strConn))
            {
                try
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("pGetParametersByProcName", cn))
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@p_ProcName", SqlDbType.VarChar, 200));
                        cmd.Parameters["@p_ProcName"].Value = cmdProc.CommandText;
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            SqlParameter sqlParameter = new SqlParameter(dr["r_ParameterName"].ToString(), DBNull.Value);
                            SqlDbType sqlDbType = SqlDbType.VarChar;
                            if (Enum.TryParse<SqlDbType>(dr["r_ParameterType"].ToString(), true, out sqlDbType))
                            {
                                sqlParameter.SqlDbType = sqlDbType;
                                int size = 0;
                                if (int.TryParse(dr["r_MaxLength"].ToString(), out size))
                                {
                                    sqlParameter.Size = size;
                                }
                            }

                            if ((bool)dr["r_IsOutput"])
                            {
                                sqlParameter.Direction = ParameterDirection.Output;
                                int size = 0;
                                if (int.TryParse(dr["r_MaxLength"].ToString(), out size))
                                {
                                    sqlParameter.Size = size;
                                }
                            }
                            cmdProc.Parameters.Add(sqlParameter);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 创建SqlCommand对象
        /// </summary>
        /// <param name="commandText">存储过程名称</param>
        /// <returns>SqlCommand对象</returns>
        public SqlCommand CreateSqlCommand(string commandText)
        {
            return CreateSqlCommand(commandText, CommandType.StoredProcedure);
        }

        /// <summary>
        /// 创建SqlCommand对象
        /// </summary>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <param name="commandType">Command类型</param>
        /// <returns>SqlCommand对象</returns>
        public SqlCommand CreateSqlCommand(string commandText, CommandType commandType)
        {
            SqlCommand cmd = new SqlCommand(commandText);
            cmd.CommandType = commandType;
            return cmd;
        }

        /// <summary>
        /// 创建SqlCommand对象
        /// </summary>
        /// <param name="commandText">存储过程名称</param>
        /// <param name="strConn">连接字符串</param>
        /// <returns>SqlCommand对象</returns>
        public SqlCommand CreateSqlCommand(string commandText, string strConn)
        {
            SqlCommand cmd = new SqlCommand(commandText);
            cmd.CommandType = CommandType.StoredProcedure;
            CreateCmdParameters(strConn, cmd);
            return cmd;
        }

        /// <summary>
        /// 执行存储过程名称或T-SQL语句并返回数据集
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <param name="commandType">Command类型</param>
        /// <returns>执行结果对象--数据集</returns>
        public DataSet ExecuteDataSet(string connectionString, string commandText, CommandType commandType)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                return ExecuteDataSet(cn, commandText, commandType);
            }
        }

        /// <summary>
        /// 执行存储过程名称或T-SQL语句并返回数据集
        /// </summary>
        /// <param name="connection">连接对象</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <param name="commandType">Command类型</param>
        /// <returns>执行结果对象--数据集</returns>
        public DataSet ExecuteDataSet(SqlConnection connection, string commandText, CommandType commandType)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                SqlCommand cmd = new SqlCommand(commandText, connection);
                cmd.CommandType = commandType;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                cmd.Dispose();
                connection.Close();
                connection.Dispose();
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        /// <summary>
        ///  执行存储过程名称或T-SQL语句并返回数据集
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="command">SqlCommand对象</param>
        /// <returns>执行结果对象--数据集</returns>
        public DataSet ExecuteDataSet(string connectionString, SqlCommand command)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    cn.Open();
                    command.Connection = cn;

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();

                    da.Fill(ds);
                    command.Dispose();
                    cn.Close();
                    cn.Dispose();
                    return ds;
                }
            }
            catch (Exception er)
            {
                throw er;
            }
            return null;
        }
        /// <summary>
        /// 执行语句
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="command">command实例</param>
        /// <param name="includeReturnValueParameter">返回值</param>
        /// <param name="path">记录错误信息地址</param>
        /// <param name="region">自定义错误标识</param>
        /// <returns></returns>
        public Int32 ExecuteNonQuery(string connectionString, SqlCommand command, bool includeReturnValueParameter, string path = "", string region = "")
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {

                cn.Open();
                command.Connection = cn;
                command.CommandTimeout = 0; //chengzb20101115
                PrintSqlLog(command);//wangbq
                //增加了try catch
                int returnValue = 0;
                try
                {
                    returnValue = command.ExecuteNonQuery();

                }
                catch (SqlException sqlex)//wangbq 20110211
                {
                    throw sqlex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    command.Dispose();
                    cn.Close();
                    cn.Dispose();
                }
                if (includeReturnValueParameter)
                {
                    return int.Parse(command.Parameters[RETURN_VALUE_PARAMETER_NAME].Value.ToString());
                }
                else
                {
                    return returnValue;
                }


            }
        }
        /// <summary>
        /// 执行语句
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="command">command实例</param>
        /// <param name="path">记录错误信息地址</param>
        /// <param name="region">自定义错误标识</param>
        /// <returns></returns>
        public Int32 ExecuteNonQuery(string connectionString, SqlCommand command, string path = "", string region = "")
        {
            return ExecuteNonQuery(connectionString, command, false, path, region);
        }

        /// <summary>
        /// 执行查询；并返回查询的第一行，第一列；忽略其它结果
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <param name="commandType">Command类型</param>
        /// <returns>查询的第一行，第一列的结果</returns>
        public object ExecuteScalar(string connectionString, string commandText, CommandType commandType)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(commandText, cn))
            {
                cn.Open();
                cmd.CommandType = commandType;
                object obj = cmd.ExecuteScalar();
                cmd.Dispose();
                cn.Close();
                cn.Dispose();
                return obj;
            }
        }

        /// <summary>
        /// 执行查询；并返回查询的第一行，第一列；忽略其它结果
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="command">SqlCommand对象</param>
        /// <returns>查询的第一行，第一列的结果</returns>
        public object ExecuteScalar(String connectionString, SqlCommand command)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();

                command.Connection = cn;

                try
                {
                    object obj = command.ExecuteScalar();
                    command.Dispose();
                    cn.Close();
                    cn.Dispose();
                    return obj;
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    command.Dispose();
                    cn.Close();
                    cn.Dispose();
                }
            }
        }

        /// <summary>
        /// 执行查询；并返回只从头读到尾的SqlDataReader对象
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <param name="commandType">Command类型</param>
        /// <returns>SqlDataReader对象</returns>
        public SqlDataReader ExecuteReader(string connectionString, string commandText, CommandType commandType)
        {
            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(commandText, cn);
            try
            {
                cn.Open();
                cmd.CommandType = commandType;
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Dispose();
                //cn.Close();
                //cn.Dispose();
                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 执行查询；并返回只从头读到尾的SqlDataReader对象
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="command">SqlCommand对象</param>
        /// <returns>SqlDataReader对象</returns>
        public SqlDataReader ExecuteReader(String connectionString, SqlCommand command)
        {
            SqlConnection cn = new SqlConnection(connectionString);
            try
            {
                command.Connection = cn;
                cn.Open();
                SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
                command.Dispose();

                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 打印执行查询的Log日志
        /// </summary>
        /// <param name="command">SqlCommand对象</param>
        private void PrintSqlLog(SqlCommand command)
        {
            string strSqlText = command.CommandText;

            string strParam = string.Empty;

            try
            {
                for (int i = 0; i < command.Parameters.Count; i++)
                {
                    string strName = Convert.ToString(command.Parameters[i]).ToString();
                    string strValue = Convert.ToString(command.Parameters[i].Value).ToString();
                    if (strParam != string.Empty)
                    {
                        strParam = "," + strParam + strName + "=" + strValue;
                    }
                    else
                    {
                        strParam = strParam + strName + "=" + strValue;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 记录错误
        /// </summary>
        /// <param name="info">错误信息</param>
        /// <param name="path">记录文件地址</param>
        public static void ErrorLog(string info, string path)
        {
            StreamWriter sw = new StreamWriter(path, true);
            sw.WriteLine(info);
            sw.Close();
            sw.Dispose();
        }

    }
}
