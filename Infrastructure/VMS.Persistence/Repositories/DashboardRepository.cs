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
    public class DashboardRepository : GenericRepository, IDashboardRepository
    {
        private IConfiguration _configuration;

        public DashboardRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<Dashboard_TotalSummary_Result>> GetDashboard_TotalSummary(Dashboard_Search_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();

            queryParameters.Add("@FromDate", parameters.FromDate);
            queryParameters.Add("@ToDate", parameters.ToDate);
            queryParameters.Add("@GateDetailsId", parameters.GateDetailsId);
            queryParameters.Add("@DepartmentId", parameters.DepartmentId);
            queryParameters.Add("@BranchId", parameters.BranchId);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<Dashboard_TotalSummary_Result>("GetDashboard_TotalSummary", queryParameters);

            return result;
        }
    }
}
