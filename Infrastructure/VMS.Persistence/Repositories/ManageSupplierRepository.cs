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
    public class ManageSupplierRepository : GenericRepository, IManageSupplierRepository
    {
        private IConfiguration _configuration;

        public ManageSupplierRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> SaveSupplier(Supplier_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@SupplierCode", parameters.SupplierCode);
            queryParameters.Add("@SupplierName", parameters.SupplierName);
            queryParameters.Add("@LandlineNumber", parameters.LandlineNumber);
            queryParameters.Add("@MobileNumber", parameters.MobileNumber);
            queryParameters.Add("@EmailId", parameters.EmailId);
            queryParameters.Add("@SpecialRemarks", parameters.SpecialRemarks);
            queryParameters.Add("@PanCardNumber", parameters.PanCardNumber);
            queryParameters.Add("@PanCardOriginalFileName", parameters.PanCardOriginalFileName);
            queryParameters.Add("@PanCardFileName", parameters.PanCardFileName);
            queryParameters.Add("@GSTNumber", parameters.GSTNumber);
            queryParameters.Add("@GSTOriginalFileName", parameters.GSTOriginalFileName);
            queryParameters.Add("@GSTFileName", parameters.GSTFileName);
            queryParameters.Add("@ContactName", parameters.ContactName);
            queryParameters.Add("@ContactMobileNumber", parameters.ContactMobileNumber);
            queryParameters.Add("@ContactEmailId", parameters.ContactEmailId);
            queryParameters.Add("@AddressLine1", parameters.AddressLine1);
            queryParameters.Add("@CountryId", parameters.CountryId);
            queryParameters.Add("@StateId", parameters.StateId);
            queryParameters.Add("@DistrictId", parameters.DistrictId);
            queryParameters.Add("@PinCode", parameters.PinCode);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveSupplier", queryParameters);
        }

        public async Task<IEnumerable<Supplier_Response>> GetSupplierList(Supplier_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<Supplier_Response>("GetSupplierList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<Supplier_Response?> GetSupplierById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<Supplier_Response>("GetSupplierById", queryParameters)).FirstOrDefault();
        }
    }
}
