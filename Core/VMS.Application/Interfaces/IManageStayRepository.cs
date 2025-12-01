using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Application.Models;

namespace VMS.Application.Interfaces
{
    public interface IManageStayRepository
    {
        #region Worker Stay
        Task<int> SaveWorkerStay(WorkerStay_Request parameters);
        Task<IEnumerable<WorkerStay_Response>> GetWorkerStayList(WorkerStay_Search parameters);
        Task<WorkerStay_Response?> GetWorkerStayById(int Id);
        #endregion
    }
}
