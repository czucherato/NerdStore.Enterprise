using System.Threading.Tasks;
using FluentValidation.Results;
using NerdStore.Enterprise.Core.Messages;

namespace NerdStore.Enterprise.Core.Mediator
{
    public interface IMediatorHandler
    {
        Task PublicarEvento<T>(T evento) where T : Event;

        Task<ValidationResult> EnviarComando<T>(T comando) where T : Command;
    }
}