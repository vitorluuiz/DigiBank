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
import { InvestidosProps } from '../../@types/Investidos';
import InfoBlock from '../../components/Investimentos/InfoBlock';
import { EmblemaProps } from '../../@types/EmblemaDiginvest';
import Emblema from '../../components/Investimentos/Emblema';
import HistoryGraph from '../../components/Investimentos/HistoryGraph';
import { HistoryOptionProps } from '../../@types/HistoryOption';

export default function InvestPost() {
  const { idInvestimentoOption } = useParams();
  const [parentWidth, setParentWidth] = useState(1000);
  const [amount, setAmount] = useState<number>(1);
  const [hexColor, setHexColor] = useState('');
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  const [historyData, setHistoryData] = useState<HistoryOptionProps[]>([]);
  const [optionData, setOptionData] = useState<InvestidosProps>({
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
  const [emblemas, setEmblemas] = useState<EmblemaProps[]>([]);

  const handleChangeAmountCotas = (value: number) => {
    setAmount(value);
  };

  const handleChangeToggle = (event: React.MouseEvent<HTMLElement>, newAlignment: number) => {
    setAmount(newAlignment);
  };

  const GetInvestOption = (idOption: string) => {
    api(`InvestimentoOptions/${idOption}/Dias/30`).then((response) => {
      if (response.status === 200) {
        setOptionData(response.data.option);
        setHexColor(response.data.option.mainHexColor);
        setIndices(response.data.indices);
        setEmblemas(response.data.emblemas);

        api(`HistoryInvest/Option/${idOption}/30`).then((responseHistory) => {
          if (responseHistory.status === 200) {
            setHistoryData(responseHistory.data.historyList);
          }
        });
      }
    });
  };

  const parentRef = useRef(null);

  useEffect(() => {
    // eslint-disable-next-line react-hooks/exhaustive-deps
    setParentWidth((parentRef.current as unknown as HTMLElement)?.offsetWidth);
    console.log('Parent Width:', parentWidth);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  useEffect(() => {
    if (idInvestimentoOption !== undefined) {
      GetInvestOption(idInvestimentoOption);
    }
  }, [idInvestimentoOption]);

  return (
    <div>
      <Header type="" />
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

          <div ref={parentRef} className="support-history">
            {historyData.length !== 0 ? (
              <HistoryGraph historyData={historyData} height={400} width={parentWidth} />
            ) : null}
          </div>
        </div>
      </main>
      <Footer />
    </div>
  );
}
