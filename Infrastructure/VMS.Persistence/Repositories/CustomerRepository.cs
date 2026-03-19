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
    public class CustomerRepository : GenericRepository, ICustomerRepository
    {
        private IConfiguration _configuration;

        public CustomerRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> SaveCustomer(Customer_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@CustomerName", parameters.CustomerName);
            queryParameters.Add("@MobileNo", parameters.MobileNo);
            queryParameters.Add("@ParentCustomerId", parameters.ParentCustomerId);
            queryParameters.Add("@CustomerTypeId", parameters.CustomerTypeId);
            queryParameters.Add("@CountryId", parameters.CountryId);
            //queryParameters.Add("@EmailId", parameters.EmailId);
            //queryParameters.Add("@ContactName", parameters.ContactName);
            //queryParameters.Add("@IsPan", parameters.IsPan);
            //queryParameters.Add("@PanNumber", parameters.PanNumber);
            //queryParameters.Add("@PanOriginalFileName", parameters.PanOriginalFileName);
            //queryParameters.Add("@PanFileName", parameters.PanFileName);
            //queryParameters.Add("@AutoPassword", parameters.AutoPassword);
            //queryParameters.Add("@EncryptPassword", !string.IsNullOrWhiteSpace(parameters.AutoPassword) ? EncryptDecryptHelper.EncryptString(parameters.AutoPassword) : string.Empty);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveCustomer", queryParameters);
        }

        public async Task<int> DeleteCustomer(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);

            return await SaveByStoredProcedure<int>("DeleteCustomer", queryParameters);
        }

        public async Task<IEnumerable<Customer_Response>> GetCustomerList(CustomerSearch_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@CustomerId", parameters.CustomerId);
            queryParameters.Add("@ParentCustomerId", parameters.ParentCustomerId);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<Customer_Response>("GetCustomerList", queryParameters);

            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<Customer_Response?> GetCustomerById(long Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<Customer_Response>("GetCustomerById", queryParameters)).FirstOrDefault();
        }
    }
}
