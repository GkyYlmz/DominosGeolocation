using DominosGeolocation.Data;
using DominosGeolocation.Data.Models;
using DominosGeolocation.Data.Services.ServiceOrder;
using DominosGeolocation.Data.Services.SourceDestination;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DominosGeolocation.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GeolocationController : ControllerBase
    {
        private readonly IRepository<WorkOrder> _repository;
        private readonly IOrderService _orderService;
        private readonly IDestinationSourceService _destinationsourceService;

        public GeolocationController(IRepository<WorkOrder> repository, IOrderService orderService, IDestinationSourceService destinationSourceService)
        {
            _repository = repository;
            _orderService = orderService;
            _destinationsourceService = destinationSourceService;
        }

        [HttpGet]
        public Guid CreateWorkOrder()
        {
            var workerId = _orderService.Insert(new WorkOrder
            {
                CreatedDate = DateTime.Now,
                IsActive = true,
                IsDbSuccess = false,
                IsMqSuccess = false
            });

            byte[] bytes = new byte[16];
            BitConverter.GetBytes(workerId).CopyTo(bytes, 0);

            return new Guid(bytes);
        }
    }
}
