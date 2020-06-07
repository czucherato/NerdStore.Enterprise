using System;
using NerdStore.Enterprise.Core.DomainObjects;

namespace NerdStore.Enterprise.Core.Data
{
    public interface IRepository<T> : IDisposable where T : IAggregateRoot 
    {
        IUnitOfWork UnitOfWork { get; }
    }
}