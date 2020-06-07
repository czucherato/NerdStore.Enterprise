using MediatR;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using NerdStore.Enterprise.Core.Messages;
using NerdStore.Enterprise.Cliente.API.Models;
using NerdStore.Enterprise.Cliente.API.Application.Events;

namespace NerdStore.Enterprise.Cliente.API.Application.Commands
{
    public class ClienteCommandHandler : CommandHandler, IRequestHandler<RegistrarClienteCommand, ValidationResult>
    {
        public ClienteCommandHandler(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        private readonly IClienteRepository _clienteRepository;

        public async Task<ValidationResult> Handle(RegistrarClienteCommand message, CancellationToken cancellationToken)
        {
            if (!message.EhValido()) return message.ValidationResult;

            var cliente = new Models.Cliente(message.Id, message.Nome, message.Email, message.Cpf);
            var clienteExistente = await _clienteRepository.ObterPorCpf(cliente.Cpf.Numero);

            if (clienteExistente != null)
            {
                AdicionarErro("Este CPF já está em uso");
                return ValidationResult;
            }

            _clienteRepository.Adicionar(cliente);
            cliente.AdicionarEvento(new ClienteRegistradoEvent(message.Id, message.Nome, message.Email, message.Cpf));
            return await PersistirDados(_clienteRepository.UnitOfWork);
        }
    }
}