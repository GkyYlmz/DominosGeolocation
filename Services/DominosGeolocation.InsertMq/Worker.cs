using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DominosGeolocation.Data;
using DominosGeolocation.Data.Models;
using DominosGeolocation.Data.Services.ServiceOrder;
using DominosGeolocation.Data.Services.SourceDestination;
using DominosGeolocation.Helper.Helpers;
using DominosGeolocation.Helper.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DominosGeolocation.InsertMq
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        public IOrderService _orderService;
        public IRabbitMqService _rabbitMqService;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
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


            _orderService = serviceProvider.GetService<IOrderService>();
            _rabbitMqService = serviceProvider.GetService<IRabbitMqService>();
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var workOrder = _orderService.GetWorkOrder();
                if (workOrder != null && workOrder.Id > 0)
                {
                    workOrder.MqStartDate = DateTime.Now;
                    _orderService.Update(workOrder);

                    var geolocationList = Calculate();

                    var connection = _rabbitMqService.GetConnection();
                    var channel = _rabbitMqService.GetModel(connection);

                    var list = new List<DestinationSource>();
                    for (int i = 0; i < geolocationList.Length - 1; i++)
                    {
                        list.Add(new DestinationSource
                        {
                            IsActive = true,
                            CreatedDate = DateTime.Now,
                            WorkOrderId = workOrder.Id,
                            DestinationLatitude = geolocationList[i].Latitude.ToString(),
                            DestinationLongitude = geolocationList[i].Longitude.ToString(),
                            SourceLatitude = geolocationList[i + 1].Latitude.ToString(),
                            SourceLongitude = geolocationList[i + 1].Longitude.ToString()
                        });
                    }

                    _rabbitMqService.AddQueue(connection, channel, list, "LocationQueue");

                    workOrder.MqEndDate = DateTime.Now;
                    workOrder.IsMqSuccess = true;

                    _orderService.Update(workOrder);
                }

                await Task.Delay(1000, stoppingToken);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }

        private Coordinate[] Calculate()
        {
            Coordinate location1 = new Coordinate { Latitude = 36.884804, Longitude = 30.704044 };
            Coordinate location2 = new Coordinate { Latitude = 37.000000, Longitude = 35.321335 };
            Coordinate location3 = new Coordinate { Latitude = 37.158333, Longitude = 38.791668 };
            Coordinate location4 = new Coordinate { Latitude = 40.766666, Longitude = 29.916668 };


            var allCoords = new List<Coordinate>();
            allCoords.Add(location1);
            allCoords.Add(location2);
            allCoords.Add(location3);
            allCoords.Add(location4);


            double minLat = allCoords.Min(x => x.Latitude);
            double minLon = allCoords.Min(x => x.Longitude);
            double maxLat = allCoords.Max(x => x.Latitude);
            double maxLon = allCoords.Max(x => x.Longitude);

            Random r = new Random();

            Coordinate[] result = new Coordinate[30000001];
            for (int i = 0; i < result.Length; i++)
            {
                Coordinate point = new Coordinate();
                do
                {
                    point.Latitude = r.NextDouble() * (maxLat - minLat) + minLat;
                    point.Longitude = r.NextDouble() * (maxLon - minLon) + minLon;
                }
                while (!IsPointInPolygon(point, allCoords));
                result[i] = point;
            }
            return result;
        }

        private bool IsPointInPolygon(Coordinate point, List<Coordinate> polygon)
        {
            int polygonLength = polygon.Count, i = 0;
            bool inside = false;

            double pointX = point.Longitude, pointY = point.Latitude;
            double startX, startY, endX, endY;
            Coordinate endPoint = polygon[polygonLength - 1];
            endX = endPoint.Longitude;
            endY = endPoint.Latitude;
            while (i < polygonLength)
            {
                startX = endX;
                startY = endY;
                endPoint = polygon[i++];
                endX = endPoint.Longitude;
                endY = endPoint.Latitude;

                inside ^= ((endY > pointY) ^ (startY > pointY)) && (pointX - endX < (pointY - endY) * (startX - endX) / (startY - endY));
            }
            return inside;
        }

    }
}
