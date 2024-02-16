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

namespace Bootcamp.AzFunc.Functions
{
    public class AzFunc_CadastroUser : ApiController
    {
        [FunctionName("AzFunc_CadastroUser")]
        public static async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", "post", "put", Route = null)] HttpRequest req)
        {   
            var controle = new ControlePessoa();
            var response = new PessoaResponse();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            try
            {
                if (req.Method == HttpMethods.Get)
                {
                    long pessoaID  = Convert.ToInt64(req.Headers["pessoaID"]);

                    response = controle.BuscarPessoa(pessoaID);
                } 
                else
                {
                    var pessoa = JsonConvert.DeserializeObject<Pessoa>(requestBody);

                    if (req.Method == HttpMethods.Post) 
                        response = controle.CadastroPessoa(pessoa);
                        
                    else if (req.Method == HttpMethods.Put) 
                        response = controle.AtualizarPessoa(pessoa);
                }

                return new OkObjectResult(response);
            }
            catch (Exception e)
            {
                response.StatusRetorno = (int)HttpStatusCode.BadRequest;
                response.Erro = $"Erro ao cadastrar pessoa: '{e.Message}'";

                return new BadRequestObjectResult(response);
            }
        }
    }
}
