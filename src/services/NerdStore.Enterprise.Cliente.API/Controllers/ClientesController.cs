using System;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Enterprise.Core.Mediator;
using Microsoft.AspNetCore.Authorization;
using NerdStore.Enterprise.WebAPI.Core.Controllers;
using NerdStore.Enterprise.Cliente.API.Application.Commands;
using System.Threading.Tasks;

namespace NerdStore.Enterprise.Cliente.API.Controllers
{
    //[Authorize]
    public class ClientesController : MainController
    {
        public ClientesController(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        private readonly IMediatorHandler _mediatorHandler;

        [HttpGet("clientes")]
        public async Task<IActionResult> Index()
        {
            var resultado = await _mediatorHandler.EnviarComando(new RegistrarClienteCommand(
                Guid.NewGuid(),
                "Carlos Alberto Zucherato",
                "carlos.zucheratto@gmail.com",
                "22338338804"));

            return CustomResponse(resultado);
        }
    }
}