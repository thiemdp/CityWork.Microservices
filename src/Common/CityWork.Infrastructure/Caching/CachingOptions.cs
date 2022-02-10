using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityWork.Infrastructure
{
    public class CachingOptions
    {
        public string Provider { get; set; }

        public InMemoryCacheOptions InMemory { get; set; }

        public RedisOptions Redis { get; set; }

        public SqlServerOptions SqlServer { get; set; }

        public bool UseInMemory => Provider == "InMemory";
        public bool UseRedis => Provider == "Redis";
        public bool UseSqlServer => Provider == "SqlServer";
    }

    public class InMemoryCacheOptions
    {
        public long? SizeLimit { get; set; }
    }
 
    public class RedisOptions
    {
        public string Configuration { get; set; }

        public string InstanceName { get; set; }
    }

    public class SqlServerOptions
    {
        public string ConnectionString { get; set; }

        public string SchemaName { get; set; }

        public string TableName { get; set; }
    }
}
