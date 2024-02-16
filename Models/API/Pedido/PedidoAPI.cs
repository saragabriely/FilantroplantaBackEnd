using System.Collections.Generic;
using Bootcamp.AzFunc;

namespace mba_es_25_grupo_02_backend.Models.API
{
    public class PedidoAPI
    {
        public Pedido Pedido { get; set; }
        public List<v_PedidoProduto> Produtos { get; set; }        
    }
}

