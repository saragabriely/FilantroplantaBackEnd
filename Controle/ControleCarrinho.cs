using System;
using System.Threading.Tasks;
using System.Data;
using mba_es_25_grupo_02_backend;
using mba_es_25_grupo_02_backend.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using mba_es_25_grupo_02_backend.Models.API;
using System.Net;
using System.Globalization;
using mba_es_25_grupo_02_backend.Models.API.Carrinho;

namespace Bootcamp.AzFunc.Controle
{
    public class ControleCarrinho 
    {
        public ControleUtil    ctrlUtil    = new ControleUtil();
        public ControleCarrinho(){}

        public void RemoverItemCarrinho(List<PedidoProduto> pedProdutos, long clienteID)
        {
            foreach (var item in pedProdutos)
            {
                RemoverItemCarrinho(clienteID, item.ProdutoID);
            }
        }

        public void RemoverItemCarrinho(long clienteID, long produtoID)
        {
            using var context = new dbmbaesproductionContext();

            var itemCarrinho = BuscaListaItemCarrinhoPorClienteProduto(clienteID, produtoID);

            if(itemCarrinho != null && itemCarrinho.ItemCarrinhoID > 0)
                context.ItemCarrinho.Remove(itemCarrinho);

            context.SaveChanges();
        }

        public bool RemoverItemCarrinhoPorID(long itemCarrinhoID)
        {
            using var context = new dbmbaesproductionContext();
            var retorno = true;

            try
            {
                var item = context.ItemCarrinho.Find(itemCarrinhoID);
                if(item != null && item.ItemCarrinhoID > 0)
                    context.ItemCarrinho.Remove(item);  

                context.SaveChanges();
            }
            catch(Exception e)
            {
                retorno = false;
            }

            return retorno;
        }

        public CarrinhoResponse RemoverItemCarrinho(long itemCarrinhoID)
        {
            var resultado = new CarrinhoResponse(); 

            try
            {
                if(RemoverItemCarrinhoPorID(itemCarrinhoID))
                    resultado.StatusRetorno = (int)HttpStatusCode.OK;
                else
                {
                    resultado.StatusRetorno = (int)HttpStatusCode.BadRequest;
                    resultado.Erro          = "Não foi possível excluir o item selecionado.";
                }
            }
            catch(Exception e)
            {
                resultado.StatusRetorno = (int)HttpStatusCode.BadRequest;
                resultado.Erro          = $"Erro ao excluir item do carrinho: '{e.Message}'";
            }

            return resultado;
        }

        public List<v_ItemCarrinho> BuscaListaItemCarrinhoPorCliente(long clienteID)
        {
            using var context     = new dbmbaesproductionContext();
            var itemCarrinho      = new v_ItemCarrinho();
            var listaItemCarrinho = new List<v_ItemCarrinho>();
                                
            var lista = 
                        from item     in context.ItemCarrinho
                        join produto  in context.Produto      on item.ProdutoID    equals produto.ProdutoID
                        join produtor in context.Pessoa       on item.ProdutorID   equals produtor.PessoaID
                        join cliente  in context.Pessoa       on item.ClienteID    equals cliente.PessoaID
                        where (cliente.PessoaID == clienteID)
                        select new 
                        {
                            ItemCarrinhoID  = item.ItemCarrinhoID,
                            ProdutoID       = item.ProdutoID,
                            NomeProduto     = produto.Descricao,
                            ProdutorID      = produtor.PessoaID,
                            NomeProdutor    = produtor.Nome,
                            ClienteID       = cliente.PessoaID,
                            Quantidade      = item.Quantidade,
                            ValorPorKG      = item.ValorPorKG,
                            ValorTotal      = item.ValorTotal,
                            Localizacao     = produtor.Cidade
                        };

            foreach (var item in lista)
            {
                itemCarrinho = new v_ItemCarrinho();

                itemCarrinho.ItemCarrinhoID  = item.ItemCarrinhoID;
                itemCarrinho.ProdutoID       = item.ProdutoID;
                itemCarrinho.NomeProduto     = item.NomeProduto;
                itemCarrinho.ProdutorID      = item.ProdutorID;
                itemCarrinho.NomeProdutor    = item.NomeProdutor;
                itemCarrinho.ClienteID       = item.ClienteID;
                itemCarrinho.Quantidade      = item.Quantidade;
                itemCarrinho.ValorPorKG      = ctrlUtil.FormatarDecimal(item.ValorPorKG);
                itemCarrinho.ValorTotal      = item.ValorTotal;
                itemCarrinho.ValorTotalFormatado = ctrlUtil.FormatarDecimal(item.ValorTotal);
                itemCarrinho.Localizacao     = item.Localizacao;

                listaItemCarrinho.Add(itemCarrinho);
            }

            return listaItemCarrinho;
        }

