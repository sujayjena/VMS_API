using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Application.Models;

namespace VMS.Application.Interfaces
{
    public interface IVehicleManagementRepository
    {
        #region Vehicle Management
        Task<int> SaveVehicleManagement(VehicleManagement_Request parameters);
        Task<IEnumerable<VehicleManagement_Response>> GetVehicleManagementList(VehicleManagement_Search parameters);
        Task<VehicleManagement_Response?> GetVehicleManagementById(int Id);

        Task<int> SaveVehicleManagementItem(VehicleManagementItem_Request parameters);
        Task<IEnumerable<VehicleManagementItem_Response>> GetVehicleManagementItemById(long VehicleManagementId, long ItemId);

        Task<int> SaveVehicleManagementGateNo(VehicleManagementGateNo_Request parameters);
        Task<IEnumerable<VehicleManagementGateNo_Response>> GetVehicleManagementGateNoById(long VehicleManagementId, long GateDetailsId);

        #endregion

        #region Material Weight
        Task<int> SaveMaterialWeight(MaterialWeight_Request parameters);
        Task<IEnumerable<MaterialWeight_Response>> GetMaterialWeightList(MaterialWeight_Search parameters);
        Task<MaterialWeight_Response?> GetMaterialWeightById(int Id);
        #endregion
    }
}
