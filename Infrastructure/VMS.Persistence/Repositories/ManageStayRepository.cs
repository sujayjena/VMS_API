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
    public class ManageStayRepository : GenericRepository, IManageStayRepository
    {
        private IConfiguration _configuration;

        public ManageStayRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        #region Employee Stay
        public async Task<int> SaveEmployeeStay(EmployeeStay_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@EmployeeId", parameters.EmployeeId);
            queryParameters.Add("@IsCompany", parameters.IsCompany);
            queryParameters.Add("@BuildingNameId", parameters.BuildingNameId);
            queryParameters.Add("@BuildingRoomNumberId", parameters.BuildingRoomNumberId);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveEmployeeStay", queryParameters);
        }

        public async Task<IEnumerable<EmployeeStay_Response>> GetEmployeeStayList(EmployeeStay_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<EmployeeStay_Response>("GetEmployeeStayList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<EmployeeStay_Response?> GetEmployeeStayById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<EmployeeStay_Response>("GetEmployeeStayById", queryParameters)).FirstOrDefault();
        }
        #endregion

        #region Worker Stay
        public async Task<int> SaveWorkerStay(WorkerStay_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@WorkerId", parameters.WorkerId);
            queryParameters.Add("@IsCompany", parameters.IsCompany);
            queryParameters.Add("@BuildingNameId", parameters.BuildingNameId);
            queryParameters.Add("@BuildingRoomNumberId", parameters.BuildingRoomNumberId);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveWorkerStay", queryParameters);
        }

        public async Task<IEnumerable<WorkerStay_Response>> GetWorkerStayList(WorkerStay_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<WorkerStay_Response>("GetWorkerStayList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<WorkerStay_Response?> GetWorkerStayById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<WorkerStay_Response>("GetWorkerStayById", queryParameters)).FirstOrDefault();
        }
        #endregion
    }
}
