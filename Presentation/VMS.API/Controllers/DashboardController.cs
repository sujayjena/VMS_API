using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VMS.Application.Helpers;
using VMS.Application.Interfaces;
using VMS.Application.Models;

namespace VMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : CustomBaseController
    {
        private ResponseModel _response;
        private IFileManager _fileManager;

        private readonly IDashboardRepository _dashboardRepository;

        public DashboardController(IFileManager fileManager, IDashboardRepository dashboardRepository)
        {
            _fileManager = fileManager;

            _response = new ResponseModel();
            _response.IsSuccess = true;
            _dashboardRepository = dashboardRepository;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetDashboard_TotalSummary(Dashboard_Search_Request parameters)
        {
            var objList = await _dashboardRepository.GetDashboard_TotalSummary(parameters);
            _response.Data = objList.ToList();
            return _response;
        }
    }
}
