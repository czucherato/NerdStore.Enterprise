using System;
using FluentValidation;
using System.Collections.Generic;
using NerdStore.Enterprise.Core.Messages;
using NerdStore.Enterprise.Pedido.API.Application.DTO;

namespace NerdStore.Enterprise.Pedido.API.Application.Commands
{
    public class AdicionarPedidoCommand : Command
    {
        public Guid ClienteId { get; set; }

        public decimal ValorTotal { get; set; }

        public List<PedidoItemDTO> PedidoItems { get; set; }

        public string VoucherCodigo { get; set; }

        public bool VoucherUtilizado { get; set; }

        public decimal Desconto { get; set; }

        public EnderecoDTO Endereco { get; set; }

        public string NumeroCartao { get; set; }

        public string NomeCartao { get; set; }

        public string ExpiracaoCartao { get; set; }

        public string CvvCartao { get; set; }

        public override bool EhValido()
        {
            ValidationResult = new AdicionarPedidoValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class AdicionarPedidoValidation : AbstractValidator<AdicionarPedidoCommand>
        {
            public AdicionarPedidoValidation()
            {
                RuleFor(c => c.ClienteId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Id do cliente inválido");

                RuleFor(c => c.PedidoItems.Count)
                    .GreaterThan(0)
                    .WithMessage("O pedido precisa ter no mínimo 1 item");

                RuleFor(c => c.ValorTotal)
                    .GreaterThan(0)
                    .WithMessage("Valor do pedido inválido");

                RuleFor(c => c.NumeroCartao)
                    .CreditCard()
                    .WithMessage("Número do cartão inválido");

                RuleFor(c => c.NomeCartao)
                    .NotNull()
                    .WithMessage("Nome do portador do cartão requerido");

                RuleFor(c => c.CvvCartao.Length)
                    .GreaterThan(2)
                    .LessThan(5)
                    .WithMessage("O CVV do cartão precisa ter 3 ou 4 números");

                RuleFor(c => c.ExpiracaoCartao)
                    .NotNull()
                    .WithMessage("Data de expiração do cartão requerida");
            }
        }
    }
}