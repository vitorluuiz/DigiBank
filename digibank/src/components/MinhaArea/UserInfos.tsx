import React from 'react';
import { MetaDestaque } from '../../@types/MetaDestaque';

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

export function InvestimentosBar({ investido }: { investido: number | undefined }) {
  return (
    <div className="suport-info-user">
      <h3>Total em investimentos</h3>
      <div>
        <span>{investido?.toLocaleString('pt-BR', { currency: 'BRL', style: 'currency' })}</span>
        <span>+9% Jul</span>
      </div>
    </div>
  );
}

export function MetasBar({ meta }: { meta: MetaDestaque | undefined }) {
  return (
    <div className="suport-info-user">
      <h3>{meta?.titulo}</h3>
      <div>
        <span>
          {meta?.arrecadado.toLocaleString('pt-BR', { currency: 'BRL', style: 'currency' })}
        </span>
        <span>{(((meta?.arrecadado ?? 0) / (meta?.valorMeta ?? 1)) * 100).toFixed(2)}%</span>
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
