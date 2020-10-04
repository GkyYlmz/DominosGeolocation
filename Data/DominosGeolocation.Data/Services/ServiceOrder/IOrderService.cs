using DominosGeolocation.Data.Models;

namespace DominosGeolocation.Data.Services.ServiceOrder
{
    public interface IOrderService
    {
        int Insert(WorkOrder model);
        void Update(WorkOrder model);
        WorkOrder GetWorkOrder();

        WorkOrder GetById(int id);
    }
}
