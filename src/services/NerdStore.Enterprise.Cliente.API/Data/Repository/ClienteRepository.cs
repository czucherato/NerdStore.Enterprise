﻿using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NerdStore.Enterprise.Core.Data;
using NerdStore.Enterprise.Cliente.API.Models;

namespace NerdStore.Enterprise.Cliente.API.Data.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ClienteContext _context;

        public ClienteRepository(ClienteContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<IEnumerable<Models.Cliente>> ObterTodos()
        {
            return await _context.Clientes.AsNoTracking().ToListAsync();
        }

        public Task<Models.Cliente> ObterPorCpf(string cpf)
        {
            return _context.Clientes.FirstOrDefaultAsync(c => c.Cpf.Numero == cpf);
        }

        public void Adicionar(Models.Cliente cliente)
        {
            _context.Clientes.Add(cliente);
        }

        public async Task<Endereco> ObterEnderecoPorId(Guid id)
        {
            return await _context.Enderecos.FirstOrDefaultAsync(e => e.ClienteId == id);
        }

        public void AdicionarEndereco(Endereco endereco)
        {
            _context.Enderecos.Add(endereco);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}