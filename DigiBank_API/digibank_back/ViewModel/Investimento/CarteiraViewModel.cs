using System;

namespace digibank_back.ViewModel.Investimento
{
    public class CarteiraViewModel
    {
        public int IdUsuario { get; set; }
        public decimal Valor { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
    }
}
