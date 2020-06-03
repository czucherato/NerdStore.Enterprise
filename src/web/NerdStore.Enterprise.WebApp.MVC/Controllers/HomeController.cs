using Microsoft.AspNetCore.Mvc;
using NerdStore.Enterprise.WebApp.MVC.Models;

namespace NerdStore.Enterprise.WebApp.MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("sistema-indisponivel")]
        public IActionResult SistemaIndisponivel()
        {
            var modelError = new ErrorViewModel
            {
                Mensagem = "O sistema está temporariamente indisponível, isto pode ocorrer em momentos de sobrecarga de usuários.",
                Titulo = "Sistema indisponível",
                ErrorCode = 500
            };

            return View("Error", modelError);
        }

        [Route("erro/{id:length(3,3)}")]
        public IActionResult Error(int id)
        {
            var modelError = new ErrorViewModel();
            if (id == 500)
            {
                modelError.ErrorCode = id;
                modelError.Titulo = "Ocorreu um erro!";
                modelError.Mensagem = "Ocorreu um erro! Tente novamente mais tarde ou contate o nosso suporte.";
            }
            else if (id == 404)
            {
                modelError.ErrorCode = id;
                modelError.Titulo = "Ops! Página não encontrada.";
                modelError.Mensagem = "A página que está procurando não existe! <br />Em caso de dúvidas entre em contato com o nosso suporte.";
            }
            else if (id == 403)
            {
                modelError.ErrorCode = id;
                modelError.Titulo = "Acesso negado!";
                modelError.Mensagem = "Você não tem permissão para fazer isto.";
            }
            else
            {
                return StatusCode(404);
            }

            return View("Error", modelError);
        }
    }
}