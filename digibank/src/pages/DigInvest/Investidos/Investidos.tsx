import React, { useEffect, useState } from 'react';
// import api from '../../../services/api';
// import { parseJwt } from '../../../services/auth';

// import { ThemeProvider, createTheme } from '@mui/material/styles';
// import Pagination from '@mui/material/Pagination';
import { Link, useNavigate } from 'react-router-dom';
import InfiniteScroll from 'react-infinite-scroll-component';
import { Autocomplete } from '@mui/material';
import Header from '../../../components/Header';
import Footer from '../../../components/Footer';
import AsideInvest from '../../../components/Investimentos/AsideInvest';
import { parseJwt } from '../../../services/auth';
import api from '../../../services/api';
import RecommendedInvestiment from '../../../components/Investimentos/RecommendedInvestment';
// import { InvestimentoOptionsProps } from '../../../@types/InvestimentoOptions';
import { MinimalOptionProps, TitleOptionProps } from '../../../@types/InvestimentoOptions';
import { CssTextField } from '../../../assets/styledComponents/input';
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
  const [componenteExibido, setComponenteExibido] = useState<number | null>(3);
  const [qntItens] = useState(6);

  const [options, setOptions] = useState<TitleOptionProps[]>([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [hasMore, setHasMore] = useState(true);
  const navigate = useNavigate();

  const exibirComponente = (componente: number) => {
    setComponenteExibido(componente);
  };

  const GetInvestidos = () => {
    api
      .get(
        `/Investimento/Usuario/${parseJwt().role}/${componenteExibido}/${currentPage}/${qntItens}`,
      )
      .then((response) => {
        if (response.status === 200) {
          const newInvestimentoList = response.data.investimentosList;
          if (newInvestimentoList.length === 0) {
            setHasMore(false);
          } else {
            setInvestidosList([...InvestidosList, ...newInvestimentoList]);
            setCurrentPage(currentPage + 1);
          }
        }
      });
  };

  function ListInvestments() {
    return InvestidosList.map((investimento) => {
      let recommendedInvestmentType = 'Big';
      if (componenteExibido === 5) {
        recommendedInvestmentType = 'cripto';
      }
      if (componenteExibido === 2) {
        recommendedInvestmentType = 'rendaFixa';
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
    setCurrentPage(1);
    setInvestidosList([]);
    setHasMore(true);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [componenteExibido]);
  useEffect(() => {
    GetInvestidos();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [componenteExibido, currentPage]);

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
        <AsideInvest
          type="investidos"
          componenteExibido={componenteExibido}
          exibirComponente={exibirComponente}
        />
        <div className="containerCarousels">
          <div className="topContainerInvestidos">
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
          </div>
          <div className="boxCarousel">
            <InfiniteScroll
              dataLength={InvestidosList.length}
              next={GetInvestidos}
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
