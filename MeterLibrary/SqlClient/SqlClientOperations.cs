using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

namespace MeterLibrary.SqlClient
{
    internal class SqlClientOperations
    {
        internal void ExecuteCommand(SqlCommand incoming)
        {
            using (SqlConnection conn = new SqlConnection(Security.DatabaseConnection.GetConnection()))
            {
                incoming.Connection = conn;
                conn.Open();

                incoming.ExecuteNonQuery();
            }
        }

        internal DataSet ExecuteGetCommand(SqlCommand incoming)
        {
            using (SqlConnection conn = new SqlConnection(Security.DatabaseConnection.GetConnection()))
            {
                incoming.Connection = conn;
                conn.Open();

                SqlDataAdapter adp = new SqlDataAdapter(incoming);
                DataSet ds = new DataSet();
                adp.Fill(ds);

                return ds;
            }
        }

        internal DataTable GetData(string sql, List<SqlParameter> sqlParams = null)
        {
            DataTable response = new DataTable();
            using (SqlConnection conn = new SqlConnection(Security.DatabaseConnection.GetConnection()))
            {

                SqlCommand oCmd = new SqlCommand(sql, conn);
                if(sqlParams != null)
                {
                    foreach (SqlParameter param in sqlParams)
                    {
                        oCmd.Parameters.Add(param);
                    }

                }
                conn.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(oCmd))
                {
                    sda.Fill(response);
                }
                conn.Close();

            }
            return response;
        }
    }
}
