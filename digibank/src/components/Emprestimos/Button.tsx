import React from 'react';

export default function GetButton({
  data,
  valor,
  isLoading,
  onPost,
}: {
  data: Date;
  valor: number;
  isLoading: boolean;
  onPost: () => void;
}) {
  return (
    <div className="support-button">
      <h1>{valor.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}</h1>
      <h4>
        Data da próxima parcela sugerida{' '}
        <span>
          {new Date(data).toLocaleDateString('pt-BR', {
            day: '2-digit',
            month: '2-digit',
            year: 'numeric',
          })}
        </span>
      </h4>
      <button disabled={isLoading} className="btnComponent" onClick={() => onPost()}>
        Pegar empréstimo
      </button>
    </div>
  );
}
