using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using NerdStore.Enterprise.BFF.Compras.Models;

namespace NerdStore.Enterprise.BFF.Compras.Services
{
    public interface ICatalogoService
    {
        Task<ItemProdutoDTO> ObterPorId(Guid id);

        Task<IEnumerable<ItemProdutoDTO>> ObterItens(IEnumerable<Guid> ids);
    }
}