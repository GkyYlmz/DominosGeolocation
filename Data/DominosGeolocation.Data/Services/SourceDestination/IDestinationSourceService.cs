using DominosGeolocation.Data.Models;
using System.Threading.Tasks;

namespace DominosGeolocation.Data.Services.SourceDestination
{
    public interface IDestinationSourceService
    {
        void Insert(DestinationSource model);
        int Update(DestinationSource model);
        void InsertAdo(DestinationSource model);
        Task<DestinationSource> AddAsync(DestinationSource dominosLocation);
        int GetDestinationSourceCountByWorkOrderId(int workOrderId);
    }
}
