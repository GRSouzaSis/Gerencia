﻿using System.Web.Mvc;

namespace GerenciaPub.Controllers
{
    //[Authorize(Roles ="Gerente")]
    public class CadastroController : Controller
    {
       
        [Authorize]
        public ActionResult MarcaProduto()
        {
            return View();
        }
        [Authorize]
        public ActionResult UnidadeMedida()
        {
            return View();
        }
        [Authorize]
        public ActionResult Produto()
        {
            return View();
        }
        [Authorize]
        public ActionResult Fornecedor()
        {
            return View();
        }
    }
}