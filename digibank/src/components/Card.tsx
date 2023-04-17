import React from 'react';

import Logo from '../assets/img/logoBranca.png';

export function Card() {
  return (
    <div className="credit-card">
      <div className="logo-credit-card">
        <img alt="logo do cartão" src={Logo} />
      </div>
      <div className="info-credit-card">
        <h2>5274 9375 6284 8372</h2>
        <span>Carlos manoel de oliveira</span>
      </div>
    </div>
  );
}

export function CardOptions() {
  return (
    <div className="options-card">
      <button>Alterar senha</button>
      <button>Bloquear cartão</button>
      <button>Solicitar extrato</button>
    </div>
  );
}
