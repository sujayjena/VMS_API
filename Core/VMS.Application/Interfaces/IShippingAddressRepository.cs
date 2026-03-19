using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Application.Models;

namespace VMS.Application.Interfaces
{
    public interface IShippingAddressRepository
    {
        Task<int> SaveShippingAddress(ShippingAddress_Request parameters);
        Task<IEnumerable<ShippingAddress_Response>> GetShippingAddressList(ShippingAddress_Search parameters);
        Task<ShippingAddress_Response?> GetShippingAddressById(int Id);
    }
}
