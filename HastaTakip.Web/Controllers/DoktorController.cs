using HastaTakip.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HastaTakip.Web.Controllers
{

    [Authorize]
    public class DoktorController: Controller
    {
        private readonly DoktorBusiness _doktorBusiness;

        public DoktorController(DoktorBusiness doktorBusiness)
        {
            _doktorBusiness= doktorBusiness;

        }
    }
}
