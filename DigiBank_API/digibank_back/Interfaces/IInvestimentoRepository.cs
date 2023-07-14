﻿using digibank_back.Domains;
using digibank_back.DTOs;
using System;
using System.Collections.Generic;

namespace digibank_back.Interfaces
{
    public interface IInvestimentoRepository
    {
        bool Comprar(Investimento newInvestimento);
        void Vender(int idInvestimento);
        void VenderCotas(int idInvestimento, decimal qntCotas);
        Investimento ListarPorId(int idInvestimento);
        List<Investimento> ListarTodos();
        List<Investimento> ListarDeUsuario(int idUsuario);
        ExtratoInvestimentos ExtratoTotalInvestido(int idUsuario);
        decimal ValorInvestimento(int idUsuario, int idTipoInvestimento);
        decimal ValorInvestimentos(int idUsuario);
    }
}
