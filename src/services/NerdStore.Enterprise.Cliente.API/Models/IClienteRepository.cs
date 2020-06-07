using System.Threading.Tasks;
using System.Collections.Generic;
using NerdStore.Enterprise.Core.Data;

namespace NerdStore.Enterprise.Cliente.API.Models
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        Task<Cliente> ObterPorCpf(string cpf);

        Task<IEnumerable<Cliente>> ObterTodos();

        void Adicionar(Cliente cliente);
    }
}