using System;
using System.Collections.Generic;

namespace DominosGeolocation.Data.Models
{
    public partial class WorkOrder : BaseEntity
    {
        public int Id { get; set; }
        public DateTime? MqStartDate { get; set; }
        public DateTime? MqEndDate { get; set; }
        public DateTime? DbStartDate { get; set; }
        public DateTime? DbEndDate { get; set; }
        public bool IsMqSuccess { get; set; }
        public bool IsDbSuccess { get; set; }
        public string FilePath { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
