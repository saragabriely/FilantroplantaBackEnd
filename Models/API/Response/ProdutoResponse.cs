using Bootcamp.AzFunc;
using System.Collections.Generic;

namespace mba_es_25_grupo_02_backend.Models.API.Response
{
    public class ProdutoResponse : ResponseBase
    {
        public List<v_Produto> vProdutos { get; set; }
        public Models.Estoque Estoque { get; set; }
    }
}

