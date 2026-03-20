using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Domain.Entities;
using VMS.Persistence.Repositories;

namespace VMS.Application.Models
{
    #region PO Received
    public class POReceived_Request : BaseEntity
    {
        public int? CustomerId { get; set; }
        public string? PONumber { get; set; }

        [DefaultValue(false)]
        public bool? IsPOReceived { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? POAmount { get; set; }
        public DateTime? ShipmentSchedule { get; set; }
        public string? POReceivedOriginalFileName { get; set; }
        public string? POReceivedFileName { get; set; }
        public string? POReceived_Base64 { get; set; }

        [DefaultValue(false)]
        public bool? IsPOStatusClosed { get; set; }
     
        public bool? IsActive { get; set; }
    }

    public class POReceivedSearch_Request : BaseSearchEntity
    {
        public int? CustomerId { get; set; }
    }

    public class POReceived_Response : BaseResponseEntity
    {
        public int? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public int? ParentCustomerId { get; set; }
        public string? ParentCustomerName { get; set; }
        public int? CountryId { get; set; }
        public string? CountryName { get; set; }
        public string? PONumber { get; set; }
        public bool? IsPOReceived { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? POAmount { get; set; }
        public DateTime? ShipmentSchedule { get; set; }
        public string? POReceivedOriginalFileName { get; set; }
        public string? POReceivedFileName { get; set; }
        public string? POReceivedURL { get; set; }
        public bool? IsPOStatusClosed { get; set; }
        public DateTime? POStatusClosedDateTime { get; set; }
        public bool? IsActive { get; set; }
    }
    #endregion
}
