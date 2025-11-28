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
    public class ManageTransporterRepository : GenericRepository, IManageTransporterRepository
    {
        private IConfiguration _configuration;

        public ManageTransporterRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> SaveTransporter(Transporter_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@TransporterCode", parameters.TransporterCode);
            queryParameters.Add("@TransporterTypeId", parameters.TransporterTypeId);
            queryParameters.Add("@TransporterName", parameters.TransporterName);
            queryParameters.Add("@LandlineNumber", parameters.LandlineNumber);
            queryParameters.Add("@MobileNumber", parameters.MobileNumber);
            queryParameters.Add("@EmailId", parameters.EmailId);
            queryParameters.Add("@Website", parameters.Website);
            queryParameters.Add("@SpecialRemarks", parameters.SpecialRemarks);
            queryParameters.Add("@TransporterRemarks", parameters.TransporterRemarks);
            queryParameters.Add("@RefParty", parameters.RefParty);
            queryParameters.Add("@GSTOriginalFileName", parameters.GSTOriginalFileName);
            queryParameters.Add("@GSTFileName", parameters.GSTFileName);
            queryParameters.Add("@ContactName", parameters.ContactName);
            queryParameters.Add("@ContactMobileNumber", parameters.ContactMobileNumber);
            queryParameters.Add("@ContactEmailId", parameters.ContactEmailId);
            queryParameters.Add("@AadharCardOriginalFileName", parameters.AadharCardOriginalFileName);
            queryParameters.Add("@AadharCardFileName", parameters.AadharCardFileName);
            queryParameters.Add("@AddressLine1", parameters.AddressLine1);
            queryParameters.Add("@CountryId", parameters.CountryId);
            queryParameters.Add("@StateId", parameters.StateId);
            queryParameters.Add("@DistrictId", parameters.DistrictId);
            queryParameters.Add("@PinCode", parameters.PinCode);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveTransporter", queryParameters);
        }

        public async Task<IEnumerable<Transporter_Response>> GetTransporterList(Transporter_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<Transporter_Response>("GetTransporterList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<Transporter_Response?> GetTransporterById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<Transporter_Response>("GetTransporterById", queryParameters)).FirstOrDefault();
        }
    }
}
