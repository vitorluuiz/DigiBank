import React, { useEffect, useState } from 'react';
import {
  Checkbox,
  FormControl,
  InputLabel,
  ListItemText,
  MenuItem,
  OutlinedInput,
  Select,
  Slider,
  ThemeProvider,
} from '@mui/material';
import { MinimalOptionProps } from '../../@types/InvestimentoOptions';
import { CustomSilder } from '../../assets/styledComponents/range';
import { useFilterBar } from '../../services/filtersProvider';

interface AsideProps {
  type: string;
  componenteExibido: number | null;
  exibirComponente: (componente: number) => void;
  listaInvestimento: MinimalOptionProps[];
}

const AsideInvest: React.FC<AsideProps> = ({
  componenteExibido,
  exibirComponente,
  type,
  listaInvestimento,
}) => {
  const [titulo, setTitulo] = useState('');
  const {
    areaInvestimentos,
    marketCapMin,
    marketCapMax,
    percentualDividendosMin,
    percentualDividendosMax,
    valorDeAcaoMin,
    valorDeAcaoMax,
    handleDividendosChange,
    handleMarketCapChange,
    handleOptionArea,
    handleValorChange,
  } = useFilterBar();

  useEffect(() => {
    if (type === '') {
      setTitulo('Investimentos Sob Demanda');
    } else if (type === 'investidos') {
      setTitulo('Meus Investimentos');
    } else if (type === 'favoritos') {
      setTitulo('Investimentos Favoritos');
    }
  }, [type]);

  return (
    <div className="containerLeftInvest">
      <div className="boxRedirects">
        <h1>{titulo}</h1>
        <button
          onClick={() => exibirComponente(3)}
          className={componenteExibido === 3 ? 'active' : ''}
        >
          Ações
        </button>
        <button
          onClick={() => exibirComponente(4)}
          className={componenteExibido === 4 ? 'active' : ''}
        >
          Fundos
        </button>
        <button
          onClick={() => exibirComponente(2)}
          className={componenteExibido === 2 ? 'active' : ''}
        >
          Renda Fixa
        </button>
        <button
          onClick={() => exibirComponente(5)}
          className={componenteExibido === 5 ? 'active' : ''}
        >
          Criptomoeda
        </button>
      </div>
      <div className="containerFiltragem">
        {/* <h2>Filtrar por:</h2> */}
        <div className="boxFiltro">
          <h3>Area de Investimento:</h3>
          <FormControl sx={{ m: 1, width: 300 }}>
            <InputLabel id="demo-multiple-checkbox-label">Area</InputLabel>
            <Select
              labelId="demo-multiple-checkbox-label"
              id="demo-multiple-checkbox"
              multiple
              value={areaInvestimentos}
              onChange={handleOptionArea}
              input={<OutlinedInput label="Area" />}
              renderValue={(selected) => selected.join(', ')}
            >
              {listaInvestimento.map((invest) => (
                <MenuItem key={invest.idAreaInvestimento} value={invest.idAreaInvestimento}>
                  <Checkbox checked={areaInvestimentos.indexOf(invest.idAreaInvestimento) > -1} />
                  <ListItemText primary={invest.areaInvestimento} />
                </MenuItem>
              ))}
            </Select>
          </FormControl>
        </div>
        <ThemeProvider theme={CustomSilder}>
          <div className="boxFiltro">
            <div className="boxTitlesFiltros">
              <h3>MarketCap</h3>
              <span>Filtre pelo Valor de Mercado</span>
            </div>
            <Slider
              getAriaLabel={() => 'Valor da ação'}
              value={[valorDeAcaoMin, valorDeAcaoMax]}
              onChange={(event, value) => handleValorChange(event, value)}
            />

            <p>{`valor ${marketCapMin}, ${marketCapMax}`}</p>
          </div>
          <div className="boxFiltro">
            <div className="boxTitlesFiltros">
              <h3>Valor de Ação</h3>
              <span>Filtre pelo Valor de ação</span>
            </div>
            <Slider
              getAriaLabel={() => 'MarketCap'}
              value={[marketCapMin, marketCapMax]}
              onChange={(event, value) => handleMarketCapChange(event, value)}
              max={20000000}
              min={0}
            />
            <p>{`valor ${valorDeAcaoMin}, ${valorDeAcaoMax}`}</p>
          </div>
          <div className="boxFiltro">
            <div className="boxTitlesFiltros">
              <h3>Percentual de dividendos:</h3>
              <span>Filtre pelo Percentual de Dividendos</span>
            </div>
            <Slider
              getAriaLabel={() => 'Percentual dividendos'}
              value={[percentualDividendosMin, percentualDividendosMax]}
              onChange={(event, value) => handleDividendosChange(event, value)}
            />
            <p>{`valor ${percentualDividendosMin}, ${percentualDividendosMax}`}</p>
          </div>
        </ThemeProvider>
      </div>
    </div>
  );
};
export default AsideInvest;
