using IExproERP.UI.Models;
using System.Web.Mvc;

namespace IExproERP.UI.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected virtual new CustomPrincipal User
        {
            get { return HttpContext.User as CustomPrincipal; }
        }


    }
}