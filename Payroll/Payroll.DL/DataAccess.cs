
using System;
using System.Data;
using System.Data.SqlClient;

namespace Payroll.DL
{
    public static class DataAccess
    {
        public static string ConnectionString { get; set; }

        private static SqlConnection connection;
        //added this
        public static string XMLPath { get; set; }


        private static void Connect()
        {
            try
            {
                if (ConnectionString == null)
                {
                    throw new Exception("Connection string was not set!");
                }

                if (connection == null || connection.State == ConnectionState.Closed)
                {
                    connection = new SqlConnection(ConnectionString);

                    connection.Open();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void CloseConnection()
        {
            if (connection != null)
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Runs a SQL Select query.
        /// </summary>
        /// <param name="sql">The command to execute.</param>
        /// <returns>Returns a DataTable filled with the result of the Select statement.</returns>

        public static DataTable Select(string sql, SqlParameter[] sqlParams = null, bool closeConnection = true)
        {
            try
            {
                Connect();

                SqlCommand command = new SqlCommand(sql, connection);

                if (sqlParams != null)
                {
                    command.Parameters.AddRange(sqlParams);
                }

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable result = new DataTable();

                adapter.Fill(result);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (closeConnection) CloseConnection();
            }
        }

        /// <summary>
        /// Runs a SQL Update, Insert, or Delete query command.
        /// </summary>
        /// <param name="sql">The command to execute.</param>
        /// <returns>Returns the number of rows affected.</returns>
        public static int RunSql(string sql, SqlParameter[] sqlParams = null, bool closeConnection = true)
        {
            try
            {
                Connect();

                SqlCommand command = new SqlCommand(sql, connection);

                if (sqlParams != null)
                {
                    command.Parameters.AddRange(sqlParams);
                }

                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (closeConnection) CloseConnection();
            }
        }
    }
}
