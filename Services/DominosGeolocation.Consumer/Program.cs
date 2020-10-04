using DominosGeolocation.Data;
using DominosGeolocation.Data.Models;
using DominosGeolocation.Data.Services.ServiceOrder;
using DominosGeolocation.Data.Services.SourceDestination;
using DominosGeolocation.Helper.Helpers;
using DominosGeolocation.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DominosGeolocation.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");

            var configuration = builder.Build();

            var serviceProvider = new ServiceCollection()
              .AddSingleton<IConfiguration>(configuration)
                 .AddDbContext<GeolocationContext>(opt => opt.UseSqlServer(configuration["ConnectionStrings"]))
                .AddScoped(typeof(IRepository<>), typeof(GeneralRepository<>))
                .AddScoped<IDestinationSourceService, DestinationSourceService>()
               .AddScoped<IOrderService, OrderService>()
               .AddScoped<IRabbitMqConfiguration, RabbitMqConfiguration>()
               .AddScoped<IRabbitMqService, RabbitMqService>()
               .BuildServiceProvider();

            var queue = "LocationQueue";
            var rabbitMq = serviceProvider.GetService<IRabbitMqService>();
            var destinationSourceService = serviceProvider.GetService<IDestinationSourceService>();
            var orderService = serviceProvider.GetService<IOrderService>();

            var connection = rabbitMq.GetConnection();
            var channel = rabbitMq.GetModel(connection);

            try
            {
              
                channel.BasicQos(0, 1000, false);

                var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.Received += (sender, ea) => DoOperation(sender, ea, channel, destinationSourceService, orderService);
                channel.BasicConsume(queue, false, consumer);
            }
            catch (Exception ex)
            {
                Logger.DLog.Debug(ex.Message, ex);
            }

            Console.ReadLine();
        }

        public static async Task DoOperation(object sender, BasicDeliverEventArgs ea, IModel channel, IDestinationSourceService destinationSourceService, IOrderService orderService)
        {
            var message = Encoding.UTF8.GetString(ea.Body.ToArray());

            try
            {
                var deserializedDestinationSource = JsonConvert.DeserializeObject<DestinationSource>(message);

                await destinationSourceService.AddAsync(deserializedDestinationSource);

                var task = FileWritingOperation(deserializedDestinationSource);
                task.Wait();
                var result = task.Result;

                var dataCount = destinationSourceService.GetDestinationSourceCountByWorkOrderId(deserializedDestinationSource.WorkOrderId);
                if (dataCount == 1)
                {
                    var workOrder = orderService.GetById(deserializedDestinationSource.WorkOrderId);
                    workOrder.DbStartDate = DateTime.Now;
                    orderService.Update(workOrder);
                }
                else if (dataCount == 30000000)
                {
                    var manager = new Manager();
                    var fileRootAndName = manager.GetFileOutputOperation();
                    var workOrder = orderService.GetById(deserializedDestinationSource.WorkOrderId);
                    workOrder.DbStartDate = DateTime.Now;
                    workOrder.DbEndDate = DateTime.Now;
                    workOrder.IsDbSuccess = true;
                    workOrder.FilePath = fileRootAndName;
                    orderService.Update(workOrder);
                }
            }
            catch (Exception ex)
            {
                Logger.DLog.Debug(ex.Message, ex);
            }

            channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
        }

        public static async Task<bool> FileWritingOperation(DestinationSource locationWriteDataQueue)
        {
          
            var manager = new Manager();
            var fileRootAndName = manager.GetFileOutputOperation();
            manager.ProcessWrite(locationWriteDataQueue, fileRootAndName).Wait();
            var data = await Task.FromResult(true);
            return data;
        }
    }
}
