using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using System.Web.Http;
using mba_es_25_grupo_02_backend.Models;

using Bootcamp.AzFunc.Controle;
using mba_es_25_grupo_02_backend.Models.API;
using mba_es_25_grupo_02_backend.Models.API.Pessoa;

namespace Bootcamp.AzFunc.Functions
{
    public class AzFunc_Logon : ApiController 
    {
        public ControlePessoa controlePessoa = new ControlePessoa();

        [FunctionName("AzFunc_Logon")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
        {
            string email = req.Headers["Email"];
            string senha = req.Headers["Senha"];

            var resultado = new LoginResponse();
            
            try
            {
                resultado = controlePessoa.ConsultaPessoaLogin(email, senha);

                return new OkObjectResult(resultado);
            }
            catch (Exception e)
            {
                resultado.StatusRetorno = 404;
                resultado.Erro          = $"Erro ao fazer login: '{e.Message}'" ;
                resultado.Pessoa        = null;

                return new BadRequestObjectResult(resultado);
            }
        }
    }
}
