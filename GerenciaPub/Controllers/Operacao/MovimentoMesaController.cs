using GerenciaPub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GerenciaPub.Controllers
{
    [Authorize(Roles = "Gerente,Operador")]
    public class MovimentoMesaController : Controller
    {
        public ActionResult Index()
        {
            return View(ViewMesaModel.RecuperarLista());

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult MesaPagina(string filtro)
        {
            var lista = ViewMesaModel.RecuperarLista(filtro);

            return Json(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult RecuperarMesa(int id)
        {
            return Json(ViewMesaModel.RecuperarPeloId(id));
        }
        /*
         [HttpPost]
         [Authorize(Roles = "Gerente")]
         [ValidateAntiForgeryToken]
         public JsonResult ExcluirMesa(int id)
         {
             return Json(ViewMesaModel.ExcluirPeloId(id));
         }*/

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SalvarMesa(ViewMesaModel model)
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
