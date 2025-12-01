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
    public class ManageStayController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IManageStayRepository _manageStayRepository;
        private IFileManager _fileManager;

        public ManageStayController(IManageStayRepository manageStayRepository, IFileManager fileManager)
        {
            _manageStayRepository = manageStayRepository;
            _fileManager = fileManager;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveWorkerStay(WorkerStay_Request parameters)
        {
            int result = await _manageStayRepository.SaveWorkerStay(parameters);

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
        public async Task<ResponseModel> GetWorkerStayList(WorkerStay_Search parameters)
        {
            IEnumerable<WorkerStay_Response> lstRoles = await _manageStayRepository.GetWorkerStayList(parameters);

            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetWorkerStayById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _manageStayRepository.GetWorkerStayById(Id);

                _response.Data = vResultObj;
            }
            return _response;
        }
    }
}