        public v_ItemCarrinho BuscarVItemCarrinhoPorID(long itemCarrinhoID)
        {
            using var context = new dbmbaesproductionContext();
            var vItemCarrinho = new v_ItemCarrinho();
                                
            var lista = 
                        from itemC    in context.ItemCarrinho
                        join produto  in context.Produto      on itemC.ProdutoID    equals produto.ProdutoID
                        join produtor in context.Pessoa       on itemC.ProdutorID   equals produtor.PessoaID
                        join cliente  in context.Pessoa       on itemC.ClienteID    equals cliente.PessoaID
                        where (itemC.ItemCarrinhoID == itemCarrinhoID)
                        select new 
                        {
                            ItemCarrinhoID  = itemC.ItemCarrinhoID,
                            ProdutoID       = itemC.ProdutoID,
                            NomeProduto     = produto.Descricao,
                            ProdutorID      = produtor.PessoaID,
                            NomeProdutor    = produtor.Nome,
                            ClienteID       = cliente.PessoaID,
                            Quantidade      = itemC.Quantidade,
                            ValorPorKG      = itemC.ValorPorKG,
                            ValorTotal      = itemC.ValorTotal,
                            Localizacao     = produtor.Cidade
                        };

            if(lista != null && lista.ToList().Count > 0)
            {
                var item = lista.FirstOrDefault();

                vItemCarrinho = new v_ItemCarrinho
                {
                    ItemCarrinhoID      = item.ItemCarrinhoID,
                    ProdutoID           = item.ProdutoID,
                    NomeProduto         = item.NomeProduto,
                    ProdutorID          = item.ProdutorID,
                    NomeProdutor        = item.NomeProdutor,
                    ClienteID           = item.ClienteID,
                    Quantidade          = item.Quantidade,
                    ValorPorKG          = ctrlUtil.FormatarDecimal(item.ValorPorKG),
                    ValorTotal          = item.ValorTotal,
                    ValorTotalFormatado = ctrlUtil.FormatarDecimal(item.ValorTotal),
                    Localizacao         = item.Localizacao
                };
            }
            
            return vItemCarrinho;
        }

        public ItemCarrinho BuscaListaItemCarrinhoPorClienteProduto(long clienteID, long produtoID)
        {
            using var context = new dbmbaesproductionContext();
            return context.ItemCarrinho.Where(i => i.ClienteID == clienteID && i.ProdutoID == produtoID).FirstOrDefault();
        }

        public CarrinhoResponse CadastrarItemCarrinho(ItemCarrinho item)
        {
            using var context   = new dbmbaesproductionContext();
            var controleEstoque = new ControleEstoque();
            var response        = new CarrinhoResponse();
            long quantidade     = 0; 

            try
            {
                if(item != null)
                {
                    quantidade = item.Quantidade;

                    // Valida se o item selecionado já existe no carrinho. Se sim, irá atualizar a quantidade já cadastrada na base
                    var itemExistente = BuscaListaItemCarrinhoPorClienteProduto(item.ClienteID, item.ProdutoID);

                    if(itemExistente != null && itemExistente.ItemCarrinhoID > 0)
                        item = AtualizarItemCarrinho(itemExistente, item);
                    else
                    {
                        item.DataAdicaoItem = item.DataUltimaAtualizacao = DateTime.Now;
                        context.ItemCarrinho.Add(item);
                        context.SaveChanges();
                    }
                    
                    response.StatusRetorno = (int)HttpStatusCode.OK;
                    response.ItemCarrinho  = item;
                    response.vItemCarrinho = BuscarVItemCarrinhoPorID(item.ItemCarrinhoID);

                    controleEstoque.AtualizarEstoqueProduto(item.ProdutoID, quantidade);
                }
                else
                {
                    response.StatusRetorno = (int)HttpStatusCode.BadRequest;
                    response.Erro = "Item inválido";      
                }
            }
            catch(Exception e)
            {
                response.StatusRetorno = (int)HttpStatusCode.BadRequest;
                response.Erro          = $"Erro ao cadastrar item carrinho: '{e.Message}'";
            }

            return response;
        }

        private ItemCarrinho AtualizarItemCarrinho(ItemCarrinho itemExistente, ItemCarrinho item)
        {
            using var context   = new dbmbaesproductionContext();

            itemExistente.Quantidade += item.Quantidade;
            itemExistente.ValorTotal += item.Quantidade * item.ValorPorKG;
            itemExistente.DataUltimaAtualizacao = DateTime.Now; 

            context.Entry(itemExistente).State = EntityState.Modified; 
            context.SaveChanges();

            return itemExistente;
        }
    }
}