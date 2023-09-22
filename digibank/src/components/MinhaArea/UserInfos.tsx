// eslint-disable-next-line eslint-comments/disable-enable-pair
/* eslint-disable no-nested-ternary */
import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { MetaDestaque } from '../../@types/MetaDestaque';

const getMonthName = (month: number, monthsToPast: number) => {
  const months = [
    'Janeiro',
    'Fevereiro',
    'Março',
    'Abril',
    'Maio',
    'Junho',
    'Julho',
    'Agosto',
    'Setembro',
    'Outubro',
    'Novembro',
    'Dezembro',
  ];

  return month - monthsToPast >= 0
    ? months[month - monthsToPast]
    : months[12 + (month - monthsToPast)];
};

export function MyPlaceBar({
  name,
  valorAtual,
  valorAnterior,
  monthsToPast,
  title,
}: {
  name: string;
  valorAtual: number;
  valorAnterior: number;
  monthsToPast: number;
  title?: string;
}) {
  const [MonthName, setMonthName] = useState<string>('');
  const [Percentual, setPercentual] = useState<number>(0);
  const CalcPercentual = (anterior: number, atual: number) =>
    anterior === 0
      ? 0
      : parseFloat(((atual / anterior - (atual / anterior < 1 ? 1 : 0)) * 100).toFixed(1));

  useEffect(() => {
    setPercentual(CalcPercentual(valorAnterior, valorAtual));
    setMonthName(getMonthName(new Date().getMonth(), monthsToPast));
  }, [monthsToPast, valorAnterior, valorAtual]);

  return (
    <div title={title} className="suport-info-user">
      <h3>{name}</h3>
      <div>
        <span>{valorAtual.toLocaleString('pt-BR', { currency: 'BRL', style: 'currency' })}</span>
        <span
          title={`${Percentual}% de variação com relação a ${MonthName}`}
          className="monthly-variation"
          style={{ color: Percentual > 0 ? '#2FD72C' : Percentual < 0 ? '#E40A0A' : 'initial' }}
        >
          {Percentual === 0 ? '~=' : `${Percentual}%`}
          {` ${MonthName}`}
        </span>
      </div>
    </div>
  );
}

MyPlaceBar.defaultProps = {
  title: '',
};

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
