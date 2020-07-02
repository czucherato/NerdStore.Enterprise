using System;
using NerdStore.Enterprise.Core.Messages;

namespace NerdStore.Enterprise.Pedido.API.Application.Events
{
    public class PedidoRealizadoEvent : Event
    {
        public PedidoRealizadoEvent(Guid pedidoId, Guid clienteId)
        {
            PedidoId = pedidoId;
            ClienteId = clienteId;
        }

        public Guid PedidoId { get; private set; }

        public Guid ClienteId { get; private set; }
    }
}