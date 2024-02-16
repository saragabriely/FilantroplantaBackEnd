using System;
using System.Threading.Tasks;
using System.Data;
using mba_es_25_grupo_02_backend;
using mba_es_25_grupo_02_backend.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using mba_es_25_grupo_02_backend.Models.API.Pessoa;
using mba_es_25_grupo_02_backend.Models.API;
using System.Net;

namespace Bootcamp.AzFunc.Controle
{
    public class ControlePessoa 
    {
        public ControleProduto controleProduto = new ControleProduto(); 
        public ControleEstoque controleEstoque = new ControleEstoque(); 
        public ControlePedido controlePedido   = new ControlePedido(); 
        public ControleCarrinho controleCarrinho = new ControleCarrinho();
        
        public Pessoa BuscarPessoaPorEmail(string email)
        {
            using var context = new dbmbaesproductionContext();
            return context.Pessoa.Where(i => i.Email.Equals(email)).FirstOrDefault();
        }
        
        public PessoaResponse CadastroPessoa(Pessoa pessoa)
        {
            var resultado = new PessoaResponse(); 

            try
            {
                if(pessoa != null)
                {
                    var buscaPessoa = BuscarPessoaPorEmail(pessoa.Email);

                    if(buscaPessoa != null && buscaPessoa.PessoaID > 0)
                    {
                        resultado.StatusRetorno = (int)HttpStatusCode.BadRequest;
                        resultado.Erro = "Usuário existente!";
                    }
                    else
                        resultado = InserirPessoa(pessoa);
                }
            }
            catch(Exception e)
            {
                resultado.StatusRetorno = (int)HttpStatusCode.BadRequest;
                resultado.Erro          = $"Erro ao cadastrar usuario: '{e.Message}'";
            }

            return resultado;
        }
        
        public PessoaResponse InserirPessoa(Pessoa pessoa)
        {
            using var context = new dbmbaesproductionContext();
            var resultado     = new PessoaResponse(); 

            try
            {
                context.Pessoa.Add(pessoa);
                context.SaveChanges();

                resultado.StatusRetorno = (int)HttpStatusCode.OK;
                resultado.Erro = "Usuário cadastrado com sucesso!";
            }
            catch(Exception e)
            {
                resultado.StatusRetorno = (int)HttpStatusCode.BadRequest;
                resultado.Erro          = $"Erro ao inserir usuario: '{e.Message}'";
            }

            return resultado;
        }
        
        public PessoaResponse AtualizarPessoa(Pessoa pessoa)
        {
            using var context = new dbmbaesproductionContext();
            var resultado     = new PessoaResponse(); 

            try
            {
                if(pessoa != null)
                {
                    var pessoaExistente = BuscarPessoaPorID(pessoa.PessoaID);

                    if(pessoaExistente != null && pessoaExistente.PessoaID > 0)
                    {
                        context.Entry(pessoa).State = EntityState.Modified;
                        context.SaveChanges();

                        resultado.StatusRetorno = (int)HttpStatusCode.OK;
                    }
                    else
                    {
                        resultado.StatusRetorno = (int)HttpStatusCode.BadRequest;
                        resultado.Erro = "Cadastro não encontrado.";
                    }
                }
            }
            catch(Exception e)
            {
                resultado.StatusRetorno = (int)HttpStatusCode.BadRequest;
                resultado.Erro          = $"Erro ao cadastrar usuario: '{e.Message}'";
            }

            return resultado;
        }

        public LoginResponse ConsultaPessoaLogin(string email, string senha)
        {
            var resultado     = new LoginResponse(); 
            resultado.Pessoa = null;

            try
            {
                var pessoa = BuscarPessoaPorEmail(email);

                if(pessoa != null && pessoa.PessoaID > 0)
                {
                    if(pessoa.Senha.Equals(senha))
                    {
                        resultado.StatusRetorno = (int)HttpStatusCode.OK;
                        resultado.Pessoa  = pessoa;
                        resultado.Produtos = pessoa.TipoPessoaID == Pessoa.Produtor ? controleProduto.BuscarListaProduto(pessoa.PessoaID)       : null;
                        resultado.Estoques = pessoa.TipoPessoaID == Pessoa.Produtor ? controleEstoque.BuscarEstoquePorProdutor(pessoa.PessoaID) : null;
                        resultado.Pedidos  = pessoa.TipoPessoaID == Pessoa.Produtor ? controlePedido.BuscarListaPedidoProduto(0, pessoa.PessoaID, 0)
                                                                                    : controlePedido.BuscarListaPedidoProduto(0, 0, pessoa.PessoaID);
                        resultado.vItensCarrinho  = pessoa.TipoPessoaID == Pessoa.Restaurante ? controleCarrinho.BuscaListaItemCarrinhoPorCliente(pessoa.PessoaID) : null;                                                          
                    }
                    else
                    {
                        resultado.StatusRetorno = (int)HttpStatusCode.BadRequest;
                        resultado.Erro          = "Senha incorreta!";
                    }
                }
                else
                {
                    resultado.StatusRetorno = (int)HttpStatusCode.BadRequest;
                    resultado.Erro          = "Usuário não encontrado";
                }
            }
            catch(Exception e)
            {
                resultado.StatusRetorno = (int)HttpStatusCode.BadRequest;
                resultado.Erro          = $"Erro ao buscar usuario: '{e.Message}'";
            }

            return resultado;
        }
       
        public Pessoa BuscarPessoaPorID(long PessoaID)
        {
            using var context = new dbmbaesproductionContext();
            return context.Pessoa.Find(PessoaID);
        }

        public PessoaResponse BuscarPessoa(long pessoaID)
        {
            var response = new PessoaResponse();

            response.StatusRetorno = (int)HttpStatusCode.OK;
            response.Pessoa = BuscarPessoaPorID(pessoaID);

            return response;
        }
    }
}
