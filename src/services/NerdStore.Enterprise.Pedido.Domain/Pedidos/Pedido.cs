﻿using System;
using System.Linq;
using System.Collections.Generic;
using NerdStore.Enterprise.Core.DomainObjects;
using NerdStore.Enterprise.Pedido.Domain.Vouchers;

namespace NerdStore.Enterprise.Pedido.Domain.Pedidos
{
    public class Pedido : Entity, IAggregateRoot
    {
        protected Pedido() { }

        public Pedido(Guid clienteId, decimal valorTotal, List<PedidoItem> pedidoItems, bool voucherUtilizado = false, decimal desconto = 0, Guid? voucherId = null)
        {
            Id = Guid.NewGuid();
            ClienteId = clienteId;
            VoucherUtilizado = voucherUtilizado;
            Desconto = desconto;
            ValorTotal = valorTotal;
            _pedidoItems = pedidoItems;
            VoucherId = voucherId;
        }

        public int Codigo { get; private set; }

        public Guid ClienteId { get; private set; }

        public bool VoucherUtilizado { get; private set; }

        public decimal Desconto { get; private set; }

        public decimal ValorTotal { get; private set; }

        public DateTime DataCadastro { get; private set; }

        public PedidoStatus PedidoStatus { get; private set; }

        private readonly List<PedidoItem> _pedidoItems;

        public IReadOnlyCollection<PedidoItem> PedidoItems => _pedidoItems;

        public Endereco Endereco { get; private set; }

        public Guid? VoucherId { get; private set; }

        public Voucher Voucher { get; private set; }

        public void AutorizarPedido()
        {
            PedidoStatus = PedidoStatus.Autorizado;
        }

        public void AtribuirVoucher(Voucher voucher)
        {
            VoucherUtilizado = true;
            VoucherId = voucher.Id;
            Voucher = voucher;
        }

        public void AtribuirEndereco(Endereco endereco)
        {
            Endereco = endereco;
        }

        public void CalcularValorPedido()
        {
            ValorTotal = PedidoItems.Sum(p => p.CalcularValor());
            CalcularValorTotalDesconto();
        }

        public void CalcularValorTotalDesconto()
        {
            if (!VoucherUtilizado) return;

            decimal desconto = 0;
            var valor = ValorTotal;

            if (Voucher.TipoDescontoVoucher == TipoDescontoVoucher.Porcentagem)
            {
                if (Voucher.Percentual.HasValue)
                {
                    desconto = (valor * Voucher.Percentual.Value) / 100;
                    valor -= desconto;
                }
            }
            else
            {
                if (Voucher.ValorDesconto.HasValue)
                {
                    desconto = Voucher.ValorDesconto.Value;
                    valor -= desconto;
                }
            }

            ValorTotal = valor < 0 ? 0 : valor;
            Desconto = desconto;
        }

        public void FinalizarPedido()
        {
            PedidoStatus = PedidoStatus.Pago;
        }

        public void CancelarPedido()
        {
            PedidoStatus = PedidoStatus.Cancelado;
        }
    }
}