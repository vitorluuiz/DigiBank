import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';

import Header from '../../components/Header';
import Footer from '../../components/Footer';
import { Card } from '../../components/Card';

import MoneyIcon from '../../assets/img/money_icon.svg';
import Payment from '../../assets/img/payment_icon.svg';
import Store from '../../assets/img/market_icon.svg';
import Invest from '../../assets/img/investiment_icon.svg';
import { parseJwt } from '../../services/auth';
import api from '../../services/api';
import { CartaoProps } from '../../@types/Cartao';
import { UsuarioProps } from '../../@types/Usuario';

function Home() {
  const [Cartao, setCartao] = useState<CartaoProps>();
  const [Usuario, setUsuario] = useState<UsuarioProps>();

  const GetCardProps = () => {
    api(`Usuarios/Infos/${parseJwt().role}`).then((response) => {
      if (response.status === 200) {
        setUsuario(response.data);
      }
    });
    api(`Cartao/Usuario/${parseJwt().role}`).then((response) => {
      if (response.status === 200) {
        setCartao(response.data[0]);
      }
    });
  };

  useEffect(() => GetCardProps(), []);

  return (
    <div>
      <Header type="" />
      <main id="home" className="container">
        <section className="left-menu-home">
          <div>
            <h1>Invista da melhor maneira com digibank</h1>
            <span>Acesse a Diginvest para começar hoje mesmo</span>
          </div>
          <nav>
            <Link to="/minha-area">
              <img alt="Ícone de dinheiro" src={MoneyIcon} />
              Saldo
            </Link>
            <Link to="/extrato">
              <img alt="Ícone de dinheiro" src={Payment} />
              Extrato
            </Link>
            <Link to="/digistore">
              <img alt="Ícone de dinheiro" src={Store} />
              Digistore
            </Link>
            <Link to="/diginvest">
              <img alt="Ícone de dinheiro" src={Invest} />
              DigInvest
            </Link>
          </nav>
        </section>
        <section className="right-menu-home">
          <Card cartao={Cartao} nomeUsuario={Usuario?.nomeCompleto} />
        </section>
      </main>
      <Footer />
    </div>
  );
}

export default Home;
