import React, { FC } from 'react';
import { Link } from 'react-router-dom';

import Logo from '../assets/img/logoVermelha.png';

interface HeaderProps {
  type: string;
}

function logOut() {
  localStorage.removeItem('usuario-login-auth');
}

const Header: FC<HeaderProps> = ({ type }) => (
  <header>
    {type !== 'simples' ? (
      <div className="suport-header container">
        <Link to="/home" className="logo-img-header">
          <img alt="Logo da Digibank" src={Logo} />
        </Link>
        <nav className="routes-nav-header">
          <Link to="/minha-area">Minha área</Link>
          <Link to="/emprestimos">Empréstimos</Link>
          <Link to="/metas">Metas</Link>
          <Link to="/">
            <button onClick={logOut}>Sair</button>
          </Link>
        </nav>
      </div>
    ) : (
      <div className="suport-header container">
        <Link to="/" className="logo-img-header centralizado">
          <img alt="Logo da Digibank" src={Logo} />
        </Link>
      </div>
    )}
  </header>
);

export default Header;
