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
    public class Supplier_Request : BaseEntity
    {
        public string? SupplierCode { get; set; }
        public string? SupplierName { get; set; }
        public string? LandlineNumber { get; set; }
        public string? MobileNumber { get; set; }
        public string? EmailId { get; set; }
        public string? SpecialRemarks { get; set; }

        public string? PanCardNumber { get; set; }

        [DefaultValue("")]
        public string? PanCardOriginalFileName { get; set; }

        [DefaultValue("")]
        public string? PanCardFileName { get; set; }

        [DefaultValue("")]
        public string? PanCard_Base64 { get; set; }

        public string? GSTNumber { get; set; }

        [DefaultValue("")]
        public string? GSTOriginalFileName { get; set; }

        [DefaultValue("")]
        public string? GSTFileName { get; set; }

        [DefaultValue("")]
        public string? GST_Base64 { get; set; }

        public string? ContactName { get; set; }
        public string? ContactMobileNumber { get; set; }
        public string? ContactEmailId { get; set; }
        public string? AddressLine1 { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? DistrictId { get; set; }
        public string? PinCode { get; set; }
        public bool? IsActive { get; set; }
    }

    public class Supplier_Search : BaseSearchEntity
    {
    }

    public class Supplier_Response : BaseResponseEntity
    {
        public string? SupplierCode { get; set; }
        public string? SupplierName { get; set; }
        public string? LandlineNumber { get; set; }
        public string? MobileNumber { get; set; }
        public string? EmailId { get; set; }
        public string? SpecialRemarks { get; set; }

        public string? PanCardNumber { get; set; }

        [DefaultValue("")]
        public string? PanCardOriginalFileName { get; set; }

        [DefaultValue("")]
        public string? PanCardFileName { get; set; }

        [DefaultValue("")]
        public string? PanCardUrl { get; set; }

        public string? GSTNumber { get; set; }

        [DefaultValue("")]
        public string? GSTOriginalFileName { get; set; }

        [DefaultValue("")]
        public string? GSTFileName { get; set; }

        [DefaultValue("")]
        public string? GSTUrl { get; set; }

        public string? ContactName { get; set; }
        public string? ContactMobileNumber { get; set; }
        public string? ContactEmailId { get; set; }
        public string? AddressLine1 { get; set; }
        public int? CountryId { get; set; }
        public string? CountryName { get; set; }
        public int? StateId { get; set; }
        public string? StateName { get; set; }
        public int? DistrictId { get; set; }
        public string? DistrictName { get; set; }
        public string? PinCode { get; set; }
        public bool? IsActive { get; set; }
    }
}
