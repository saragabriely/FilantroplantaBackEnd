using Bootcamp.AzFunc;
using System.Collections.Generic;

namespace mba_es_25_grupo_02_backend.Models.API.Carrinho
{
    public class CarrinhoResponse : ResponseBase
    {
        public ItemCarrinho ItemCarrinho { get; set; }
        public v_ItemCarrinho vItemCarrinho { get; set; }
        public List<v_ItemCarrinho> vItensCarrinho { get; set; }
        public List<ItemCarrinho> ItensCarrinho { get; set; }
    }
}

