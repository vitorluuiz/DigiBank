import React, { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import Autocomplete from '@mui/material/Autocomplete';
import Header from '../../components/Header';
import Footer from '../../components/Footer';
import UserIcon from '../../assets/img/person_icon.svg';
import BookMarkIcon from '../../assets/img/bookmark_icon.svg';
import InventoryIcon from '../../assets/img/inventory_icon.svg';
import { CssTextField } from '../../assets/styledComponents/input';
import api from '../../services/api';
import Seta from '../../assets/img/SetaVerMais.svg';
import BannerStore from '../../components/MarketPlace/Banner';
import CarouselPosts from '../../components/MarketPlace/Carousel/CarouselPosts';

type OptionType = {
  idPost: number;
  titulo: string;
  valor: number;
  mainImg: string;
};

interface OptionProps {
  option: OptionType;
}

function Option({ option }: OptionProps) {
  return (
    <Link to={`/post/${option.idPost}`} className="linkPost">
      <div className="boxLabelSearch">
        <div className="boxLeftSearch">
          <img
            src={`http://localhost:5000/img/${option.mainImg}`}
            alt="Imagem principal"
            className="imgLabelSearch"
          />
          <span className="labelSearch">{option.titulo}</span>
        </div>
        <span className="labelSearch">{option.valor} BRL</span>
      </div>
    </Link>
  );
}
export default function MarketPlace() {
  const navigate = useNavigate();
  const [options, setOptions] = useState<Array<OptionType>>([]);
  // const [listarMais, setListarMais] = useState(false);
  const [nextSection, setNextSection] = useState(0);
  // const [showButton, setShowButton] = useState(true);
  const sections = [
    {
      title: 'Comprar Novamente',
      link: '/digistore/comprados',
      type: 'comprados',
    },
    {
      title: 'Abaixo de R$50',
      link: '/digistore/50',
      type: 'valor',
      maxValue: 50,
    },
    {
      title: 'Abaixo de R$15',
      link: '/digistore/25',
      type: 'valor',
      maxValue: 25,
    },
    {
      title: 'Abaixo de R$5',
      link: '/digistore/5',
      type: 'valor',
      maxValue: 5,
    },
    {
      title: 'Gratuitos',
      link: '/digistore/0',
      type: 'valor',
      maxValue: 0,
    },
  ];
  // const handleListarMais = () => {
  //   setListarMais(true);
  //   // setShowButton(false);
  // };
  const handleListarMais = () => {
    setNextSection((prevSection) => prevSection + 1);
  };

  // eslint-disable-next-line react-hooks/exhaustive-deps
  const searchedResults = async (searchValue: any) => {
    try {
      const response = await api.get('/Marketplace/Recomendadas/21');
      const { data } = response;
      const filteredOptions = data.filter((option: any) =>
        option.titulo.toLowerCase().includes(searchValue.toLowerCase()),
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
    if (option && option.idPost) {
      const postId = option.idPost;
      navigate(`/post/${postId}`);
    }
    return null;
  };

  useEffect(() => {
    const searchedValue = ''; // Defina o valor de pesquisa atual aqui
    searchedResults(searchedValue);
  }, []);
  useEffect(() => {
    const handleScroll = () => {
      if (
        window.innerHeight + document.documentElement.scrollTop >=
        document.documentElement.offsetHeight - 100
      ) {
        handleListarMais();
      }
    };

    window.addEventListener('scroll', handleScroll);

    return () => {
      window.removeEventListener('scroll', handleScroll);
    };
  }, []);

  // useEffect(() => {
  //   const handleScroll = () => {
  //     // Verifica se o usuário chegou ao final da página
  //     if (
  //       window.innerHeight + document.documentElement.scrollTop ===
  //       document.documentElement.offsetHeight
  //     ) {
  //       handleListarMais();
  //     }
  //   };

  //   // Adiciona o evento de scroll ao carregar o componente
  //   window.addEventListener('scroll', handleScroll);

  //   // Remove o evento de scroll ao desmontar o componente
  //   return () => {
  //     window.removeEventListener('scroll', handleScroll);
  //   };
  // }, []);

  return (
    <div>
      <Header type="" />
      <main id="digistore" className="container">
        <section className="banner-suport">
          <div className="banner-store">
            {/* <h1>Efetue transferências e movimente seu saldo</h1>
            <span>Para adquirir pontos e utilziar nossa Loja repleta de descontos</span> */}
            <BannerStore />
          </div>
          <div className="store-header">
            <nav className="store-navigate">
              <Link to="publicados">
                <img alt="Usuário" src={UserIcon} />
              </Link>
              <Link to="wishlist">
                <img alt="Carrinho de compras" src={BookMarkIcon} />
              </Link>
              <Link to="inventario">
                <img alt="Inventário" src={InventoryIcon} />
              </Link>
            </nav>
            <div className="store-search-suport">
              <Link to="/cadastro-post" className="btnCadastrarStore">
                Cadastrar
              </Link>
              <Autocomplete
                fullWidth
                disablePortal
                options={options}
                noOptionsText="Nenhum Produto Encontrado!"
                getOptionLabel={(option) => option?.titulo ?? ''}
                renderOption={(props, option) => (
                  // eslint-disable-next-line react/jsx-props-no-spreading
                  <Option {...props} option={option} />
                )}
                renderInput={(params) => (
                  <CssTextField
                    // eslint-disable-next-line react/jsx-props-no-spreading
                    {...params}
                    fullWidth
                    variant="outlined"
                    label="Categorias"
                    type="text"
                    style={{ backgroundColor: 'white' }}
                    onChange={handleInputChange}
                  />
                )}
                onChange={handleOptionSelected}
              />
            </div>
          </div>
        </section>
        <section className="suport-list">
          <div className="box-top-support">
            <h2>Mais vendidos</h2>
            <Link to="/digistore/vendas">
              Ver Mais <img src={Seta} alt="seta ver mais" />
            </Link>
          </div>
          <CarouselPosts type="normal" />
        </section>
        <section className="suport-list">
          <div className="box-top-support">
            <h2>Melhores Avaliados</h2>
            <Link to="/digistore/avaliacao">
              Ver Mais <img src={Seta} alt="seta ver mais" />
            </Link>
          </div>
          <CarouselPosts type="slim" />
        </section>
        {sections.map((section, index) => {
          if (index <= nextSection) {
            return (
              // eslint-disable-next-line react/no-array-index-key
              <section key={index} className="suport-list">
                <div className="box-top-support">
                  <h2>{section.title}</h2>
                  <Link to={section.link}>
                    Ver Mais <img src={Seta} alt="seta ver mais" />
                  </Link>
                </div>
                <CarouselPosts type={section.type} maxValue={section.maxValue} />
              </section>
            );
          }
          return null;
        })}
      </main>
      <Footer />
    </div>
  );
}
