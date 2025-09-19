using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Application.Models;

namespace VMS.Application.Interfaces
{
    public interface IManageAttendanceRepository
    {
        Task<int> SaveAttendanceDetails(ManageAttendance_Request parameters);
        Task<IEnumerable<ManageAttendance_Response>> GetAttendanceDetailsList(ManageAttendance_Search parameters);
    }
}
