import React, { useEffect, useReducer, useState } from 'react';
import { Link } from 'react-router-dom';

import reducer from '../../services/reducer';
import api from '../../services/api';
import { parseJwt } from '../../services/auth';

import { UsuarioProps } from '../../@types/Usuario';
import { CartaoProps } from '../../@types/Cartao';

import Header from '../../components/Header';
import Footer from '../../components/Footer';
import SideBar from '../../components/SideBar';
import { Card, CardOptions } from '../../components/Card';
import { MetasBar, MyPlaceBar } from '../../components/MinhaArea/UserInfos';
import ModalTransferir from '../../components/MinhaArea/ModalTransferir';
import CustomSnackbar from '../../assets/styledComponents/snackBar';
import { useSnackBar } from '../../services/snackBarProvider';
import { HistoryOptionProps } from '../../@types/HistoryOption';

function MinhaArea() {
  const [Usuario, setUsuario] = useState<UsuarioProps>({
    apelido: '',
    cpf: '',
    digiPoints: 0,
    email: '',
    idUsuario: parseJwt().role,
    investido: 0,
    nomeCompleto: '',
    rendaFixa: 0,
    saldo: 0,
    telefone: '',
    metaDestaque: { arrecadado: 0, idMeta: 0, idUsuario: 0, titulo: '', valorMeta: 0 },
  });
  const [Cartao, setCartao] = useState<CartaoProps>();
  const [HistoryInvestido, setHistoryInvestido] = useState<HistoryOptionProps[]>([]);
  const [SaldoAnterior, setSaldoAnterior] = useState<number>(0);

  const updateStage = {
    count: 0,
  };

  const [updates, dispatch] = useReducer(reducer, updateStage);
  const { currentMessage, handleCloseSnackBar } = useSnackBar();

  function calcularBalanco(saldoAtual: number) {
    const primeiroDiaDoMesAtual = new Date();
    primeiroDiaDoMesAtual.setDate(0);

    api
      .post('Transacoes/FluxoTemporario', {
        idUsuario: parseJwt().role,
        startDate: primeiroDiaDoMesAtual,
      })
      .then((resposta) => {
        if (resposta.status === 200) {
          setSaldoAnterior(saldoAtual - resposta.data.saldo);
        }
      })

      .catch((erro) => console.log(erro));
  }

  async function getUserProps() {
    await api(`Usuarios/Infos/${parseJwt().role}`).then((response) => {
      if (response.status === 200) {
        setUsuario(response.data);
        calcularBalanco(response.data.saldo);
      }
    });

    await api(`Cartao/Usuario/${parseJwt().role}`).then((response) => {
      if (response.status === 200) {
        setCartao(response.data[0]);
      }
    });

    await api(`HistoryInvest/Investimento/Saldo/${parseJwt().role}/1`).then((response) => {
      if (response.status === 200) {
        setHistoryInvestido(response.data.historyList);
      }
    });
  }

  useEffect(() => {
    getUserProps();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [updates.count]);

  return (
    <div>
      <CustomSnackbar message={currentMessage} onClose={handleCloseSnackBar} />
      <Header type="" />
      <main id="minha-area" className="container">
        <h1>Olá seja bem vindo, {Usuario?.apelido}</h1>
        <div className="suport-minha-area">
          <section className="left-section">
            <section className="user-menu-infos">
              <MyPlaceBar
                name="Saldo atual"
                valorAnterior={SaldoAnterior}
                valorAtual={Usuario.saldo}
                monthsToPast={1}
                title="Seu saldo atual disponível"
              />
              <MyPlaceBar
                name="Total investido"
                valorAnterior={HistoryInvestido.length > 0 ? HistoryInvestido[0].valor : 0}
                monthsToPast={1}
                valorAtual={HistoryInvestido.length > 0 ? HistoryInvestido[1].valor : 0}
                title="Seu dinheiro total investido"
              />
              <MetasBar meta={Usuario?.metaDestaque} />
              <MyPlaceBar
                name="Saldo digipoints"
                valorAnterior={0}
                monthsToPast={1}
                valorAtual={Usuario.digiPoints}
                title="Total de digipoints"
              />
            </section>
            <section className="card-menu-suport">
              <Card cartao={Cartao} nomeUsuario={Usuario?.nomeCompleto} />
              <CardOptions dispatch={dispatch} cartao={Cartao} />
            </section>
          </section>
          <section className="right-section">
            <Link
              title="Ver lista de empréstimos pré-aprovados"
              to="/emprestimos"
              id="emprestimo"
              className="user-button"
            >
              Emprestimo
            </Link>
            <ModalTransferir onClose={() => getUserProps()} />
            <Link title="Visualizar seu estrato" to="/extrato" id="extrato" className="user-button">
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
