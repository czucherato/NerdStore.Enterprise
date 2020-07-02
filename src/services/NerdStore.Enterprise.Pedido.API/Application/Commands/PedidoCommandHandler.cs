using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using NerdStore.Enterprise.Core.Messages;
using NerdStore.Enterprise.Pedido.Domain.Pedidos;
using NerdStore.Enterprise.Pedido.Domain.Vouchers;
using NerdStore.Enterprise.Pedido.API.Application.DTO;
using NerdStore.Enterprise.Pedido.Domain.Vouchers.Specs;
using NerdStore.Enterprise.Pedido.API.Application.Events;

namespace NerdStore.Enterprise.Pedido.API.Application.Commands
{
    public class PedidoCommandHandler : CommandHandler,
        IRequestHandler<AdicionarPedidoCommand, ValidationResult>
    {
        public PedidoCommandHandler(
            IPedidoRepository pedidoRepository,
            IVoucherRepository voucherRepository)
        {
            _pedidoRepository = pedidoRepository;
            _voucherRepository = voucherRepository;
        }

        private readonly IPedidoRepository _pedidoRepository;
        private readonly IVoucherRepository _voucherRepository;        

        public async Task<ValidationResult> Handle(AdicionarPedidoCommand message, CancellationToken cancellationToken)
        {
            if (!message.EhValido()) return message.ValidationResult;

            var pedido = MapearPedido(message);

            if (!await AplicarVoucher(message, pedido)) return ValidationResult;

            if (!ValidarPedido(pedido)) return ValidationResult;

            if (!ProcessarPagamento(pedido)) return ValidationResult;

            pedido.AutorizarPedido();

            pedido.AdicionarEvento(new PedidoRealizadoEvent(pedido.Id, pedido.ClienteId));

            _pedidoRepository.Adicionar(pedido);

            return await PersistirDados(_pedidoRepository.UnitOfWork);
        }

        private Domain.Pedidos.Pedido MapearPedido(AdicionarPedidoCommand message)
        {
            var endereco = new Endereco
            {
                Logradouro = message.Endereco.Logradouro,
                Numero = message.Endereco.Numero,
                Complemento = message.Endereco.Complemento,
                Bairro = message.Endereco.Bairro,
                Cep = message.Endereco.Cep,
                Cidade = message.Endereco.Cidade,
                Estado = message.Endereco.Estado
            };

            var pedido = new Domain.Pedidos.Pedido(
                message.ClienteId, 
                message.ValorTotal, 
                message.PedidoItems.Select(PedidoItemDTO.ParaPedidoItem).ToList(), 
                message.VoucherUtilizado, 
                message.Desconto);

            pedido.AtribuirEndereco(endereco);

            return pedido;
        }

        private async Task<bool> AplicarVoucher(AdicionarPedidoCommand message, Domain.Pedidos.Pedido pedido)
        {
            if (!message.VoucherUtilizado) return true;

            var voucher = await _voucherRepository.ObterVoucherPorCodigo(message.VoucherCodigo);
            if (voucher == null)
            {
                AdicionarErro("O voucher informado não existe!");
                return false;
            }

            var voucherValidation = new VoucherValidation().Validate(voucher);
            if (!voucherValidation.IsValid)
            {
                voucherValidation.Errors.ToList().ForEach(m => AdicionarErro(m.ErrorMessage));
                return false;
            }

            pedido.AtribuirVoucher(voucher);
            voucher.DebitarQuantidade();

            _voucherRepository.Atualizar(voucher);

            return true;
        }

        private bool ValidarPedido(Domain.Pedidos.Pedido pedido)
        {
            var pedidoValorOriginal = pedido.ValorTotal;
            var pedidoDesconto = pedido.Desconto;

            pedido.CalcularValorPedido();

            if (pedido.ValorTotal != pedidoValorOriginal)
            {
                AdicionarErro("O valor total do pedido não confere com o cálculo do pedido");
                return false;
            }

            if (pedido.Desconto != pedidoDesconto)
            {
                AdicionarErro("O valor total não confere com o cálculo do pedido");
                return false;
            }

            return true;
        }

        private bool ProcessarPagamento(Domain.Pedidos.Pedido pedido)
        {
            return true;
        }
    }
}