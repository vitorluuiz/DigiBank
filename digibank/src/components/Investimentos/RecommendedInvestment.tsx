import React from 'react';
import { Link } from 'react-router-dom';

import { IMGROOT } from '../../services/api';
import { InvestimentoOptionsProps } from '../../@types/InvestimentoOptions';

export default function RecommendedInvestiment({
  type,
  investimento,
}: {
  type: string;
  investimento: InvestimentoOptionsProps;
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
        to={`/investimento/${investimento.idInvestimentoOption}`}
        className="recomendado-support"
        onClick={handlePostClick}
      >
        <img
          alt="Logo da postagem recomendada"
          src={`${IMGROOT}/${investimento.mainImg}`}
          style={{ backgroundColor: `#${investimento.mainColorHex}` }}
        />

        <div className="recomendado-infos">
          <div>
            <h3>{investimento.nome}</h3>
            <h4>{investimento.idAreaInvestimento}</h4>
          </div>
          <div className="avaliacao-recomendado">
            <span>{investimento.sigla}</span>
            <h5>{`${investimento.valorAcao === 0 ? 'Grátis' : `${investimento.valorAcao}BRL`}`}</h5>
          </div>
        </div>
      </Link>
    );
  }
  if (type === 'slim') {
    return (
      <Link
        to={`/post/${investimento.idInvestimentoOption}`}
        className="recomendado-support"
        onClick={handlePostClick}
      >
        <img
          alt="Logo da postagem recomendada"
          src={`${IMGROOT}/${investimento.mainImg}`}
          style={{ backgroundColor: `#${investimento.mainColorHex}` }}
        />

        <div className="recomendado-infos">
          <div>
            <h3>{investimento.nome}</h3>
            <h4>{investimento.idAreaInvestimento}</h4>
          </div>
          <div className="avaliacao-recomendado">
            <span>{investimento.sigla}</span>
            <h5>{`${investimento.valorAcao === 0 ? 'Grátis' : `${investimento.valorAcao}BRL`}`}</h5>
          </div>
        </div>
      </Link>
    );
  }
  return null;
}
