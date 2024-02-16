using System;

namespace mba_es_25_grupo_02_backend.Models
{
    public class ItemCarrinho
    {
        public long ItemCarrinhoID { get; set; }
        public long ProdutoID { get; set; }
        public long ProdutorID { get; set; }
        public long ClienteID { get; set; }
        public long Quantidade { get; set; }
        public decimal ValorPorKG { get; set; }
        public decimal ValorTotal { get; set; }
        public string Localizacao { get; set; }
        public DateTime DataAdicaoItem { get; set; }
        public DateTime DataUltimaAtualizacao { get; set; }
    }
}