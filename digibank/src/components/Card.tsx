import React, { Dispatch, useState } from 'react';
import { toast } from 'react-toastify';
import { CartaoProps } from '../@types/Cartao';

import Logo from '../assets/img/logoBranca.png';
import api from '../services/api';
import { BloquearBtn, DesbloquearBtn } from './CardOption';
import ModalNovoCartao from './ModalNovoCartao';

import LockIcon from '../assets/img/lock_icon.svg';
import UnlockIcon from '../assets/img/unlock_icon.svg';

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
        <span>{cartao?.nome}</span>
        <h2>{FormatNumero(cartao?.numero)}</h2>
        <span>{nomeUsuario}</span>
      </div>
    </div>
  ) : (
    <div className="credit-card">
      <div className="logo-credit-card">
        <img alt="logo do cartão" src={Logo} />
      </div>
      <div className="info-credit-card">
        <h2>5263 9503 0482 7389</h2>
        <span>DigiBank</span>
      </div>
    </div>
  );
}

export function CardOptions({
  cartao,
  onClick,
  dispatch,
}: {
  cartao: CartaoProps | undefined;
  onClick: () => void;
  dispatch: Dispatch<any>;
}) {
  const [Senha, setSenha] = useState<string>();
  const [isTypeAble, setTypeAble] = useState<boolean>(false);

  function AlterarSenha() {
    api
      .patch(`Cartao/AlterarSenha/${cartao?.idCartao}`, {
        newToken: Senha,
      })
      .then((response) => {
        if (response.status === 200) {
          toast.success('Senha alterada');
        }
      })
      .catch(() => {
        toast.error('Não foi possível alterar a senha');
      });
  }

  function Excluir(idCartao: number) {
    api.delete(`Cartao/${idCartao}`).then((response) => {
      if (response.status === 200) {
        toast.success('Cartão excluído');
        dispatch({ type: 'update' });
      }
    });
  }

  const handleKeyPress = (event: React.KeyboardEvent<HTMLInputElement>) => {
    if (event.key === 'Enter') {
      AlterarSenha();
      setTypeAble(false);
    }
  };

  return cartao === undefined ? (
    <div className="options-card" style={{ justifyContent: 'center' }}>
      <ModalNovoCartao dispatch={dispatch} />
    </div>
  ) : (
    <div className="options-card">
      {isTypeAble ? (
        <input
          type="password"
          maxLength={8}
          value={Senha}
          onChange={(evt) => setSenha(evt.target.value)}
          onBlur={() => {
            AlterarSenha();
            setTypeAble(false);
          }}
          onKeyPress={handleKeyPress}
          // eslint-disable-next-line jsx-a11y/no-autofocus
          autoFocus
          className="card-option"
          placeholder="Alterar Senha"
        />
      ) : (
        <button className="card-option" onClick={() => setTypeAble(true)}>
          Alterar senha
        </button>
      )}
      {cartao?.isValid ? (
        <BloquearBtn onClick={onClick} idCartao={cartao.idCartao} />
      ) : (
        <DesbloquearBtn onClick={onClick} idCartao={cartao.idCartao} />
      )}
      <button
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
