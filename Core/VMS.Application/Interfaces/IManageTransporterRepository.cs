using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Application.Models;

namespace VMS.Application.Interfaces
{
    public interface IManageTransporterRepository
    {
        Task<int> SaveTransporter(Transporter_Request parameters);
        Task<IEnumerable<Transporter_Response>> GetTransporterList(Transporter_Search parameters);
        Task<Transporter_Response?> GetTransporterById(int Id);
    }
}
