using GerenciaPub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GerenciaPub.Controllers
{
    [Authorize(Roles = "Gerente")]
    public class CadUfController : Controller
    {
        public ActionResult Index()
        {
            return View(UfModel.RecuperarLista());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecuperarEstado(int id)
        {
            return Json(UfModel.RecuperarPeloId(id));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirUf(int id)
        {
            return Json(UfModel.ExcluirPeloId(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SalvarUf(UfModel model)
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var idSalvo = string.Empty;

            if (!ModelState.IsValid)
            {
                resultado = "AVISO";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            }
            else
            {
                try
                {
                    var id = model.Salvar();
                    if (id > 0)
                    {
                        idSalvo = id.ToString();
                    }
                    else
                    {
                        resultado = "ERRO";
                    }
                }
                catch (Exception ex)
                {
                    resultado = "ERRO";
                }
            }

            return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo });
        }
    }
}