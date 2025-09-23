using VMS.Application.Models;
using VMS.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMS.Application.Interfaces
{
    public interface ICompanyRepository
    {
        Task<int> SaveCompany(Company_Request parameters);

        Task<IEnumerable<Company_Response>> GetCompanyList(CompanySearch_Request parameters);

        Task<Company_Response?> GetCompanyById(int Id);

        Task<int> SaveAMCReminderEmail(CompanyAMCRminderEmail_Request parameters);
    }
}
