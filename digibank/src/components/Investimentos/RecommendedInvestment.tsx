import React from 'react';
import { Link } from 'react-router-dom';

import {
  InvestidoOptionProps,
  MinimalOptionProps,
  OptionPropsGenerico,
} from '../../@types/InvestimentoOptions';

export default function RecommendedInvestiment({
  type,
  investimento,
  isInvestido,
}: {
  type: string;
  investimento: MinimalOptionProps | InvestidoOptionProps;
  isInvestido?: boolean;
}) {
  const mapInvestimento = (
    Data: MinimalOptionProps | InvestidoOptionProps,
  ): OptionPropsGenerico => {
    let investment: OptionPropsGenerico;

    if ('idInvestimentoOptionNavigation' in Data) {
      investment = {
        dataAquisicao: Data.dataAquisicao,
        depositoInicial: Data.depositoInicial,
        areaInvestimento: Data.idInvestimentoOptionNavigation.areaInvestimento,
        IdInvestimento: Data.IdInvestimento,
        idUsuario: Data.idUsuario,
        isEntrada: Data.isEntrada,
        qntCotas: Data.qntCotas,
        idInvestimentoOption: Data.idInvestimentoOptionNavigation.idInvestimentoOption,
        idTipoInvestimento: Data.idInvestimentoOptionNavigation.idTipoInvestimento,
        idAreaInvestimento: Data.idInvestimentoOptionNavigation.idAreaInvestimento,
        nome: Data.idInvestimentoOptionNavigation.nome,
        sigla: Data.idInvestimentoOptionNavigation.sigla,
        descricao: Data.idInvestimentoOptionNavigation.descricao,
        logo: Data.idInvestimentoOptionNavigation.logo,
        mainImg: Data.idInvestimentoOptionNavigation.mainImg,
        mainColorHex: Data.idInvestimentoOptionNavigation.mainColorHex,
        valor: Data.idInvestimentoOptionNavigation.valor,
        colaboradores: Data.idInvestimentoOptionNavigation.colaboradores,
        qntCotasTotais: Data.idInvestimentoOptionNavigation.qntCotasTotais,
        fundacao: Data.idInvestimentoOptionNavigation.fundacao,
        abertura: Data.idInvestimentoOptionNavigation.abertura,
        variacaoPercentual: Data.idInvestimentoOptionNavigation.variacaoPercentual,
      };
    } else {
      investment = {
        dataAquisicao: '',
        areaInvestimento: Data.areaInvestimento,
        depositoInicial: 0,
        IdInvestimento: 0,
        idUsuario: 0,
        isEntrada: false,
        qntCotas: 0,
        idInvestimentoOption: Data.idInvestimentoOption,
        idTipoInvestimento: Data.idTipoInvestimento,
        idAreaInvestimento: Data.idAreaInvestimento,
        nome: Data.nome,
        sigla: Data.sigla,
        descricao: Data.descricao,
        logo: Data.logo,
        mainImg: Data.mainImg,
        mainColorHex: Data.mainColorHex,
        valor: Data.valor,
        colaboradores: Data.colaboradores,
        qntCotasTotais: Data.qntCotasTotais,
        fundacao: Data.fundacao,
        abertura: Data.abertura,
        variacaoPercentual: Data.variacaoPercentual,
      };
    }
    return investment;
  };

  const mappedInvestimento = mapInvestimento(investimento);

  // console.log(mapInvestimento(investimento));

  const handlePostClick = () => {
    window.scrollTo({
      top: 0,
      behavior: 'smooth',
    });
  };

  if (type === 'Big') {
    return (
      <Link
        to={`/diginvest/investimento/${mappedInvestimento.idInvestimentoOption}`}
        className="recomendado-support diginvest"
        onClick={handlePostClick}
      >
        <img alt="Logo da postagem recomendada" src={mappedInvestimento.logo} />
        <span
          id="sigla"
          style={{
            color: '#FFF',
            backgroundColor: `#${mappedInvestimento.mainColorHex}`,
          }}
        >
          {mappedInvestimento.sigla}
        </span>

        <div className="recomendado-infos">
          <h3>{mappedInvestimento.nome}</h3>
          <h4>{mappedInvestimento.areaInvestimento}</h4>
          <div className="avaliacao-recomendado">
            {isInvestido ? (
              <span>Cotas: {mappedInvestimento.qntCotas}</span>
            ) : (
              <span
                style={{
                  color: mappedInvestimento.variacaoPercentual >= 0 ? '#2FD72C' : '#E40A0A',
                }}
              >
                {mappedInvestimento.variacaoPercentual}%
              </span>
            )}

            <h5>
              {mappedInvestimento.valor.toFixed(2)}
              BRL
            </h5>
          </div>
        </div>
      </Link>
    );
  }
  if (type === 'cripto') {
    return (
      <Link
        to={`/diginvest/investimento/${mappedInvestimento.idInvestimentoOption}`}
        className="recomendado-support slim"
        onClick={handlePostClick}
      >
        <div className="recomendado-infos-slim diginvest">
          <div className="box-infos">
            <div>
              <h3>{mappedInvestimento.nome}</h3>
              <h4>{mappedInvestimento.sigla}</h4>
            </div>
            <div className="box-img">
              <img alt="Logo da postagem recomendada" src={mappedInvestimento.logo} />
            </div>
          </div>
          <div className="avaliacao-recomendado-slim">
            <span>{mappedInvestimento.variacaoPercentual} Hoje</span>
            <h3>{`R$${
              mappedInvestimento.valor === 0 ? 'Grátis' : `${mappedInvestimento.valor}`
            }`}</h3>
          </div>
        </div>
      </Link>
    );
  }
  if (type === 'rendaFixa') {
    return (
      <Link
        to={`/diginvest/investimento/${mappedInvestimento.idInvestimentoOption}`}
        className="recomendado-support slim"
        onClick={handlePostClick}
      >
        <div className="recomendado-infos-slim">
          <div className="box-infos rendaFixa">
            <div>
              <h3>{mappedInvestimento.nome}</h3>
            </div>
          </div>
          <div className="avaliacao-recomendado-slim">
            <div>
              <span>Rentabilidade</span>
              <span>101% CDI</span>
            </div>
            <div>
              <span>Aplicação minima</span>
              <h3>{`R$${
                mappedInvestimento.valor === 0 ? 'Grátis' : `${mappedInvestimento.valor}`
              }`}</h3>
            </div>
          </div>
        </div>
      </Link>
    );
  }
  return null;
}

RecommendedInvestiment.defaultProps = {
  isInvestido: false,
};
