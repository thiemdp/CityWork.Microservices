namespace CityWork.Infrastructure
{
    public class LoggerOptions
    {
        public FileOptions File { get; set; }

        public ElasticsearchOptions Elasticsearch { get; set; }
    }
}
