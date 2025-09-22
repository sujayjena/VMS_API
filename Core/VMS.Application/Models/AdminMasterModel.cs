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
    public class AdminMasterModel
    {
    }

    #region Gender

    public class Gender_Request : BaseEntity
    {
        [DefaultValue("")]
        public string? GenderName { get; set; }

        public bool? IsActive { get; set; }
    }

    public class Gender_Response : BaseResponseEntity
    {
        public string? GenderName { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Visitor Type
    public class VisitorType_Request : BaseEntity
    {
        [DefaultValue("")]
        public string? VisitorType { get; set; }

        public bool? IsActive { get; set; }
    }

    public class VisitorType_Response : BaseResponseEntity
    {
        public string? VisitorType { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Visit Type
    public class VisitType_Request : BaseEntity
    {
        [DefaultValue("")]
        public string? VisitType { get; set; }

        public bool? IsActive { get; set; }
    }

    public class VisitType_Response : BaseResponseEntity
    {
        public string? VisitType { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Vehicle Type
    public class VehicleType_Request : BaseEntity
    {
        [DefaultValue("")]
        public string? VehicleType { get; set; }

        public bool? IsActive { get; set; }
    }

    public class VehicleType_Response : BaseResponseEntity
    {
        public string? VehicleType { get; set; }

        public bool? IsActive { get; set; }
    }
    #endregion

    #region Document Type
    public class DocumentType_Request : BaseEntity
    {
        [DefaultValue("")]
        public string? DocumentId { get; set; }

        [DefaultValue("")]
        public string? DocumentType { get; set; }

        [DefaultValue("")]
        public string? Purpose { get; set; }

        public bool? IsActive { get; set; }
    }

    public class DocumentType_Response : BaseResponseEntity
    {
        public string? DocumentId { get; set; }
        public string? DocumentType { get; set; }
        public string? Purpose { get; set; }
        public bool? IsActive { get; set; }
    }

    #endregion

    #region UOM
    public class UOM_Request : BaseEntity
    {
        [DefaultValue("")]
        public string? UOMName { get; set; }

        [DefaultValue("")]
        public string? UOMDesc { get; set; }

        public bool? IsActive { get; set; }
    }

    public class UOM_Response : BaseResponseEntity
    {
        public string? UOMName { get; set; }
        public string? UOMDesc { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Contract Type
    public class ContractType_Request : BaseEntity
    {
        [DefaultValue("")]
        public string? ContractType { get; set; }

        public bool? IsActive { get; set; }
    }

    public class ContractType_Response : BaseResponseEntity
    {
        public string? ContractType { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Leave Type
    public class LeaveType_Request : BaseEntity
    {
        [DefaultValue("")]
        public string? LeaveType { get; set; }

        public bool? IsActive { get; set; }
    }

    public class LeaveType_Response : BaseResponseEntity
    {
        public string? LeaveType { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Gate Type
    public class GateType_Request : BaseEntity
    {
        [DefaultValue("")]
        public string? GateType { get; set; }

        public bool? IsActive { get; set; }
    }

    public class GateType_Response : BaseResponseEntity
    {
        public string? GateType { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Gate Name
    public class GateName_Request : BaseEntity
    {
        [DefaultValue("")]
        public string? GateName { get; set; }

        public bool? IsActive { get; set; }
    }

    public class GateName_Response : BaseResponseEntity
    {
        public string? GateName { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Gate Details
    public class GateDetails_Request : BaseEntity
    {
        public string? GateNumber { get; set; }
        public int? GateNameId { get; set; }

        [DefaultValue("")]
        public string? Remarks { get; set; }

        public bool? IsActive { get; set; }
    }

    public class GateDetails_Response : BaseResponseEntity
    {
        public string? GateNumber { get; set; }
        public int? GateNameId { get; set; }
        public string? GateName { get; set; }
        public string? Remarks { get; set; }
        public bool? IsActive { get; set; }
    }

    #endregion

    #region Worker Type
    public class WorkerType_Request : BaseEntity
    {
        [DefaultValue("")]
        public string? WorkerType { get; set; }

        public bool? IsActive { get; set; }
    }

    public class WorkerType_Response : BaseResponseEntity
    {
        public string? WorkerType { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Meeting Type
    public class MeetingType_Request : BaseEntity
    {
        [DefaultValue("")]
        public string? MeetingType { get; set; }

        public bool? IsActive { get; set; }
    }

    public class MeetingType_Response : BaseResponseEntity
    {
        public string? MeetingType { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Item Details
    public class ItemDetails_Request : BaseEntity
    {
        [DefaultValue("")]
        public string? ItemCode { get; set; }

        [DefaultValue("")]
        public string? ItemName { get; set; }

        [DefaultValue("")]
        public string? ItemDesc { get; set; }

        public int? UOMId { get; set; }

        public decimal? ItemRate { get; set; }

        [DefaultValue("")]
        public string? Serial { get; set; }

        [DefaultValue("")]
        public string? Batch { get; set; }

        public int? CompanyId { get; set; }
        public int? BranchId { get; set; }

        public bool? IsActive { get; set; }
    }

    public class ItemDetails_Response : BaseResponseEntity
    {
        public string? ItemCode { get; set; }
        public string? ItemName { get; set; }
        public string? ItemDesc { get; set; }
        public int? UOMId { get; set; }
        public string? UOMName { get; set; }
        public string? ItemRate { get; set; }
        public string? Serial { get; set; }
        public string? Batch { get; set; }
        public int? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public int? BranchId { get; set; }
        public string? BranchName { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region ID Type
    public class IDType_Request : BaseEntity
    {
        [DefaultValue("")]
        public string? IDType { get; set; }

        public bool? IsActive { get; set; }
    }

    public class IDType_Response : BaseResponseEntity
    {
        public string? IDType { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Contractor Type
    public class ContractorType_Request : BaseEntity
    {
        [DefaultValue("")]
        public string? ContractorType { get; set; }

        public bool? IsActive { get; set; }
    }

    public class ContractorType_Response : BaseResponseEntity
    {
        public string? ContractorType { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Marital Status
    public class MaritalStatus_Request : BaseEntity
    {
        [DefaultValue("")]
        public string? MaritalStatus { get; set; }

        public bool? IsActive { get; set; }
    }

    public class MaritalStatus_Response : BaseResponseEntity
    {
        public string? MaritalStatus { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Blood Group
    public class BloodGroup_Request : BaseEntity
    {
        [DefaultValue("")]
        public string? BloodGroup { get; set; }

        public bool? IsActive { get; set; }
    }

    public class BloodGroup_Response : BaseResponseEntity
    {
        public string? BloodGroup { get; set; }

        public bool? IsActive { get; set; }
    }
    #endregion

    #region Pass Type
    public class PassType_Request : BaseEntity
    {
        [DefaultValue("")]
        public string? PassType { get; set; }

        public bool? IsActive { get; set; }
    }

    public class PassType_Response : BaseResponseEntity
    {
        public string? PassType { get; set; }

        public bool? IsActive { get; set; }
    }
    #endregion

    #region Item Type
    public class ItemType_Request : BaseEntity
    {
        [DefaultValue("")]
        public string? ItemType { get; set; }

        public bool? IsActive { get; set; }
    }

    public class ItemType_Response : BaseResponseEntity
    {
        public string? ItemType { get; set; }

        public bool? IsActive { get; set; }
    }
    #endregion

    #region Days
    public class Days_Request : BaseEntity
    {
        [DefaultValue("")]
        public string? DaysName { get; set; }

        public bool? IsActive { get; set; }
    }

    public class Days_Response : BaseResponseEntity
    {
        public string? DaysName { get; set; }

        public bool? IsActive { get; set; }
    }
    #endregion

    #region Material Details
    public class MaterialDetails_Search : BaseSearchEntity
    {
    }

    public class MaterialDetails_Request : BaseEntity
    {
        [DefaultValue("")]
        public string? MaterialCode { get; set; }

        [DefaultValue("")]
        public string? MaterialName { get; set; }
        public decimal? CurrentStock { get; set; }
        public decimal? Quantity { get; set; }
        public int? UOMId { get; set; }
        public string? Remarks { get; set; }
        public bool? IsActive { get; set; }
    }

    public class MaterialDetails_Response : BaseResponseEntity
    {
        public string? MaterialCode { get; set; }
        public string? MaterialName { get; set; }
        public decimal? CurrentStock { get; set; }
        public decimal? Quantity { get; set; }
        public int? UOMId { get; set; }
        public string? UOMName { get; set; }
        public string? Remarks { get; set; }
        public bool? IsActive { get; set; }
    }

    #endregion

    #region User Type
    public class UserType_Request : BaseEntity
    {

        [DefaultValue("")]
        public string? UserType { get; set; }
        public bool? IsActive { get; set; }
    }

    public class UserType_Response : BaseResponseEntity
    {
        public string? UserType { get; set; }
        public bool? IsActive { get; set; }
    }
    #endregion

    #region Work Place
    public class WorkPlace_Search : BaseSearchEntity
    {
        public int? BranchId { get; set; }
    }

    public class WorkPlace_Request : BaseEntity
    {
        public int? BranchId { get; set; }

        [DefaultValue("")]
        public string? WorkPlace { get; set; }
        public bool? IsActive { get; set; }
    }

    public class WorkPlace_Response : BaseResponseEntity
    {
        public int? BranchId { get; set; }
        public string? BranchName { get; set; }
        public string? WorkPlace { get; set; }
        public bool? IsActive { get; set; }
    }
    #endregion

    #region Transporter Type
    public class TransporterType_Request : BaseEntity
    {
        [DefaultValue("")]
        public string? TransporterType { get; set; }

        public bool? IsActive { get; set; }
    }

    public class TransporterType_Response : BaseResponseEntity
    {
        public string? TransporterType { get; set; }

        public bool? IsActive { get; set; }
    }
    #endregion

    #region Building Name
    public class BuildingName_Request : BaseEntity
    {
        [DefaultValue("")]
        public string? BuildingName { get; set; }
        public int? NoofRooms { get; set; }
        public bool? IsActive { get; set; }
    }

    public class BuildingName_Response : BaseResponseEntity
    {
        public string? BuildingName { get; set; }
        public int? NoofRooms { get; set; }
        public bool? IsActive { get; set; }
    }
    #endregion
}
