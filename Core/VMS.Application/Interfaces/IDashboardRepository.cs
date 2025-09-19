using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Application.Models;

namespace VMS.Application.Interfaces
{
    public interface IDashboardRepository
    {
        Task<IEnumerable<Dashboard_TotalSummary_Result>> GetDashboard_TotalSummary(Dashboard_Search_Request parameters);
    }
}
