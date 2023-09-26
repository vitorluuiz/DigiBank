import { FC } from 'react';
import { Link } from 'react-router-dom';
import { ConfigProvider, Menu, MenuProps } from 'antd';
import Logo from '../assets/img/logoVermelha.png';
import { parseJwt } from '../services/auth';

interface HeaderProps {
  type: string;
}

function logOut() {
  localStorage.setItem('save-login?', 'false');
  localStorage.removeItem('usuario-login-auth');
}

const items: MenuProps['items'] = [
  {
    label: 'MyPlace',
    key: 'MyPlace',
    children: [
      {
        type: 'group',
        label: '',
        children: [
          {
            label: (
              <Link className="links-menu-header" to="/home">
                Home
              </Link>
            ),
            key: 'home',
          },
          {
            label: (
              <Link className="links-menu-header" to="/minha-area">
                Saldo
              </Link>
            ),
            key: 'minha-area',
          },
          {
            label: (
              <Link className="links-menu-header" to="/extrato">
                Extrato
              </Link>
            ),
            key: 'extrato',
          },
          {
            label: (
              <Link className="links-menu-header" to="/emprestimos">
                Empréstimos
              </Link>
            ),
            key: 'emprestimos',
          },
          {
            label: (
              <Link className="links-menu-header" to="/metas">
                Metas
              </Link>
            ),
            key: 'metas',
          },
        ],
      },
    ],
  },
  {
    label: 'MarketPlace',
    key: 'MarketPlace',
    children: [
      {
        type: 'group',
        label: '',
        children: [
          {
            label: (
              <Link className="links-menu-header" to="/digistore">
                DigiStore
              </Link>
            ),
            key: 'digistore',
          },
          {
            label: (
              <Link className="links-menu-header" to="/digistore/inventario">
                Inventario
              </Link>
            ),
            key: 'inventario',
          },
          {
            label: (
              <Link className="links-menu-header" to="/digistore/publicados">
                Minhas Publicações
              </Link>
            ),
            key: 'publicacoes',
          },
          {
            label: (
              <Link className="links-menu-header" to="/digistore/wishlist">
                Lista de Favoritos
              </Link>
            ),
            key: 'wishlist',
          },
          {
            label: (
              <Link className="links-menu-header" to="/cadastro-post">
                Cadastrar Um Produto
              </Link>
            ),
            key: 'cadastroPost',
          },
        ],
      },
    ],
  },
  {
    label: 'InvestPlace',
    key: 'InvestPlace',
    children: [
      {
        type: 'group',
        label: '',
        children: [
          {
            label: (
              <Link className="links-menu-header" to="/diginvest">
                DigInvest
              </Link>
            ),
            key: 'DigInvest',
          },
          {
            label: (
              <Link className="links-menu-header" to="/diginvest/carteira">
                Carteira
              </Link>
            ),
            key: 'carteira',
          },
          {
            label: (
              <Link className="links-menu-header" to="/diginvest/poupanca">
                Poupança
              </Link>
            ),
            key: 'poupanca',
          },
          {
            label: (
              <Link className="links-menu-header" to="/diginvest/investidos">
                Meus Investimentos
              </Link>
            ),
            key: 'investidos',
          },
          {
            label: (
              <Link className="links-menu-header" to="/diginvest/favoritos">
                Investimentos Favoritos
              </Link>
            ),
            key: 'favoritos',
          },
        ],
      },
    ],
  },
];

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
    <div>
      <header>
        {parseJwt().role === 'undefined' ? (
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
        ) : (
          <div className="suport-header container">
            <Link to="/home" className="logo-img-header">
              <img alt="Logo da Digibank" src={Logo} />
            </Link>
            <nav className="routes-nav-header-auth">
              <ConfigProvider
                theme={{
                  components: {
                    Menu: {
                      fontSize: 16,
                    },
                  },
                }}
              >
                <Menu
                  mode="horizontal"
                  items={items}
                  style={{
                    width: '100%',
                    justifyContent: 'center',
                    border: 'none',
                  }}
                />
              </ConfigProvider>
              <Link to="/">
                <button onClick={logOut}>Sair</button>
              </Link>
            </nav>
          </div>
        )}
      </header>
    </div>
  );
};

export default Header;
