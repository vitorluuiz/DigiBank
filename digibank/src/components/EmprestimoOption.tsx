import React from 'react';
import { EmprestimoProps, OptionProps } from '../@types/Emprestimo';
import ModalEmprestimo from './Emprestimos/MainModal';

export function EmprestimoOption({
  option,
  onUpdate,
}: {
  option: OptionProps;
  onUpdate: () => void;
}) {
  return (
    <div className="suport-emprestimo-option">
      <h2>
        Empréstimo:{' '}
        {option.valor.toLocaleString('pt-BR', {
          style: 'currency',
          currency: 'BRL',
        })}
      </h2>
      <div className="infos-emprestimo-option">
        <span>Prazo estimado: {option.prazoEstimado} dias</span>
        <span>Taxa de juros: {option.taxaJuros}% a.m</span>
      </div>
      {/* <button onClick={onClick}>Pegar empréstimo</button> */}
      <ModalEmprestimo onUpdate={onUpdate} data={option} type="option" />
    </div>
  );
}

export function Emprestimo({
  emprestimo,
  onUpdate,
}: {
  emprestimo: EmprestimoProps;
  onUpdate: () => void;
}) {
  return (
    <div className="suport-emprestimo-option">
      <h2>
        Valor:{' '}
        {emprestimo.idEmprestimoOptionsNavigation.valor.toLocaleString('pt-BR', {
          style: 'currency',
          currency: 'BRL',
        })}
      </h2>
      <div className="infos-emprestimo-option">
        <span>Taxa de juros: {emprestimo.idEmprestimoOptionsNavigation.taxaJuros}% a.m</span>
        <span>
          Data inicial:{' '}
          {new Date(emprestimo.dataInicial).toLocaleDateString('pt-BR', {
            day: '2-digit',
            month: '2-digit',
            year: 'numeric',
          })}
        </span>
        <span>
          Data final:{' '}
          {new Date(emprestimo.dataFinal).toLocaleDateString('pt-BR', {
            day: '2-digit',
            month: '2-digit',
            year: 'numeric',
          })}
        </span>
        <span>
          Valor pago:{' '}
          {emprestimo.valorPago.toLocaleString('pt-BR', {
            currency: 'BRL',
            style: 'currency',
          })}
        </span>
        {emprestimo.ultimoPagamento === emprestimo.dataInicial ? (
          <span>Último pagamento: Nada pago</span>
        ) : (
          <span>
            Último pagamento:{' '}
            {new Date(emprestimo.ultimoPagamento).toLocaleDateString('pt-BR', {
              day: '2-digit',
              month: '2-digit',
              year: 'numeric',
            })}
          </span>
        )}
      </div>
      <ModalEmprestimo onUpdate={onUpdate} data={emprestimo} type="emprestimo" />
    </div>
  );
}
