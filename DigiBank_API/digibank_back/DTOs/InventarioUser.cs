using System;
using System.Collections.Generic;

namespace digibank_back.DTOs
{
    public class InventarioUser
    {
        public int IdInventario { get; set; }
        public int IdUsuario { get; set; }
        public DateTime DataAquisicao { get; set; }
        public List<string> Imgs { get; set; }
    }
}
