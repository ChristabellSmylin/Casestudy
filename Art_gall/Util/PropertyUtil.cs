using System;
using Microsoft.Data.SqlClient;
using System.IO;
using Newtonsoft.Json;


namespace Art_gall.Util
{
    public  class PropertyUtil
    {
        private static SqlConnection connection;

        public static string GetPropertyString()
        {
            string server = "DESKTOP-U9L54I2";
            string dbname = "VirtualArt";
            string username = "smylin";
            string password = "christabell123$";
            string trustedconnection = "true";


            // This is where you construct your connection string
            string connectionString = $"Server={server};Database={dbname};User Id={username};Password={password}; TrustServerCertificate ={trustedconnection}";
            //Console.WriteLine($"Connection Strings:{ connectionString}");
            return connectionString;
        }




    }
}
