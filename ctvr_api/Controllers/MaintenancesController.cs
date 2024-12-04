using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ctvr_api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MaintenancesController : ControllerBase
    {
        private IConfiguration _configuration;

        public MaintenancesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


    }
}
