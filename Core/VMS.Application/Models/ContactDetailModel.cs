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
    public class ContactDetailModel
    {
    }

    public class ContactDetail_Search : BaseSearchEntity
    {
        public long RefId { get; set; }

        [DefaultValue("Supplier")]
        public string? RefType { get; set; }
    }

    public class ContactDetail_Request : BaseEntity
    {
        public int? RefId { get; set; }

        [DefaultValue("Supplier")]
        public string? RefType { get; set; }
        public string? ContactName { get; set; }
        public string? MobileNumber { get; set; }
        public string? EmailId { get; set; }
        public string? AadharCardImageFileName { get; set; }
        public string? AadharCardOriginalFileName { get; set; }
        public string? AadharCardImage_Base64 { get; set; }
        public bool? IsDefault { get; set; }
        public bool? IsActive { get; set; }
    }

    public class ContactDetail_Response : BaseResponseEntity
    {
        public int? RefId { get; set; }
        public string? RefType { get; set; }
        public string? ContactName { get; set; }
        public string? MobileNumber { get; set; }
        public string? EmailId { get; set; }
        public string? AadharCardImageFileName { get; set; }
        public string? AadharCardOriginalFileName { get; set; }
        public string? AadharCardImageURL { get; set; }
        public bool? IsDefault { get; set; }
        public bool? IsActive { get; set; }
    }
}
