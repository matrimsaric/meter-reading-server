using System;
using System.Collections.Generic;
using System.Text;

namespace MeterLibrary.Security
{
    public static class DatabaseConnection
    {
        private static string connectionString = null;

        public static string GetConnection()
        {
            return connectionString;
        }
        public static void SetConnection(string connection)
        {
            if(connectionString == null)
            {
                // only permit this to be set once so cannot be overridden or changed
                connectionString = connection;
            }
        }
    }
}
