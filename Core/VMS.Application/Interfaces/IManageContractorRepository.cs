using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Application.Models;
using VMS.Persistence.Repositories;

namespace VMS.Application.Interfaces
{
   
    public interface IManageContractorRepository
    {
        #region Contractor
        Task<int> SaveContractor(Contractor_Request parameters);

        Task<IEnumerable<Contractor_Response>> GetContractorList(ContractorSearch_Request parameters);

        Task<Contractor_Response?> GetContractorById(int Id);
        #endregion

        #region Contractor Insurance
        Task<int> SaveContractorInsurance(ContractorInsurance_Request parameters);

        Task<IEnumerable<ContractorInsurance_Response>> GetContractorInsuranceList(ContractorInsuranceSearch_Request parameters);

        Task<ContractorInsurance_Response?> GetContractorInsuranceById(int Id);
        #endregion
    }
}
