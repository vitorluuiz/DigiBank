import React, { useEffect, useReducer, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

import reducer from '../../services/reducer';
import api from '../../services/api';
import { parseJwt } from '../../services/auth';

import { UsuarioProps } from '../../@types/Usuario';
import { CartaoProps } from '../../@types/Cartao';

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
import ModalTransferir from '../../components/ModalTransferir';

function MinhaArea() {
  const [Usuario, setUsuario] = useState<UsuarioProps>();
  const [Cartao, setCartao] = useState<CartaoProps>();
  const navigate = useNavigate();

  const updateStage = {
    count: 0,
  };

  const [updates, dispatch] = useReducer(reducer, updateStage);

  async function GetUserProps() {
    await api(`Usuarios/Infos/${parseJwt().role}`)
      .then((response) => {
        if (response.status === 200) {
          setUsuario(response.data);
        }
      })
      .catch(() => {
        navigate('/login');
      });

    await api(`Cartao/Usuario/${parseJwt().role}`).then((response) => {
      if (response.status === 200) {
        setCartao(response.data[0]);
      }
    });
  }

  useEffect(() => {
    GetUserProps();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [updates.count]);

  return (
    <div>
      <ToastContainer position="top-center" autoClose={1800} />
      <Header type="" />
      <main id="minha-area" className="container">
        <h1>Olá seja bem vindo, {Usuario?.apelido}</h1>
        <div className="suport-minha-area">
          <section className="left-section">
            <section className="user-menu-infos">
              <SaldoBar saldo={Usuario?.saldo} />
              <InvestimentosBar investido={Usuario?.investido} />
              <MetasBar meta={Usuario?.metaDestaque} />
              <PontosBar pontos={Usuario?.digiPoints} />
            </section>
            <section className="card-menu-suport">
              <Card cartao={Cartao} nomeUsuario={Usuario?.nomeCompleto} />
              <CardOptions
                dispatch={dispatch}
                onClick={() => {
                  GetUserProps();
                }}
                cartao={Cartao}
              />
            </section>
          </section>
          <section className="right-section">
            <Link to="/emprestimos" id="emprestimo" className="user-button">
              Emprestimos
            </Link>
            <ModalTransferir dispatch={dispatch} />
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
