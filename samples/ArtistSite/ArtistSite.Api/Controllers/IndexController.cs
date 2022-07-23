using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace ArtistSite.Api.Controllers
{
    [AllowAnonymous]
    [Route("/")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class IndexController : Controller
    {
        public string AppVersion
        {
            get
            {
                string retVal = Assembly.GetExecutingAssembly().
                    GetCustomAttribute<AssemblyInformationalVersionAttribute>().
                    InformationalVersion;

                return retVal;
            }
        }

        public ActionResult<string> Index()
        {
            var retVal = $"Version {AppVersion}";

            return retVal;
        }
    }
}