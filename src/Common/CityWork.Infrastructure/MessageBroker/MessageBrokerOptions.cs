using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityWork.Infrastructure
{
    public class MessageBrokerOptions
    {
        public string Provider { get; set; }
        public RabbitMQOptions RabbitMQ { get; set; }
        public KafkaOptions Kafka { get; set; }
        public bool UsedMessageBroker()
        {
            return !string.IsNullOrEmpty(Provider);
        }
        public bool UsedRabbitMQ => Provider == "RabbitMQ";

        public bool UsedKafka => Provider == "Kafka";
         
    }
     
    public class RabbitMQOptions
    {
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ExchangeName { get; set; }

        public Dictionary<string, string> RoutingKeys { get; set; }

        public Dictionary<string, string> QueueNames { get; set; }

        public string ConnectionString
        {
            get
            {
                return $"amqp://{UserName}:{Password}@{HostName}/%2f";
            }
        }
    }
    public class KafkaOptions
    {
        public string BootstrapServers { get; set; }

        public string GroupId { get; set; }

        public Dictionary<string, string> Topics { get; set; }
    }
}
