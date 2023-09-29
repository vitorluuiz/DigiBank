import React, { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import InfiniteScroll from 'react-infinite-scroll-component';
import Autocomplete from '@mui/material/Autocomplete';
import Qs from 'qs';
import { CircularProgress, MenuItem, Select } from '@mui/material';

import Header from '../../components/Header';
import AsideInvest from '../../components/Investimentos/AsideInvest';
import api from '../../services/api';
import { CssTextField } from '../../assets/styledComponents/input';
import Footer from '../../components/Footer';
import RecommendedInvestiment from '../../components/Investimentos/RecommendedInvestment';
import { MinimalOptionProps, TitleOptionProps } from '../../@types/InvestimentoOptions';
import { useFilterBar } from '../../services/filtersProvider';
import { calculateValue } from '../../utils/valueScale';
import SortIcon from '../../assets/img/sortIcon.svg';

interface OptionProps {
  option: TitleOptionProps;
}
function Option({ option }: OptionProps) {
  return (
    <Link to={`investimento/${option.idInvestimentoOption}`} className="linkPost">
      <div className="boxLabelSearch">
        <div className="boxLeftSearch" style={{ height: '4rem' }}>
          <img
            src={option.logo}
            alt="Imagem principal"
            className="imgLabelSearch"
            style={{ width: '4rem', height: '4rem' }}
          />
          <span className="labelSearch" style={{ maxWidth: '60%' }}>
            {option.nome}
          </span>
          <span className="labelSearch">{option.valor.toFixed(2)} BRL</span>
        </div>
      </div>
    </Link>
  );
}

export default function InvestPlace() {
  const navigate = useNavigate();
  const [isLoading, setLoading] = useState<boolean>(false);
  const [investimentoList, setInvestimentoList] = useState<MinimalOptionProps[]>([]);
  const [options, setOptions] = useState<TitleOptionProps[]>([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [hasMore, setHasMore] = useState(true);
  const [ordenador, setOrdenador] = useState('valorDesc');

  const {
    areas,
    minMarketCap,
    maxMarketCap,
    minDividendo,
    minValorAcao,
    maxValorAcao,
    componenteExibido,
  } = useFilterBar();

  const plusPage = () => setCurrentPage(currentPage + 1);

  const ListarOptions = () => {
    const itensPerPage = 9;
    setHasMore(false);
    setLoading(true);

    api
      .get(
        `InvestimentoOptions/${componenteExibido}/${currentPage}/${itensPerPage}?${Qs.stringify(
          { areas },
          { arrayFormat: 'repeat' },
        )}`,
        {
          params: {
            minMarketCap: calculateValue(minMarketCap, 1000000000),
            maxMarketCap: calculateValue(maxMarketCap, 1000000000),
            minDividendo,
            minValorAcao: calculateValue(minValorAcao, 1),
            maxValorAcao: calculateValue(maxValorAcao, 1),
            ordenador,
          },
        },
      )
      .then((response) => {
        if (response.status === 200) {
          const { optionsList } = response.data;

          if (optionsList.length < itensPerPage) {
            setHasMore(false);
          } else {
            setHasMore(true);
          }

          if (currentPage === 1) {
            setInvestimentoList(optionsList);
          } else {
            setInvestimentoList([...investimentoList, ...optionsList]);
          }

          setLoading(false);
        }
      })
      .catch(() => setLoading(false));
  };

  const resetPages = () => (currentPage === 1 ? ListarOptions() : setCurrentPage(1));

  function ListInvestments() {
    return investimentoList.map((investimento) => {
      let recommendedInvestmentType = 'Big';
      if (componenteExibido === 5) {
        recommendedInvestmentType = 'cripto';
      }
      if (componenteExibido === 2) {
        recommendedInvestmentType = 'rendaFixa';
      }

      return (
        <RecommendedInvestiment type={recommendedInvestmentType} investimento={investimento} />
      );
    });
  }

  const searchedResults = async (searchValue: any) => {
    try {
      const response = await api.get(`/InvestimentoOptions/Buscar/${componenteExibido}/100`);
      const { data } = response;
      const filteredOptions = data.filter((option: any) =>
        option.nome.toLowerCase().includes(searchValue.toLowerCase()),
      );
      setOptions(filteredOptions);
    } catch (error) {
      console.error(error);
    }
  };

  const handleInputChange = (value: any) => {
    searchedResults(value);
  };
  const handleOptionSelected = (_: any, option: any) => {
    if (option && option.idInvestimentoOption) {
      const investimentoId = option.idInvestimentoOption;
      navigate(`/investimento/${investimentoId}`);
    }
    return null;
  };

  useEffect(() => {
    resetPages();
    setInvestimentoList([]);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [componenteExibido]);

  useEffect(() => {
    resetPages();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [
    minValorAcao,
    maxValorAcao,
    minDividendo,
    minMarketCap,
    maxMarketCap,
    areas,
    ordenador,
    componenteExibido,
  ]);

  useEffect(() => {
    ListarOptions();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [currentPage]);

  useEffect(() => {
    const searchedValue = '';
    searchedResults(searchedValue);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [componenteExibido]);

  return (
    <div>
      <Header type="" />
      <main className="container" id="diginvest">
        <AsideInvest type="" />

        <div className="containerCarousels">
          <div className="topMainCarousel">
            <Autocomplete
              disablePortal
              options={options}
              style={{ width: '65%', alignSelf: 'flex-start' }}
              noOptionsText="Nenhum Produto Encontrado!"
              getOptionLabel={(option) => option?.nome ?? ''}
              renderOption={(props, option) => (
                // eslint-disable-next-line react/jsx-props-no-spreading
                <Option {...props} option={option} />
              )}
              renderInput={(params) => (
                <CssTextField
                  // eslint-disable-next-line react/jsx-props-no-spreading
                  {...params}
                  // fullWidth
                  // variant="outlined"
                  label="Investimentos"
                  type="text"
                  style={{ backgroundColor: 'white' }}
                  onChange={handleInputChange}
                />
              )}
              onChange={handleOptionSelected}
            />
            <Select
              value={ordenador}
              onChange={(evt) => setOrdenador(evt.target.value)}
              sx={{
                width: '31%',
                backgroundColor: '#fff',
                borderRadius: '5px',
                borderBottom: 'none',
              }}
              variant="filled"
              // eslint-disable-next-line react/no-unstable-nested-components
              IconComponent={() => (
                <img
                  src={SortIcon}
                  alt="Custom Icon"
                  style={{ width: '4rem', height: '1.75rem' }}
                />
              )}
            >
              <MenuItem value="marketcapDesc">Marcas Mais Valiosas</MenuItem>
              <MenuItem value="marketcapAsc">Marcas Menos Valiosas</MenuItem>
              <MenuItem value="valorAsc">Ações Mais Baratas</MenuItem>
              <MenuItem value="valorDesc">Ações Mais Caras</MenuItem>
              <MenuItem value="dividendoDesc">Mais Rentáveis</MenuItem>
            </Select>
          </div>
          <div className="boxCarousel">
            <InfiniteScroll
              dataLength={investimentoList.length}
              next={plusPage}
              hasMore={hasMore}
              loader={<h4>Carregando...</h4>}
              className="boxScrollInfinito"
            >
              {ListInvestments()}
              {isLoading ? <CircularProgress color="inherit" /> : undefined}
            </InfiniteScroll>
          </div>
        </div>
      </main>
      <Footer />
    </div>
  );
}
