import React, { useState } from 'react';
import { toast } from 'react-toastify';

import { CartaoProps } from '../@types/Cartao';

import Logo from '../assets/img/logoBranca.png';
import api from '../services/api';
import { BloquearBtn, DesbloquearBtn } from './CardOption';

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

  return (
    <div className="credit-card">
      <div className="logo-credit-card">
        <img alt="logo do cartão" src={Logo} />
      </div>
      <div className="info-credit-card">
        <h2>{FormatNumero(cartao?.numero)}</h2>
        <span>{nomeUsuario}</span>
      </div>
    </div>
  );
}

export function CardOptions({
  cartao,
  onClick,
}: {
  cartao: CartaoProps | undefined;
  onClick: () => void;
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

  const handleKeyPress = (event: React.KeyboardEvent<HTMLInputElement>) => {
    if (event.key === 'Enter') {
      AlterarSenha();
      setTypeAble(false);
    }
  };

  return (
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
        <BloquearBtn onClick={onClick} idCartao={cartao?.idCartao} />
      ) : (
        <DesbloquearBtn onClick={onClick} idCartao={cartao?.idCartao} />
      )}
      <button className="card-option">Solicitar extrato</button>
    </div>
  );
}
