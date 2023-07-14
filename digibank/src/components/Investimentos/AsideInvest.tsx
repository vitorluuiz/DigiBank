import React from 'react';

interface AsideProps {
  componenteExibido: number | null;
  exibirComponente: (componente: number) => void;
}

const AsideInvest: React.FC<AsideProps> = ({ componenteExibido, exibirComponente }) => (
  <div className="boxRedirects">
    <h1>Investimentos sob demanda</h1>
    <button onClick={() => exibirComponente(3)} className={componenteExibido === 3 ? 'active' : ''}>
      Ações
    </button>
    <button onClick={() => exibirComponente(4)} className={componenteExibido === 4 ? 'active' : ''}>
      Fundos
    </button>
    <button onClick={() => exibirComponente(2)} className={componenteExibido === 2 ? 'active' : ''}>
      Renda Fixa
    </button>
    <button onClick={() => exibirComponente(5)} className={componenteExibido === 5 ? 'active' : ''}>
      Criptomoeda
    </button>
  </div>
);

export default AsideInvest;
