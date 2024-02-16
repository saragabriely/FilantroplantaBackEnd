using System.Collections.Generic;
using Bootcamp.AzFunc;

namespace mba_es_25_grupo_02_backend.Models.API.Pessoa
{
    public class LoginResponse : ResponseBase
    {
        public Models.Pessoa Pessoa { get; set; }
        public List<Produto> Produtos { get; set; }
        public List<Models.Estoque> Estoques { get; set; }
        public List<PedidoAPI> Pedidos { get; set; }
        public List<v_ItemCarrinho> vItensCarrinho { get; set; }
    }
}

