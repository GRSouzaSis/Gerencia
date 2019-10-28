using GerenciaPub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GerenciaPub.Controllers
{
    [Authorize(Roles = "Gerente")]
    public class CadCidadeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.ListaUf = UfModel.RecuperarLista();
            return View(CidadeModel.RecuperarLista());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CidadePagina(string filtro)
        {
            var lista = CidadeModel.RecuperarLista(filtro);

            return Json(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecuperarCidade(int id)
        {
            return Json(CidadeModel.RecuperarPeloId(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirCidade(int id)
        {
            return Json(CidadeModel.ExcluirPeloId(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SalvarCidade(CidadeModel model)
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