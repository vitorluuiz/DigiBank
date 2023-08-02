import React, { useEffect, useRef, useState } from 'react';
import { ThemeProvider, ToggleButton, ToggleButtonGroup } from '@mui/material';
import { useParams } from 'react-router-dom';

import Header from '../../components/Header';
import Footer from '../../components/Footer';

// import Banner from '../../assets/img/store-post.png';
import FavoritoIcon from '../../assets/img/bookmark-add_blackicon.svg';
// import Logo from '../../assets/img/spotify.png';
import { ThemeToggleButton } from '../../assets/styledComponents/toggleButton';
import { CssTextField } from '../../assets/styledComponents/input';
import LinearRating from '../../components/LinearIndice';
import api from '../../services/api';
import { IndicesOptionProps } from '../../@types/Digindices';
import InfoBlock from '../../components/Investimentos/InfoBlock';
import { EmblemaProps } from '../../@types/EmblemaDiginvest';
import Emblema from '../../components/Investimentos/Emblema';
import HistoryGraph from '../../components/Investimentos/HistoryGraph';
import { HistoryOptionProps } from '../../@types/HistoryOption';
import { FullOptionProps, StatsOptionProps } from '../../@types/InvestimentoOptions';

export default function InvestPost() {
  const { idInvestimentoOption } = useParams();
  const [parentWidth, setParentWidth] = useState(0);
  const [amount, setAmount] = useState<number>(1);
  const [hexColor, setHexColor] = useState('');
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  const [historyData, setHistoryData] = useState<HistoryOptionProps[]>([]);
  const [optionData, setOptionData] = useState<FullOptionProps>({
    abertura: new Date(),
    fundacao: new Date(),
    idOption: 1,
    idTipo: 1,
    idArea: 1,
    nome: '',
    sigla: '',
    tipo: '',
    area: '',
    descricao: '',
    dividendos: 0,
    colaboradores: '0',
    fundador: 'Sem informações',
    logo: '',
    mainColorHex: '',
    mainImg: '',
    marketCap: 0,
    sede: 'Sem informações',
    valor: 0,
    variacaoPercentual: 0,
  });
  const [indices, setIndices] = useState<IndicesOptionProps>({
    marketCap: 0,
    dividendos: 0,
    valorizacao: 0,
    confiabilidade: 0,
  });
  const [stats, setStats] = useState<StatsOptionProps>({
    coeficienteVariativo: 0,
    dividendos: 0,
    marketCap: 0,
    max: 0,
    media: 0,
    min: 0,
    minMax: 0,
    minMaxPercentual: 0,
    valor: 0,
    variacaoPeriodo: 0,
    variacaoPeriodoPercentual: 0,
  });
  const [inicio, setInicio] = useState<number>(0);
  const [fim, setFim] = useState<number>(0);
  const [emblemas, setEmblemas] = useState<EmblemaProps[]>([]);

  const handleChangeAmountCotas = (value: number) => {
    setAmount(value);
  };

  const handleChangeToggle = (event: React.MouseEvent<HTMLElement>, newAlignment: number) => {
    setAmount(newAlignment);
  };

  const GetInvestOption = (idOption: string) => {
    api(`InvestimentoOptions/${idOption}/Dias/12410001`).then((response) => {
      if (response.status === 200) {
        setOptionData(response.data.option);
        setHexColor(response.data.option.mainHexColor);
        setIndices(response.data.indices);
        setEmblemas(response.data.emblemas);
        setStats(response.data.stats);

        api(`HistoryInvest/Option/${idOption}/90432434`).then((responseHistory) => {
          if (responseHistory.status === 200) {
            const history: HistoryOptionProps[] = responseHistory.data.historyList;
            setHistoryData(history);
            setInicio(history[0].valor);
            setFim(history[history.length - 1].valor);
          }
        });
      }
    });
  };

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

  useEffect(() => {
    if (idInvestimentoOption !== undefined) {
      GetInvestOption(idInvestimentoOption);
    }
  }, [idInvestimentoOption]);

  return (
    <div>
      <Header type="digInvest" />
      <main id="diginvest-post">
        <div
          className="diginvest-banner"
          style={{ backgroundColor: `#${optionData.mainColorHex}` }}
        >
          <img alt="Banner teste" src={optionData.mainImg} />
        </div>
        <div className="support-diginvest-post container">
          <div className="main-diginvest-stats">
            <div className="invest-title-box">
              <h1>
                {optionData?.nome} <span>{optionData?.sigla}</span>
                <img alt="Icone favoritar" src={FavoritoIcon} />
              </h1>
              <img alt="logo do investimento" src={optionData.logo} />
            </div>
            <div className="invest-desc-box">
              <div className="desc-emblemas">
                <p>{optionData?.descricao}</p>
                <div className="emblemas-box">
                  {emblemas.map((emblema) =>
                    emblema !== null ? (
                      <Emblema
                        key={emblema.idEmblema}
                        name={emblema.emblema}
                        valor={emblema.valor}
                      />
                    ) : null,
                  )}
                </div>
              </div>
              <div className="indices-diginvest">
                <LinearRating name="Indice valor de mercado" value={indices.marketCap} />
                <LinearRating name="Indice dividendos" value={indices.dividendos} />
                <LinearRating name="Indice valorização" value={indices.valorizacao} />
                <LinearRating name="Indice confiabilidade" value={indices.confiabilidade} />
              </div>
            </div>
            <div className="invest-stats">
              <h2>
                {`${
                  amount === null ? optionData.valor : (optionData.valor * amount).toFixed(2)
                }BRL`}
                <span>{optionData.variacaoPercentual}% hoje</span>
              </h2>
              <form className="invest-buy-box">
                <ThemeProvider theme={ThemeToggleButton}>
                  <ToggleButtonGroup
                    color="primary"
                    value={amount}
                    exclusive
                    onChange={handleChangeToggle}
                    aria-label="Platform"
                  >
                    <ToggleButton id="amount-option" value={1}>
                      1X
                    </ToggleButton>
                    <ToggleButton id="amount-option" value={5}>
                      5X
                    </ToggleButton>
                    <ToggleButton id="amount-option" value={10}>
                      10X
                    </ToggleButton>
                  </ToggleButtonGroup>
                </ThemeProvider>
                <CssTextField
                  autoComplete="off"
                  label="Personalizado"
                  variant="outlined"
                  value={amount}
                  type="number"
                  onChange={(evt) => {
                    handleChangeAmountCotas(
                      evt.target.value === '' ? 0 : parseFloat(evt.target.value),
                    );
                  }}
                />
                <button id="buy-btn" style={{ color: hexColor, backgroundColor: `${hexColor}50` }}>
                  Investir
                </button>
              </form>
            </div>
          </div>
          <h2>Sobre</h2>
          <section className="support-infos-option">
            <InfoBlock name="Valor de mercado" valor={optionData.marketCap} isCurrency />
            <InfoBlock name="Colaboradores" valor={optionData.colaboradores} />
            <InfoBlock
              name="Fundação"
              valor={new Date(optionData.fundacao).toLocaleDateString('pt-BR', {
                month: '2-digit',
                year: 'numeric',
              })}
            />
            <InfoBlock
              name="Abertura"
              valor={new Date(optionData.abertura).toLocaleDateString('pt-BR', {
                month: '2-digit',
                year: 'numeric',
              })}
            />
            <div>
              <InfoBlock name="Sede" valor={optionData.sede} size="big" />
              <InfoBlock name="Fundador" valor={optionData.fundador} size="big" />
            </div>
          </section>
          <h2>Detalhes financeiros</h2>
          <section ref={parentRef} className="support-history">
            {historyData.length !== 0 ? (
              <HistoryGraph historyData={historyData} height={400} width={parentWidth} />
            ) : null}
          </section>
          <section className="support-history-infos">
            <InfoBlock name="Média" valor={stats.media} isCurrency />
            <InfoBlock name="Início" valor={inicio} isCurrency />
            <InfoBlock name="Atual" valor={fim} isCurrency />
            <InfoBlock name="Variação" valor={stats.variacaoPeriodo} isCurrency />
            <InfoBlock name="Variação %" valor={`${stats.variacaoPeriodoPercentual}%`} />
          </section>
          <section className="support-history-infos">
            <InfoBlock name="Min" valor={stats.min} isCurrency />
            <InfoBlock name="Max" valor={stats.max} isCurrency />
            <InfoBlock name="Amplitude" valor={stats.minMax} isCurrency />
            <InfoBlock name="Amplitude %" valor={`${stats.minMaxPercentual}%`} />
          </section>
        </div>
      </main>
      <Footer />
    </div>
  );
}
