// eslint-disable-next-line eslint-comments/disable-enable-pair
/* eslint-disable react-hooks/exhaustive-deps */
import React, { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import InfiniteScroll from 'react-infinite-scroll-component';
import Autocomplete from '@mui/material/Autocomplete';
import Header from '../../components/Header';
import AsideInvest from '../../components/Investimentos/AsideInvest';
// import CarouselInvestimentos from '../../components/Investimentos/CarouselInvestments';
import api from '../../services/api';
import { CssTextField } from '../../assets/styledComponents/input';
import Footer from '../../components/Footer';
import RecommendedInvestiment from '../../components/Investimentos/RecommendedInvestment';
import { MinimalOptionProps, TitleOptionProps } from '../../@types/InvestimentoOptions';

interface OptionProps {
  option: TitleOptionProps;
}
function Option({ option }: OptionProps) {
  return (
    <Link to={`investimento/${option.idInvestimentoOption}`} className="linkPost">
      <div className="boxLabelSearch">
        <div className="boxLeftSearch" style={{ height: '4rem¿¿¿' }}>
          <img
            src={option.logo}
            alt="Imagem principal"
            className="imgLabelSearch"
            style={{ width: '4rem' }}
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

  const exibirComponente = (componente: number) => {
    setComponenteExibido(componente);
  };

  const ListarOptions = () => {
    api.get(`InvestimentoOptions/${componenteExibido}/${currentPage}/${9}`).then((response) => {
      if (response.status === 200) {
        const newInvestimentoList = response.data.optionsList;
        if (newInvestimentoList.length === 0) {
          setHasMore(false);
        } else {
          setInvestimentoList([...investimentoList, ...newInvestimentoList]);
          setCurrentPage(currentPage + 1);
        }
      }
    });
  };

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
    setCurrentPage(1);
    setInvestimentoList([]);
    setHasMore(true);
  }, [componenteExibido]);

  useEffect(() => {
    if (currentPage === 1) {
      ListarOptions();
    }
  }, [currentPage, componenteExibido]);

  useEffect(() => {
    const searchedValue = '';
    searchedResults(searchedValue);
  }, [componenteExibido]);

  return (
    <div>
      <Header type="digInvest" />
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
          <div className="boxCarousel">
            <InfiniteScroll
              dataLength={investimentoList.length}
              next={ListarOptions}
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
