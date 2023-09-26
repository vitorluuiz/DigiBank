import React, { SyntheticEvent, useEffect, useState } from 'react';
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
import { CustomSilder } from '../../assets/styledComponents/range';
import { useFilterBar } from '../../services/filtersProvider';
import { calculateValue, valueLabelFormat } from '../../utils/valueScale';
import api from '../../services/api';

interface AreaInvestimentoProps {
  idAreaInvestimento: number;
  idTipoInvestimento: number;
  area: string;
}

interface AsideProps {
  type: string;
  componenteExibido: number | null;
  exibirComponente: (componente: number) => void;
}

const AsideInvest: React.FC<AsideProps> = ({ componenteExibido, exibirComponente, type }) => {
  const [titulo, setTitulo] = useState('');
  const [areasList, setAreasList] = useState<AreaInvestimentoProps[]>([]);
  const [minCap, setMinCap] = useState<number>(0);
  const [maxCap, setMaxCap] = useState<number>(100);
  const [minValor, setMinValor] = useState<number>(0);
  const [maxValor, setMaxValor] = useState<number>(100);
  const [dividendoMin, setDividendoMin] = useState<number>(0);
  const {
    areas,
    handleDividendosChange,
    handleMarketCapChange,
    handleOptionArea,
    handleValorChange,
  } = useFilterBar();

  const getSelectAreas = () => {
    api(`AreaInvestimento/Tipo/${componenteExibido}`).then((response) => {
      if (response.status === 200) {
        handleOptionArea(null);
        setAreasList(response.data);
      }
    });
  };

  const handleValorAcao = (
    event: Event | SyntheticEvent<Element, Event>,
    newValue: number | number[],
  ) => {
    if (typeof newValue !== 'number') {
      setMinValor(newValue[0]);
      setMaxValor(newValue[1]);
    }
  };

  const handleMarketCap = (
    event: Event | SyntheticEvent<Element, Event>,
    newValue: number | number[],
  ) => {
    if (typeof newValue !== 'number') {
      setMinCap(newValue[0]);
      setMaxCap(newValue[1]);
    }
  };

  const handleDividendos = (event: Event | SyntheticEvent<Element, Event>, newValue: number) => {
    setDividendoMin(newValue);
  };

  // eslint-disable-next-line react-hooks/exhaustive-deps
  useEffect(() => getSelectAreas(), [componenteExibido]);

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
              value={areas}
              onChange={handleOptionArea}
              input={<OutlinedInput label="Area" />}
              renderValue={(selected) => selected.join(', ')}
            >
              {areasList.map((area) => (
                <MenuItem key={area.idAreaInvestimento} value={area.idAreaInvestimento}>
                  <Checkbox checked={areas.indexOf(area.idAreaInvestimento) > -1} />
                  <ListItemText primary={area.area} />
                </MenuItem>
              ))}
            </Select>
          </FormControl>
        </div>
        <ThemeProvider theme={CustomSilder}>
          <div className="boxFiltro">
            <div className="boxTitlesFiltros">
              <h3>Valor de ação</h3>
              <span>Filtre pelo Valor da ação</span>
            </div>
            <Slider
              getAriaLabel={() => 'Valor da ação'}
              value={[minValor, maxValor]}
              getAriaValueText={() => `${valueLabelFormat}dwa`}
              valueLabelFormat={(value) => `${calculateValue(value, 1)} BRL`}
              max={100}
              min={0}
              valueLabelDisplay="auto"
              onChange={handleValorAcao}
              onChangeCommitted={(event, value) => handleValorChange(event, value)}
            />
          </div>
          <div className="boxFiltro">
            <div className="boxTitlesFiltros">
              <h3>MarketCap</h3>
              <span>Filtre pelo valor de mercado</span>
            </div>
            <Slider
              getAriaLabel={() => 'MarketCap'}
              value={[minCap, maxCap]}
              max={100}
              min={0}
              valueLabelDisplay="auto"
              scale={(value) => calculateValue(value, 1000)}
              getAriaValueText={valueLabelFormat}
              valueLabelFormat={valueLabelFormat}
              onChange={handleMarketCap}
              onChangeCommitted={(event, value) => handleMarketCapChange(event, value)}
            />
          </div>
          <div className="boxFiltro">
            <div className="boxTitlesFiltros">
              <h3>Percentual de dividendos:</h3>
              <span>Filtre pelo Percentual mínimo de dividendos</span>
            </div>
            <Slider
              getAriaLabel={() => 'Percentual dividendos'}
              value={dividendoMin}
              valueLabelDisplay="auto"
              max={10}
              min={0}
              onChange={(event, value) =>
                handleDividendos(event, typeof value === 'number' ? value : 0)
              }
              onChangeCommitted={(event, value) =>
                handleDividendosChange(event, typeof value === 'number' ? value : 0)
              }
            />
          </div>
        </ThemeProvider>
      </div>
    </div>
  );
};
export default AsideInvest;
