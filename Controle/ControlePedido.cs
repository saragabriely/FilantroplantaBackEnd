using System;
using System.Threading.Tasks;
using System.Data;
using mba_es_25_grupo_02_backend;
using mba_es_25_grupo_02_backend.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using mba_es_25_grupo_02_backend.Models.API;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using System.Net;

namespace Bootcamp.AzFunc.Controle
{
    public class ControlePedido
    {
        public ControlePedido(){}
        
        public Pedido CadastrarPedido(Pedido pedido)
        {
            using var context = new dbmbaesproductionContext();
            context.Pedido.Add(pedido);
            context.SaveChanges();
            return pedido;
        }

        public List<Pedido> BuscarListaPedidosPorCliente(long clienteID)
        {
            using var context = new dbmbaesproductionContext();
            return context.Pedido.Where(i => i.ClienteID == clienteID).ToList();
        }

        public Pedido BuscarPedidoPorID(long PedidoID)
        {
            using var context = new dbmbaesproductionContext();
            return context.Pedido.Find(PedidoID);
        }

        private void ExcluirPedido(Pedido pedido)
        {
            using var context = new dbmbaesproductionContext();
            context.Pedido.Remove(pedido);
            context.SaveChanges();
        }
        
        public PedidoResponse BuscarPedidos(long pedidoID, long produtorID, long clienteID)
        {
            var retorno          = new PedidoResponse(); 
            var lstPedidoProduto = new List<PedidoAPI>();

            try
            {
                lstPedidoProduto = BuscarListaPedidoProduto(pedidoID, produtorID, clienteID);

                if(lstPedidoProduto.Count > 0)
                {
                    retorno.Pedidos       = lstPedidoProduto;
                    retorno.StatusRetorno = (int)HttpStatusCode.OK;
                }
                else
                {
                    retorno.StatusRetorno = (int)HttpStatusCode.BadRequest;
                    retorno.Erro          = "Nenhum Pedido encontrado."; 
                }
            }
            catch(Exception e)
            {
                retorno.StatusRetorno = (int)HttpStatusCode.BadRequest;
                retorno.Erro          = $"Erro ao buscar pedido(s): '{e.Message}'";
            } 

            return retorno;
        }

        public List<PedidoAPI> BuscarListaPedidoProduto(long pedidoID, long produtorID, long clienteID)
        {
            var ctrlPedidoProduto = new ControlePedidoProduto();
            var lstPedidoProduto      = new List<PedidoAPI>();

            if(pedidoID > 0)
            {
                var pedido = BuscarPedidoPorID(pedidoID);

                if(pedido != null && pedido.PedidoID > 0)
                    lstPedidoProduto = ctrlPedidoProduto.BuscarPedidoEProdutos(pedidoID, 0, 0, null);
            }
            else if(clienteID > 0)
            {
                var pedidos = BuscarListaPedidosPorCliente(clienteID);

                if(pedidos != null && pedidos.Count > 0)
                    lstPedidoProduto = ctrlPedidoProduto.BuscarPedidoEProdutos(0, 0, clienteID, pedidos);
            }
            else
            {
                var qtdePedidoProduto = ctrlPedidoProduto.CountPedidoProdutosProdutor(produtorID);

                if(qtdePedidoProduto > 0)
                    lstPedidoProduto = ctrlPedidoProduto.BuscarPedidoEProdutos(0, produtorID, 0, null);
            }

            return lstPedidoProduto;
        }

