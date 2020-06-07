using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NerdStore.Enterprise.Core.Data;
using NerdStore.Enterprise.Cliente.API.Models;

namespace NerdStore.Enterprise.Cliente.API.Data.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        public ClienteRepository(ClienteContext context)
        {
            _context = context;
        }

        private readonly ClienteContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public async Task<Models.Cliente> ObterPorCpf(string cpf)
        {
            return await _context.Clientes.FirstOrDefaultAsync(c => c.Cpf.Numero == cpf);
        }

        public async Task<IEnumerable<Models.Cliente>> ObterTodos()
        {
            return await _context.Clientes.AsNoTracking().ToListAsync();
        }

        public void Adicionar(Models.Cliente cliente)
        {
            _context.Clientes.Add(cliente);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}