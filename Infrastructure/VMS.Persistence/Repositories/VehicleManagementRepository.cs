using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Application.Helpers;
using VMS.Application.Interfaces;
using VMS.Application.Models;

namespace VMS.Persistence.Repositories
{
    public class VehicleManagementRepository : GenericRepository, IVehicleManagementRepository
    {
        private IConfiguration _configuration;

        public VehicleManagementRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        #region Vehicle Management
        public async Task<int> SaveVehicleManagement(VehicleManagement_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@ReceivingDate", parameters.ReceivingDate);
            queryParameters.Add("@PONumber", parameters.PONumber);
            queryParameters.Add("@SupplierId", parameters.SupplierId);
            queryParameters.Add("@ItemId", parameters.ItemId);
            queryParameters.Add("@TruckNumber", parameters.TruckNumber);
            queryParameters.Add("@DriverName", parameters.DriverName);
            queryParameters.Add("@DriverLicenceNumber", parameters.DriverLicenceNumber);
            queryParameters.Add("@LicenceValidFrom", parameters.LicenceValidFrom);
            queryParameters.Add("@LicenceValidTo", parameters.LicenceValidTo);
            queryParameters.Add("@AttachmentOriginalFileName", parameters.AttachmentOriginalFileName);
            queryParameters.Add("@AttachmentFileName", parameters.AttachmentFileName);
            queryParameters.Add("@DocumentTypeId", parameters.DocumentTypeId);
            queryParameters.Add("@DocumentOriginalFileName", parameters.DocumentOriginalFileName);
            queryParameters.Add("@DocumentFileName", parameters.DocumentFileName);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveVehicleManagement", queryParameters);
        }

        public async Task<IEnumerable<VehicleManagement_Response>> GetVehicleManagementList(VehicleManagement_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<VehicleManagement_Response>("GetVehicleManagementList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<VehicleManagement_Response?> GetVehicleManagementById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<VehicleManagement_Response>("GetVehicleManagementById", queryParameters)).FirstOrDefault();
        }
        #endregion

        #region Material Weight
        public async Task<int> SaveMaterialWeight(MaterialWeight_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@VehicleManagementId", parameters.VehicleManagementId);
            queryParameters.Add("@PartyWeight", parameters.PartyWeight);
            queryParameters.Add("@FirstWeight", parameters.FirstWeight);
            queryParameters.Add("@SecondWeight", parameters.SecondWeight);
            queryParameters.Add("@NetWeight", parameters.NetWeight);
            queryParameters.Add("@DeductionWeight", parameters.DeductionWeight);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveMaterialWeight", queryParameters);
        }

        public async Task<IEnumerable<MaterialWeight_Response>> GetMaterialWeightList(MaterialWeight_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<MaterialWeight_Response>("GetMaterialWeightList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<MaterialWeight_Response?> GetMaterialWeightById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<MaterialWeight_Response>("GetMaterialWeightById", queryParameters)).FirstOrDefault();
        }
        #endregion
    }
}
