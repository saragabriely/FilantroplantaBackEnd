using System;
using System.Threading.Tasks;
using System.Data;
using mba_es_25_grupo_02_backend;
using mba_es_25_grupo_02_backend.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using mba_es_25_grupo_02_backend.Models.API;

namespace Bootcamp.AzFunc.Controle
{
    public class ControlePedidoProduto 
    {
        public ControlePedido controlePedido = new ControlePedido();

        public ControlePedidoProduto (){}

        public List<PedidoProduto> BuscarPedidoProdutosPorPedido(long pedidoID)
        {
            using var context = new dbmbaesproductionContext();
            return context.PedidoProduto.Where(i => i.PedidoID == pedidoID).ToList();
        }

        public List<long> BuscarPedidoIDsProdutor(long produtorID)
        {
            using var context = new dbmbaesproductionContext();
            return context.PedidoProduto.Where(i => i.ProdutorID == produtorID).Select(i => i.PedidoID).Distinct().ToList();
        }
        
        public long CountPedidoProdutosProdutor(long produtorID)
        {
            return BuscarPedidoIDsProdutor(produtorID).Count();
        }

        public void ExcluirListaPedidoProduto(List<PedidoProduto> lista)
        {
            using var context = new dbmbaesproductionContext();
            context.PedidoProduto.RemoveRange(lista);
            context.SaveChanges();
        }
                
        public List<PedidoProduto> CadastrarPedidoProduto(List<PedidoProduto> lista)
        {
            using var context = new dbmbaesproductionContext();

            try
            {
                if(lista != null && lista.Count > 0)
                {
                    context.PedidoProduto.AddRange(lista);
                    context.SaveChanges();
                }

                return lista;
            }
            catch(Exception e)
            {
                throw;
            }
        }

        public List<PedidoAPI> BuscarPedidoEProdutos(long pedidoID, long produtorID, long clienteID, List<Pedido> pedidos)
        {
            var lstPedidoProduto = new List<PedidoAPI>();

            try
            {
                if(pedidoID > 0)
                    lstPedidoProduto.Add(BuscarListaPedidoPorPedidoID(pedidoID));
                else if(produtorID > 0)
                    lstPedidoProduto = BuscarListaPedidoPorProdutor(produtorID);
                else
                    lstPedidoProduto = BuscarListaPedidoPorCliente(clienteID, pedidos);

                return lstPedidoProduto;
            }
            catch(Exception e)
            {
                throw;
            }
        }

        public PedidoAPI BuscarListaPedidoPorPedidoID(long pedidoID)
        {
            using var context   = new dbmbaesproductionContext();
            var pedidoAPI       = new PedidoAPI();
            var v_pedidoProd    = new v_PedidoProduto();

            var listaPP = BuscarPedidoProdutosPorPedido(pedidoID);

            if(listaPP != null && listaPP.Count > 0)
            {
                pedidoAPI.Pedido   = controlePedido.BuscarPedidoPorID(pedidoID);
                pedidoAPI.Produtos = new List<v_PedidoProduto>();
                    
                var lista = 
                            from pe       in context.Pedido
                            join pp      in context.PedidoProduto on pe.PedidoID          equals pp.PedidoID
                            join produto in context.Produto       on pp.ProdutoID         equals produto.ProdutoID
                            join produtor in context.Pessoa       on produto.ProdutorID   equals produtor.PessoaID
                            join cliente  in context.Pessoa       on pe.ClienteID         equals cliente.PessoaID
                            where (pp.PedidoID == pedidoID)
                            select new 
                            {
                                PedidoID          = pe.PedidoID,
                                PedidoProdutoID   = pp.PedidoProdutoID,
                                ProdutoID         = produto.ProdutoID,
                                StatusPedidoID    = pe.StatusPedidoID,
                                ClienteID         = cliente.PessoaID,
                                NomeCliente       = cliente.Nome,
                                DescricaoProduto  = produto.Descricao,
                                Quantidade        = pp.Quantidade,
                                ValorPorKg        = pp.ValorPorKg,
                                ValorTotalProduto = pp.ValorTotalProduto,
                                Local             = produtor.Cidade,
                                StatusPedidoProdutoID = pp.StatusPedidoProdutoID
                            };

                foreach (var item in lista)
                {
                    v_pedidoProd = new v_PedidoProduto();
                    v_pedidoProd.PedidoID          = item.PedidoID;
                    v_pedidoProd.PedidoProdutoID   = item.PedidoProdutoID;
                    v_pedidoProd.ProdutoID         = item.ProdutoID;
                    v_pedidoProd.StatusPedidoID    = item.StatusPedidoID;
                    v_pedidoProd.ClienteID         = item.ClienteID;
                    v_pedidoProd.NomeCliente       = item.NomeCliente;
                    v_pedidoProd.DescricaoProduto  = item.DescricaoProduto;
                    v_pedidoProd.Quantidade        = item.Quantidade;
                    v_pedidoProd.ValorPorKg        = item.ValorPorKg;
                    v_pedidoProd.ValorTotalProduto = item.ValorTotalProduto;
                    v_pedidoProd.Local             = item.Local;
                    v_pedidoProd.StatusPedidoProdutoID = item.StatusPedidoProdutoID;

                    pedidoAPI.Produtos.Add(v_pedidoProd);
                }
            }

            return pedidoAPI;
        }

