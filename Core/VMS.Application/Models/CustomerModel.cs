using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using VMS.Domain.Entities;
using VMS.Persistence.Repositories;

namespace VMS.Application.Models
{
    public class Customer_Request : BaseEntity
    {
        public Customer_Request()
        {
            ContactDetailList = new List<ContactDetail_Request>();
            BillingAddressList = new List<Address_Request>();
            ShippingAddressList = new List<ShippingAddress_Request>();
        }

        public string? CustomerName { get; set; }
        public string? MobileNo { get; set; }
        //public string? MobileNo2 { get; set; }
        public int? ParentCustomerId { get; set; }
        public int? CustomerTypeId { get; set; }
        public int? CountryId { get; set; }
        public bool? IsActive { get; set; }

        //[JsonIgnore]
        //public string? AutoPassword { get; set; }

        public List<ContactDetail_Request>? ContactDetailList { get; set; }
        public List<Address_Request>? BillingAddressList { get; set; }
        public List<ShippingAddress_Request>? ShippingAddressList { get; set; }
    }

    public class Customer_Response : BaseResponseEntity
    {
        public string? CustomerName { get; set; }
        public string? MobileNo { get; set; }
        public int? ParentCustomerId { get; set; }
        public string? ParentCustomer { get; set; }
        public int? CustomerTypeId { get; set; }
        public string? CustomerType { get; set; }
        public int? CountryId { get; set; }
        public string? CountryName { get; set; }
        public bool? IsActive { get; set; }
    }

    public class CustomerSearch_Request : BaseSearchEntity
    {
        [DefaultValue(0)]
        public int CustomerId { get; set; }

        [DefaultValue(0)]
        public int ParentCustomerId { get; set; }
    }

    public class Search_Request : BaseSearchEntity
    {
        [DefaultValue(0)]
        public int RefId { get; set; }

        [DefaultValue("Customer")]
        public string? RefType { get; set; }
    }

    public class AutoGenPassword_Response
    {
        public string? AutoPassword { get; set; }
    }
}
