using System;
using System.Threading.Tasks;
using System.Data;
using mba_es_25_grupo_02_backend;
using mba_es_25_grupo_02_backend.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using mba_es_25_grupo_02_backend.Models.API;
using System.Globalization;
using System.Net;
using mba_es_25_grupo_02_backend.Models.API.Response;

namespace Bootcamp.AzFunc.Controle
{
    public class ControleProduto
    {
        public ControleUtil controleUtil = new ControleUtil();

        public ControleProduto(){}
        
        public List<Produto> BuscarListaProduto(long produtorID)
        {
            using var context = new dbmbaesproductionContext();
            return context.Produto.Where(i => i.ProdutorID == produtorID).ToList();
        }

        public List<v_Produto> BuscarListaProduto(string descricaoProduto)
        {
            using var context = new dbmbaesproductionContext();
            var vProduto      = new v_Produto();
            var listavProduto = new List<v_Produto>();
                                
            var lista = 
                        from produto  in context.Produto
                        join produtor in context.Pessoa   on produto.ProdutorID equals produtor.PessoaID
                        join estoque  in context.Estoque  on produto.ProdutoID  equals estoque.ProdutoID
                        where (produto.Descricao.Contains(descricaoProduto) && estoque.QuantidadeDisponivel > 0 )
                        select new 
                        {
                            ProdutoID       = produto.ProdutoID,
                            Descricao       = produto.Descricao,
                            NomeProdutor    = produtor.Nome,
                            ValorPorKg      = produto.ValorPorKg,
                            ProdutorID      = produtor.PessoaID,
                            Localizacao     = produtor.Cidade
                        };

            foreach (var item in lista)
            {
                vProduto = new v_Produto();
                vProduto.ProdutoID     = item.ProdutoID;
                vProduto.Descricao     = item.Descricao;
                vProduto.NomeProdutor  = item.NomeProdutor;
                vProduto.ValorPorKG    = item.ValorPorKg;
                vProduto.ValorPorKGFormatado = controleUtil.FormatarDecimal(item.ValorPorKg);
                vProduto.ProdutorID    = item.ProdutorID;
                vProduto.Localizacao   = item.Localizacao;

                listavProduto.Add(vProduto);
            }
            
            return listavProduto;
        }
        
        public ProdutoResponse CadastroProduto(ProdutoRequest json)
        {
            using var context = new dbmbaesproductionContext();
            var response     = new ProdutoResponse(); 

            try
            {
                if(json.Produto != null)
                {
                    var produto = json.Produto;

                    produto.DataCadastro = produto.DataUltimaAtualizacao = DateTime.Now;

                    context.Produto.Add(produto);
                    context.SaveChanges();

                    response.StatusRetorno = (int)HttpStatusCode.OK;
                    response.Erro = "";
                    response.ID   = produto.ProdutoID;

                    var ctrlEstoque = new ControleEstoque();
                    response.Estoque = ctrlEstoque.CadastrarEstoque(produto, json.Quantidade);
                }
                else
                {
                    response.StatusRetorno = (int)HttpStatusCode.BadRequest;
                    response.Erro = "Produto inválido";      
                }
            }
            catch(Exception e)
            {
                response.StatusRetorno = (int)HttpStatusCode.BadRequest;
                response.Erro          = $"Erro ao cadastrar produto:  '{e.Message}'";
            }

            return response;
        }

        public ProdutoResponse AtualizarProduto(Produto produto)
        {
            using var context = new dbmbaesproductionContext();
            var response      = new ProdutoResponse(); 

            try
            {
                if(produto != null && produto.ProdutoID > 0)
                {
                    produto.DataUltimaAtualizacao = DateTime.Now;

                    context.Entry(produto).State = EntityState.Modified;
                    context.SaveChanges();

                    response.StatusRetorno = (int)HttpStatusCode.OK;
                }
                else
                {
                    response.StatusRetorno = (int)HttpStatusCode.BadRequest;
                    response.Erro = "Cadastro do produto não encontrado.";
                }
            }
            catch(Exception e)
            {
                response.StatusRetorno = (int)HttpStatusCode.BadRequest;
                response.Erro          = $"Erro ao atualizar produto: '{e.Message}'";
            }

            return response;
        }

        public ProdutoResponse ExcluirProduto(long produtoID)
        {
            using var context = new dbmbaesproductionContext();
            var     response  = new ProdutoResponse(); 

            try
            {
                var produto = BuscarProdutoPorID(produtoID);

                if(produto != null && produto.ProdutoID > 0)
                {
                    var controleEstoque = new ControleEstoque();

                    var estoque = controleEstoque.BuscarEstoquePorProdutoID(produtoID);
                    if(estoque != null && estoque.EstoqueID > 0)
                        controleEstoque.ExcluirEstoque(estoque.EstoqueID);

                    context.Remove(produto);
                    context.SaveChanges();

                    response.StatusRetorno = (int)HttpStatusCode.OK;
                }
                else
                {
                    response.StatusRetorno = (int)HttpStatusCode.BadRequest;
                    response.Erro = "Cadastro do produto não encontrado.";
                }
            }
            catch(Exception e)
            {
                response.StatusRetorno = (int)HttpStatusCode.BadRequest;
                response.Erro          = $"Erro ao excluir produto: '{e.Message}'";
            }

            return response;
        }

        public Produto BuscarProdutoPorID(long ProdutoID)
        {
            using var context = new dbmbaesproductionContext();
            return context.Produto.Find(ProdutoID);
        }
    }
}