        private List<PedidoAPI> BuscarListaPedidoPorCliente(long clienteID, List<Pedido> pedidos)
        {
            using var context    = new dbmbaesproductionContext();
            var v_pedidoProd     = new v_PedidoProduto();
            var lstPedidoRetorno = new List<PedidoAPI>();
            var pedidoAPI        = new PedidoAPI();

            foreach (var pedido in pedidos)
            {
                pedidoAPI          = new PedidoAPI();
                pedidoAPI.Pedido   = pedido;
                pedidoAPI.Produtos = new List<v_PedidoProduto>();
                
                var lista = 
                            from pe       in context.Pedido
                            join pp       in context.PedidoProduto on pe.PedidoID          equals pp.PedidoID
                            join produto  in context.Produto       on pp.ProdutoID         equals produto.ProdutoID
                            join produtor in context.Pessoa        on produto.ProdutorID   equals produtor.PessoaID
                            join cliente  in context.Pessoa        on pe.ClienteID         equals cliente.PessoaID
                            where (cliente.PessoaID == clienteID && pe.PedidoID == pedido.PedidoID)
                            select new 
                            {
                                PedidoID          = pe.PedidoID,
                                PedidoProdutoID   = pp.PedidoProdutoID,
                                ProdutoID         = produto.ProdutoID,
                                StatusPedidoID    = pe.StatusPedidoID,
                                ClienteID         = cliente.PessoaID,
                                NomeProdutor      = produtor.Nome,
                                NomeCliente       = cliente.Nome,
                                DescricaoProduto  = produto.Descricao,
                                Quantidade        = pp.Quantidade,
                                ValorPorKg        = pp.ValorPorKg,
                                ValorTotalProduto = pp.ValorTotalProduto,
                                Local             = produtor.Cidade,
                                StatusPedidoProdutoID = pp.StatusPedidoProdutoID
                            };

                foreach (var item in lista)
                {
                    v_pedidoProd = new v_PedidoProduto();
                    v_pedidoProd.PedidoID          = item.PedidoID;
                    v_pedidoProd.PedidoProdutoID   = item.PedidoProdutoID;
                    v_pedidoProd.ProdutoID         = item.ProdutoID;
                    v_pedidoProd.StatusPedidoID    = item.StatusPedidoID;
                    v_pedidoProd.ClienteID         = item.ClienteID;
                    v_pedidoProd.NomeProdutor      = item.NomeProdutor;
                    v_pedidoProd.NomeCliente       = item.NomeCliente;
                    v_pedidoProd.DescricaoProduto  = item.DescricaoProduto;
                    v_pedidoProd.Quantidade        = item.Quantidade;
                    v_pedidoProd.ValorPorKg        = item.ValorPorKg;
                    v_pedidoProd.ValorTotalProduto = item.ValorTotalProduto;
                    v_pedidoProd.Local             = item.Local;
                    v_pedidoProd.StatusPedidoProdutoID = item.StatusPedidoProdutoID;

                    pedidoAPI.Produtos.Add(v_pedidoProd);
                }

                lstPedidoRetorno.Add(pedidoAPI);
            }

            return lstPedidoRetorno;
        }

