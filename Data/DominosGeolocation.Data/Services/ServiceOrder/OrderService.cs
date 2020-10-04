using DominosGeolocation.Data.Models;
using DominosGeolocation.Logging;
using System.Linq;

namespace DominosGeolocation.Data.Services.ServiceOrder
{
    public class OrderService : IOrderService
    {
        #region Fields           
        private readonly IRepository<WorkOrder> _orderRepository;
        #endregion

        #region Ctor

        public OrderService(IRepository<WorkOrder> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        #endregion

        public int Insert(WorkOrder model)
        {
            try
            {
                _orderRepository.Insert(model);

            }
            catch (System.Exception ex)
            {
                Logger.DLog.Debug(ex.Message, ex);
            }
            return model.Id;
        }

        public WorkOrder GetWorkOrder()
        {
            var workOrder = new WorkOrder();
            try
            {
                workOrder = _orderRepository.Table.Where(a => a.IsActive && !a.IsMqSuccess).OrderBy(a => a.Id).FirstOrDefault();
            }
            catch (System.Exception ex)
            {
                Logger.DLog.Debug(ex.Message, ex);
            }
            return workOrder;
        }

        public WorkOrder GetById(int id)
        {
            var workOrder = new WorkOrder();
            try
            {
                workOrder = _orderRepository.GetById(id);
            }
            catch (System.Exception ex)
            {
                Logger.DLog.Debug(ex.Message, ex);
            }
            return workOrder;
        }

        public void Update(WorkOrder model)
        {
            try
            {
                _orderRepository.Update(model);
            }
            catch (System.Exception ex)
            {
                Logger.DLog.Debug(ex.Message, ex);
            }

        }
    }
}