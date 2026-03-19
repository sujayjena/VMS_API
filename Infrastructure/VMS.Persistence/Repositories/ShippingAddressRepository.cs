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
    public class ShippingAddressRepository : GenericRepository, IShippingAddressRepository
    {
        private IConfiguration _configuration;

        public ShippingAddressRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> SaveShippingAddress(ShippingAddress_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();

            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@RefId", parameters.RefId);
            queryParameters.Add("@RefType", parameters.RefType);
            queryParameters.Add("@IsNational_Or_International", parameters.IsNational_Or_International);
            queryParameters.Add("@Address1", parameters.Address1);
            queryParameters.Add("@CountryId", parameters.CountryId);
            queryParameters.Add("@StateId", parameters.StateId);
            queryParameters.Add("@DistrictId", parameters.DistrictId);
            queryParameters.Add("@CityId", parameters.CityId);
            queryParameters.Add("@PinCode", parameters.PinCode);
            queryParameters.Add("@IsGST", parameters.IsGST);
            queryParameters.Add("@GSTNumber", parameters.GSTNumber);
            queryParameters.Add("@GSTOriginalFileName", parameters.GSTOriginalFileName);
            queryParameters.Add("@GSTFileName", parameters.GSTFileName);
            queryParameters.Add("@IsDefault", parameters.IsDefault);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveShippingAddress", queryParameters);
        }

        public async Task<IEnumerable<ShippingAddress_Response>> GetShippingAddressList(ShippingAddress_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();

            queryParameters.Add("@RefId", parameters.RefId);
            queryParameters.Add("@RefType", parameters.RefType);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<ShippingAddress_Response>("GetShippingAddressList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<ShippingAddress_Response?> GetShippingAddressById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();

            queryParameters.Add("@Id", Id);

            return (await ListByStoredProcedure<ShippingAddress_Response>("GetShippingAddressById", queryParameters)).FirstOrDefault();
        }
    }
}
