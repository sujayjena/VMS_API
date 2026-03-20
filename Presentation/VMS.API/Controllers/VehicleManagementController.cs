using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VMS.Application.Enums;
using VMS.Application.Helpers;
using VMS.Application.Interfaces;
using VMS.Application.Models;
using VMS.Persistence.Repositories;

namespace VMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleManagementController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IVehicleManagementRepository _vehicleManagementRepository;
        private IFileManager _fileManager;
        private readonly IAssignGateNoRepository _assignGateNoRepository;
        private readonly IBarcodeRepository _barcodeRepository;

        public VehicleManagementController(IVehicleManagementRepository vehicleManagementRepository, IFileManager fileManager, IAssignGateNoRepository assignGateNoRepository, IBarcodeRepository barcodeRepository)
        {
            _vehicleManagementRepository = vehicleManagementRepository;
            _fileManager = fileManager;

            _response = new ResponseModel();
            _response.IsSuccess = true;
            _assignGateNoRepository = assignGateNoRepository;
            _barcodeRepository = barcodeRepository;
        }

        #region Vehicle Management
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveVehicleManagement(VehicleManagement_Request parameters)
        {
            //Attachment Upload
            if (parameters! != null && !string.IsNullOrWhiteSpace(parameters.Attachment_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.Attachment_Base64, "\\Uploads\\VehicleManagement\\", parameters.AttachmentOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.AttachmentFileName = vUploadFile;
                }
            }

            //Document Upload
            if (parameters! != null && !string.IsNullOrWhiteSpace(parameters.Document_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.Document_Base64, "\\Uploads\\VehicleManagement\\", parameters.DocumentOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.DocumentFileName = vUploadFile;
                }
            }

            int result = await _vehicleManagementRepository.SaveVehicleManagement(parameters);

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

                #region // Add/Update Item

                // Delete Assign
                var vItemDELETEObj = new VehicleManagementItem_Request()
                {
                    Action = "DELETE",
                    VehicleManagementId = result,
                    Itemid = 0
                };
                int resultItemDELETE = await _vehicleManagementRepository.SaveVehicleManagementItem(vItemDELETEObj);


                // add new Item details
                foreach (var vItem in parameters.ItemList)
                {
                    var vItemMapObj = new VehicleManagementItem_Request()
                    {
                        Action = "INSERT",
                        VehicleManagementId = result,
                        Itemid = vItem.Itemid
                    };

                    int resultitem = await _vehicleManagementRepository.SaveVehicleManagementItem(vItemMapObj);
                }

                #endregion

                #region // Add/Update GateNo

                var vVehicleManagement = await _vehicleManagementRepository.GetVehicleManagementById(result);
                if (vVehicleManagement != null)
                {
                    #region // Add/Update Assign GateNo

                    // Delete Assign
                    var vGateNoDELETEObj = new AssignGateNo_Request()
                    {
                        Action = "DELETE",
                        RefId = vVehicleManagement.VisitorId,
                        RefType = "Visitor",
                        GateDetailsId = 0
                    };
                    int resultGateNoDELETE = await _assignGateNoRepository.SaveAssignGateNo(vGateNoDELETEObj);


                    // add new gate details
                    foreach (var vGateitem in parameters.GateNumberList)
                    {
                        var vGateNoMapObj = new AssignGateNo_Request()
                        {
                            Action = "INSERT",
                            RefId = vVehicleManagement.VisitorId,
                            RefType = "Visitor",
                            GateDetailsId = vGateitem.GateDetailsId
                        };

                        int resultGateNo = await _assignGateNoRepository.SaveAssignGateNo(vGateNoMapObj);
                    }

                    #endregion
                }

                #endregion

                #region Generate barcode
                if (parameters.Id == 0)
                {
                    if (vVehicleManagement != null)
                    {
                        var vGenerateBarcode = _barcodeRepository.GenerateBarcode(vVehicleManagement.VisitNumber);
                        if (vGenerateBarcode.Barcode_Unique_Id != "")
                        {
                            var vBarcode_Request = new Barcode_Request()
                            {
                                Id = 0,
                                BarcodeNo = vVehicleManagement.VisitNumber,
                                BarcodeType = "Visitor",
                                Barcode_Unique_Id = vGenerateBarcode.Barcode_Unique_Id,
                                BarcodeOriginalFileName = vGenerateBarcode.BarcodeOriginalFileName,
                                BarcodeFileName = vGenerateBarcode.BarcodeFileName,
                                RefId = vVehicleManagement.VisitorId
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
        public async Task<ResponseModel> GetVehicleManagementList(VehicleManagement_Search parameters)
        {
            IEnumerable<VehicleManagement_Response> lstRoles = await _vehicleManagementRepository.GetVehicleManagementList(parameters);

            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetVehicleManagementById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _vehicleManagementRepository.GetVehicleManagementById(Id);
                if (vResultObj != null)
                {
                    var itemlistObj = await _vehicleManagementRepository.GetVehicleManagementItemById(vResultObj.Id, 0);
                    vResultObj.ItemList = itemlistObj.ToList();

                    var gateNolistObj = await _assignGateNoRepository.GetAssignGateNoById(Convert.ToInt32(vResultObj.VisitorId), "Visitor", 0);
                    vResultObj.GateNumberList = gateNolistObj.ToList();
                }

                _response.Data = vResultObj;
            }
            return _response;
        }
        #endregion

        #region Material Weight
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveMaterialWeight(MaterialWeight_Request parameters)
        {
            int result = await _vehicleManagementRepository.SaveMaterialWeight(parameters);

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
        public async Task<ResponseModel> GetMaterialWeightList(MaterialWeight_Search parameters)
        {
            IEnumerable<MaterialWeight_Response> lstRoles = await _vehicleManagementRepository.GetMaterialWeightList(parameters);

            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetMaterialWeightById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _vehicleManagementRepository.GetMaterialWeightById(Id);

                _response.Data = vResultObj;
            }
            return _response;
        }
        #endregion
    }
}
