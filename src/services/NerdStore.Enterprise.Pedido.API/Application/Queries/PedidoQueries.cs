using Dapper;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using NerdStore.Enterprise.Pedido.Domain.Pedidos;
using NerdStore.Enterprise.Pedido.API.Application.DTO;

namespace NerdStore.Enterprise.Pedido.API.Application.Queries
{
    public class PedidoQueries : IPedidoQueries
    {
        public PedidoQueries(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        private readonly IPedidoRepository _pedidoRepository;

        public async Task<PedidoDTO> ObterUltimoPedido(Guid clientId)
        {
            const string sql = @"SELECT
                                P.ID AS 'ProdutoId', P.CODIGO, P.VOUCHERUTILIZADO, P.DESCONTO, P.VALORTOTAL, P.PEDIDOSTATUS,
                                P.LOGRADOURO, P.NUMERO, P.BAIRRO, P.COMPLEMENTO, P.CEP, P.CIDADE, P.ESTADO,
                                PIT.ID AS 'ProdutoItemId', PIT.PRODUTONOME, PIT.QUANTIDADE, PIT.PRODUTOIMAGEM, PIT.VALORUNITARIO
                                FROM PEDIDOS P
                                INNER JOIN PEDIDOITEMS PIT ON P.ID = PIT.PEDIDOID
                                WHERE P.CLIENTEID = @clienteId
                                AND P.DATACADASTRO between DATEADD(minute, -3, GETDATE()) and DATEADD(minute, 0, GETDATE())
                                ORDER BY P.DATACADASTRO DESC";

            var pedido = await _pedidoRepository.ObterConexao()
                .QueryAsync<dynamic>(sql, new { clienteId = clientId });

            return MapearPedido(pedido);
        }

        public async Task<IEnumerable<PedidoDTO>> ObterListaPorClienteId(Guid clienteId)
        {
            var pedidos = await _pedidoRepository.ObterListaPorClienteId(clienteId);
            return pedidos.Select(PedidoDTO.ParaPedidoDTO);
        }

        private PedidoDTO MapearPedido(dynamic result)
        {
            var pedido = new PedidoDTO
            {
                Codigo = result[0].CODIGO,
                Status = result[0].PEDIDOSTATUS,
                ValorTotal = result[0].VALORTOTAL,
                Desconto = result[0].DESCONTO,
                VoucherUtilizado = result[0].VOUCHERUTILIZADO,
                PedidoItems = new List<PedidoItemDTO>(),
                Endereco = new EnderecoDTO
                {
                    Logradouro = result[0].LOGRADOURO,
                    Numero = result[0].NUMERO,
                    Bairro = result[0].BAIRRO,
                    Complemento = result[0].COMPLEMENTO,
                    Cep = result[0].CEP,
                    Cidade = result[0].CIDADE,
                    Estado = result[0].ESTADO
                }
            };

            foreach (var item in result)
            {
                var pedidoItem = new PedidoItemDTO
                {
                    Nome = item.PRODUTONOME,
                    Valor = item.VALORUNITARIO,
                    Quantidade = item.QUANTIDADE,
                    Imagem = item.PRODUTOIMAGEM
                };

                pedido.PedidoItems.Add(pedidoItem);
            }

            return pedido;
        }

        public async Task<PedidoDTO> ObterPedidosAutorizados()
        {
            const string sql = @"SELECT TOP 1
                                P.ID as 'PedidoId', P.ID, P.CLIENTEID,
                                PI.ID as 'PedidoItemId', PI.ID, PI.PRODUTOID, PI.QUANTIDADE
                                FROM PEDIDOS P
                                INNER JOIN PEDIDOITEMS PI ON P.ID = PI.PEDIDOID
                                WHERE P.PEDIDOSTATUS = 1
                                ORDER BY P.DATACADASTRO";

            var pedido = await _pedidoRepository.ObterConexao().QueryAsync<PedidoDTO, PedidoItemDTO, PedidoDTO>(sql,
                (p, pi) =>
                {
                    p.PedidoItems = new List<PedidoItemDTO> { pi };
                    return p;
                }, splitOn: "PedidoId,PedidoItemId");

            return pedido.FirstOrDefault();
        }
    }
}