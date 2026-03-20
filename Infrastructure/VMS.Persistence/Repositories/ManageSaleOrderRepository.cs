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
    public class ManageSaleOrderRepository : GenericRepository, IManageSaleOrderRepository
    {
        private IConfiguration _configuration;

        public ManageSaleOrderRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        #region PO Received
        public async Task<int> SavePOReceived(POReceived_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@CustomerId", parameters.CustomerId);
            queryParameters.Add("@PONumber", parameters.PONumber);
            queryParameters.Add("@IsPOReceived", parameters.IsPOReceived);
            queryParameters.Add("@Quantity", parameters.Quantity);
            queryParameters.Add("@POAmount", parameters.POAmount);
            queryParameters.Add("@ShipmentSchedule", parameters.ShipmentSchedule);
            queryParameters.Add("@POReceivedOriginalFileName", parameters.POReceivedOriginalFileName);
            queryParameters.Add("@POReceivedFileName", parameters.POReceivedFileName);
            queryParameters.Add("@IsPOStatusClosed", parameters.IsPOStatusClosed);
            
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SavePOReceived", queryParameters);
        }

        public async Task<IEnumerable<POReceived_Response>> GetPOReceivedList(POReceivedSearch_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@CustomerId", parameters.CustomerId);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<POReceived_Response>("GetPOReceivedList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<POReceived_Response?> GetPOReceivedById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<POReceived_Response>("GetPOReceivedById", queryParameters)).FirstOrDefault();
        }
        #endregion
    }
}
