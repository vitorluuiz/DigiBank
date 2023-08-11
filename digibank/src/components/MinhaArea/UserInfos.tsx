import React from 'react';
import { Link } from 'react-router-dom';
import { MetaDestaque } from '../../@types/MetaDestaque';

export function SaldoBar({ saldo }: { saldo: number | undefined }) {
  return (
    <div title="Seu saldo disponível" className="suport-info-user">
      <h3>Saldo disponível</h3>
      <div>
        <span>{saldo?.toLocaleString('pt-BR', { currency: 'BRL', style: 'currency' })}</span>
        {saldo !== undefined ? (
          <span style={{ color: saldo >= 0 ? '#2FD72C' : '#E40A0A' }}>+25% Jul</span>
        ) : null}
      </div>
    </div>
  );
}

export function InvestimentosBar({ investido }: { investido: number | undefined }) {
  return (
    <div title="Aportes iniciais em seus investimentos somados" className="suport-info-user">
      <h3>Total em investimentos</h3>
      <div>
        <span>{investido?.toLocaleString('pt-BR', { currency: 'BRL', style: 'currency' })}</span>
        {investido !== undefined ? (
          <span style={{ color: investido >= 0 ? '#2FD72C' : '#E40A0A' }}>+9% Jul</span>
        ) : null}
      </div>
    </div>
  );
}

export function MetasBar({ meta }: { meta: MetaDestaque | undefined }) {
  return meta !== undefined ? (
    <div title={`Sua meta destaque é ${meta?.titulo}`} className="suport-info-user">
      <h3>{meta?.titulo}</h3>
      <div>
        <span>
          {meta?.arrecadado.toLocaleString('pt-BR', { currency: 'BRL', style: 'currency' })}
        </span>
        {meta !== undefined ? (
          <span
            style={{
              color:
                ((meta?.arrecadado ?? 0) / (meta?.valorMeta ?? 1)) * 100 >= 0
                  ? '#2FD72C'
                  : '#E40A0A',
            }}
          >
            {(((meta?.arrecadado ?? 0) / (meta?.valorMeta ?? 1)) * 100).toFixed(2)}%
          </span>
        ) : null}
      </div>
    </div>
  ) : (
    <Link to="/metas" className="suport-info-user btnPressionavel">
      <h3>Crie uma meta agora</h3>
      <div>
        <span>Não leva nem um minuto</span>
      </div>
    </Link>
  );
}

export function PontosBar({ pontos }: { pontos: number | undefined }) {
  return (
    <div title="Seus pontos de vantagem DigiBank" className="suport-info-user">
      <h3>DigiPoints</h3>
      <div>
        <span>{pontos} pontos</span>
        <span>+4% Jul</span>
      </div>
    </div>
  );
}
