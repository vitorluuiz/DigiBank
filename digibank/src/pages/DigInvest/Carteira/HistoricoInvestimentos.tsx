import React, { useEffect, useRef, useState } from 'react';
import api from '../../../services/api';
import { HistoryOptionProps } from '../../../@types/HistoryOption';
import HistoryGraph from '../../../components/Investimentos/HistoryGraph';
import Header from '../../../components/Header';
import Footer from '../../../components/Footer';

export default function HistoricoInvestimentos() {
  const [historyData, setHistoryData] = useState<HistoryOptionProps[]>([]);
  const [parentWidth, setParentWidth] = useState<number>(0);
  const [parentHeight, setParentHeight] = useState<number>(0);

  const getHistoryData = () => {
    api(`HistoryInvest/Option/3/365`).then((responseHistory) => {
      if (responseHistory.status === 200) {
        const history: HistoryOptionProps[] = responseHistory.data.historyList;
        setHistoryData(history);
      }
    });
  };

  useEffect(() => getHistoryData(), []);

  const parentRef = useRef(null);
  useEffect(() => {
    const parentElement = parentRef.current;

    if (!parentElement) return;

    // Função para ser executada quando o tamanho do elemento for alterado
    const handleResize = (entries: ResizeObserverEntry[]) => {
      entries.forEach((entry) => {
        setParentWidth(entry.contentRect.width * 2);
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

  return (
    <main id="carteiraGraphs" className="container">
      <Header type="" />
      <h1>Meus investimentos</h1>
      <section className="support-graphs">
        <article ref={parentRef} className="graph-box">
          <h2>Ações</h2>
          <div>
            {historyData.length !== 0 ? (
              <HistoryGraph historyData={historyData} height={parentHeight} width={parentWidth} />
            ) : null}
          </div>
        </article>
        <article className="graph-box">
          <h2>Fundos</h2>
          <div>
            {historyData.length !== 0 ? (
              <HistoryGraph historyData={historyData} height={parentHeight} width={parentWidth} />
            ) : null}
          </div>
        </article>
        <article className="graph-box">
          <h2>Renda fixa</h2>
          <div>
            {historyData.length !== 0 ? (
              <HistoryGraph historyData={historyData} height={parentHeight} width={parentWidth} />
            ) : null}
          </div>
        </article>
        <article className="graph-box">
          <h2>Poupança</h2>
          <div>
            {historyData.length !== 0 ? (
              <HistoryGraph historyData={historyData} height={parentHeight} width={parentWidth} />
            ) : null}
          </div>
        </article>
      </section>
      <Footer />
    </main>
  );
}
