using System;
using NerdStore.Enterprise.Core.Messages;

namespace NerdStore.Enterprise.Cliente.API.Application.Events
{
    public class ClienteRegistradoEvent : Event
    {
        public ClienteRegistradoEvent(Guid id, string nome, string email, string cpf)
        {
            AggregateId = id;
            Id = id;
            Nome = nome;
            Email = email;
            Cpf = cpf;
        }

        public Guid Id { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public string Cpf { get; set; }
    }
}