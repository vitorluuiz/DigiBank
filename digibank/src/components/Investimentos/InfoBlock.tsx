import React from 'react';
import { formatNumber } from '../../services/formater';

export default function InfoBlock({
  name,
  valor,
  isCurrency,
  size,
  variation,
}: {
  name: string;
  valor: string | number;
  isCurrency?: boolean;
  size?: string;
  variation?: number;
}) {
  return (
    <article className={`option-info-box ${size}`}>
      <h3>
        {typeof valor === 'number' && isCurrency ? `${formatNumber(valor)}` : valor}
        <span>{variation !== 0 ? `${variation?.toPrecision(2)}%` : ''}</span>
      </h3>
      <span>{name}</span>
    </article>
  );
}

InfoBlock.defaultProps = {
  isCurrency: false,
  size: 'slim',
  variation: 0,
};
