import React, { useEffect, useState } from 'react';

interface AsideProps {
  type: string;
  componenteExibido: number | null;
  exibirComponente: (componente: number) => void;
}

const AsideInvest: React.FC<AsideProps> = ({ componenteExibido, exibirComponente, type }) => {
  const [titulo, setTitulo] = useState('');

  useEffect(() => {
    if (type === '') {
      setTitulo('Investimentos Sob Demanda');
    } else if (type === 'investidos') {
      setTitulo('Meus Investimentos');
    } else if (type === 'favoritos') {
      setTitulo('Investimentos Favoritos');
    }
  }, [type]);

  return (
    <div className="boxRedirects">
      <h1>{titulo}</h1>
      <button
        onClick={() => exibirComponente(3)}
        className={componenteExibido === 3 ? 'active' : ''}
      >
        Ações
      </button>
      <button
        onClick={() => exibirComponente(4)}
        className={componenteExibido === 4 ? 'active' : ''}
      >
        Fundos
      </button>
      <button
        onClick={() => exibirComponente(2)}
        className={componenteExibido === 2 ? 'active' : ''}
      >
        Renda Fixa
      </button>
      <button
        onClick={() => exibirComponente(5)}
        className={componenteExibido === 5 ? 'active' : ''}
      >
        Criptomoeda
      </button>
    </div>
  );
};
export default AsideInvest;
