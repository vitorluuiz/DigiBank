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
import MinhaArea from './pages/MinhaArea/MinhaArea';
import Metas from './pages/Metas/Metas';
import MetaUnica from './pages/Metas/MetaUnica';
<<<<<<< HEAD
import NotFound from './pages/Erros/NotFound';
import Forbidden from './pages/Erros/Forbidden';
import Unauthorized from './pages/Erros/Unauthorized';
=======
import MarketPlace from './pages/DigiStore/MarketPlace';
>>>>>>> 47cdda6ed062f7304e903299f7bcf051d38a59f8

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
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
<<<<<<< HEAD
        <Route path="/404" element={<NotFound />} />
        <Route path="/403" element={<Forbidden />} />
        <Route path="/401" element={<Unauthorized />} />
        <Route path="*" element={<Navigate to="/404" />} />
=======
        <Route path="/digistore" element={<MarketPlace />} />
>>>>>>> 47cdda6ed062f7304e903299f7bcf051d38a59f8
      </Routes>
    </BrowserRouter>
    {/* </Provider> */}
  </React.StrictMode>,
);
