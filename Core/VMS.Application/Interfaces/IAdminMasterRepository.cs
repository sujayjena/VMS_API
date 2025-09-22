using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Application.Helpers;
using VMS.Application.Models;
using VMS.Persistence.Repositories;

namespace VMS.Application.Interfaces
{
    public interface IAdminMasterRepository
    {
        #region Gender
        Task<int> SaveGender(Gender_Request parameters);

        Task<IEnumerable<Gender_Response>> GetGenderList(BaseSearchEntity parameters);

        Task<Gender_Response?> GetGenderById(int Id);
        #endregion

        #region Visitor Type
        Task<int> SaveVisitorType(VisitorType_Request parameters);

        Task<IEnumerable<VisitorType_Response>> GetVisitorTypeList(BaseSearchEntity parameters);

        Task<VisitorType_Response?> GetVisitorTypeById(int Id);
        #endregion

        #region Visit Type
        Task<int> SaveVisitType(VisitType_Request parameters);

        Task<IEnumerable<VisitType_Response>> GetVisitTypeList(BaseSearchEntity parameters);

        Task<VisitType_Response?> GetVisitTypeById(int Id);
        #endregion

        #region Vehicle Type
        Task<int> SaveVehicleType(VehicleType_Request parameters);

        Task<IEnumerable<VehicleType_Response>> GetVehicleTypeList(BaseSearchEntity parameters);

        Task<VehicleType_Response?> GetVehicleTypeById(int Id);
        #endregion

        #region Document Type
        Task<int> SaveDocumentType(DocumentType_Request parameters);

        Task<IEnumerable<DocumentType_Response>> GetDocumentTypeList(BaseSearchEntity parameters);

        Task<DocumentType_Response?> GetDocumentTypeById(int Id);
        #endregion

        #region UOM
        Task<int> SaveUOM(UOM_Request parameters);

        Task<IEnumerable<UOM_Response>> GetUOMList(BaseSearchEntity parameters);

        Task<UOM_Response?> GetUOMById(int Id);
        #endregion

        #region Contract Type
        Task<int> SaveContractType(ContractType_Request parameters);

        Task<IEnumerable<ContractType_Response>> GetContractTypeList(BaseSearchEntity parameters);

        Task<ContractType_Response?> GetContractTypeById(int Id);
        #endregion

        #region Leave Type
        Task<int> SaveLeaveType(LeaveType_Request parameters);

        Task<IEnumerable<LeaveType_Response>> GetLeaveTypeList(BaseSearchEntity parameters);

        Task<LeaveType_Response?> GetLeaveTypeById(int Id);
        #endregion

        #region Gate Type
        Task<int> SaveGateType(GateType_Request parameters);

        Task<IEnumerable<GateType_Response>> GetGateTypeList(BaseSearchEntity parameters);

        Task<GateType_Response?> GetGateTypeById(int Id);
        #endregion

        #region Gate Name
        Task<int> SaveGateName(GateName_Request parameters);

        Task<IEnumerable<GateName_Response>> GetGateNameList(BaseSearchEntity parameters);

        Task<GateName_Response?> GetGateNameById(int Id);
        #endregion

        #region Gate Details
        Task<int> SaveGateDetails(GateDetails_Request parameters);

        Task<IEnumerable<GateDetails_Response>> GetGateDetailsList(BaseSearchEntity parameters);

        Task<GateDetails_Response?> GetGateDetailsById(int Id);
        #endregion

        #region Worker Type
        Task<int> SaveWorkerType(WorkerType_Request parameters);

        Task<IEnumerable<WorkerType_Response>> GetWorkerTypeList(BaseSearchEntity parameters);

        Task<WorkerType_Response?> GetWorkerTypeById(int Id);
        #endregion

        #region Meeting Type
        Task<int> SaveMeetingType(MeetingType_Request parameters);

        Task<IEnumerable<MeetingType_Response>> GetMeetingTypeList(BaseSearchEntity parameters);

