using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Options;
using System;
using Microsoft.EntityFrameworkCore;
using mba_es_25_grupo_02_backend;

[assembly: FunctionsStartup(typeof(Bootcamp.AzFunc.Startup))]

namespace Bootcamp.AzFunc
{
    public class Startup : FunctionsStartup
    {
        public string cs = "Server=tcp:db-server-mba-es-production.database.windows.net,1433;Initial Catalog=db-mba-es-production;Persist Security Info=False;User ID=admindb;Password=FaculdadeImpact@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public override void Configure(IFunctionsHostBuilder builder)
        {
            //var keyVaultUrl = new Uri(Environment.GetEnvironmentVariable("KeyVaultUrl"));
            //var secretClient = new SecretClient(keyVaultUrl, new DefaultAzureCredential());
            //var cs = secretClient.GetSecret("sql").Value.Value;
            
            builder.Services.AddDbContextPool<dbmbaesproductionContext>(Options => Options.UseSqlServer(cs));
        }
    }
}