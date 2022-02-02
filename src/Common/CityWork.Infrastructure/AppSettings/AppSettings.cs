﻿using System;
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
    }

    public class ConnectionStrings
    {
        public string Provider { get; set; }
        public string CityWorkConnectionString { get; set; }
    }

}

