import React, { Dispatch } from 'react';
import { toast } from 'react-toastify';

import api from '../services/api';

export function BloquearBtn({ idCartao, dispatch }: { idCartao: number; dispatch: Dispatch<any> }) {
  function Bloquear(id: number) {
    api
      .patch(`Cartao/Bloquear/${id}`)
      .then((response) => {
        if (response.status === 200) {
          toast.success('Cartão bloqueado');
          dispatch({ type: 'update' });
        }
      })
      .catch(() => {
        toast.error('Não foi possível bloquear');
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
  function Desbloquear(id: number) {
    api
      .patch(`Cartao/Desbloquear/${id}`)
      .then((response) => {
        if (response.status === 200) {
          toast.success('Cartão desbloqueado');
          dispatch({ type: 'update' });
        }
      })
      .catch(() => {
        toast.error('Não foi possível desbloquear');
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
