using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NerdStore.Enterprise.WebAPI.Core.Controllers;
using NerdStore.Enterprise.Pedido.API.Application.DTO;
using NerdStore.Enterprise.Pedido.API.Application.Queries;

namespace NerdStore.Enterprise.Pedido.API.Controllers
{
    [Authorize]
    public class VoucherController : MainController
    {
        public VoucherController(IVoucherQueries voucherQueries)
        {
            _voucherQueries = voucherQueries;
        }

        private readonly IVoucherQueries _voucherQueries;

        [HttpGet("voucher/{codigo}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(VoucherDTO), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ObterPorCodigo(string codigo)
        {
            if (string.IsNullOrEmpty(codigo)) return NotFound();

            var voucher = await _voucherQueries.ObterVoucherPorCodigo(codigo);

            return voucher == null ? NotFound() : CustomResponse(voucher);
        }
    }
}