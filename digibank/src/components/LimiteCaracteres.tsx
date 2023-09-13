import React from 'react';

export default function LimiteCaracteres({
  caracteresAtual,
  caracteresLimite,
}: {
  caracteresAtual: number | undefined;
  caracteresLimite: number;
}) {
  return (
    <span className={`caracteres-limite ${caracteresAtual === caracteresLimite ? 'limite' : ''}`}>
      {caracteresAtual}/{caracteresLimite}
    </span>
  );
}
