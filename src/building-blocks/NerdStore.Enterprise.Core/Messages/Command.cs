using System;
using MediatR;
using FluentValidation.Results;

namespace NerdStore.Enterprise.Core.Messages
{
    public abstract class Command : Message, IRequest<ValidationResult>
    {
        protected Command()
        {
            TimeStamp = DateTime.Now;
        }

        public DateTime TimeStamp { get; private set; }

        public ValidationResult ValidationResult { get; set; }

        public virtual bool EhValido()
        {
            throw new NotImplementedException();
        }
    }
}