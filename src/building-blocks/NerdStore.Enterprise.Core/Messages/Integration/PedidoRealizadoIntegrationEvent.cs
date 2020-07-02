using System;

namespace NerdStore.Enterprise.Core.Messages.Integration
{
    public class PedidoRealizadoIntegrationEvent : IntegrationEvent
    {
        public PedidoRealizadoIntegrationEvent(Guid clienteId)
        {
            ClienteId = clienteId;
        }

        public Guid ClienteId { get; private set; }
    }
}