using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mba_es_25_grupo_02_backend.Models
{
    public class Pedido
    {
        [Key]
        public long PedidoID { get; set; }
        public long StatusPedidoID { get; set; }
        public decimal ValorTotalPedido { get; set; }
        public long ClienteID { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataUltimaAtualizacao { get; set; }
    }
}
