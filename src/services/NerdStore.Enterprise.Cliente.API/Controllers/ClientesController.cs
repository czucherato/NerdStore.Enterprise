using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Enterprise.Core.Mediator;
using Microsoft.AspNetCore.Authorization;
using NerdStore.Enterprise.Cliente.API.Models;
using NerdStore.Enterprise.WebAPI.Core.Usuario;
using NerdStore.Enterprise.WebAPI.Core.Controllers;
using NerdStore.Enterprise.Cliente.API.Application.Commands;

namespace NerdStore.Enterprise.Cliente.API.Controllers
{
    [Authorize]
    public class ClientesController : MainController
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IMediatorHandler _mediator;
        private readonly IAspNetUser _user;

        public ClientesController(IClienteRepository clienteRepository, IMediatorHandler mediator, IAspNetUser user)
        {
            _clienteRepository = clienteRepository;
            _mediator = mediator;
            _user = user;
        }

        [HttpGet("cliente/endereco")]
        public async Task<IActionResult> ObterEndereco()
        {
            var endereco = await _clienteRepository.ObterEnderecoPorId(_user.ObterUserId());

            return endereco == null ? NotFound() : CustomResponse(endereco);
        }

        [HttpPost("cliente/endereco")]
        public async Task<IActionResult> AdicionarEndereco(AdicionarEnderecoCommand endereco)
        {
            endereco.ClienteId = _user.ObterUserId();
            return CustomResponse(await _mediator.EnviarComando(endereco));
        }
    }
}