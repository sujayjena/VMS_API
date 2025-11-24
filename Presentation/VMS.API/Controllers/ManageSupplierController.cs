using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using VMS.Application.Enums;
using VMS.Application.Helpers;
using VMS.Application.Interfaces;
using VMS.Application.Models;

namespace VMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageSupplierController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IManageSupplierRepository _manageSupplierRepository;
        private IFileManager _fileManager;

        public ManageSupplierController(IManageSupplierRepository manageSupplierRepository, IFileManager fileManager)
        {
            _manageSupplierRepository = manageSupplierRepository;
            _fileManager = fileManager;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveSupplier(Supplier_Request parameters)
        {
            // PanCard Upload
            if (parameters! != null && !string.IsNullOrWhiteSpace(parameters.PanCard_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.PanCard_Base64, "\\Uploads\\Supplier\\", parameters.PanCardOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.PanCardFileName = vUploadFile;
                }
            }

            // GST Upload
            if (parameters! != null && !string.IsNullOrWhiteSpace(parameters.GST_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.GST_Base64, "\\Uploads\\Supplier\\", parameters.GSTOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.GSTFileName = vUploadFile;
                }
            }

            int result = await _manageSupplierRepository.SaveSupplier(parameters);

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveOperationEnums.ReocrdExists)
            {
                _response.Message = "Record is already exists";
            }
            else if (result == (int)SaveOperationEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetSupplierList(Supplier_Search parameters)
        {
            IEnumerable<Supplier_Response> lstRoles = await _manageSupplierRepository.GetSupplierList(parameters);

            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetSupplierById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _manageSupplierRepository.GetSupplierById(Id);

                _response.Data = vResultObj;
            }
            return _response;
        }

        /*
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ExportSupplierData(Supplier_Search parameters)
        {
            _response.IsSuccess = false;
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            IEnumerable<Supplier_Response> lstObj = await _manageSupplierRepository.GetSupplierList(parameters);

            using (MemoryStream msExportDataFile = new MemoryStream())
            {
                using (ExcelPackage excelExportData = new ExcelPackage())
                {
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("Supplier");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "Supplier Code";
                    WorkSheet1.Cells[1, 2].Value = "Supplier Name";
                    WorkSheet1.Cells[1, 3].Value = "Landline Number";
                    WorkSheet1.Cells[1, 4].Value = "Mobile Number";
                    WorkSheet1.Cells[1, 5].Value = "EmailId";
                    WorkSheet1.Cells[1, 6].Value = "Special Remarks";
                    WorkSheet1.Cells[1, 7].Value = "Pan Card Number";
                    WorkSheet1.Cells[1, 8].Value = "GST Number";
                    WorkSheet1.Cells[1, 9].Value = "Customer Name";
                    WorkSheet1.Cells[1, 10].Value = "Customer Mobile";
                    WorkSheet1.Cells[1, 11].Value = "Customer Email";
                    WorkSheet1.Cells[1, 12].Value = "Address Line 1";
                    WorkSheet1.Cells[1, 13].Value = "Country";
                    WorkSheet1.Cells[1, 14].Value = "State";
                    WorkSheet1.Cells[1, 15].Value = "District";
                    WorkSheet1.Cells[1, 16].Value = "IsActive";
                    WorkSheet1.Cells[1, 17].Value = "Created Date";
                    WorkSheet1.Cells[1, 18].Value = "Created By";

                    recordIndex = 2;

                    foreach (var items in lstObj)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = items.SupplierCode;
                        WorkSheet1.Cells[recordIndex, 2].Value = items.SupplierName;
                        WorkSheet1.Cells[recordIndex, 3].Value = items.LandlineNumber;
                        WorkSheet1.Cells[recordIndex, 4].Value = items.MobileNumber;
                        WorkSheet1.Cells[recordIndex, 5].Value = items.EmailId;
                        WorkSheet1.Cells[recordIndex, 6].Value = items.SpecialRemarks;
                        WorkSheet1.Cells[recordIndex, 7].Value = items.PanCardNumber;
                        WorkSheet1.Cells[recordIndex, 8].Value = items.GSTNumber;
                        WorkSheet1.Cells[recordIndex, 9].Value = items.CustomerName;
                        WorkSheet1.Cells[recordIndex, 10].Value = items.CustMobileNumber;
                        WorkSheet1.Cells[recordIndex, 11].Value = items.CustEmailId;
                        WorkSheet1.Cells[recordIndex, 12].Value = items.AddressLine1;
                        WorkSheet1.Cells[recordIndex, 13].Value = items.CountryName;
                        WorkSheet1.Cells[recordIndex, 14].Value = items.StateName;
                        WorkSheet1.Cells[recordIndex, 15].Value = items.DistrictName;
                        WorkSheet1.Cells[recordIndex, 16].Value = items.IsActive == true ? "Active" : "Inactive";
                        WorkSheet1.Cells[recordIndex, 17].Value = items.CreatedDate.ToString("dd/MM/yyyy");
                        WorkSheet1.Cells[recordIndex, 18].Value = items.CreatorName;

                        recordIndex += 1;
                    }

                    WorkSheet1.Columns.AutoFit();

                    excelExportData.SaveAs(msExportDataFile);
                    msExportDataFile.Position = 0;
                    result = msExportDataFile.ToArray();
                }
            }

            if (result != null)
            {
                _response.Data = result;
                _response.IsSuccess = true;
                _response.Message = "Exported successfully";
            }

            return _response;
        }
        */
    }
}
