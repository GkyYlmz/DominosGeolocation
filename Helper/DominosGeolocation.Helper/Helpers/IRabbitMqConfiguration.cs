using Microsoft.Extensions.Configuration;

namespace DominosGeolocation.Helper.Helpers
{
    public class RabbitMqConfiguration : IRabbitMqConfiguration
    {
        public IConfiguration Configuration { get; }

        public RabbitMqConfiguration(IConfiguration configuration) => Configuration = configuration;

        public string HostName => Configuration.GetSection("RabbitMqOptions:HostName").Value;
        public string UserName => Configuration.GetSection("RabbitMqOptions:UserName").Value;
        public string Password => Configuration.GetSection("RabbitMqOptions:Password").Value;
        public string Port => Configuration.GetSection("RabbitMqOptions:Port").Value;
        public string VirtualHost => Configuration.GetSection("RabbitMqOptions:VirtualHost").Value;
    }
    public interface IRabbitMqConfiguration
    {
        string HostName { get; }
        string UserName { get; }
        string Password { get; }
        string Port { get; }
        string VirtualHost { get; }
    }
}