        private List<PedidoAPI> BuscarListaPedidoPorProdutor(long produtorID)
        {
            using var context           = new dbmbaesproductionContext();
            var pedidoAPI               = new PedidoAPI();
            var lstPedidoProdutoRetorno = new List<PedidoAPI>();
            var v_pedidoProd            = new v_PedidoProduto();

            var pedidosID = BuscarPedidoIDsProdutor(produtorID);

            foreach (var pedidoID in pedidosID)
            {
                v_pedidoProd = new v_PedidoProduto();
                pedidoAPI    = new PedidoAPI();

                pedidoAPI.Pedido   = controlePedido.BuscarPedidoPorID(pedidoID);
                pedidoAPI.Produtos = new List<v_PedidoProduto>();
                    
                var lista = 
                            from pe       in context.Pedido
                            join pp      in context.PedidoProduto on pe.PedidoID          equals pp.PedidoID
                            join produto in context.Produto       on pp.ProdutoID         equals produto.ProdutoID
                            join produtor in context.Pessoa       on produto.ProdutorID   equals produtor.PessoaID
                            join cliente  in context.Pessoa       on pe.ClienteID         equals cliente.PessoaID
                            where (produtor.PessoaID == produtorID && pp.PedidoID == pedidoID)
                            select new 
                            {
                                PedidoID          = pe.PedidoID,
                                PedidoProdutoID   = pp.PedidoProdutoID,
                                ProdutoID         = produto.ProdutoID,
                                StatusPedidoID    = pe.StatusPedidoID,
                                ClienteID         = cliente.PessoaID,
                                NomeCliente       = cliente.Nome,
                                DescricaoProduto  = produto.Descricao,
                                Quantidade        = pp.Quantidade,
                                ValorPorKg        = pp.ValorPorKg,
                                ValorTotalProduto = pp.ValorTotalProduto,
                                Local             = produtor.Cidade,
                                StatusPedidoProdutoID = pp.StatusPedidoProdutoID
                            };

                foreach (var item in lista)
                {
                    v_pedidoProd = new v_PedidoProduto();
                    v_pedidoProd.PedidoID          = item.PedidoID;
                    v_pedidoProd.PedidoProdutoID   = item.PedidoProdutoID;
                    v_pedidoProd.ProdutoID         = item.ProdutoID;
                    v_pedidoProd.StatusPedidoID    = item.StatusPedidoID;
                    v_pedidoProd.ClienteID         = item.ClienteID;
                    v_pedidoProd.NomeCliente       = item.NomeCliente;
                    v_pedidoProd.DescricaoProduto  = item.DescricaoProduto;
                    v_pedidoProd.Quantidade        = item.Quantidade;
                    v_pedidoProd.ValorPorKg        = item.ValorPorKg;
                    v_pedidoProd.ValorTotalProduto = item.ValorTotalProduto;
                    v_pedidoProd.Local             = item.Local;
                    v_pedidoProd.StatusPedidoProdutoID = item.StatusPedidoProdutoID;

                    pedidoAPI.Produtos.Add(v_pedidoProd);
                }

                lstPedidoProdutoRetorno.Add(pedidoAPI);
            }

            return lstPedidoProdutoRetorno;
        }

        public PedidoProduto PopularPedidoProduto(v_PedidoProduto produto, long pedidoID)
        {
            var controleProduto = new ControleProduto();
            var _produto = controleProduto.BuscarProdutoPorID(produto.ProdutoID);

            return new PedidoProduto 
                    {
                        PedidoProdutoID   = 0,
                        PedidoID          = pedidoID,
                        ProdutoID         = produto.ProdutoID,
                        Quantidade        = produto.Quantidade,
                        ValorPorKg        = produto.ValorPorKg,
                        ValorTotalProduto = produto.ValorTotalProduto,
                        ProdutorID        = _produto != null ? _produto.ProdutorID : 0
                    };
        }
    }
}
