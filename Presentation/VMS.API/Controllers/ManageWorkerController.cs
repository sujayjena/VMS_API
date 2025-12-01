using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using VMS.Application.Enums;
using VMS.Application.Helpers;
using VMS.Application.Interfaces;
using VMS.Application.Models;
using VMS.Persistence.Repositories;

namespace VMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageWorkerController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IManageWorkerRepository _manageWorkerRepository;
        private readonly IManageContractorRepository _manageContractorRepository;
        private readonly IAssignGateNoRepository _assignGateNoRepository;
        private readonly IBarcodeRepository _barcodeRepository;
        private readonly IManageVisitorsRepository _manageVisitorsRepository;
        private IFileManager _fileManager;

        public ManageWorkerController(IManageWorkerRepository manageWorkerRepository, IFileManager fileManager, IManageContractorRepository manageContractorRepository, IAssignGateNoRepository assignGateNoRepository, IBarcodeRepository barcodeRepository, IManageVisitorsRepository manageVisitorsRepository)
        {
            _manageWorkerRepository = manageWorkerRepository;
            _fileManager = fileManager;
            _manageContractorRepository = manageContractorRepository;
            _assignGateNoRepository = assignGateNoRepository;
            _barcodeRepository = barcodeRepository;
            _manageVisitorsRepository = manageVisitorsRepository;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveWorker(Worker_Request parameters)
        {
            /*
            #region User Restriction 

            int vNoofContractedWorker = 0;
            int totalWorkderRegistered = 0;

            if (parameters.Id == 0)
            {
                var vWorkerSearch = new WorkerSearch_Request();
                vWorkerSearch.ContractorId = 0;
                vWorkerSearch.BranchId = 0;
                vWorkerSearch.IsBlackList = false;
                vWorkerSearch.IsActive = true;

                var vWorker = await _manageWorkerRepository.GetWorkerList(vWorkerSearch);

                #region Contractor Wise Worker Check

                if (parameters.ContractorId > 0)
                {
                    //get total worker count
                    totalWorkderRegistered = vWorker.Where(x => x.ContractorId == parameters.ContractorId).Count();

                    //get total NoofContractedWorkers 
                    var vContractor = await _manageContractorRepository.GetContractorById(Convert.ToInt32(parameters.ContractorId));
                    if (vContractor != null)
                    {
                        vNoofContractedWorker = vContractor.NoofContractedWorkers ?? 0;
                    }
                }

                // Total Contractor check with register worker
                if (totalWorkderRegistered >= vNoofContractedWorker)
                {
                    _response.Message = "You are not allowed to create worker more than " + vNoofContractedWorker + ", Please contact your administrator to access this feature!";
                    return _response;
                }

                #endregion
            }

            #endregion
            */

            //Document Upload
            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.Document_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.Document_Base64, "\\Uploads\\Worker\\", parameters.DocumentOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.DocumentFileName = vUploadFile;
                }
            }

            //photo Upload
            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.WorkerPhoto_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.WorkerPhoto_Base64, "\\Uploads\\Worker\\", parameters.WorkerPhotoOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.WorkerPhotoFileName = vUploadFile;
                }
            }

            int result = await _manageWorkerRepository.SaveWorker(parameters);

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

                #region // Add/Update Assign GateNo

                // Delete Assign
                var vGateNoDELETEObj = new AssignGateNo_Request()
                {
                    Action = "DELETE",
                    RefId = result,
                    RefType = "Worker",
                    GateDetailsId = 0
                };
                int resultGateNoDELETE = await _assignGateNoRepository.SaveAssignGateNo(vGateNoDELETEObj);


                // add new gate details
                foreach (var vGateitem in parameters.GateNumberList)
                {
                    var vGateNoMapObj = new AssignGateNo_Request()
                    {
                        Action = "INSERT",
                        RefId = result,
                        RefType = "Worker",
                        GateDetailsId = vGateitem.GateDetailsId
                    };

                    int resultGateNo = await _assignGateNoRepository.SaveAssignGateNo(vGateNoMapObj);
                }

                #endregion

                #region Worker Pass

                if (parameters.Id == 0)
                {
                    var vWorkerPass = new WorkerPass_Request()
                    {
                        Id = 0,
                        WorkerId = result,
                        PassNumber = "",
                        ValidFromDate = parameters.ValidFromDate,
                        ValidToDate = parameters.ValidToDate,
                        IsActive = parameters.IsActive
                    };

                    int resultWorkerPass = await _manageWorkerRepository.SaveWorkerPass(vWorkerPass);
                }
                
                #endregion

                #region Generate Barcode
                if (parameters.Id == 0)
                {
                    var vWorker = await _manageWorkerRepository.GetWorkerById(result);
                    if (vWorker != null)
                    {
                        var vGenerateBarcode = _barcodeRepository.GenerateBarcode(vWorker.PassNumber);
                        if (vGenerateBarcode.Barcode_Unique_Id != "")
                        {
                            var vBarcode_Request = new Barcode_Request()
                            {
                                Id = 0,
                                BarcodeNo = vWorker.PassNumber,
                                BarcodeType = "Worker",
                                Barcode_Unique_Id = vGenerateBarcode.Barcode_Unique_Id,
                                BarcodeOriginalFileName = vGenerateBarcode.BarcodeOriginalFileName,
                                BarcodeFileName = vGenerateBarcode.BarcodeFileName,
                                RefId = result
                            };
                            var resultBarcode = _barcodeRepository.SaveBarcode(vBarcode_Request);
                        }
                    }
                }
                #endregion
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetWorkerList(WorkerSearch_Request parameters)
        {
            IEnumerable<Worker_Response> lstWorkers = await _manageWorkerRepository.GetWorkerList(parameters);
            _response.Data = lstWorkers.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetWorkerById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _manageWorkerRepository.GetWorkerById(Id);
                if(vResultObj != null)
                {
                    var gateNolistObj = await _assignGateNoRepository.GetAssignGateNoById(vResultObj.Id, "Worker", 0);
                    vResultObj.GateNumberList = gateNolistObj.ToList();
                }
                _response.Data = vResultObj;
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveWorkerPass(WorkerPass_Request parameters)
        {
            int result = await _manageWorkerRepository.SaveWorkerPass(parameters);

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

                #region Generate Barcode
                if (parameters.Id == 0)
                {
                    var vWorker = await _manageWorkerRepository.GetWorkerById(Convert.ToInt32(parameters.WorkerId));
                    if (vWorker != null)
                    {
                        var vGenerateBarcode = _barcodeRepository.GenerateBarcode(vWorker.PassNumber);
                        if (vGenerateBarcode.Barcode_Unique_Id != "")
                        {
                            var vBarcode_Request = new Barcode_Request()
                            {
                                Id = 0,
                                BarcodeNo = vWorker.PassNumber,
                                BarcodeType = "Worker",
                                Barcode_Unique_Id = vGenerateBarcode.Barcode_Unique_Id,
                                BarcodeOriginalFileName = vGenerateBarcode.BarcodeOriginalFileName,
                                BarcodeFileName = vGenerateBarcode.BarcodeFileName,
                                RefId = result
                            };
                            var resultBarcode = _barcodeRepository.SaveBarcode(vBarcode_Request);
                        }
                    }
                }
                #endregion
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ExportWorkerAttendanceData(WorkerSearch_Request parameters)
        {
            _response.IsSuccess = false;
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            IEnumerable<Worker_Response> lstSizeObj = await _manageWorkerRepository.GetWorkerList(parameters);

            using (MemoryStream msExportDataFile = new MemoryStream())
            {
                using (ExcelPackage excelExportData = new ExcelPackage())
                {
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("WorkerAttendance");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "SR.NO";
                    WorkSheet1.Cells[1, 2].Value = "WORKER ID";
                    WorkSheet1.Cells[1, 3].Value = "WORKER NAME";
                    WorkSheet1.Cells[1, 4].Value = "BRANCH";
                    WorkSheet1.Cells[1, 5].Value = "GATE NO";
                    WorkSheet1.Cells[1, 6].Value = "STATUS";
                    WorkSheet1.Cells[1, 7].Value = "REMARK";
                    WorkSheet1.Cells[1, 8].Value = "CREATED DATE";
                    WorkSheet1.Cells[1, 9].Value = "CREATED BY";

                    recordIndex = 2;

                    int i = 1;

                    foreach (var items in lstSizeObj)
                    {
                        //log history list
                        var vCheckedInOutLogHistory_Search = new CheckedInOutLogHistory_Search();
                        vCheckedInOutLogHistory_Search.RefId = items.Id;
                        vCheckedInOutLogHistory_Search.RefType = "Worker";
                        vCheckedInOutLogHistory_Search.GateDetailsId = 0;
                        vCheckedInOutLogHistory_Search.IsReject = null;

                        int j = 0;
                        IEnumerable<CheckedInOutLogHistory_Response> lstMUserObj = await _manageVisitorsRepository.GetCheckedInOutLogHistoryList(vCheckedInOutLogHistory_Search);
                        if (lstMUserObj.ToList().Count > 0)
                        {
                            foreach (var mitems in lstMUserObj)
                            {
                                if (j == 0)
                                {
                                    WorkSheet1.Cells[recordIndex, 1].Value = i.ToString();
                                }
                                else
                                {
                                    WorkSheet1.Cells[recordIndex, 1].Value = i + "." + j;
                                }
                                WorkSheet1.Cells[recordIndex, 2].Value = items.WorkerId;
                                WorkSheet1.Cells[recordIndex, 3].Value = items.WorkerName == null ? "" : items.WorkerName.ToUpper();
                                WorkSheet1.Cells[recordIndex, 4].Value = items.BranchName == null ? "" : items.BranchName.ToUpper();
                                WorkSheet1.Cells[recordIndex, 5].Value = mitems.GateNumber;
                                WorkSheet1.Cells[recordIndex, 6].Value = mitems.CheckedStatus == null ? "" : mitems.CheckedStatus.ToUpper();
                                WorkSheet1.Cells[recordIndex, 7].Value = mitems.CheckedRemark == null ? "" : mitems.CheckedRemark.ToUpper();
                                WorkSheet1.Cells[recordIndex, 8].Value = Convert.ToDateTime(mitems.CreatedDate).ToString("dd/MM/yyyy");
                                WorkSheet1.Cells[recordIndex, 9].Value = mitems.CreatorName == null ? "" : mitems.CreatorName.ToUpper();

                                recordIndex += 1;

                                j++;
                            }
                        }
                        else
                        {
                            WorkSheet1.Cells[recordIndex, 1].Value = i.ToString();
                            WorkSheet1.Cells[recordIndex, 2].Value = items.WorkerId;
                            WorkSheet1.Cells[recordIndex, 3].Value = items.WorkerName == null ? "" : items.WorkerName.ToUpper();
                            WorkSheet1.Cells[recordIndex, 4].Value = items.BranchName == null ? "" : items.BranchName.ToUpper();

                            recordIndex += 1;
                        }

                        i++;
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

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ExportWorkerData(WorkerSearch_Request parameters)
        {
            _response.IsSuccess = false;
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            IEnumerable<Worker_Response> lstData = await _manageWorkerRepository.GetWorkerList(parameters);

            using (MemoryStream msExportDataFile = new MemoryStream())
            {
                using (ExcelPackage excelExportData = new ExcelPackage())
                {
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("Worker");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "WorkerName";
                    WorkSheet1.Cells[1, 2].Value = "Worker Pass ID";
                    WorkSheet1.Cells[1, 3].Value = "Worker Type";
                    WorkSheet1.Cells[1, 4].Value = "Mobile Number";
                    WorkSheet1.Cells[1, 5].Value = "Valid From";
                    WorkSheet1.Cells[1, 6].Value = "Valid To";
                    WorkSheet1.Cells[1, 7].Value = "Date of Birth";
                    WorkSheet1.Cells[1, 8].Value = "Blood Group";
                    WorkSheet1.Cells[1, 9].Value = "Identification Mark";
                    WorkSheet1.Cells[1, 10].Value = "Branch";
                    WorkSheet1.Cells[1, 11].Value = "WorkPlace";
                    WorkSheet1.Cells[1, 12].Value = "Gate Number";
                    WorkSheet1.Cells[1, 13].Value = "Address";
                    WorkSheet1.Cells[1, 14].Value = "Country";
                    WorkSheet1.Cells[1, 15].Value = "State";
                    WorkSheet1.Cells[1, 16].Value = "Province";
                    WorkSheet1.Cells[1, 17].Value = "Pincode";
                    WorkSheet1.Cells[1, 18].Value = "Document Type";
                    WorkSheet1.Cells[1, 19].Value = "Document Number";
                    WorkSheet1.Cells[1, 20].Value = "Blacklisted?";
                    WorkSheet1.Cells[1, 21].Value = "Status";
                    WorkSheet1.Cells[1, 22].Value = "CreatedDate";
                    WorkSheet1.Cells[1, 23].Value = "CreatedBy";

                    recordIndex = 2;

                    foreach (var items in lstData)
                    {
                        string strGateNumberList = string.Empty;

                        var vSecurityGateDetail = await _assignGateNoRepository.GetAssignGateNoById(RefId: Convert.ToInt32(items.Id), "Worker", GateDetailsId: 0);
                        if (vSecurityGateDetail.ToList().Count > 0)
                        {
                            strGateNumberList = string.Join(",", vSecurityGateDetail.ToList().Select(x => x.GateNumber));
                        }

                        WorkSheet1.Cells[recordIndex, 1].Value = items.WorkerName;
                        WorkSheet1.Cells[recordIndex, 2].Value = items.WorkerId;
                        WorkSheet1.Cells[recordIndex, 3].Value = items.WorkerType;
                        WorkSheet1.Cells[recordIndex, 4].Value = items.WorkerMobileNo;
                        WorkSheet1.Cells[recordIndex, 5].Value = items.ValidFromDate.HasValue ? items.ValidFromDate.Value.ToString("dd/MM/yyyy") : string.Empty;
                        WorkSheet1.Cells[recordIndex, 6].Value = items.ValidToDate.HasValue ? items.ValidToDate.Value.ToString("dd/MM/yyyy") : string.Empty;
                        WorkSheet1.Cells[recordIndex, 7].Value = items.DOB.HasValue ? items.DOB.Value.ToString("dd/MM/yyyy") : string.Empty;
                        WorkSheet1.Cells[recordIndex, 8].Value = items.BloodGroup;
                        WorkSheet1.Cells[recordIndex, 9].Value = items.IdentificationMark;
                        WorkSheet1.Cells[recordIndex, 10].Value = items.BranchName;
                        WorkSheet1.Cells[recordIndex, 11].Value = items.WorkPlace;
                        WorkSheet1.Cells[recordIndex, 12].Value = strGateNumberList;
                        WorkSheet1.Cells[recordIndex, 13].Value = items.Address;
                        WorkSheet1.Cells[recordIndex, 14].Value = items.CountryName;
                        WorkSheet1.Cells[recordIndex, 15].Value = items.StateName;
                        WorkSheet1.Cells[recordIndex, 16].Value = items.DistrictName;
                        WorkSheet1.Cells[recordIndex, 17].Value = items.Pincode;
                        WorkSheet1.Cells[recordIndex, 18].Value = items.IDType;
                        WorkSheet1.Cells[recordIndex, 19].Value = items.DocumentNumber;
                        WorkSheet1.Cells[recordIndex, 20].Value = items.IsBlackList == true ? "Yes" : "No";
                        WorkSheet1.Cells[recordIndex, 21].Value = items.IsActive == true ? "Active" : "Inactive";
                        WorkSheet1.Cells[recordIndex, 22].Value = Convert.ToDateTime(items.CreatedDate).ToString("dd/MM/yyyy");
                        WorkSheet1.Cells[recordIndex, 23].Value = items.CreatorName;

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
