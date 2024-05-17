using Microsoft.Data.SqlClient;
using System;
using System.IO;

namespace Art_gall.Util
{
    public static class DBPropertyUtil
    {
        public static class DBConnection
        {

            private static SqlConnection connection;

            public static SqlConnection GetConnection()
            {
                // Get the connection string
                string connectionString = PropertyUtil.GetPropertyString();

                // Create a SqlConnection object
                SqlConnection connection = new SqlConnection(connectionString);

                return connection;
            }
         
        }
    }
 }
