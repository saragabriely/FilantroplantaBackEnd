using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using mba_es_25_grupo_02_backend.Models.API;
using Bootcamp.AzFunc.Controle;
using System.Web.Http;
using System.Collections.Generic;
using System.Net;
using mba_es_25_grupo_02_backend.Models;

namespace Bootcamp.AzFunc.Functions
{
    public class AzFunc_Pedido : ApiController
    {
        [FunctionName("AzFunc_Pedido")]
        public static async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", "post", "put",  Route = null)] HttpRequest req)
        {
            var controle = new ControlePedido();
            var retorno  = new PedidoResponse();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            
            try
            {
                if (req.Method == HttpMethods.Get)
                {
                    long pedidoID   = Convert.ToInt64(req.Headers["pedidoID"]);
                    long produtorID = Convert.ToInt64(req.Headers["produtorID"]);
                    long clienteID  = Convert.ToInt64(req.Headers["clienteID"]);

                    retorno = controle.BuscarPedidos(pedidoID, produtorID, clienteID);
                }
                else
                {
                   if (req.Method == HttpMethods.Post) 
                    {
                        var json = JsonConvert.DeserializeObject<PedidoAPI>(requestBody);
                        if(json.Produtos != null)
                            retorno = controle.CadastroPedido(json);
                    }
                            
                    else if (req.Method == HttpMethods.Put) 
                    {
                        var json = JsonConvert.DeserializeObject<Pedido>(requestBody);
                        if(json != null && json.PedidoID > 0)
                            retorno = controle.AtualizarPedido(json);
                    }
                }
                
                return new OkObjectResult(retorno);
            }
            catch (Exception e)
            {
                retorno.StatusRetorno = (int)HttpStatusCode.BadRequest;
                retorno.Erro = $"Erro ao manipular Pedido: '{e.Message}'";

                return new BadRequestObjectResult(retorno);
            }
        }
    }
}
