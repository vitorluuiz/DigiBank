import React, { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { Autocomplete } from '@mui/material';
import Header from '../../components/Header';
import Footer from '../../components/Footer';
import UserIcon from '../../assets/img/user_icon.svg';
import ShoppingCartIcon from '../../assets/img/shopping-cart_icon.svg';
import InventoryIcon from '../../assets/img/inventory_icon.svg';
import { CssTextField } from '../../assets/styledComponents/input';
// import Carousel from '../../components/MarketPlace/Carousel';
import api from '../../services/api';
import Seta from '../../assets/img/SetaVerMais.svg';
import BannerStore from '../../components/MarketPlace/Banner';
import Carousel from '../../components/MarketPlace/Carousel';

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

  // useEffect(() => {
  const ListarTodos = async () => {
    try {
      const response = await api.get('/Marketplace/Recomendadas/21', {
        params: {
          qntItens: 20,
        },
      });
      const { data } = response;
      setOptions(data);
    } catch (error) {
      console.error(error);
    }
  };

  // eslint-disable-next-line react-hooks/exhaustive-deps
  const searchedResults = async (searchValue: any) => {
    try {
      const response = await api.get('/Marketplace/Recomendadas/21', {
        params: {
          qntItens: 20,
          search: searchValue,
        },
      });
      const { data } = response;
      const filteredOptions = data.filter((option: any) =>
        option.titulo.toLowerCase().includes(searchValue.toLowerCase()),
      );
      setOptions(filteredOptions);
    } catch (error) {
      console.error(error);
    }
  };

  useEffect(() => {
    ListarTodos();
  }, []);

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
  }, [searchedResults]);

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
              <Link to="/User">
                <img alt="Usuário" src={UserIcon} />
              </Link>
              <Link to="wishlist">
                <img alt="Carrinho de compras" src={ShoppingCartIcon} />
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
          <Carousel type="normal" />
        </section>
        <section className="suport-list">
          <div className="box-top-support">
            <h2>Melhores Avaliados</h2>
            <Link to="/digistore/avaliacao">
              Ver Mais <img src={Seta} alt="seta ver mais" />
            </Link>
          </div>
          <Carousel type="slim" />
        </section>
        <button id="ver_mais-btn">Listar mais itens</button>
      </main>
      <Footer />
    </div>
  );
}
