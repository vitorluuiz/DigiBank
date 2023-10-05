import React, { useEffect, useState } from 'react';
// import api from '../../../services/api';
// import { parseJwt } from '../../../services/auth';

// import { ThemeProvider, createTheme } from '@mui/material/styles';
// import Pagination from '@mui/material/Pagination';
import { Link, useNavigate } from 'react-router-dom';
import InfiniteScroll from 'react-infinite-scroll-component';
import { Autocomplete, CircularProgress, MenuItem, Select } from '@mui/material';
import Qs from 'qs';
import Header from '../../../components/Header';
import Footer from '../../../components/Footer';
import AsideInvest from '../../../components/Investimentos/AsideInvest';
import { parseJwt } from '../../../services/auth';
import api from '../../../services/api';
import RecommendedInvestiment from '../../../components/Investimentos/RecommendedInvestment';
// import { InvestimentoOptionsProps } from '../../../@types/InvestimentoOptions';
import { MinimalOptionProps, TitleOptionProps } from '../../../@types/InvestimentoOptions';
import { CssTextField } from '../../../assets/styledComponents/input';
import { useFilterBar } from '../../../services/filtersProvider';
import { calculateValue } from '../../../utils/valueScale';
import SortIcon from '../../../assets/img/sortIcon.svg';
// import Empty from '../../../components/Empty';

interface OptionProps {
  option: TitleOptionProps;
}
function Option({ option }: OptionProps) {
  return (
    <Link to={`investimento/${option.idInvestimentoOption}`} className="linkPost">
      <div className="boxLabelSearch">
        <div className="boxLeftSearch">
          <img src={option.logo} alt="Imagem principal" className="imgLabelSearch" />
          <span className="labelSearch">{option.nome}</span>
        </div>
        {option.valor === 0 ? (
          <span className="labelSearch">Grátis</span>
        ) : (
          <span className="labelSearch">{option.valor.toFixed(2)} BRL</span>
        )}
      </div>
    </Link>
  );
}

export default function Investidos() {
  const [InvestidosList, setInvestidosList] = useState<MinimalOptionProps[]>([]);

  const [isLoading, setLoading] = useState<boolean>(false);
  const [options, setOptions] = useState<TitleOptionProps[]>([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [hasMore, setHasMore] = useState(true);
  const navigate = useNavigate();
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

  const GetInvestidos = () => {
    const itensPerPage = 9;
    setHasMore(false);
    setLoading(true);

    api
      .get(
        `/InvestimentoOptions/${componenteExibido}/comprados/${parseJwt().role}?${Qs.stringify(
          { areas },
          { arrayFormat: 'repeat' },
        )}`,
        {
          params: {
            pagina: currentPage,
            qntItens: itensPerPage,
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
          const { optionList } = response.data;

          if (optionList.length < itensPerPage) {
            setHasMore(false);
          } else {
            setHasMore(true);
          }

          if (currentPage === 1) {
            setInvestidosList(optionList);
          } else {
            setInvestidosList([...InvestidosList, ...optionList]);
          }

          setLoading(false);
        }
      })
      .catch(() => setLoading(false));
  };

  const resetPages = () => (currentPage === 1 ? GetInvestidos() : setCurrentPage(1));

  function ListInvestments() {
    return InvestidosList.map((investimento) => {
      let recommendedInvestmentType = 'Big';
      if (componenteExibido === 5) {
        recommendedInvestmentType = 'cripto';
      }

      return (
        <RecommendedInvestiment
          type={recommendedInvestmentType}
          key={investimento.idInvestimentoOption}
          investimento={investimento}
          isInvestido
        />
      );
    });
  }

  const searchedResults = async (searchValue: any) => {
    try {
      const response = await api.get(
        `Investimento/Usuario/${parseJwt().role}/${componenteExibido}/${1}/${100}`,
      );
      const { data } = response;
      const filteredOptions = data.filter((option: any) =>
        option.nome.toLowerCase().includes(searchValue.toLowerCase()),
      );
      setOptions(filteredOptions);
    } catch (error) {
      console.error(error);
    }
  };

  useEffect(() => {
    resetPages();
    setInvestidosList([]);
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
    GetInvestidos();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [currentPage]);

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
    const searchedValue = '';
    searchedResults(searchedValue);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [componenteExibido]);

  return (
    <div>
      <Header type="" />
      <main id="investidos" className="container">
        <AsideInvest type="investidos" />
        <div className="containerCarousels">
          <div className="topMainCarousel">
            <Autocomplete
              disablePortal
              options={options}
              style={{ width: '60%', alignSelf: 'flex-start' }}
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
                  variant="outlined"
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
              dataLength={InvestidosList.length}
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
