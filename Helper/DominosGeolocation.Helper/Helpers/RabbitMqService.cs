using DominosGeolocation.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DominosGeolocation.Helper.Helpers
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly IRabbitMqConfiguration _rabbitMQConfiguration;

        public RabbitMqService(IRabbitMqConfiguration rabbitMQConfiguration)
        {
            _rabbitMQConfiguration = rabbitMQConfiguration ?? throw new ArgumentNullException(nameof(rabbitMQConfiguration));
        }
        public IConnection GetConnection()
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = _rabbitMQConfiguration.HostName,
                    UserName = _rabbitMQConfiguration.UserName,
                    Password = _rabbitMQConfiguration.Password,
                    Port = Convert.ToInt32(_rabbitMQConfiguration.Port)
                };

                
                factory.AutomaticRecoveryEnabled = true;
                factory.DispatchConsumersAsync = true;
                
                factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(10);
                
                factory.TopologyRecoveryEnabled = false;

                return factory.CreateConnection();
            }
            catch (BrokerUnreachableException ex)
            {
                Logger.DLog.Debug(ex.Message, ex);
                Thread.Sleep(5000);
                
                return GetConnection();
            }
            catch (Exception ec)
            {
                Logger.DLog.Debug(ec.Message, ec);
                return GetConnection();
            }
        }

        public IModel GetModel(IConnection connection)
        {
            return connection.CreateModel();
        }

        public void AddQueue<T>(IConnection connection, IModel channel, List<T> datas, string queueName)
        {
            try
            {
                using (channel)
                {
                    channel.QueueDeclare(queue: queueName,
                                         durable: true,      
                                         exclusive: false,   
                                         autoDelete: false,  
                                         arguments: null);  

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;
                    properties.Expiration = (1000 * 60 * 60 * 2).ToString();

                    datas.ForEach(data =>
                    {

                        var body = Encoding.UTF8.GetBytes(data.ToJson());
                        channel.BasicPublish(exchange: "",
                                             routingKey: queueName,
                                             mandatory: false,
                                             basicProperties: properties,
                                             body: body);
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.DLog.Debug(ex.Message, ex);
            }
        }
    }
}