        Task<MeetingType_Response?> GetMeetingTypeById(int Id);
        #endregion

        #region Item Details
        Task<int> SaveItemDetails(ItemDetails_Request parameters);

        Task<IEnumerable<ItemDetails_Response>> GetItemDetailsList(BaseSearchEntity parameters);

        Task<ItemDetails_Response?> GetItemDetailsById(int Id);
        #endregion

        #region ID Type
        Task<int> SaveIDType(IDType_Request parameters);

        Task<IEnumerable<IDType_Response>> GetIDTypeList(BaseSearchEntity parameters);

        Task<IDType_Response?> GetIDTypeById(int Id);
        #endregion

        #region Contractor Type
        Task<int> SaveContractorType(ContractorType_Request parameters);

        Task<IEnumerable<ContractorType_Response>> GetContractorTypeList(BaseSearchEntity parameters);

        Task<ContractorType_Response?> GetContractorTypeById(int Id);
        #endregion

        #region Marital Status
        Task<int> SaveMaritalStatus(MaritalStatus_Request parameters);

        Task<IEnumerable<MaritalStatus_Response>> GetMaritalStatusList(BaseSearchEntity parameters);

        Task<MaritalStatus_Response?> GetMaritalStatusById(int Id);
        #endregion

        #region Blood Group
        Task<int> SaveBloodGroup(BloodGroup_Request parameters);

        Task<IEnumerable<BloodGroup_Response>> GetBloodGroupList(BaseSearchEntity parameters);

        Task<BloodGroup_Response?> GetBloodGroupById(int Id);

        #endregion

        #region Pass Type
        Task<int> SavePassType(PassType_Request parameters);

        Task<IEnumerable<PassType_Response>> GetPassTypeList(BaseSearchEntity parameters);

        Task<PassType_Response?> GetPassTypeById(int Id);

        #endregion

        #region Days
        Task<int> SaveDays(Days_Request parameters);

        Task<IEnumerable<Days_Response>> GetDaysList(BaseSearchEntity parameters);

        Task<Days_Response?> GetDaysById(int Id);

        #endregion

        #region Material Details
        Task<int> SaveMaterialDetails(MaterialDetails_Request parameters);
        Task<IEnumerable<MaterialDetails_Response>> GetMaterialDetailsList(MaterialDetails_Search parameters);
        Task<MaterialDetails_Response?> GetMaterialDetailsById(int Id);
        #endregion

        #region User Type
        Task<int> SaveUserType(UserType_Request parameters);

        Task<IEnumerable<UserType_Response>> GetUserTypeList(BaseSearchEntity parameters);

        Task<UserType_Response?> GetUserTypeById(int Id);

        #endregion

        #region Work Place
        Task<int> SaveWorkPlace(WorkPlace_Request parameters);

        Task<IEnumerable<WorkPlace_Response>> GetWorkPlaceList(WorkPlace_Search parameters);

        Task<WorkPlace_Response?> GetWorkPlaceById(int Id);

        #endregion

        #region Transporter Type
        Task<int> SaveTransporterType(TransporterType_Request parameters);

        Task<IEnumerable<TransporterType_Response>> GetTransporterTypeList(BaseSearchEntity parameters);

        Task<TransporterType_Response?> GetTransporterTypeById(int Id);

        #endregion

        #region Building Name
        Task<int> SaveBuildingName(BuildingName_Request parameters);

        Task<IEnumerable<BuildingName_Response>> GetBuildingNameList(BaseSearchEntity parameters);

        Task<BuildingName_Response?> GetBuildingNameById(int Id);

        #endregion

        #region Building Room Number
        Task<int> SaveBuildingRoomNumber(BuildingRoomNumber_Request parameters);

        Task<IEnumerable<BuildingRoomNumber_Response>> GetBuildingRoomNumberList(BaseSearchEntity parameters);

        Task<BuildingRoomNumber_Response?> GetBuildingRoomNumberById(int Id);

        #endregion
    }
}
