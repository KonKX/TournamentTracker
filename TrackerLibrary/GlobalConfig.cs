using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TrackerLibrary
{
    public static class GlobalConfig
    {
        public static IDataConnection? Connections { get; private set; } 
        public static void InitializeConnections(DatabaseType db) 
        {
            if (db == DatabaseType.sql)
            {
                Connections = new SqlConnector();
            }
            else if (db == DatabaseType.text)
            {
                Connections = new TextConnector();
            }
        }
        public static string CnnString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
