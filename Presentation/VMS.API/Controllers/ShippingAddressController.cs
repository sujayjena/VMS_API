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
    public class ShippingAddressController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IShippingAddressRepository _shippingAddressRepository;
        private IFileManager _fileManager;

        public ShippingAddressController(IShippingAddressRepository shippingAddressRepository, IFileManager fileManager)
        {
            _shippingAddressRepository = shippingAddressRepository;

            _response = new ResponseModel();
            _response.IsSuccess = true;
            _fileManager = fileManager;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveShippingAddress(ShippingAddress_Request parameters)
        {
            //GST Upload
            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.GST_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.GST_Base64, "\\Uploads\\ShippingAddress\\", parameters.GSTOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.GSTFileName = vUploadFile;
                }
            }

            int result = await _shippingAddressRepository.SaveShippingAddress(parameters);

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveOperationEnums.ReocrdExists)
            {
                _response.Message = "Record already exists";
            }
            else if (result == (int)SaveOperationEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.Message = "Record details saved sucessfully";
            }

            _response.Id = result;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetShippingAddressList(ShippingAddress_Search parameters)
        {
            var objList = await _shippingAddressRepository.GetShippingAddressList(parameters);
            _response.Data = objList.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetShippingAddressById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _shippingAddressRepository.GetShippingAddressById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }
    }
}

