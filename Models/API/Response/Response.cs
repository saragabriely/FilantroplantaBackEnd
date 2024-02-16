using Bootcamp.AzFunc;
using System.Collections.Generic;

namespace mba_es_25_grupo_02_backend.Models.API
{
    public class ResponseBase
    {
        public int StatusRetorno { get; set; }
        public string Erro { get; set; }
        public long ID { get; set; }
    }
}

