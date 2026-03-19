using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Application.Models;

namespace VMS.Application.Interfaces
{
    public interface ICustomerRepository
    {
        Task<int> SaveCustomer(Customer_Request parameters);
        Task<int> DeleteCustomer(int Id);
        Task<IEnumerable<Customer_Response>> GetCustomerList(CustomerSearch_Request parameters);
        Task<Customer_Response?> GetCustomerById(long Id);
    }
}
