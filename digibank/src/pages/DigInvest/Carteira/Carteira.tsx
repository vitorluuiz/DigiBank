import React, { useEffect, useRef, useState } from 'react';
import { Link } from 'react-router-dom';
import Header from '../../../components/Header';
import Footer from '../../../components/Footer';
import InfoBlock from '../../../components/Investimentos/InfoBlock';
import SetaIcon from '../../../assets/img/SetaVerMais.svg';
import FakeGraficoIcon from '../../../assets/img/fakeGrafico.svg';
import HistoryGraph from '../../../components/Investimentos/HistoryGraph';
import { HistoryOptionProps } from '../../../@types/HistoryOption';
import api from '../../../services/api';
import { parseJwt } from '../../../services/auth';
import { ExtratoInvestimentoProps } from '../../../@types/Investidos';

export function GuiaInvestimento({ name, valor }: { name: string; valor: number }) {
  return (
    <Link className="invest-info" to={`${name}`}>
      <img alt="grafico" src={FakeGraficoIcon} />
      <h3>
        {name}
        <span>{valor.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}</span>
      </h3>
    </Link>
  );
}

export default function Carteira() {
  const [historyData, setHistoryData] = useState<HistoryOptionProps[]>([]);
  const [parentWidth, setParentWidth] = useState(0);
  const [variation, setVariation] = useState<number>(0);
  const [extrato, setExtrato] = useState<ExtratoInvestimentoProps>({
    acoes: 0,
    criptomoedas: 0,
    fundos: 0,
    horario: new Date(),
    idUsuario: parseJwt().role,
    poupanca: 0,
    rendaFixa: 0,
    total: 0,
  });

  const GetHistoryData = () => {
    api(`HistoryInvest/Investimento/Saldo/${parseJwt().role}/12`).then((responseHistory) => {
      if (responseHistory.status === 200) {
        const data: HistoryOptionProps[] = responseHistory.data.historyList;
        setHistoryData(data);
        setVariation(
          (data[data.length - 1].valor - data[data.length - 2].valor) / data[data.length - 1].valor,
        );
      }
    });
  };

  const GetInvestido = () => {
    api(`Investimento/Investido/${parseJwt().role}`).then((response) => {
      if (response.status === 200) {
        setExtrato(response.data);
      }
    });
  };

  useEffect(() => {
    GetHistoryData();
    GetInvestido();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const parentRef = useRef(null);

  useEffect(() => {
    const parentElement = parentRef.current;

    if (!parentElement) return;

    // Função para ser executada quando o tamanho do elemento for alterado
    const handleResize = (entries: ResizeObserverEntry[]) => {
      entries.forEach((entry) => {
        setParentWidth(entry.contentRect.width);
      });
    };

    const resizeObserver = new ResizeObserver(handleResize);
    resizeObserver.observe(parentElement);

    // eslint-disable-next-line consistent-return
    return () => {
      resizeObserver.unobserve(parentElement);
    };
  }, []);

  return (
    <div>
      <Header type="digInvest" />
      <main id="carteira" className="container">
        <h1>Minha carteira</h1>
        <div className="display-carteira">
          <section className="user-invests-static full">
            <InfoBlock
              name="Total investido"
              valor={extrato.total}
              isCurrency
              variation={variation}
              size="full"
            />
            <section className="user-invests-infos">
              <Link className="link" to="meus">
                Meus investimentos
                <img alt="seta" src={SetaIcon} />
              </Link>
              <GuiaInvestimento name="Ações" valor={extrato.acoes} />
              <GuiaInvestimento name="Fundos" valor={extrato.fundos} />
              <GuiaInvestimento name="Criptomoedas" valor={extrato.criptomoedas} />
              <GuiaInvestimento name="Poupança" valor={extrato.poupanca} />
              <GuiaInvestimento name="Renda fixa" valor={extrato.rendaFixa} />
            </section>
          </section>
          <section className="user-invest-interactive">
            <div className="invest-nav">
              <Link className="link" to="/diginvest/poupanca">
                Poupança
              </Link>
              <Link className="link" to="/diginvest">
                Diginvest
              </Link>
              <Link className="link" to="/diginvest/investimentos/favoritos">
                Favoritos
              </Link>
            </div>
            <div className="graph">
              <h3>Dinheiro investido</h3>
              <div ref={parentRef} className="graph-support">
                {historyData.length !== 0 ? (
                  <HistoryGraph historyData={historyData} height={382} width={parentWidth} />
                ) : null}
              </div>
            </div>
          </section>
        </div>
      </main>
      <Footer />
    </div>
  );
}
