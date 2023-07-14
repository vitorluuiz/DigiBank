import React, { useState } from 'react';
import Header from '../../components/Header';
import AsideInvest from '../../components/Investimentos/AsideInvest';
import CarouselInvestimentos from '../../components/Investimentos/CarouselInvestments';

export default function InvestPlace() {
  const [componenteExibido, setComponenteExibido] = useState<number | null>(3);

  const exibirComponente = (componente: number) => {
    setComponenteExibido(componente);
  };

  return (
    <div>
      <Header type="" />
      <main className="container" id="diginvest">
        <AsideInvest componenteExibido={componenteExibido} exibirComponente={exibirComponente} />
        <CarouselInvestimentos type="comprados" typeInvestimento={componenteExibido} />
      </main>
    </div>
  );
}
