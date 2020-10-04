using System;
using System.Collections.Generic;

namespace DominosGeolocation.Data.Models
{
    public partial class DestinationSource: BaseEntity
    {
        public int Id { get; set; }
        public int WorkOrderId { get; set; }
        public string SourceLatitude { get; set; }
        public string SourceLongitude { get; set; }
        public string DestinationLatitude { get; set; }
        public string DestinationLongitude { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
