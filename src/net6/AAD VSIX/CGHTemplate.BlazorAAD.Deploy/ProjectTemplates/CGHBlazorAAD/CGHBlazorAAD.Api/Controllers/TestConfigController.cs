namespace $safeprojectname$.Controllers
{
    using $ext_safeprojectname$.Shared.Constants;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Text;

    //[AllowAnonymous]
    [Authorize(Policy = Consts.ACCESS_ADMIN)]
    [Route("api/[controller]")]
    [ApiController]
    public class TestConfigController : Controller
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public TestConfigController(ILogger<TestConfigController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var DefaultConnection = _configuration.GetConnectionString("DefaultConnection");

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"DefaultConnection: {DefaultConnection}");

            return Ok(sb.ToString());
        }
    }
}
