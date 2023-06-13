import React from 'react';
import { Link } from 'react-router-dom';

import { Autocomplete } from '@mui/material';

import Header from '../../components/Header';
import Footer from '../../components/Footer';
// import { PostBlockSlim } from '../../components/MarketPlace/PostBlock';

import UserIcon from '../../assets/img/user_icon.svg';
import ShoppingCartIcon from '../../assets/img/shopping-cart_icon.svg';
import InventoryIcon from '../../assets/img/inventory_icon.svg';
// import StorePostIcon from '../../assets/img/store-post.png';
// import Teste from '../../assets/img/teste.png';
// import Teste2 from '../../assets/img/netflix.png';
// import Teste3 from '../../assets/img/psn.png';
// import Teste4 from '../../assets/img/spotify.png';
import { CssTextField } from '../../assets/styledComponents/input';
import Carousel from '../../components/MarketPlace/Carousel';

export default function MarketPlace() {
  return (
    <div>
      <Header type="" />
      <main id="digistore" className="container">
        <section className="banner-suport">
          <div className="banner-store">
            <h1>Efetue transferências e movimente seu saldo</h1>
            <span>Para adquirir pontos e utilziar nossa Loja repleta de descontos</span>
          </div>
          <div className="store-header">
            <nav className="store-navigate">
              <Link to="/User">
                <img alt="Usuário" src={UserIcon} />
              </Link>
              <Link to="/Carrinho">
                <img alt="Carrinho de compras" src={ShoppingCartIcon} />
              </Link>
              <Link to="inventario">
                <img alt="Inventário" src={InventoryIcon} />
              </Link>
            </nav>
            <div className="store-search-suport">
              <Link to="/cadastro-post">Cadastrar</Link>
              <Autocomplete
                fullWidth
                disablePortal
                options={['Opa', 'Opa2', 'Nada a ver']}
                renderInput={(params) => (
                  <CssTextField
                    // eslint-disable-next-line react/jsx-props-no-spreading
                    {...params}
                    fullWidth
                    variant="outlined"
                    label="Categorias"
                    type="text"
                    style={{ backgroundColor: 'white' }}
                  />
                )}
              />
            </div>
          </div>
        </section>
        <section className="suport-list">
          <h2>Mais vendidos</h2>
          <Carousel type="normal" />
          {/* <div className="suport-carousel">
            <PostBlock link="/2" img={Teste2} />
            <PostBlock link="/2" img={Teste3} />
            <PostBlock link="/2" img={Teste} />
          </div> */}
        </section>
        <section className="suport-list">
          <h2>Melhores avaliados</h2>
          {/* <div className="suport-carousel">
            <PostBlockSlim link="/2" img={Teste2} />
            <PostBlockSlim link="/2" img={Teste3} />
            <PostBlockSlim link="/2" img={Teste} />
            <PostBlockSlim link="/2" img={Teste4} />
            <PostBlockSlim link="/2" img={StorePostIcon} />
          </div> */}
          <Carousel type="slim" />
        </section>
        <button id="ver_mais-btn">Listar mais itens</button>
      </main>
      <Footer />
    </div>
  );
}
