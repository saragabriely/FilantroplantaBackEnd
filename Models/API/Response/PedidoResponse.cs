using System.Collections.Generic;
using Bootcamp.AzFunc;

namespace mba_es_25_grupo_02_backend.Models.API
{
    public class PedidoResponse : ResponseBase
    {
        public Pedido Pedido { get; set; }
        public PedidoAPI PedidoAPI { get; set; }
        public List<PedidoAPI> Pedidos { get; set; }
        public List<v_ItemCarrinho> vItensCarrinho { get; set; }   
    }
}

