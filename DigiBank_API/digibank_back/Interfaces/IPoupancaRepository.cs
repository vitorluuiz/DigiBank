using System;

namespace digibank_back.Interfaces
{
    public interface IPoupancaRepository
    {
        bool Sacar(int idUsuario, decimal quantidade);
        bool Depositar(int idUsuario, decimal quantidade);
        decimal CalcularGanhos(int idUsuario, DateTime inicio, DateTime fim);
    }
}
