import { FC } from 'react';
import { Link } from 'react-router-dom';
import Logo from '../assets/img/logoVermelha.png';

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
  return (
    <header>
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
    </header>
  );
};

export default Header;
