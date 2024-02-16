using System;
using System.ComponentModel.DataAnnotations;

namespace mba_es_25_grupo_02_backend.Models
{
    public class Produto
    {
        [Key]
        public long ProdutoID { get; set; }
        public string Descricao { get; set; }
        public decimal ValorPorKg { get; set; }
        public long ProdutorID { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataUltimaAtualizacao { get; set; }
    }
}