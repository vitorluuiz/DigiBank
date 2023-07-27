import React from 'react';

export default function InfoBlock({
  name,
  valor,
  isCurrency,
  size,
}: {
  name: string;
  valor: string | number;
  isCurrency?: boolean;
  size?: string;
}) {
  return (
    <article className={`option-info-box ${size}`}>
      <h2>
        {typeof valor === 'number' && isCurrency
          ? `${valor.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}`
          : valor}
      </h2>
      <span>{name}</span>
    </article>
  );
}

InfoBlock.defaultProps = {
  isCurrency: false,
  size: 'slim',
};
