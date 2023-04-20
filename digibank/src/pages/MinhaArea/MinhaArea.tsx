import React, { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';

import api from '../../services/api';
import { parseJwt } from '../../services/auth';

import { UsuarioProps } from '../../@types/Usuario';

import Header from '../../components/Header';
import Footer from '../../components/Footer';
import SideBar from '../../components/SideBar';
import { Card, CardOptions } from '../../components/Card';
import {
  InvestimentosBar,
  MetasBar,
  PontosBar,
  SaldoBar,
} from '../../components/MinhaArea/UserInfos';

function MinhaArea() {
  const [Usuario, setUsuario] = useState<UsuarioProps>();
  const navigate = useNavigate();

  function GetUserProps() {
    api(`Usuarios/${parseJwt().role}`)
      .then((response) => {
        if (response.status === 200) {
          setUsuario(response.data);
        }
      })
      .catch(() => {
        navigate('/login');
      });
  }

  useEffect(() => {
    GetUserProps();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <div>
      <Header type="" />
      <main id="minha-area" className="container">
        <h1>Olá seja bem vindo, {Usuario?.apelido}</h1>
        <div className="suport-minha-area">
          <section className="left-section">
            <section className="user-menu-infos">
              <SaldoBar saldo={Usuario?.saldo} />
              <InvestimentosBar />
              <MetasBar />
              <PontosBar pontos={Usuario?.digiPoints} />
            </section>
            <section className="card-menu-suport">
              <Card />
              <CardOptions />
            </section>
          </section>
          <section className="right-section">
            <Link to="/emprestimos" id="emprestimo" className="user-button">
              Empréstimo
            </Link>
            <button id="transferencia" className="user-button">
              Transferir
            </button>
            <Link to="/extrato" id="extrato" className="user-button">
              Visualizar Extrato
            </Link>
          </section>
        </div>
        <SideBar />
      </main>
      <Footer />
    </div>
  );
}

export default MinhaArea;
