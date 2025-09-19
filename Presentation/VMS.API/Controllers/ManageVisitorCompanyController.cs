using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using VMS.API.CustomAttributes;
using VMS.Application.Enums;
using VMS.Application.Helpers;
using VMS.Application.Interfaces;
using VMS.Application.Models;
using VMS.Persistence.Repositories;

namespace VMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageVisitorCompanyController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IManageVisitorCompanyRepository _manageVisitorCompanyRepository;
        private IFileManager _fileManager;

        public ManageVisitorCompanyController(IManageVisitorCompanyRepository manageVisitorCompanyRepository, IFileManager fileManager)
        {
            _manageVisitorCompanyRepository = manageVisitorCompanyRepository;
            _fileManager = fileManager;

            _response = new ResponseModel();
            _response.IsSuccess = true;
          
        }

        [Route("[action]")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ResponseModel> SaveVisitorCompany(VisitorCompany_Request parameters)
        {
            // GSt Upload
            if (parameters! != null && !string.IsNullOrWhiteSpace(parameters.GSTFile_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.GSTFile_Base64, "\\Uploads\\Visitors\\", parameters.GSTOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.GSTFileName = vUploadFile;
                }
            }

            // Pan Card Upload
            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.PanCardFile_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.PanCardFile_Base64, "\\Uploads\\Visitors\\", parameters.PanCardOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.PanCardFileName = vUploadFile;
                }
            }

            int result = await _manageVisitorCompanyRepository.SaveVisitorCompany(parameters);

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

        [AllowAnonymous]
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetVisitorCompanyList(BaseSearchEntity parameters)
        {
            IEnumerable<VisitorCompany_Response> lstVisitorCompanys = await _manageVisitorCompanyRepository.GetVisitorCompanyList(parameters);
            _response.Data = lstVisitorCompanys.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetVisitorCompanyById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _manageVisitorCompanyRepository.GetVisitorCompanyById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> DownloadVisitorCompanyTemplate()
        {
            byte[]? formatFile = await Task.Run(() => _fileManager.GetFormatFileFromPath("Template_VisitorCompany.xlsx"));

            if (formatFile != null)
            {
                _response.Data = formatFile;
            }

            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportVisitorCompany([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;

            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            int noOfCol, noOfRow;

            List<string[]> data = new List<string[]>();
            List<VisitorCompany_ImportData> lstUser_ImportData = new List<VisitorCompany_ImportData>();
            IEnumerable<VisitorCompany_ImportDataValidation> lst_ImportDataValidation;

            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file";
                return _response;
            }

            using (MemoryStream stream = new MemoryStream())
            {
                request.FileUpload.CopyTo(stream);
                using ExcelPackage package = new ExcelPackage(stream);
                currentSheet = package.Workbook.Worksheets;
                workSheet = currentSheet.First();
                noOfCol = workSheet.Dimension.End.Column;
                noOfRow = workSheet.Dimension.End.Row;

                if (!string.Equals(workSheet.Cells[1, 1].Value.ToString(), "CompanyName", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "Address", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 3].Value.ToString(), "Country", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 4].Value.ToString(), "State", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 5].Value.ToString(), "Province", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 6].Value.ToString(), "Pincode", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 7].Value.ToString(), "CompanyPhone", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 8].Value.ToString(), "IsGST", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 9].Value.ToString(), "GSTNumber", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 10].Value.ToString(), "IsPAN", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 11].Value.ToString(), "PanNumber", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 12].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    if (!string.IsNullOrWhiteSpace(workSheet.Cells[rowIterator, 1].Value?.ToString()))
                    {
                        lstUser_ImportData.Add(new VisitorCompany_ImportData()
                        {
                            CompanyName = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                            Address = workSheet.Cells[rowIterator, 2].Value?.ToString(),
                            Country = workSheet.Cells[rowIterator, 3].Value?.ToString(),
                            State = workSheet.Cells[rowIterator, 4].Value?.ToString(),
                            Province = workSheet.Cells[rowIterator, 5].Value?.ToString(),
                            Pincode = workSheet.Cells[rowIterator, 6].Value?.ToString(),
                            CompanyPhone = workSheet.Cells[rowIterator, 7].Value?.ToString(),
                            IsGST = workSheet.Cells[rowIterator, 8].Value?.ToString(),
                            GSTNumber = workSheet.Cells[rowIterator, 9].Value?.ToString(),
                            IsPAN = workSheet.Cells[rowIterator, 10].Value?.ToString(),
                            PanNumber = workSheet.Cells[rowIterator, 11].Value?.ToString(),
                            IsActive = workSheet.Cells[rowIterator, 12].Value?.ToString()
                        });
                    }
                }
            }

            if (lstUser_ImportData.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lst_ImportDataValidation = await _manageVisitorCompanyRepository.ImportVisitorCompany(lstUser_ImportData);

            #region Generate Excel file for Invalid Data

            if (lst_ImportDataValidation.ToList().Count > 0)
            {
                _response.IsSuccess = false;
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidImportDataFile(lst_ImportDataValidation);
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Record imported successfully";
            }

            #endregion

            return _response;
        }

        private byte[] GenerateInvalidImportDataFile(IEnumerable<VisitorCompany_ImportDataValidation> lst_ImportDataValidation)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "CompanyName";
                    WorkSheet1.Cells[1, 2].Value = "Address";
                    WorkSheet1.Cells[1, 3].Value = "Country";
                    WorkSheet1.Cells[1, 4].Value = "State";
                    WorkSheet1.Cells[1, 5].Value = "Province";
                    WorkSheet1.Cells[1, 6].Value = "Pincode";
                    WorkSheet1.Cells[1, 7].Value = "CompanyPhone";
                    WorkSheet1.Cells[1, 8].Value = "IsGST";
                    WorkSheet1.Cells[1, 9].Value = "GSTNumber";
                    WorkSheet1.Cells[1, 10].Value = "IsPAN";
                    WorkSheet1.Cells[1, 11].Value = "PanNumber";  
                    WorkSheet1.Cells[1, 12].Value = "IsActive";
                    WorkSheet1.Cells[1, 13].Value = "ErrorMessage";

                    recordIndex = 2;

                    foreach (VisitorCompany_ImportDataValidation record in lst_ImportDataValidation)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.CompanyName;
                        WorkSheet1.Cells[recordIndex, 2].Value = record.Address;
                        WorkSheet1.Cells[recordIndex, 3].Value = record.Country;
                        WorkSheet1.Cells[recordIndex, 4].Value = record.State;
                        WorkSheet1.Cells[recordIndex, 5].Value = record.Province;
                        WorkSheet1.Cells[recordIndex, 6].Value = record.Pincode;
                        WorkSheet1.Cells[recordIndex, 7].Value = record.CompanyPhone;
                        WorkSheet1.Cells[recordIndex, 8].Value = record.IsGST;
                        WorkSheet1.Cells[recordIndex, 9].Value = record.GSTNumber;
                        WorkSheet1.Cells[recordIndex, 10].Value = record.IsPAN;
                        WorkSheet1.Cells[recordIndex, 11].Value = record.PanNumber;
                        WorkSheet1.Cells[recordIndex, 12].Value = record.IsActive;
                        WorkSheet1.Cells[recordIndex, 13].Value = record.ValidationMessage;

                        recordIndex += 1;
                    }

                    WorkSheet1.Columns.AutoFit();

                    excelInvalidData.SaveAs(msInvalidDataFile);
                    msInvalidDataFile.Position = 0;
                    result = msInvalidDataFile.ToArray();
                }
            }

            return result;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ExportVisitorCompanyData(BaseSearchEntity parameters)
        {
            _response.IsSuccess = false;
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            IEnumerable<VisitorCompany_Response> lstData = await _manageVisitorCompanyRepository.GetVisitorCompanyList(parameters);

            using (MemoryStream msExportDataFile = new MemoryStream())
            {
                using (ExcelPackage excelExportData = new ExcelPackage())
                {
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("VisitorCompany");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "Company Name";
                    WorkSheet1.Cells[1, 2].Value = "Address";
                    WorkSheet1.Cells[1, 3].Value = "Country";
                    WorkSheet1.Cells[1, 4].Value = "State";
                    WorkSheet1.Cells[1, 5].Value = "Province";
                    WorkSheet1.Cells[1, 6].Value = "Pincode";
                    WorkSheet1.Cells[1, 7].Value = "Company Phone";
                    WorkSheet1.Cells[1, 8].Value = "GST";
                    WorkSheet1.Cells[1, 9].Value = "CreatedDate";
                    WorkSheet1.Cells[1, 10].Value = "CreatedBy";

                    recordIndex = 2;

                    foreach (var items in lstData)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = items.CompanyName;
                        WorkSheet1.Cells[recordIndex, 2].Value = items.CompanyAddress;
                        WorkSheet1.Cells[recordIndex, 3].Value = items.CountryName;
                        WorkSheet1.Cells[recordIndex, 4].Value = items.StateName;
                        WorkSheet1.Cells[recordIndex, 5].Value = items.DistrictName;
                        WorkSheet1.Cells[recordIndex, 6].Value = items.Pincode;
                        WorkSheet1.Cells[recordIndex, 7].Value = items.CompanyPhone;
                        WorkSheet1.Cells[recordIndex, 8].Value = items.GSTNo;

                        WorkSheet1.Cells[recordIndex, 9].Value = Convert.ToDateTime(items.CreatedDate).ToString("dd/MM/yyyy");
                        WorkSheet1.Cells[recordIndex, 10].Value = items.CreatorName;

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
    }
}
