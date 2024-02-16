using Bootcamp.AzFunc;
using System.Collections.Generic;

namespace mba_es_25_grupo_02_backend.Models.API.Estoque
{
    public class EstoqueResponse : ResponseBase
    {
        public Models.Estoque Estoque { get; set; }
        public List<Models.Estoque> Estoques { get; set; }
    }
}

