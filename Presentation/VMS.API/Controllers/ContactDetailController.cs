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
    public class ContactDetailController : CustomBaseController
    {
        private ResponseModel _response;
        private IFileManager _fileManager;
        private readonly IContactDetailRepository _contactDetailRepository;

        public ContactDetailController(IFileManager fileManager, IContactDetailRepository contactDetailRepository)
        {
            _fileManager = fileManager;
            _contactDetailRepository = contactDetailRepository;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveContactDetail(ContactDetail_Request parameters)
        {
            if (string.IsNullOrEmpty(parameters.RefType))
            {
                _response.IsSuccess = false;
                _response.Message = "RefType is required.";
                return _response;
            }

            // Image Upload
            if (parameters! != null && !string.IsNullOrWhiteSpace(parameters.AadharCardImage_Base64))
            {
                var vUploadFile_AadharCardImage = _fileManager.UploadDocumentsBase64ToFile(parameters.AadharCardImage_Base64, "\\Uploads\\" + parameters.RefType + "\\", parameters.AadharCardOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile_AadharCardImage))
                {
                    parameters.AadharCardImageFileName = vUploadFile_AadharCardImage;
                }
            }

            int result = await _contactDetailRepository.SaveContactDetail(parameters);

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
        public async Task<ResponseModel> GetContactDetailList(ContactDetail_Search parameters)
        {
            var objList = await _contactDetailRepository.GetContactDetailList(parameters);
            _response.Data = objList.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetContactDetailById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _contactDetailRepository.GetContactDetailById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }
    }
}
