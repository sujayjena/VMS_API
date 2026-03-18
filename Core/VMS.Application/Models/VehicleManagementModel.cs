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
    #region Vehicle Management
    public class VehicleManagement_Request : BaseEntity
    {
        public VehicleManagement_Request()
        {
            ItemList = new List<VehicleManagementItem_Request>();
            GateNumberList = new List<VehicleManagementGateNo_Request>();
        }
        public DateTime? ReceivingDate { get; set; }
        public string? PONumber { get; set; }
        public int? SupplierId { get; set; }
        public int? ItemId { get; set; }
        public string? TruckNumber { get; set; }
        public string? DriverName { get; set; }
        public string? DriverMobileNo { get; set; }
        public string? DriverLicenceNumber { get; set; }
        public DateTime? LicenceValidFrom { get; set; }
        public DateTime? LicenceValidTo { get; set; }

        [DefaultValue("")]
        public string? AttachmentOriginalFileName { get; set; }
        public string? AttachmentFileName { get; set; }

        [DefaultValue("")]
        public string? Attachment_Base64 { get; set; }
        public int? DocumentTypeId { get; set; }
        public string? DocumentNumber { get; set; }

        [DefaultValue("")]
        public string? DocumentOriginalFileName { get; set; }
        public string? DocumentFileName { get; set; }

        [DefaultValue("")]
        public string? Document_Base64 { get; set; }
        public bool? IsActive { get; set; }

        public List<VehicleManagementItem_Request> ItemList { get; set; }
        public List<VehicleManagementGateNo_Request> GateNumberList { get; set; }
    }

    public class VehicleManagement_Search : BaseSearchEntity
    {
        [DefaultValue(null)]
        public DateTime? FromDate { get; set; }

        [DefaultValue(null)]
        public DateTime? ToDate { get; set; }
    }

    public class VehicleManagement_Response : BaseResponseEntity
    {
        public DateTime? ReceivingDate { get; set; }
        public string? PONumber { get; set; }
        public int? SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public int? ItemId { get; set; }
        public string? ItemName { get; set; }
        public string? TruckNumber { get; set; }
        public string? DriverName { get; set; }
        public string? DriverMobileNo { get; set; }
        public string? DriverLicenceNumber { get; set; }
        public DateTime? LicenceValidFrom { get; set; }
        public DateTime? LicenceValidTo { get; set; }
        public string? AttachmentOriginalFileName { get; set; }
        public string? AttachmentFileName { get; set; }
        public string? AttachmentUrl { get; set; }
        public int? DocumentTypeId { get; set; }
        public string? DocumentType { get; set; }
        public string? DocumentNumber { get; set; }
        public string? DocumentOriginalFileName { get; set; }
        public string? DocumentFileName { get; set; }
        public string? DocumentUrl { get; set; }
        public bool? IsActive { get; set; }

        public List<VehicleManagementItem_Response> ItemList { get; set; }
        public List<VehicleManagementGateNo_Response> GateNumberList { get; set; }
    }

    public class VehicleManagementItem_Request
    {
        public int Id { get; set; }

        [JsonIgnore]
        public string? Action { get; set; }

        [JsonIgnore]
        public int? VehicleManagementId { get; set; }
        public int? Itemid { get; set; }
    }

    public class VehicleManagementItem_Response
    {
        public int Id { get; set; }
        public int? VehicleManagementId { get; set; }
        public int? Itemid { get; set; }
        public string? ItemName { get; set; }
    }

    public class VehicleManagementGateNo_Request
    {
        public int Id { get; set; }

        [JsonIgnore]
        public string? Action { get; set; }

        [JsonIgnore]
        public int? VehicleManagementId { get; set; }
        public int? GateDetailsId { get; set; }
    }

    public class VehicleManagementGateNo_Response
    {
        public int Id { get; set; }
        public int? VehicleManagementId { get; set; }
        public int? GateDetailsId { get; set; }
        public string? GateNumber { get; set; }
    }
    #endregion

    #region Material Weight
    public class MaterialWeight_Request : BaseEntity
    {
        public int? VehicleManagementId { get; set; }
        public decimal? PartyWeight { get; set; }
        public decimal? FirstWeight { get; set; }
        public decimal? SecondWeight { get; set; }
        public decimal? NetWeight { get; set; }
        public decimal? DeductionWeight { get; set; }
        public bool? IsActive { get; set; }
    }

    public class MaterialWeight_Search : BaseSearchEntity
    {
    }

    public class MaterialWeight_Response : BaseResponseEntity
    {
        public int? VehicleManagementId { get; set; }
        public string? PONumber { get; set; }
        public int? ItemId { get; set; }
        public string? ItemName { get; set; }
        public string? TruckNumber { get; set; }
        public string? DriverName { get; set; }
        public decimal? PartyWeight { get; set; }
        public decimal? FirstWeight { get; set; }
        public decimal? SecondWeight { get; set; }
        public decimal? NetWeight { get; set; }
        public decimal? DeductionWeight { get; set; }
        public bool? IsActive { get; set; }
    }
    #endregion
}
