import React from 'react';

export function SaldoBar({ saldo }: { saldo: number | undefined }) {
  return (
    <div className="suport-info-user">
      <h3>Saldo disponível</h3>
      <div>
        <span>{saldo?.toLocaleString('pt-BR', { currency: 'BRL', style: 'currency' })}</span>
        <span>+25% Jul</span>
      </div>
    </div>
  );
}

export function InvestimentosBar() {
  return (
    <div className="suport-info-user">
      <h3>Total em investimentos</h3>
      <div>
        <span>R$5.930,00</span>
        <span>+9% Jul</span>
      </div>
    </div>
  );
}

export function MetasBar() {
  return (
    <div className="suport-info-user">
      <h3>Bicicleta nova</h3>
      <div>
        <span>R$5.930,00</span>
        <span>14%</span>
      </div>
    </div>
  );
}

export function PontosBar({ pontos }: { pontos: number | undefined }) {
  return (
    <div className="suport-info-user">
      <h3>DigiPoints</h3>
      <div>
        <span>{pontos} pontos</span>
        <span>+4% Jul</span>
      </div>
    </div>
  );
}
