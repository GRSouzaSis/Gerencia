using GerenciaPub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GerenciaPub.Controllers
{
    [Authorize(Roles = "Gerente")]
    public class CadSubGrupoProdutoController : Controller
    {
        public ActionResult Index()
        {
            return View(SubGrupoProdutoModel.RecuperarLista());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SubGrupoProdutoPagina(string filtro)
        {
            var lista = SubGrupoProdutoModel.RecuperarLista(filtro);

            return Json(lista);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecuperarSubGrupoProduto(int id)
        {
            return Json(SubGrupoProdutoModel.RecuperarPeloId(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirSubGrupoProduto(int id)
        {
            return Json(SubGrupoProdutoModel.ExcluirPeloId(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SalvarSubGrupoProduto(SubGrupoProdutoModel model)
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