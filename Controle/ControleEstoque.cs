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
using mba_es_25_grupo_02_backend.Models.API.Estoque;

namespace Bootcamp.AzFunc.Controle
{
    public class ControleEstoque 
    {
        public ControleProduto controleProduto = new ControleProduto(); 

        public ControleEstoque(){}

        public List<Estoque> BuscaListaEstoquePorProdutor(long produtorID)
        {
            using var context = new dbmbaesproductionContext();
            return context.Estoque.Where(i => i.ProdutorID == produtorID).ToList();
        }

        public Estoque BuscarEstoquePorProdutoID(long produtoID)
        {
            using var context = new dbmbaesproductionContext();            
            return context.Estoque.Where(i => i.ProdutoID == produtoID).FirstOrDefault();
        }

        public Estoque BuscarEstoquePorID(long estoqueID)
        {
            using var context = new dbmbaesproductionContext();            
            return context.Estoque.Find(estoqueID);
        }

        public EstoqueResponse CadastrarOuAtualizarEstoque(Estoque estoque)
        {
            var response = new EstoqueResponse();

            try
            {
                using var context = new dbmbaesproductionContext();
                estoque.DataUltimaAtualizacao = DateTime.Now;

                if(estoque != null && estoque.EstoqueID > 0)
                    context.Entry(estoque).State = EntityState.Modified;   
                else
                    context.Add(estoque);

                context.SaveChanges();

                response.Estoque = estoque;
                response.StatusRetorno =  (int)HttpStatusCode.OK;
            }
            catch(Exception e)
            {
                response.Erro = $"Erro ao cadastrar/atualizar estoque: '{e.Message}'";
                response.StatusRetorno =  (int)HttpStatusCode.BadRequest;
            }

            return response;
        }

        public EstoqueResponse AtualizarEstoqueProduto(long produtoID, long QuantidadeReservada)
        {
            var response = new EstoqueResponse(); 

            try
            {
                var estoque = BuscarEstoquePorProdutoID(produtoID);

                if(estoque != null && estoque.EstoqueID > 0)
                    response = AtualizarEstoque(estoque, QuantidadeReservada);
            }
            catch(Exception e)
            {
                response.StatusRetorno = (int)HttpStatusCode.BadRequest;
                response.Erro          = $"Erro ao atualizar estoque: '{e.Message}'";
            }

            return response;
        }

        public EstoqueResponse AtualizarEstoque(Estoque estoque, long QuantidadeReservada)
        {
            var response = new EstoqueResponse(); 

            try
            {
                if(estoque != null && estoque.EstoqueID > 0)
                {
                    estoque.QuantidadeDisponivel -= QuantidadeReservada;
                    estoque.QuantidadeReservada  += QuantidadeReservada;
                    response = CadastrarOuAtualizarEstoque(estoque);
                }
                else
                {
                    response.StatusRetorno = (int)HttpStatusCode.BadRequest;
                    response.Erro          = "Estoque não encontrado";
                }
            }
            catch(Exception e)
            {
                response.StatusRetorno = (int)HttpStatusCode.BadRequest;
                response.Erro          = $"Erro ao atualizar estoque: '{e.Message}'";
            }

            return response;
        }

        public Estoque CadastrarEstoque(Produto produto, long quantidade)
        {
            try
            {
                var estoque = new Estoque
                {
                    ProdutoID            = produto.ProdutoID,
                    ProdutorID           = produto.ProdutorID,
                    QuantidadeDisponivel = quantidade,
                    QuantidadeVendida    = 0,
                    QuantidadeReservada  = 0
                };

                return CadastrarOuAtualizarEstoque(estoque).Estoque;
            }
            catch(Exception e)
            {
                throw;
            }
        }

        public List<Estoque> BuscarEstoquePorProdutor(long pessoaID)
        {
            var listaRetorno  = new List<Estoque>();
            var listaProdutor = BuscaListaEstoquePorProdutor(pessoaID);

            if(listaProdutor != null && listaProdutor.Count > 0)
                    listaRetorno = listaProdutor;
            
            return listaRetorno;
        }

        public EstoqueResponse ExcluirEstoque(long estoqueID)
        {
            using var context = new dbmbaesproductionContext();
            var response      = new EstoqueResponse(); 

            try
            {
                var estoque = BuscarEstoquePorID(estoqueID);

                if(estoque != null && estoque.EstoqueID > 0)
                {
                    context.Remove(estoque);
                    context.SaveChanges();

                    response.StatusRetorno = (int)HttpStatusCode.OK;
                }
                else
                {
                    response.StatusRetorno = (int)HttpStatusCode.BadRequest;
                    response.Erro = "Cadastro do estoque não encontrado.";
                }
            }
            catch(Exception e)
            {
                response.StatusRetorno = (int)HttpStatusCode.BadRequest;
                response.Erro          = $"Erro ao excluir estoque: '{e.Message}'";
            }

            return response;
        }
    }
}