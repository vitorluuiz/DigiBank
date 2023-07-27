import React from 'react';
import { Link } from 'react-router-dom';

import { InvestidosProps } from '../../@types/Investidos';

export default function RecommendedInvestiment({
  type,
  investimento,
}: {
  type: string;
  investimento: InvestidosProps;
}) {
  const handlePostClick = () => {
    window.scrollTo({
      top: 0,
      behavior: 'smooth',
    });
  };

  if (type === 'Big') {
    return (
      <Link
        to={`/diginvest/investimento/${investimento.idInvestimentoOption}`}
        className="recomendado-support diginvest"
        onClick={handlePostClick}
      >
        <img
          alt="Logo da postagem recomendada"
          src={investimento.mainImg}
          style={{ backgroundColor: `#${investimento.mainColorHex}` }}
        />

        <div className="recomendado-infos">
          <div>
            <h3>{investimento.nome}</h3>
            <h4>{investimento.sigla}</h4>
          </div>
          <div className="avaliacao-recomendado">
            <span>{investimento.variacaoPercentual}%</span>
            <h5>{`${investimento.valor === 0 ? 'Grátis' : `${investimento.valor}BRL`}`}</h5>
          </div>
        </div>
      </Link>
    );
  }
  if (type === 'cripto') {
    return (
      <Link
        to={`/diginvest/investimento/${investimento.idInvestimentoOption}`}
        className="recomendado-support slim"
        onClick={handlePostClick}
      >
        <div className="recomendado-infos-slim diginvest">
          <div className="box-infos">
            <div>
              <h3>{investimento.nome}</h3>
              <h4>{investimento.sigla}</h4>
            </div>
            <div className="box-img">
              <img alt="Logo da postagem recomendada" src={investimento.logo} />
            </div>
          </div>
          <div className="avaliacao-recomendado-slim">
            <span>{investimento.variacaoPercentual} Hoje</span>
            <h3>{`R$${investimento.valor === 0 ? 'Grátis' : `${investimento.valor}`}`}</h3>
          </div>
        </div>
      </Link>
    );
  }
  if (type === 'rendaFixa') {
    return (
      <Link
        to={`/diginvest/investimento/${investimento.idInvestimentoOption}`}
        className="recomendado-support slim"
        onClick={handlePostClick}
      >
        <div className="recomendado-infos-slim">
          <div className="box-infos rendaFixa">
            <div>
              <h3>{investimento.nome}</h3>
            </div>
          </div>
          <div className="avaliacao-recomendado-slim">
            <div>
              <span>Rentabilidade</span>
              <span>101% CDI</span>
            </div>
            <div>
              <span>Aplicação minima</span>
              <h3>{`R$${investimento.valor === 0 ? 'Grátis' : `${investimento.valor}`}`}</h3>
            </div>
          </div>
        </div>
      </Link>
    );
  }
  return null;
}
