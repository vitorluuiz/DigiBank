import React, { useEffect, useState } from 'react';
import InfiniteScroll from 'react-infinite-scroll-component';
import { CircularProgress, MenuItem, Select } from '@mui/material';
import Qs from 'qs';
import Header from '../../../components/Header';
import Footer from '../../../components/Footer';
import AsideInvest from '../../../components/Investimentos/AsideInvest';
import { parseJwt } from '../../../services/auth';
import api from '../../../services/api';
import RecommendedInvestiment from '../../../components/Investimentos/RecommendedInvestment';
import { MinimalOptionProps } from '../../../@types/InvestimentoOptions';
import { useFilterBar } from '../../../services/filtersProvider';
import { calculateValue } from '../../../utils/valueScale';
import SortIcon from '../../../assets/img/sortIcon.svg';

export default function Investidos() {
  const [InvestidosList, setInvestidosList] = useState<MinimalOptionProps[]>([]);

  const [isLoading, setLoading] = useState<boolean>(false);
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

  const GetInvestidos = (page: number) => {
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
            pagina: page,
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

          if (page === 1) {
            setInvestidosList(optionList);
          } else {
            setInvestidosList([...InvestidosList, ...optionList]);
          }

          setLoading(false);
        }
      })
      .catch(() => setLoading(false));
  };

  const resetPages = () => GetInvestidos(1);

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
        />
      );
    });
  }

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

  return (
    <div>
      <Header type="" />
      <main id="investidos" className="container">
        <AsideInvest type="investidos" />
        <div className="containerCarousels">
          <div className="topMainCarousel unico">
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
              next={() => {
                GetInvestidos(currentPage + 1);
                setCurrentPage(currentPage + 1);
              }}
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
