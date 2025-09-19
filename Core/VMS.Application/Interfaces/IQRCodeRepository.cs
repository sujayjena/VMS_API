using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Application.Models;

namespace VMS.Application.Interfaces
{
    public interface IQRCodeRepository
    {
        QRCodeGenerate_Response GenerateQRCode(string value);
        Task<int> SaveQRCode(QRCode_Request parameters);
        Task<QRCode_Response?> GetQRCodeById(string QRCodeNo);
    }
}
