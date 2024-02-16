using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using mba_es_25_grupo_02_backend.Models.API;
using Bootcamp.AzFunc.Controle;
using mba_es_25_grupo_02_backend.Models;
using System.Net;
using mba_es_25_grupo_02_backend.Models.API.Carrinho;

namespace Company.Function
{
    public static class AzFunc_Carrinho
    {
        [FunctionName("AzFunc_Carrinho")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var controle = new ControleCarrinho();
            var response = new CarrinhoResponse();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            
            try
            {
                if (req.Method == HttpMethods.Get)
                {
                    var clienteID  = Convert.ToInt64(req.Headers["clienteID"]);
                    response.vItensCarrinho = controle.BuscaListaItemCarrinhoPorCliente(clienteID);
                }
                else if (req.Method == HttpMethods.Delete)
                {
                    string itemCarrinhoID = req.Headers["ItemCarrinhoID"];
                    if(!string.IsNullOrEmpty(itemCarrinhoID))
                        response = controle.RemoverItemCarrinho(Convert.ToInt64(itemCarrinhoID));
                }
                else
                {
                    var item = JsonConvert.DeserializeObject<ItemCarrinho>(requestBody);

                    if(item != null)
                    {
                        if (req.Method == HttpMethods.Post) 
                            response = controle.CadastrarItemCarrinho(item);
                    }
                }
                
                if(response.StatusRetorno == (int)HttpStatusCode.OK)
                    return new OkObjectResult(response);
                else 
                    return new BadRequestObjectResult(response);
            }
            catch (Exception e)
            {
                response.StatusRetorno = (int)HttpStatusCode.BadRequest;
                response.Erro = $"Erro ao manipular Pedido: '{e.Message}'";

                return new BadRequestObjectResult(response);
            }
        }
    }
}
