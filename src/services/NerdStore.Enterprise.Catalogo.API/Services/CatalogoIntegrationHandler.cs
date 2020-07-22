using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Hosting;
using NerdStore.Enterprise.MessageBus;
using NerdStore.Enterprise.Core.DomainObjects;
using Microsoft.Extensions.DependencyInjection;
using NerdStore.Enterprise.Catalogo.API.Models;
using NerdStore.Enterprise.Core.Messages.Integration;

namespace NerdStore.Enterprise.Catalogo.API.Services
{
    public class CatalogoIntegrationHandler : BackgroundService
    {
        public CatalogoIntegrationHandler(IMessageBus bus, IServiceProvider serviceProvider)
        {
            _bus = bus;
            _serviceProvider = serviceProvider;
        }

        private readonly IMessageBus _bus;

        private readonly IServiceProvider _serviceProvider;

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetSubscribers();
            return Task.CompletedTask;
        }

        private void SetSubscribers()
        {
            _bus.SubscribeAsync<PedidoAutorizadoIntegrationEvent>("PedidoAutorizado", async request => await BaixarEstoque(request));
        }

        private async Task BaixarEstoque(PedidoAutorizadoIntegrationEvent message)
        {
            using var scope = _serviceProvider.CreateScope();
            var produtosComEstoque = new List<Produto>();
            var produtoRepository = scope.ServiceProvider.GetRequiredService<IProdutoRepository>();
            var idsProdutos = string.Join(",", message.Itens.Select(c => c.Key));
            var produtos = await produtoRepository.ObterProdutosPorId(idsProdutos);

            if (produtos.Count != message.Itens.Count)
            {
                CancelarPedidoSemEstoque(message);
                return;
            }

            foreach (var produto in produtos)
            {
                var quantidadeProduto = message.Itens.FirstOrDefault(p => p.Key == produto.Id).Value;
                if (produto.EstaDisponivel(quantidadeProduto))
                {
                    produto.RetirarEstoque(quantidadeProduto);
                    produtosComEstoque.Add(produto);
                }
            }

            if (produtosComEstoque.Count != message.Itens.Count)
            {
                CancelarPedidoSemEstoque(message);
                return;
            }

            foreach (var produto in produtosComEstoque)
            {
                produtoRepository.Atualizar(produto);
            }

            if (!await produtoRepository.UnitOfWork.Commit())
            {
                throw new DomainException($"Problemas ao atualizar estoquedo pedido {message.PedidoId}");
            }

            var pedidoBaixado = new PedidoBaixadoEstoqueIntegrationEvent(message.ClienteId, message.PedidoId);
            await _bus.PublishAsync(pedidoBaixado);
        }

        public async void CancelarPedidoSemEstoque(PedidoAutorizadoIntegrationEvent message)
        {
            var pedidoCancelado = new PedidoCanceladoIntegrationEvent(message.ClienteId, message.PedidoId);
            await _bus.PublishAsync(pedidoCancelado);
        }
    }
}