using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GerenciaPub.Controllers
{
    public class RelatorioController : Controller
    {
        [Authorize]
        public ActionResult Ressuprimento()
        {
            return View();
        }
        [Authorize]
        public ActionResult EstoqueProduto()
        {
            return View();
        }
    }
}