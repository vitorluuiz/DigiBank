import React, { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import InfiniteScroll from 'react-infinite-scroll-component';
import Autocomplete from '@mui/material/Autocomplete';
import Qs from 'qs';

import Header from '../../components/Header';
import AsideInvest from '../../components/Investimentos/AsideInvest';
// import CarouselInvestimentos from '../../components/Investimentos/CarouselInvestments';
import api from '../../services/api';
import { CssTextField } from '../../assets/styledComponents/input';
import Footer from '../../components/Footer';
import RecommendedInvestiment from '../../components/Investimentos/RecommendedInvestment';
import { MinimalOptionProps, TitleOptionProps } from '../../@types/InvestimentoOptions';
import { useFilterBar } from '../../services/filtersProvider';
import { calculateValue } from '../../utils/valueScale';

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
  const [investimentoList, setInvestimentoList] = useState<MinimalOptionProps[]>([]);
  const [componenteExibido, setComponenteExibido] = useState<number | null>(3);
  const [options, setOptions] = useState<TitleOptionProps[]>([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [hasMore, setHasMore] = useState(true);

  const { areas, minMarketCap, maxMarketCap, minDividendo, minValorAcao, maxValorAcao } =
    useFilterBar();

  const plusPage = () => setCurrentPage(currentPage + 1);

  const exibirComponente = (componente: number) => setComponenteExibido(componente);

  const ListarOptions = () => {
    const itensPerPage = 9;
    setHasMore(false);

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
        }
      });
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
  }, [minValorAcao, maxValorAcao, minDividendo, minMarketCap, maxMarketCap, areas]);

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
        <AsideInvest
          type=""
          componenteExibido={componenteExibido}
          exibirComponente={exibirComponente}
        />

        <div className="containerCarousels">
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
                variant="outlined"
                label="Investimentos"
                type="text"
                style={{ backgroundColor: 'white' }}
                onChange={handleInputChange}
              />
            )}
            onChange={handleOptionSelected}
          />
          <div className="boxCarousel">
            <InfiniteScroll
              dataLength={investimentoList.length}
              next={plusPage}
              hasMore={hasMore}
              loader={<h4>Carregando...</h4>}
              className="boxScrollInfinito"
            >
              {ListInvestments()}
            </InfiniteScroll>
          </div>
        </div>
      </main>
      <Footer />
    </div>
  );
}
