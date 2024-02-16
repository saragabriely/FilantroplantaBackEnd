using System;
using System.ComponentModel.DataAnnotations;

namespace mba_es_25_grupo_02_backend.Models
{
    public class v_Produto
    {
        public long ProdutoID { get; set; }
        public string Descricao { get; set; }
        public string NomeProdutor { get; set; }
        public decimal ValorPorKG { get; set; }
        public string ValorPorKGFormatado { get; set; }
        public long ProdutorID { get; set; }
        public long QuantidadeDisponivel { get; set; }
        public string Localizacao { get; set; }
    }
}