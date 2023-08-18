import React, { Dispatch } from 'react';
import api from '../services/api';
import { useSnackBar } from '../services/snackBarProvider';

export function BloquearBtn({ idCartao, dispatch }: { idCartao: number; dispatch: Dispatch<any> }) {
  const { postMessage } = useSnackBar();

  function Bloquear(id: number) {
    api
      .patch(`Cartao/Bloquear/${id}`)
      .then((response) => {
        if (response.status === 200) {
          postMessage({
            message: 'Cartão bloqueado com sucesso',
            severity: 'success',
            timeSpan: 3000,
          });
          dispatch({ type: 'update' });
        }
      })
      .catch(() => {
        postMessage({ message: 'Não foi possível bloquear', severity: 'error', timeSpan: 3000 });
      });
  }

  return (
    <button
      title="Bloqueie e desbloqueie seu cartão quando desejar"
      onClick={() => {
        Bloquear(idCartao);
      }}
      className="card-option"
    >
      Bloquear cartão
    </button>
  );
}

export function DesbloquearBtn({
  idCartao,
  dispatch,
}: {
  idCartao: number;
  dispatch: Dispatch<any>;
}) {
  const { postMessage } = useSnackBar();

  function Desbloquear(id: number) {
    api
      .patch(`Cartao/Desbloquear/${id}`)
      .then((response) => {
        if (response.status === 200) {
          postMessage({
            message: 'Cartão desbloqueado com sucesso',
            severity: 'success',
            timeSpan: 3000,
          });
          dispatch({ type: 'update' });
        }
      })
      .catch(() => {
        postMessage({ message: 'Não foi possível desbloquear', severity: 'error', timeSpan: 3000 });
      });
  }

  return (
    <button
      title="Bloqueie e desbloqueie seu cartão quando desejar"
      onClick={() => {
        Desbloquear(idCartao);
      }}
      className="card-option"
    >
      Desbloquear
    </button>
  );
}
