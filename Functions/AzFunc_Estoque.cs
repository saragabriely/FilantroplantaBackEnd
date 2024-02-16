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
using mba_es_25_grupo_02_backend.Models.API.Estoque;

namespace Bootcamp.AzFunc.Functions 
{
    public class AzFunc_Estoque : ApiController
    {
        [FunctionName("AzFunc_Estoque")]
        public static async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", "put", Route = null)] HttpRequest req)
        {
            var controle = new ControleEstoque();
            var response = new EstoqueResponse();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            try
            {
                if (req.Method == HttpMethods.Put) 
                {
                    var estoque = JsonConvert.DeserializeObject<Estoque>(requestBody);
                    response    = controle.CadastrarOuAtualizarEstoque(estoque);

                    return new OkObjectResult(response);
                }
                else if (req.Method == HttpMethods.Get) 
                {
                    string produtorID = req.Headers["produtorID"];
                    response.Estoques = controle.BuscarEstoquePorProdutor(Convert.ToInt64(produtorID));

                    return new OkObjectResult(response);
                }

                response.StatusRetorno = (int)HttpStatusCode.BadRequest;
                response.Erro = $"Metodo Não disponivel: '{req.Method}'";

                return new BadRequestObjectResult(response);
            }
            catch (Exception e)
            {
                response.StatusRetorno = (int)HttpStatusCode.BadRequest;
                response.Erro = $"Erro ao atualizar estoque: {e.Message}";

                return new BadRequestObjectResult(response);
            }
        }
    }
}
