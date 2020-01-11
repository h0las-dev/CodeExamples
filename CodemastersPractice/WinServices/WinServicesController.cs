namespace BailManagement.Web.Controllers
{
    using System.Web.Http;
    using BailManagement.Common.Services.WinServices;
    using BailManagement.Common.Domain.WinServices;

    public class WinServicesController : OperationController
    {
        private readonly IWinServicesManager _winServicesManager;

        public WinServicesController(IWinServicesManager winServicesManager)
        {
            _winServicesManager = winServicesManager;
        }

        [HttpGet]
        public WinService[] GetWinServices()
        {
            return _winServicesManager.GetWinServices();
        }

        [HttpPost]
        public void ChangeServiceStatus(WinService service)
        {
            _winServicesManager.ChangeWinServiceStatus(service);
        }
    }
}