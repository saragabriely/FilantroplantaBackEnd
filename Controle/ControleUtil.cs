using System;
using System.Threading.Tasks;
using System.Data;
using mba_es_25_grupo_02_backend;
using mba_es_25_grupo_02_backend.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using mba_es_25_grupo_02_backend.Models.API;
using System.Net;
using System.Globalization;

namespace Bootcamp.AzFunc.Controle
{
    public class ControleUtil 
    {
        public string FormatarDecimal(decimal value)
        {
            return value.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
        }

    }
}