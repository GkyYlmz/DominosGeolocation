using RabbitMQ.Client;
using System.Collections.Generic;

namespace DominosGeolocation.Helper.Helpers
{
    public interface IRabbitMqService
    {
        IConnection GetConnection();

        IModel GetModel(IConnection connection);

        void AddQueue<T>(IConnection connection, IModel channel, List<T> queueDataModels, string queueName);
    }
}
