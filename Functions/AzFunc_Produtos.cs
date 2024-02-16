using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using mba_es_25_grupo_02_backend.Models;
using mba_es_25_grupo_02_backend.Models.API;
using Bootcamp.AzFunc.Controle;
using System.Web.Http;
using System.Net;
using mba_es_25_grupo_02_backend.Models.API.Response;

namespace Bootcamp.AzFunc.Functions
{
    public class AzFunc_Produtos : ApiController
    {
        [FunctionName("AzFunc_Produtos")]
        public static async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", "post", "put", "delete", Route = null)] HttpRequest req)
        {
            var controle = new ControleProduto();
            var response = new ProdutoResponse();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            try
            {
                if (req.Method == HttpMethods.Delete) 
                {
                    string produtoID = req.Headers["produtoID"];
                    if(!string.IsNullOrEmpty(produtoID))
                        response = controle.ExcluirProduto(Convert.ToInt64(produtoID));
                }
                else if (req.Method == HttpMethods.Post) 
                {
                    var json = JsonConvert.DeserializeObject<ProdutoRequest>(requestBody);
                    response = controle.CadastroProduto(json);
                }
                else if (req.Method == HttpMethods.Put) 
                {
                    var produto = JsonConvert.DeserializeObject<Produto>(requestBody);
                    if(produto != null)
                        response = controle.AtualizarProduto(produto);
                } 
                else if (req.Method == HttpMethods.Get) 
                {
                    string nomeProduto = req.Headers["nomeProduto"];

                    if(!string.IsNullOrEmpty(nomeProduto))
                        response.vProdutos = controle.BuscarListaProduto(nomeProduto);
                }         

                return new OkObjectResult(response);
            }
            catch (Exception e)
            {
                response.StatusRetorno = (int)HttpStatusCode.BadRequest;
                response.Erro = $"Erro ao cadastrar/atualizar/buscar produto: '{e.Message}'";

                return new BadRequestObjectResult(response);
            }
        }
    }
}