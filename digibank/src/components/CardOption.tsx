import React from 'react';
import { toast } from 'react-toastify';

import api from '../services/api';

export function BloquearBtn({
  idCartao,
  onClick,
}: {
  idCartao: number | undefined;
  onClick: () => void;
}) {
  function Bloquear(id: number | undefined) {
    api
      .patch(`Cartao/Bloquear/${id}`)
      .then((response) => {
        if (response.status === 200) {
          toast.success('Cartão bloqueado');
        }
      })
      .catch(() => {
        toast.error('Não foi possível bloquear');
      });
  }

  return (
    <button
      onClick={() => {
        Bloquear(idCartao);
        onClick();
      }}
      className="card-option"
    >
      Bloquear cartão
    </button>
  );
}

export function DesbloquearBtn({
  idCartao,
  onClick,
}: {
  idCartao: number | undefined;
  onClick: () => void;
}) {
  function Desbloquear(id: number | undefined) {
    api
      .patch(`Cartao/Desbloquear/${id}`)
      .then((response) => {
        if (response.status === 200) {
          toast.success('Cartão desbloqueado');
        }
      })
      .catch(() => {
        toast.error('Cartão bloqueado');
      });
  }

  return (
    <button
      onClick={() => {
        Desbloquear(idCartao);
        onClick();
      }}
      className="card-option"
    >
      Desbloquear
    </button>
  );
}
