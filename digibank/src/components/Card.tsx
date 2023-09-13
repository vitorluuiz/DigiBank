import React, { Dispatch } from 'react';
import { CartaoProps } from '../@types/Cartao';

import Logo from '../assets/img/logoBranca.png';
import api from '../services/api';
import { BloquearBtn, DesbloquearBtn } from './CardOption';
import ModalNovoCartao from './MinhaArea/ModalNovoCartao';

import LockIcon from '../assets/img/lock_icon.svg';
import UnlockIcon from '../assets/img/unlock_icon.svg';
import ModalAltSenha from './MinhaArea/ModalAlterarSenha';
import { useSnackBar } from '../services/snackBarProvider';

export function Card({
  cartao,
  nomeUsuario,
}: {
  cartao: CartaoProps | undefined;
  nomeUsuario: string | undefined;
}) {
  function FormatNumero(numero: string | undefined) {
    const numeroFormatado =
      typeof numero !== 'undefined' ? numero.replace(/\d{4}(?=\d)/g, '$& ') : 'Sem cartão';

    return numeroFormatado;
  }

  return cartao !== undefined ? (
    <div className="credit-card">
      <div className="logo-credit-card">
        <img alt="logo do cartão" src={Logo} />
        {cartao.isValid ? (
          <img alt="Icone de desbloqueio" src={UnlockIcon} className="lockIcon" />
        ) : (
          <img alt="Icone de bloqueio" src={LockIcon} className="lockIcon" />
        )}
      </div>
      <div className="info-credit-card">
        <span title={cartao.nome}>{cartao.nome}</span>
        <h2 title={FormatNumero(cartao.numero)}>{FormatNumero(cartao.numero)}</h2>
        <span title={nomeUsuario}>{nomeUsuario}</span>
      </div>
    </div>
  ) : (
    <div className="credit-card">
      <div className="logo-credit-card">
        <img alt="logo do cartão" src={Logo} />
      </div>
      <div className="info-credit-card">
        <h2>Sem cartão ativo</h2>
        <span>Crie um cartão agora</span>
      </div>
    </div>
  );
}

export function CardOptions({
  cartao,
  dispatch,
}: {
  cartao: CartaoProps | undefined;
  dispatch: Dispatch<any>;
}) {
  const { postMessage } = useSnackBar();

  function Excluir(idCartao: number) {
    api.delete(`Cartao/${idCartao}`).then((response) => {
      if (response.status === 200) {
        postMessage({
          message: 'Cartão excluído',
          severity: 'success',
          timeSpan: 3000,
        });
        dispatch({ type: 'update' });
      }
    });
  }

  return cartao === undefined ? (
    <div className="options-card" style={{ justifyContent: 'center', height: '50%' }}>
      <ModalNovoCartao dispatch={dispatch} />
    </div>
  ) : (
    <div className="options-card">
      <ModalAltSenha dispatch={dispatch} idCartao={cartao.idCartao} />
      {cartao?.isValid ? (
        <BloquearBtn dispatch={dispatch} idCartao={cartao.idCartao} />
      ) : (
        <DesbloquearBtn dispatch={dispatch} idCartao={cartao.idCartao} />
      )}
      <button
        title="Excluir permanentemente seu cartão"
        className="card-option"
        onClick={() => {
          Excluir(cartao.idCartao);
        }}
      >
        Excluir Cartão
      </button>
    </div>
  );
}
