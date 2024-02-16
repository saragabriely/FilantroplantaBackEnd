using System.Collections.Generic;
using Bootcamp.AzFunc;

namespace mba_es_25_grupo_02_backend.Models.API
{
    public class PessoaResponse : ResponseBase
    {
        public Models.Pessoa Pessoa { get; set; }
    }
}