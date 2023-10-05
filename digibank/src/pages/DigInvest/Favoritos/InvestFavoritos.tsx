import React, { useEffect, useState } from 'react';
import Qs from 'qs';
import { Link, useNavigate } from 'react-router-dom';
import InfiniteScroll from 'react-infinite-scroll-component';
import { Autocomplete, CircularProgress, MenuItem, Select } from '@mui/material';
import Footer from '../../../components/Footer';
import Header from '../../../components/Header';
import api from '../../../services/api';
import { parseJwt } from '../../../services/auth';
import RecommendedInvestiment from '../../../components/Investimentos/RecommendedInvestment';
import { WishlishedInvestment } from '../InvestPost';
import { MinimalOptionProps, TitleOptionProps } from '../../../@types/InvestimentoOptions';
import AsideInvest from '../../../components/Investimentos/AsideInvest';
import { useFilterBar } from '../../../services/filtersProvider';
import { calculateValue } from '../../../utils/valueScale';
import SortIcon from '../../../assets/img/sortIcon.svg';
import { CssTextField } from '../../../assets/styledComponents/input';

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

export default function FavortosInvest() {
  const navigate = useNavigate();
  const [investList, setInvestList] = useState<MinimalOptionProps[]>([]);
  const [isLoading, setLoading] = useState<boolean>(false);
  const [currentPage, setCurrentPage] = useState(1);
  const [hasMore, setHasMore] = useState(true);
  const [ordenador, setOrdenador] = useState('valorDesc');
  const [options, setOptions] = useState<TitleOptionProps[]>([]);

  const {
    areas,
    minMarketCap,
    maxMarketCap,
    minDividendo,
    minValorAcao,
    maxValorAcao,
    componenteExibido,
  } = useFilterBar();

  function ListFavoritos() {
    return investList.map((investimento) => {
      let recommendedInvestmentType = 'Big';
      if (componenteExibido === 5) {
        recommendedInvestmentType = 'cripto';
      }

      return (
        <RecommendedInvestiment type={recommendedInvestmentType} investimento={investimento} />
      );
    });
  }

  const plusPage = () => setCurrentPage(currentPage + 1);

  const GetWishlistFromServer = (idsInvestimento: number[]) => {
    const itensPerPage = 3;
    setHasMore(false);
    setLoading(true);

    api
      .get(
        `InvestimentoOptions/Favoritos/${componenteExibido}?${Qs.stringify(
          { ids: idsInvestimento },
          { arrayFormat: 'repeat' },
        )}&${Qs.stringify({ areas }, { arrayFormat: 'repeat' })}`,
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
          const { optionsList } = response.data;

          if (optionsList.length < itensPerPage) {
            setHasMore(false);
          } else {
            setHasMore(true);
          }

          if (currentPage === 1) {
            setInvestList(optionsList);
          } else {
            setInvestList([...investList, ...optionsList]);
          }

          setLoading(false);
        }
      })
      .catch(() => setLoading(false));
  };

  const GetWishlistFromLocal = () => {
    if (localStorage.getItem('wishlistInvest')) {
      const localData: WishlishedInvestment[] = JSON.parse(
        localStorage.getItem('wishlistInvest') ?? '[]',
      );
      const idInvestimentos: number[] = [];
      localData.forEach((item) => {
        if (item.idUsuario === parseJwt().role) {
          idInvestimentos.push(item.idInvestimentoOption);
        }
      });
      GetWishlistFromServer(idInvestimentos);
    } else {
      localStorage.setItem('wishlistInvest', '[]');
    }
  };

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

  const resetPages = () => (currentPage === 1 ? GetWishlistFromLocal() : setCurrentPage(1));

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
    if (currentPage > 1) {
      GetWishlistFromLocal();
    }
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
      <main id="investFavoritos" className="container">
        <AsideInvest type="favoritos" />
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
            {' '}
            <InfiniteScroll
              dataLength={investList.length}
              next={plusPage}
              hasMore={hasMore}
              loader={<h4>Carregando...</h4>}
              className="boxScrollInfinito"
            >
              {ListFavoritos()}
              {isLoading ? <CircularProgress color="inherit" /> : undefined}
            </InfiniteScroll>
          </div>
        </div>
      </main>
      <Footer />
    </div>
  );
}
