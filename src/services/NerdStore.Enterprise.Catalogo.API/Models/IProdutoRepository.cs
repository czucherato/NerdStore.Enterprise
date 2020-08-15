using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using NerdStore.Enterprise.Core.Data;

namespace NerdStore.Enterprise.Catalogo.API.Models
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<PagedResult<Produto>> ObterTodos(int pageSize, int pageIndex, string query = null);

        Task<Produto> ObterPorId(Guid id);

        Task<List<Produto>> ObterProdutosPorId(string ids);

        void Adicionar(Produto produto);

        void Atualizar(Produto produto);
    }
}