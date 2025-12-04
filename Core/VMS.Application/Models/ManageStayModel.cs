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
    #region Employee Stay
    public class EmployeeStay_Request : BaseEntity
    {
        public int? EmployeeId { get; set; }

        [DefaultValue(false)]
        public bool? IsCompany { get; set; }
        public int? BuildingNameId { get; set; }
        public int? BuildingRoomNumberId { get; set; }
        public bool? IsActive { get; set; }
    }

    public class EmployeeStay_Search : BaseSearchEntity
    {
    }

    public class EmployeeStay_Response : BaseResponseEntity
    {
        public int? EmployeeId { get; set; }
        public string? UserCode { get; set; }
        public string? EmployeeName { get; set; }
        public int? UserTypeId { get; set; }
        public string? UserType { get; set; }
        public string? MobileNumber { get; set; }
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int? RoleId { get; set; }
        public string? RoleName { get; set; }
        public int? ReportingTo { get; set; }
        public string? ReportingToName { get; set; }
        public bool? IsCompany { get; set; }
        public int? BuildingNameId { get; set; }
        public string? BuildingName { get; set; }
        public int? BuildingRoomNumberId { get; set; }
        public int? RoomNumber { get; set; }
        public bool? IsActive { get; set; }
    }
    #endregion

    #region Worker Stay
    public class WorkerStay_Request : BaseEntity
    {
        public int? WorkerId { get; set; }

        [DefaultValue(false)]
        public bool? IsCompany { get; set; }
        public int? BuildingNameId { get; set; }
        public int? BuildingRoomNumberId { get; set; }
        public bool? IsActive { get; set; }
    }

    public class WorkerStay_Search : BaseSearchEntity
    {
    }

    public class WorkerStay_Response : BaseResponseEntity
    {
        public int? WorkerId { get; set; }
        public string? WorkerCode { get; set; }
        public string? WorkerName { get; set; }
        public int? WorkerTypeId { get; set; }
        public string? WorkerType { get; set; }
        public string? WorkerMobileNo { get; set; }
        public bool? IsCompany { get; set; }
        public int? BuildingNameId { get; set; }
        public string? BuildingName { get; set; }
        public int? BuildingRoomNumberId { get; set; }
        public int? RoomNumber { get; set; }
        public bool? IsActive { get; set; }
    }
    #endregion
}
