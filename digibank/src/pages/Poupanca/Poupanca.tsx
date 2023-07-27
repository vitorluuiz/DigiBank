import { useEffect, useReducer, useState } from 'react';
import Footer from '../../components/Footer';
import Header from '../../components/Header';
import { PoupancaProps } from '../../@types/Poupanca';
import api from '../../services/api';
import { parseJwt } from '../../services/auth';
import ModalPoupanca from '../../components/Poupanca/ModalPoupanca';
import reducer from '../../services/reducer';

export default function Poupanca() {
  const [poupanca, setPoupanca] = useState<PoupancaProps>();
  const [porcentagem, setPorcentagem] = useState<number | null>(null);
  const [porcentagemDiaria, setPorcentagemDiaria] = useState<number | null>(null);
  const [porcentagemMensal, setPorcentagemMensal] = useState<number | null>(null);
  const [porcentagemAnual, setPorcentagemAnual] = useState<number | null>(null);

  const updateStage = {
    count: 0,
  };
  const [updates, dispatch] = useReducer(reducer, updateStage);

  function ListarPoupanca() {
    api
      .get(`Poupanca/${parseJwt().role}`, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('usuario-login-auth')}`,
        },
      })

      .then((resposta) => {
        if (resposta.status === 200) {
          setPoupanca(resposta.data);
        }
      })

      .catch((erro) => console.log(erro));
  }

  useEffect(() => {
    if (poupanca?.saldo !== undefined) {
      const porcentagemValue = poupanca.saldo / poupanca.totalInvestido;
      setPorcentagem(porcentagemValue);
    }

    if (poupanca?.ganhoDiario !== undefined && poupanca?.saldo !== undefined) {
      const porcentagemValue = poupanca.ganhoDiario / (poupanca.saldo - poupanca.ganhoDiario);
      setPorcentagemDiaria(porcentagemValue);
    }

    if (poupanca?.ganhoMensal !== undefined && poupanca?.saldo !== undefined) {
      const porcentagemMensalValue = poupanca.ganhoMensal / (poupanca.saldo - poupanca.ganhoMensal);
      setPorcentagemMensal(porcentagemMensalValue);
    }

    if (poupanca?.ganhoAnual !== undefined && poupanca?.saldo !== undefined) {
      const porcentagemAnualValue = poupanca.ganhoAnual / (poupanca.saldo - poupanca.ganhoAnual);
      setPorcentagemAnual(porcentagemAnualValue);
    }
  }, [poupanca]);
  useEffect(() => {
    ListarPoupanca();
  }, [updates.count]);
  return (
    <div>
      <Header type="digInvest" />
      <main id="poupanca" className="container">
        <div className="headerMain">
          <div className="boxInfo1">
            <span>Total investido na poupança</span>
            <p>{poupanca?.totalInvestido}</p>
          </div>
          <div className="boxInfo1 borderDiv">
            <span>Total Atual</span>
            <div>
              <p>{poupanca?.saldo}</p>
              <p className="porcentagem">
                {porcentagem !== null ? `${(porcentagem * 100).toFixed(3)}%` : 'N/A'}
              </p>
            </div>
          </div>
          <div className="boxInfoButtons">
            <ModalPoupanca dispatch={dispatch} type="sacar" />
            <ModalPoupanca
              dispatch={dispatch}
              type="investir"
              dataInicio={poupanca?.dataAquisicao}
            />
          </div>
        </div>
        <div className="bodyMain">
          <div className="graficoBody">
            <span>HAMBURGUER</span>
          </div>
          <div className="infosValorizacao">
            <div className="boxValorizacao">
              <span>Ultimo Dia:</span>
              <div>
                <p>{poupanca?.ganhoDiario}</p>
                <p className="porcentagem">
                  {porcentagemDiaria !== null ? `${(porcentagemDiaria * 100).toFixed(3)}%` : 'N/A'}
                </p>
              </div>
            </div>
            <div className="boxValorizacao">
              <span>Ultimo Mês:</span>
              <div>
                <p>{poupanca?.ganhoMensal}</p>
                <p className="porcentagem">
                  {porcentagemMensal !== null ? `${(porcentagemMensal * 100).toFixed(3)}%` : 'N/A'}
                </p>
              </div>
            </div>
            <div className="boxValorizacao">
              <span>No Ano:</span>
              <div>
                <p>{poupanca?.ganhoAnual}</p>
                <p className="porcentagem">
                  {porcentagemAnual !== null ? `${(porcentagemAnual * 100).toFixed(3)}%` : 'N/A'}
                </p>
              </div>
            </div>
            <div className="boxPrevisao">
              <div className="dataInicio">
                <span>data de inicio</span>
                <p>
                  {poupanca?.dataAquisicao &&
                    new Date(poupanca.dataAquisicao).toLocaleDateString('pt-BR', {
                      day: '2-digit',
                      month: '2-digit',
                      year: 'numeric',
                    })}
                </p>
              </div>
              <ModalPoupanca dispatch={dispatch} type="" />
            </div>
          </div>
        </div>
      </main>
      <Footer />
    </div>
  );
}
