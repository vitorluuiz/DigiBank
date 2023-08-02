import React, { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import Autocomplete from '@mui/material/Autocomplete';
import Header from '../../components/Header';
import AsideInvest from '../../components/Investimentos/AsideInvest';
// import CarouselInvestimentos from '../../components/Investimentos/CarouselInvestments';
import api from '../../services/api';
import { CssTextField } from '../../assets/styledComponents/input';
import Footer from '../../components/Footer';
import { InvestidosProps } from '../../@types/Investidos';
import RecommendedInvestiment from '../../components/Investimentos/RecommendedInvestment';

interface OptionProps {
  option: InvestidosProps;
}
function Option({ option }: OptionProps) {
  return (
    <Link to={`/investimento/${option.idOption}`} className="linkPost">
      <div className="boxLabelSearch">
        <div className="boxLeftSearch">
          <img src={option.mainImg} alt="Imagem principal" className="imgLabelSearch" />
          <span className="labelSearch">{option.nome}</span>
        </div>
        {option.valor === 0 ? (
          <span className="labelSearch">Grátis</span>
        ) : (
          <span className="labelSearch">{option.valor} BRL</span>
        )}
      </div>
    </Link>
  );
}

export default function InvestPlace() {
  const navigate = useNavigate();
  const [investimentoList, setInvestimentoList] = useState<InvestidosProps[]>([]);
  const [componenteExibido, setComponenteExibido] = useState<number | null>(3);
  const [options, setOptions] = useState<Array<InvestidosProps>>([]);

  const exibirComponente = (componente: number) => {
    setComponenteExibido(componente);
  };
  function ListarOptions() {
    api.get(`InvestimentoOptions/${componenteExibido}/${1}/${213321}/`).then((response) => {
      if (response.status === 200) {
        // const data = Array.isArray(response.data) ? response.data : [];
        // setInvestimentoList(data);
        setInvestimentoList(response.data);
        console.log(response.data);
      }
    });
  }

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
      console.log('Aoba', filteredOptions);
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
    ListarOptions();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [componenteExibido]);

  useEffect(() => {
    const searchedValue = '';
    searchedResults(searchedValue);
    // eslint-disable-next-line react-hooks/exhaustive-deps
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
                label="Categorias"
                type="text"
                style={{ backgroundColor: 'white' }}
                onChange={handleInputChange}
              />
            )}
            onChange={handleOptionSelected}
          />
          <div className="boxCarousel">
            {ListInvestments()}
            {/* <h2>Mais Vendidos</h2>
            <CarouselInvestimentos type="vendas" typeInvestimento={componenteExibido} />
          </div>
          <div className="boxCarousel">
            <h2>Investir novamente</h2>
            <CarouselInvestimentos type="comprados" typeInvestimento={componenteExibido} />
          </div>
          <div className="boxCarousel">
            <h2>Em alta</h2>
            <CarouselInvestimentos type="emAlta" typeInvestimento={componenteExibido} />
          </div>
          <div className="boxCarousel">
            <h2>Até R$50,00</h2>
            <CarouselInvestimentos
              type="valor"
              typeInvestimento={componenteExibido}
              maxValue={50}
            /> */}
          </div>
        </div>
      </main>
      <Footer />
    </div>
  );
}
