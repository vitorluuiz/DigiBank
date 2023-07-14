using System;

namespace digibank_back.Interfaces
{
    public interface IRendaFixaRepository
    {
        decimal Saldo(int idUsuario, DateTime inicio, DateTime fim);
        decimal CalcularLucros(int idUsuario, DateTime inicio, DateTime fim);
    }
}
