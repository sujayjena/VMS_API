using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Application.Models;
using VMS.Domain.Entities;
using VMS.Persistence.Repositories;

namespace VMS.Application.Interfaces
{
    public interface IManageVisitorCompanyRepository
    {
        Task<int> SaveVisitorCompany(VisitorCompany_Request parameters);
        Task<IEnumerable<VisitorCompany_Response>> GetVisitorCompanyList(BaseSearchEntity parameters);
        Task<VisitorCompany_Response?> GetVisitorCompanyById(int Id);
        Task<IEnumerable<VisitorCompany_ImportDataValidation>> ImportVisitorCompany(List<VisitorCompany_ImportData> parameters);
    }
}
