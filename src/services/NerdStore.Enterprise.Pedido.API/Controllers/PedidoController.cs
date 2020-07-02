using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NerdStore.Enterprise.Core.Mediator;
using NerdStore.Enterprise.WebAPI.Core.Usuario;
using NerdStore.Enterprise.WebAPI.Core.Controllers;
using NerdStore.Enterprise.Pedido.API.Application.Queries;
using NerdStore.Enterprise.Pedido.API.Application.Commands;

namespace NerdStore.Enterprise.Pedido.API.Controllers
{
    [Authorize]
    public class PedidoController : MainController
    {
        public PedidoController(
            IAspNetUser user, 
            IPedidoQueries pedidoQueries, 
            IMediatorHandler mediatorHandler)
        {
            _user = user;
            _pedidoQueries = pedidoQueries;
            _mediatorHandler = mediatorHandler;
        }

        private readonly IAspNetUser _user;

        private readonly IPedidoQueries _pedidoQueries;

        private readonly IMediatorHandler _mediatorHandler;

        [HttpPost("pedido")]
        public async Task<IActionResult> AdicionarPedido(AdicionarPedidoCommand pedido)
        {
            pedido.ClienteId = _user.ObterUserId();
            return CustomResponse(await _mediatorHandler.EnviarComando(pedido));
        }

        [HttpGet("pedido/ultimo")]
        public async Task<IActionResult> UltimoPedido()
        {
            var pedido = await _pedidoQueries.ObterUltimoPedido(_user.ObterUserId());
            return pedido == null ? NotFound() : CustomResponse(pedido);
        }

        [HttpGet("pedido/lista-cliente")]
        public async Task<IActionResult> ListaPorCliente()
        {
            var pedidos = await _pedidoQueries.ObterListaPorClienteId(_user.ObterUserId());
            return pedidos == null ? NotFound() : CustomResponse(pedidos);
        }
    }
}