using System;
using System.ComponentModel.DataAnnotations;

namespace mba_es_25_grupo_02_backend.Models
{
    public class Estoque
    {
        [Key]
        public long EstoqueID { get; set; }
        public long ProdutoID { get; set; }
        public long ProdutorID { get; set; }
        public long QuantidadeDisponivel { get; set; }
        public long QuantidadeVendida { get; set; }
        public long QuantidadeReservada { get; set; }
        public DateTime DataUltimaAtualizacao { get; set; }
    }
}