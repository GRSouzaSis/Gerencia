using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GerenciaPub.Controllers
{
    public class OperacaoController : Controller
    {
        [Authorize]
        public ActionResult EntradaProduto()
        {
            return View();
        }
        [Authorize]
        public ActionResult SaidaProduto()
        {
            return View();
        }
        [Authorize]
        public ActionResult PerdaProduto()
        {
            return View();
        }       
    }
}