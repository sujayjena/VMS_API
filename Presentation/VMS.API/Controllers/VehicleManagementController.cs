using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VMS.Application.Enums;
using VMS.Application.Helpers;
using VMS.Application.Interfaces;
using VMS.Application.Models;

namespace VMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleManagementController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IVehicleManagementRepository _vehicleManagementRepository;
        private IFileManager _fileManager;

        public VehicleManagementController(IVehicleManagementRepository vehicleManagementRepository, IFileManager fileManager)
        {
            _vehicleManagementRepository = vehicleManagementRepository;
            _fileManager = fileManager;

            _response = new ResponseModel();
            _response.IsSuccess = true;
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
