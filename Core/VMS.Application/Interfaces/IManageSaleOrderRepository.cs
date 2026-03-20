using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Application.Models;

namespace VMS.Application.Interfaces
{
    public interface IManageSaleOrderRepository
    {
        #region PO Received
        Task<int> SavePOReceived(POReceived_Request parameters);
        Task<IEnumerable<POReceived_Response>> GetPOReceivedList(POReceivedSearch_Request parameters);
        Task<POReceived_Response?> GetPOReceivedById(int Id);
        #endregion
    }
}
