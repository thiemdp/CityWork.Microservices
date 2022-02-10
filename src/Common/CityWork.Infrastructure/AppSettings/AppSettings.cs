using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityWork.Infrastructure
{
    public class AppSettings
    {
        public ConnectionStrings ConnectionStrings { get; set; }

        public LoggingOptions Logging { get; set; }

        public CachingOptions Caching { get; set; }

        public MessageBrokerOptions MessageBroker { get; set; }
    }

    public class ConnectionStrings
    {
        public string Provider { get; set; }
        public string CityWorkConnectionString { get; set; }
        public bool UseMSSQL => this.Provider == "MSSQL";
        public bool UseSQLite => this.Provider == "SQLite";
        public bool UseMySQL => this.Provider == "MySQL";
        public bool UsePostgreSQL => this.Provider == "PostgreSQL";

    }

}

