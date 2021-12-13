using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Xml.Linq;
using System.Linq;
using System.IO;

namespace Infrastructure.ConfigurationProvider
{
    public class SQLConnectionStringProvider : IConnectionStringProvider
    {
        string _connectionString;

        public SQLConnectionStringProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string GetConnectionString()
        {
            return _connectionString;
        }     
    }

    public class AccessConnectionStringProvider : IConnectionStringProvider
    {
        string _connectionString;

        public AccessConnectionStringProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string GetConnectionString()
        {
            return _connectionString;
        }
    }

    public class ConnectionStringManager
    {
        public static SQLConnectionStringProvider SqlConnectionStringInstance { get; private set; }
        public static AccessConnectionStringProvider AccessConnectionStringInstance { get; private set; }

        public static void SetConnectionString(string connection)
        {
            SqlConnectionStringInstance = new SQLConnectionStringProvider(connection);
        }
        
        static ConnectionStringManager()
        {
            try
            {
                Dictionary<string, string> configurationItems = new Dictionary<string, string>();

                string configurationPath = string.Empty;

                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Configuration.xml"))
                    configurationPath = AppDomain.CurrentDomain.BaseDirectory + "\\Configuration.xml";
                else
                    configurationPath = AppDomain.CurrentDomain.BaseDirectory + "\\bin\\Configuration.xml";

                XElement xElement = XElement.Load(configurationPath);
                {
                    foreach (var item in xElement.Elements())
                    {
                        configurationItems.Add(item.Attribute("key").Value, item.Attribute("value").Value);
                    }
                }

                SqlConnectionStringInstance = new SQLConnectionStringProvider(configurationItems["SQLServerConnectionString"]);
            }
            catch (Exception)
            {
                 
            }
           
            //AccessConnectionStringInstance = new AccessConnectionStringProvider(configurationItems["MSAccessConnectionString"]);
        }
    } 
}
