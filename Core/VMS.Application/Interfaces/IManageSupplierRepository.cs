using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Application.Models;

namespace VMS.Application.Interfaces
{
    public interface IManageSupplierRepository
    {
        Task<int> SaveSupplier(Supplier_Request parameters);
        Task<IEnumerable<Supplier_Response>> GetSupplierList(Supplier_Search parameters);
        Task<Supplier_Response?> GetSupplierById(int Id);
    }
}
