import React from 'react';

export default function FlowBox({
  name,
  valor,
  saldo,
}: {
  name: string;
  valor: number;
  saldo?: boolean;
}) {
  return (
    <div className={`flow-box ${saldo ? 'saldo' : ''}`}>
      <h3>{name}</h3>
      <h3>
        {valor.toLocaleString('pt-BR', {
          currency: 'BRL',
          style: 'currency',
        })}
      </h3>
    </div>
  );
}

FlowBox.defaultProps = {
  saldo: false,
};
