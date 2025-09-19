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
    public class ManageVisitorCompanyModel
    {
    }

    public class VisitorCompany_Request : BaseEntity
    {
        public string? CompanyName { get; set; }
        public string? CompanyAddress { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? DistrictId { get; set; }
        public int? CityId { get; set; }
        public int? Pincode { get; set; }
        public string? CompanyPhone { get; set; }

        [DefaultValue(false)]
        public bool? IsGST { get; set; }
        public string? GSTNo { get; set; }
        public string? GSTOriginalFileName { get; set; }
        [JsonIgnore]
        public string? GSTFileName { get; set; }
        public string? GSTFile_Base64 { get; set; }

        [DefaultValue(false)]
        public bool? IsPan { get; set; }
        public string? PanCardNumber { get; set; }
        public string? PanCardOriginalFileName { get; set; }
        [JsonIgnore]
        public string? PanCardFileName { get; set; }
        public string? PanCardFile_Base64 { get; set; }
        public bool? IsActive { get; set; }
    }

    public class VisitorCompany_Response : BaseResponseEntity
    {
        public string? CompanyName { get; set; }
        public string? CompanyAddress { get; set; }
        public int? CountryId { get; set; }
        public string? CountryName { get; set; }
        public int? StateId { get; set; }
        public string? StateName { get; set; }
        public int? DistrictId { get; set; }
        public string? DistrictName { get; set; }
        public int? CityId { get; set; }
        public string? CityName { get; set; }
        public int? Pincode { get; set; }
        public string? CompanyPhone { get; set; }
        public bool? IsGST { get; set; }
        public string? GSTNo { get; set; }
        public string? GSTOriginalFileName { get; set; }
        public string? GSTFileName { get; set; }
        public string? GSTURL { get; set; }

        public bool? IsPan { get; set; }
        public string? PanCardNumber { get; set; }
        public string? PanCardOriginalFileName { get; set; }
        [JsonIgnore]
        public string? PanCardFileName { get; set; }
        public string? PanCardURL { get; set; }
        public bool? IsActive { get; set; }
    }
    public class VisitorCompany_ImportData
    {
        public string? CompanyName { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? Province { get; set; }
        public string? Pincode { get; set; }
        public string? CompanyPhone { get; set; }
        public string? IsGST { get; set; }
        public string? GSTNumber { get; set; }
        public string? IsPAN { get; set; }
        public string? PanNumber { get; set; }
        public string? IsActive { get; set; }
    }

    public class VisitorCompany_ImportDataValidation
    {
        public string? CompanyName { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? Province { get; set; }
        public string? Pincode { get; set; }
        public string? CompanyPhone { get; set; }
        public string? IsGST { get; set; }
        public string? GSTNumber { get; set; }
        public string? IsPAN { get; set; }
        public string? PanNumber { get; set; }
        public string? IsActive { get; set; }
        public string ValidationMessage { get; set; }
    }
}
