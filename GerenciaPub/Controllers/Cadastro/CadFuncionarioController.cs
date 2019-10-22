using GerenciaPub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GerenciaPub.Controllers.Cadastro
{
    [Authorize(Roles = "Gerente")]
    public class CadFuncionarioController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.ListaFuncionario = PessoaModel.RecuperarListaAtivos();
            return View(FuncionarioModel.RecuperarLista());           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecuperarFuncionario(int id)
        {
            return Json(FuncionarioModel.RecuperarPeloId(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirFuncionario(int id)
        {
            return Json(FuncionarioModel.ExcluirPeloId(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SalvarFuncionario(FuncionarioModel model)
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
