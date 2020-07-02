using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NerdStore.Enterprise.WebApp.MVC.Models;
using NerdStore.Enterprise.WebApp.MVC.Services;

namespace NerdStore.Enterprise.WebApp.MVC.Controllers
{
    [Authorize]
    public class ClienteController : MainController
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpPost]
        public async Task<IActionResult> NovoEndereco(EnderecoViewModel endereco)
        {
            var response = await _clienteService.AdicionarEndereco(endereco);

            if (ResponsePossuiErros(response)) TempData["Erros"] =
                ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();

            return RedirectToAction("EnderecoEntrega", "Pedido");
        }
    }
}
