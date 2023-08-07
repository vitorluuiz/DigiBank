import { FC } from 'react';
import { Link } from 'react-router-dom';
import Logo from '../assets/img/logoVermelha.png';
import { parseJwt } from '../services/auth';

interface HeaderProps {
  type: string;
}

function logOut() {
  localStorage.setItem('save-login?', 'false');
  localStorage.removeItem('usuario-login-auth');
}

const Header: FC<HeaderProps> = ({ type }) => {
  if (type === 'landing') {
    return (
      <header>
        <div className="suport-header container">
          <Link to="/home" className="logo-img-header">
            <img alt="Logo da Digibank" src={Logo} />
          </Link>
          <nav className="routes-nav-header-landing">
            <Link to="/login" className="btnHeaderLanding">
              Login
            </Link>
          </nav>
        </div>
      </header>
    );
  }
  if (type === 'simples') {
    return (
      <header>
        <div className="suport-header container">
          <Link to="/" className="logo-img-header centralizado">
            <img alt="Logo da Digibank" src={Logo} />
          </Link>
        </div>
      </header>
    );
  }
  if (type === 'digiStore') {
    return (
      <header>
        <div className="suport-header container">
          <Link to="/home" className="logo-img-header">
            <img alt="Logo da Digibank" src={Logo} />
          </Link>
          <nav className="routes-nav-header">
            <Link to="/digistore">MarketPlace</Link>
            <Link to="/digistore/inventario">Inventario</Link>
            <Link to="/wishlist">Lista de desejos</Link>
            <Link to="/">
              <button onClick={logOut}>Sair</button>
            </Link>
          </nav>
        </div>
      </header>
    );
  }
  if (type === 'digInvest') {
    return (
      <header>
        <div className="suport-header container">
          <Link to="/home" className="logo-img-header">
            <img alt="Logo da Digibank" src={Logo} />
          </Link>
          <nav className="routes-nav-header">
            <Link to="/minha-area">MyPlace</Link>
            <Link to="/diginvest">InvestPlace</Link>
            <Link to="/diginvest/carteira">Carteira</Link>
            <Link to="/diginvest/favoritos">Favoritos</Link>
            <Link to="/">
              <button onClick={logOut}>Sair</button>
            </Link>
          </nav>
        </div>
      </header>
    );
  }
  return (
    <div>
      {parseJwt().role === 'undefined' ? (
        <header>
          <div className="suport-header container">
            <Link to="/" className="logo-img-header">
              <img alt="Logo da Digibank" src={Logo} />
            </Link>
            <nav className="routes-nav-header-auth">
              <Link to="/login">
                <button>Login</button>
              </Link>
              <Link to="/cadastro">
                <button>Cadastre-se</button>
              </Link>
            </nav>
          </div>
        </header>
      ) : (
        <header>
          <div className="suport-header container">
            <Link to="/home" className="logo-img-header">
              <img alt="Logo da Digibank" src={Logo} />
            </Link>
            <nav className="routes-nav-header">
              <Link to="/minha-area">MyPlace</Link>
              <Link to="/emprestimos">Empréstimos</Link>
              <Link to="/metas">Metas</Link>
              <Link to="/">
                <button onClick={logOut}>Sair</button>
              </Link>
            </nav>
          </div>
        </header>
      )}
    </div>
  );
};

export default Header;
