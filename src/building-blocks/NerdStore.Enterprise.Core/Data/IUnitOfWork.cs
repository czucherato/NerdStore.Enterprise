using System.Threading.Tasks;

namespace NerdStore.Enterprise.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}