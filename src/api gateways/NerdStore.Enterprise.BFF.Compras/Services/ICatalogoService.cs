using System;
using System.Threading.Tasks;
using NerdStore.Enterprise.BFF.Compras.Models;

namespace NerdStore.Enterprise.BFF.Compras.Services
{
    public interface ICatalogoService
    {
        Task<ItemProdutoDTO> ObterPorId(Guid id);
    }
}