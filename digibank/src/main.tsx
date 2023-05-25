import React from 'react';
import ReactDOM from 'react-dom/client';
// import { Provider } from 'react-redux';
import { BrowserRouter, Route, Routes } from 'react-router-dom';

import Login from './pages/Login/Login';
import Cadastro from './pages/Cadastro/Cadastro';
import Extratos from './pages/Extrato/Extrato';
import App from './App';
import Home from './pages/Home/Home';
import Emprestimos from './pages/Emprestimo/Emprestimo';
import MinhaArea from './pages/MinhaArea/MinhaArea';
import Metas from './pages/Metas/Metas';
import MetaUnica from './pages/Metas/MetaUnica';
import MarketPlace from './pages/DigiStore/MarketPlace';

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    {/* <Provider store={store}> */}
    <BrowserRouter>
      <Routes>
        <Route path="/seila" element={<App />} />
        <Route path="/cadastro" element={<Cadastro />} />
        <Route path="/extrato" element={<Extratos />} />
        <Route path="/" element={<Login />} />
        <Route path="/emprestimos" element={<Emprestimos />} />
        <Route path="/minha-area" element={<MinhaArea />} />
        <Route path="/metas" element={<Metas />} />
        <Route path="/meta/:idMeta" element={<MetaUnica />} />
        <Route path="/home" element={<Home />} />
        <Route path="/digistore" element={<MarketPlace />} />
      </Routes>
    </BrowserRouter>
    {/* </Provider> */}
  </React.StrictMode>,
);
