using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Application.Models;

namespace VMS.Application.Interfaces
{
    public interface IBarcodeRepository
    {
        BarcodeGenerate_Response GenerateBarcode(string value);
        Task<int> SaveBarcode(Barcode_Request parameters);
        Task<Barcode_Response?> GetBarcodeById(string BarcodeNo);
    }
}
