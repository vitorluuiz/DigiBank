import React from 'react';
import ReactDOM from 'react-dom/client';
// import { Provider } from 'react-redux';
import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom';

import Login from './pages/Login/Login';
import Cadastro from './pages/Cadastro/Cadastro';
import Extratos from './pages/Extrato/Extrato';
import App from './App';
import Home from './pages/Home/Home';
import Emprestimos from './pages/Emprestimo/Emprestimo';
import MinhaArea from './pages/MyPlace/MinhaArea';
import Metas from './pages/Metas/Metas';
import MetaUnica from './pages/Metas/MetaUnica';
import NotFound from './pages/Erros/NotFound';
import Forbidden from './pages/Erros/Forbidden';
import Unauthorized from './pages/Erros/Unauthorized';
import MarketPlace from './pages/DigiStore/MarketPlace';
import Post from './pages/DigiStore/Post';
import CadastroPost from './pages/DigiStore/CadastroPost';
import ServiceUnavailable from './pages/Erros/ServiceUnavailable';
import Inventario from './pages/DigiStore/Inventario/Inventario';
import Catalogo from './pages/DigiStore/MarketPlaceCatalogo';
import Wishlist from './pages/DigiStore/MarketPlaceWishlist';
import MeusPosts from './pages/DigiStore/MeusPosts';
import InvestPlace from './pages/DigInvest/InvestPlace';
import Investidos from './pages/DigInvest/Investidos/Investidos';
import Poupanca from './pages/Poupanca/Poupanca';
import InvestPost from './pages/DigInvest/InvestPost';
import Carteira from './pages/DigInvest/Carteira/Carteira';
import FavortosInvest from './pages/DigInvest/Favoritos/InvestFavoritos';
import { SnackBarProvider } from './services/snackBarProvider';
import HistoricoInvestimentos from './pages/DigInvest/Carteira/HistoricoInvestimentos';
import { FiltersProvider } from './services/filtersProvider';

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <SnackBarProvider>
      <FiltersProvider>
        {/* <Provider store={store}> */}
        <BrowserRouter>
          <Routes>
            <Route path="/" element={<App />} />
            <Route path="/cadastro" element={<Cadastro />} />
            <Route path="/extrato" element={<Extratos />} />
            <Route path="/login" element={<Login />} />
            <Route path="/emprestimos" element={<Emprestimos />} />
            <Route path="/minha-area" element={<MinhaArea />} />
            <Route path="/metas" element={<Metas />} />
            <Route path="/meta/:idMeta" element={<MetaUnica />} />
            <Route path="/home" element={<Home />} />
            <Route path="/503" element={<ServiceUnavailable />} />
            <Route path="/404" element={<NotFound />} />
            <Route path="/403" element={<Forbidden />} />
            <Route path="/401" element={<Unauthorized />} />
            <Route path="*" element={<Navigate to="/404" />} />
            <Route path="/digistore" element={<MarketPlace />} />
            <Route path="/digistore/inventario" element={<Inventario />} />
            <Route path="/digistore/:filtro" element={<Catalogo />} />
            <Route path="/digistore/inventario" element={<Inventario />} />
            <Route path="/digistore/wishlist" element={<Wishlist />} />
            <Route path="/digistore/publicados" element={<MeusPosts />} />
            <Route path="/cadastro-post" element={<CadastroPost />} />
            <Route path="/post/:idPost" element={<Post />} />
            <Route path="/diginvest" element={<InvestPlace />} />
            <Route path="/diginvest/investidos" element={<Investidos />} />
            <Route path="/diginvest/favoritos" element={<FavortosInvest />} />
            <Route path="/diginvest/investimento/:idInvestimentoOption" element={<InvestPost />} />
            <Route path="/diginvest/poupanca" element={<Poupanca />} />
            <Route path="/diginvest/carteira" element={<Carteira />} />
            <Route path="/diginvest/carteira/investimentos" element={<HistoricoInvestimentos />} />
            <Route path="/diginvest/investimento/:idInvestimentoOption" element={<InvestPost />} />
          </Routes>
        </BrowserRouter>
      </FiltersProvider>
      {/* </Provider> */}
    </SnackBarProvider>
  </React.StrictMode>,
);
