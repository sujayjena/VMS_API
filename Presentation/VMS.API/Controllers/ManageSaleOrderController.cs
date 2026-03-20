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
    public class ManageSaleOrderController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IManageSaleOrderRepository _manageSaleOrderRepository;
        private IFileManager _fileManager;

        public ManageSaleOrderController(IManageSaleOrderRepository manageSaleOrderRepository, IFileManager fileManager)
        {
            _manageSaleOrderRepository = manageSaleOrderRepository;
            _fileManager = fileManager;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        #region PO Received
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SavePOReceived(POReceived_Request parameters)
        {
            // PO Received Upload
            if (parameters! != null && !string.IsNullOrWhiteSpace(parameters.POReceived_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.POReceived_Base64, "\\Uploads\\POReceived\\", parameters.POReceivedOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.POReceivedFileName = vUploadFile;
                }
            }

            int result = await _manageSaleOrderRepository.SavePOReceived(parameters);

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
        public async Task<ResponseModel> GetPOReceivedList(POReceivedSearch_Request parameters)
        {
            IEnumerable<POReceived_Response> lstPOReceiveds = await _manageSaleOrderRepository.GetPOReceivedList(parameters);
            _response.Data = lstPOReceiveds.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetPOReceivedById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _manageSaleOrderRepository.GetPOReceivedById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }
        #endregion
    }
}
