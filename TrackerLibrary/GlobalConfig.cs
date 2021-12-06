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
                //TODO - connect to the database
                Connections = new SqlConnector();
            }
            else if (db == DatabaseType.text)
            {
                //TODO - create text connection
                Connections = new TextConnector();
            }
        }
        public static string CnnString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
