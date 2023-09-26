import { useEffect, useReducer, useRef, useState } from 'react';
import Footer from '../../components/Footer';
import Header from '../../components/Header';
import { PoupancaProps } from '../../@types/Poupanca';
import api from '../../services/api';
import { parseJwt } from '../../services/auth';
import ModalPoupanca from '../../components/Poupanca/ModalPoupanca';
import reducer from '../../services/reducer';
import HistoryGraph from '../../components/Investimentos/HistoryGraph';
import { HistoryOptionProps } from '../../@types/HistoryOption';
import CustomSnackbar from '../../assets/styledComponents/snackBar';
import { useSnackBar } from '../../services/snackBarProvider';

export default function Poupanca() {
  const [poupanca, setPoupanca] = useState<PoupancaProps>();
  const [parentWidth, setParentWidth] = useState(1000);
  const [parentHeight, setParentHeight] = useState(1000);
  const [porcentagem, setPorcentagem] = useState<number>(0);
  const [porcentagemDiaria, setPorcentagemDiaria] = useState<number | null>(null);
  const [porcentagemMensal, setPorcentagemMensal] = useState<number | null>(null);
  const [porcentagemAnual, setPorcentagemAnual] = useState<number | null>(null);
  const [historyData, setHistoryData] = useState<HistoryOptionProps[]>([]);

  const { currentMessage, handleCloseSnackBar } = useSnackBar();

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
  const GetInvestOption = () => {
    api(`HistoryInvest/Investimento/Saldo/Poupanca/${parseJwt().role}/90`).then(
      (responseHistory) => {
        if (responseHistory.status === 200) {
          setHistoryData(responseHistory.data.historyList);
        }
      },
    );
  };
  const parentRef = useRef(null);

  useEffect(() => {
    const parentElement = parentRef.current;

    if (!parentElement) return;

    // Função para ser executada quando o tamanho do elemento for alterado
    const handleResize = (entries: ResizeObserverEntry[]) => {
      entries.forEach((entry) => {
        setParentWidth(entry.contentRect.width);
        setParentHeight(entry.contentRect.height);
      });
    };

    const resizeObserver = new ResizeObserver(handleResize);
    resizeObserver.observe(parentElement);

    // eslint-disable-next-line consistent-return
    return () => {
      resizeObserver.unobserve(parentElement);
    };
  }, []);

  useEffect(() => {
    GetInvestOption();
  }, []);

  useEffect(() => {
    if (poupanca?.saldo !== undefined) {
      const porcentagemValue = (poupanca.saldo - poupanca.totalInvestido) / poupanca.totalInvestido;
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
      <CustomSnackbar message={currentMessage} onClose={handleCloseSnackBar} />
      <Header type="" />
      <main id="poupanca" className="container">
        <div className="headerMain">
          <div className="boxInfos">
            <div className="boxInfo">
              <span>Em aportes</span>
              <p>
                {poupanca?.totalInvestido.toLocaleString('pt-BR', {
                  style: 'currency',
                  currency: 'BRL',
                })}
              </p>
            </div>
            <hr />
            <div className="boxInfo">
              <span>Valor total</span>
              <p>
                {poupanca?.saldo.toLocaleString('pt-BR', {
                  style: 'currency',
                  currency: 'BRL',
                })}
                <span style={{ color: porcentagem > 0 ? '#2fd72c' : '#E40A0A' }}>
                  {porcentagem !== null ? `${(porcentagem * 100).toFixed(2)}%` : 'N/A'}
                </span>
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
            <div ref={parentRef} className="boxGrafico">
              <HistoryGraph historyData={historyData} height={parentHeight} width={parentWidth} />
            </div>
          </div>
          <div className="dataBody">
            <div className="infosValorizacao">
              <div className="boxValorizacao">
                <span>Ultimo Dia:</span>
                <div>
                  <p>{poupanca?.ganhoDiario.toFixed(2)}</p>
                  <p className="porcentagem">
                    {porcentagemDiaria !== null
                      ? `${(porcentagemDiaria * 100).toPrecision(2)}%`
                      : 'N/A'}
                  </p>
                </div>
              </div>
              <div className="boxValorizacao">
                <span>Ultimo Mês:</span>
                <div>
                  <p>{poupanca?.ganhoMensal.toFixed(2)}</p>
                  <p className="porcentagem">
                    {porcentagemMensal !== null
                      ? `${(porcentagemMensal * 100).toPrecision(2)}%`
                      : 'N/A'}
                  </p>
                </div>
              </div>
              <div className="boxValorizacao">
                <span>No Ano:</span>
                <div>
                  <p>{poupanca?.ganhoAnual.toFixed(2)}</p>
                  <p className="porcentagem">
                    {porcentagemAnual !== null
                      ? `${(porcentagemAnual * 100).toPrecision(2)}%`
                      : 'N/A'}
                  </p>
                </div>
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
