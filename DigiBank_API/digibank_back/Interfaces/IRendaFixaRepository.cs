using System;

namespace digibank_back.Interfaces
{
    public interface IRendaFixaRepository
    {
        decimal Saldo(int idUsuario, DateTime data);
        decimal CalcularLucros(int idUsuario, DateTime inicio, DateTime fim);
    }
}
