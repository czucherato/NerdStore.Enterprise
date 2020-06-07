using System.Threading.Tasks;
using FluentValidation.Results;
using NerdStore.Enterprise.Core.Data;

namespace NerdStore.Enterprise.Core.Messages
{
    public abstract class CommandHandler
    {
        protected CommandHandler()
        {
            ValidationResult = new ValidationResult();
        }

        protected ValidationResult ValidationResult;

        protected void AdicionarErro(string mensagem)
        {
            ValidationResult.Errors.Add(new ValidationFailure(string.Empty, mensagem));
        }

        protected async Task<ValidationResult> PersistirDados(IUnitOfWork unitOfWork)
        {
            if (!await unitOfWork.Commit()) AdicionarErro("Houve um erro ao persistir os dados");

            return ValidationResult;
        }
    }
}