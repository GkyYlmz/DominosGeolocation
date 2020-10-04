using DominosGeolocation.Data.Models;
using DominosGeolocation.Logging;
using Microsoft.Data.SqlClient;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DominosGeolocation.Data.Services.SourceDestination
{
    public class DestinationSourceService : IDestinationSourceService
    {
        #region Fields           
        private readonly IRepository<DestinationSource> _destinationServiceRepository;
        #endregion

        #region Ctor

        public DestinationSourceService(IRepository<DestinationSource> destinationServiceRepository)
        {
            _destinationServiceRepository = destinationServiceRepository;
        }

        #endregion


        public async Task<DestinationSource> AddAsync(DestinationSource dominosLocation)
        {
            var result = await _destinationServiceRepository.AddAsync(dominosLocation);
            return result;
        }

        public void Insert(DestinationSource model)
        {
            try
            {
                _destinationServiceRepository.Insert(model);
            }
            catch (Exception ex)
            {
                Logger.DLog.Debug(ex.Message, ex);
            }
        }

        public int Update(DestinationSource model)
        {
            throw new NotImplementedException();
        }

        public void InsertAdo(DestinationSource model)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection("Server =.; Database = Geolocation; Trusted_Connection = True;"))
                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandText = $"INSERT INTO [Geolocation].[dbo].[DestinationSource] (WorkOrderId, Source_latitude, Source_longitude,Destination_latitude,Destination_longitude,CreatedDate,IsActive) VALUES({model.WorkOrderId},'{model.SourceLatitude}','{model.SourceLongitude}','{model.DestinationLatitude}','{model.DestinationLongitude}','{DateTime.Now}','{true}')";
                    command.Connection = connection;
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Logger.DLog.Debug(ex.Message, ex);
            }

        }

        public int GetDestinationSourceCountByWorkOrderId(int workOrderId)
        {
           return _destinationServiceRepository.Table.Where(a => a.WorkOrderId == workOrderId && a.IsActive == true).Count();
        }

    }
}
