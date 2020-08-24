using System;
using System.Collections.Generic;

namespace NerdStore.Enterprise.Core.Messages.Integration
{
    public class PedidoAutorizadoIntegrationEvent : IntegrationEvent
    {
        public PedidoAutorizadoIntegrationEvent(Guid clienteId, Guid pedidoId, IDictionary<Guid, int> itens)
        {
            ClienteId = clienteId;
            PedidoId = pedidoId;
            Itens = itens;
        }

        public Guid ClienteId { get; private set; }

        public Guid PedidoId { get; private set; }

        public IDictionary<Guid, int> Itens { get; private set; }
    }
}