        public PedidoResponse CadastroPedido(PedidoAPI json)
        {
            using var context    = new dbmbaesproductionContext();

            var retorno          = new PedidoResponse(); 
            var pedido           = new Pedido();
            var listaCadastrada  = new List<PedidoProduto>();

            var ctrlEstoque  = new ControleEstoque();
            var ctrlCarrinho = new ControleCarrinho();
            var ctrlPedidoProduto = new ControlePedidoProduto();

            try
            {
                if(json.Pedido != null && json.Produtos != null)
                {
                    var lstProdutosDisponiveis  = new List<v_PedidoProduto>();
                    var estoque                 = new Estoque();
                    var pedidoProduto           = new PedidoProduto();
                    var lstPedidoProduto        = new List<PedidoProduto>();
                    var lstEstoqueDisponiveis   = new List<Estoque>();

                    // verifica se todos os produtos selecionados tem o estoque necessário para a qtde solicitada
                    foreach(var produtoPedido in json.Produtos)
                    {
                        estoque = ctrlEstoque.BuscarEstoquePorProdutoID(produtoPedido.ProdutoID);

                        if(estoque.QuantidadeDisponivel >= produtoPedido.Quantidade)
                        {
                            lstProdutosDisponiveis.Add(produtoPedido);
                            lstEstoqueDisponiveis.Add(estoque); 
                        }
                    }

                    if(lstProdutosDisponiveis != null && lstProdutosDisponiveis.Count > 0)
                    {
                        // insere o pedido na base para gerar o ID que será usado na tabela PedidoProduto
                        json.Pedido.DataCadastro = json.Pedido.DataUltimaAtualizacao = DateTime.Now;

                        pedido = CadastrarPedido(json.Pedido);

                        if(pedido != null && pedido.PedidoID > 0)
                        {
                            foreach (var produto in lstProdutosDisponiveis)
                            {
                                // monta os objetos PedidoProduto (tabela intermediaria)
                                lstPedidoProduto.Add(ctrlPedidoProduto.PopularPedidoProduto(produto, pedido.PedidoID));

                                // atualiza o estoque com a quantidade solicitada
                                estoque = lstEstoqueDisponiveis.Where(i => i.ProdutoID == pedidoProduto.ProdutoID).FirstOrDefault();

                                if(estoque != null && estoque.EstoqueID > 0)
                                    ctrlEstoque.AtualizarEstoque(estoque, pedidoProduto.Quantidade);
                            }

                            // insere os produtos na tabela de relacionamento (PedidoProduto)
                            listaCadastrada = ctrlPedidoProduto.CadastrarPedidoProduto(lstPedidoProduto);

                            // Atualiza Carrinho - remove os itens que foram cadastrados no pedido
                            ctrlCarrinho.RemoverItemCarrinho(listaCadastrada, json.Pedido.ClienteID);

                            retorno.StatusRetorno  = (int)HttpStatusCode.OK;
                            retorno.PedidoAPI      = ctrlPedidoProduto.BuscarListaPedidoPorPedidoID(json.Pedido.PedidoID);
                            retorno.vItensCarrinho = ctrlCarrinho.BuscaListaItemCarrinhoPorCliente(json.Pedido.ClienteID);
                        }
                    }
                    else
                    {
                        retorno.StatusRetorno = (int)HttpStatusCode.BadRequest;
                        retorno.Erro = "Nenhum produto selecionado tem estoque suficiente.";
                    }
                }
                else
                {
                    retorno.StatusRetorno = (int)HttpStatusCode.BadRequest;
                    retorno.Erro = "Pedido inválido";      
                }
            }
            catch(Exception e)
            {
                if(listaCadastrada != null && listaCadastrada.Count > 0)
                    ctrlPedidoProduto.ExcluirListaPedidoProduto(listaCadastrada);

                if(pedido != null && pedido.PedidoID > 0)
                    ExcluirPedido(pedido);

                retorno.StatusRetorno = (int)HttpStatusCode.BadRequest;
                retorno.Erro          = $"Erro ao cadastrar pedido: '{e.Message}'";
            }

            return retorno;
        }

        public PedidoResponse AtualizarPedido(Pedido pedido)
        {
            using var context = new dbmbaesproductionContext();
            var response     = new PedidoResponse(); 

            try
            {
                if(pedido != null && pedido.PedidoID > 0)
                {
                    pedido.DataUltimaAtualizacao = DateTime.Now;

                    context.Entry(pedido).State = EntityState.Modified;
                    context.SaveChanges();

                    response.StatusRetorno = (int)HttpStatusCode.OK;
                    response.ID   = pedido.PedidoID;
                }
                else
                {
                    response.StatusRetorno = (int)HttpStatusCode.BadRequest;
                    response.Erro = "Pedido não encontrado.";
                }                
            }
            catch(Exception e)
            {
                response.StatusRetorno = (int)HttpStatusCode.BadRequest;
                response.Erro          = $"Erro ao atualizar Pedido: '{e.Message}'";
            }

            return response;
        }
    }
}
