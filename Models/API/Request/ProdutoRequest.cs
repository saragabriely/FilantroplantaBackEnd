using System.Collections.Generic;
using Bootcamp.AzFunc;

namespace mba_es_25_grupo_02_backend.Models.API
{
    public class ProdutoRequest
    {
        public Produto Produto { get; set; }
        public long Quantidade { get; set; }       
    }
}

