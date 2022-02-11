using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityWork.Infrastructure
{
    public class AppSettings
    {
        public ConnectionStrings ConnectionStrings { get; set; } = default!;

        public LoggingOptions Logging { get; set; } = default!;

        public CachingOptions Caching { get; set; } = default!;

        public MessageBrokerOptions MessageBroker { get; set; } = default!;

        public MonitoringOptions Monitoring { get; set; } = default!;
    }

    public class ConnectionStrings
    {
        public string Provider { get; set; } = default!;
        public string CityWorkConnectionString { get; set; } = default!;
        public bool UseMSSQL => this.Provider == "MSSQL";
        public bool UseSQLite => this.Provider == "SQLite";
        public bool UseMySQL => this.Provider == "MySQL";
        public bool UsePostgreSQL => this.Provider == "PostgreSQL";

    }

}

