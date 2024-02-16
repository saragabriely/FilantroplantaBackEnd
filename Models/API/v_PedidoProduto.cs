using System.Collections.Generic;
using Bootcamp.AzFunc;

namespace mba_es_25_grupo_02_backend.Models.API
{
    public class v_PedidoProduto
    {
        public long PedidoID { get; set; }
        public long PedidoProdutoID { get; set; }
        public long ProdutoID { get; set; }
        public long ProdutorID { get; set; }
        public long StatusPedidoID { get; set; }
        public long ClienteID { get; set; }
        public string NomeProdutor { get; set; }
        public string NomeCliente { get; set; }
        public string DescricaoProduto { get; set; }
        public long Quantidade { get; set; }
        public decimal ValorPorKg { get; set; }
        public decimal ValorTotalProduto { get; set; }
        public string Local { get; set; } 
        public int StatusPedidoProdutoID { get; set; }
    }
}
