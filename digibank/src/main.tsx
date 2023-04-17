import React from 'react';
import ReactDOM from 'react-dom/client';
// import { Provider } from 'react-redux';
import { BrowserRouter, Route, Routes } from 'react-router-dom';

import App from './App';
import Login from './pages/Login/Login';
import Cadastro from './pages/Cadastro/Cadastro';
import Extratos from './pages/Extrato/Extrato';

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    {/* <Provider store={store}> */}
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<App />} />
        <Route path="/login" element={<Login />} />
        <Route path="/cadastro" element={<Cadastro />} />
        <Route path="/extrato" element={<Extratos />} />
      </Routes>
    </BrowserRouter>
    {/* </Provider> */}
  </React.StrictMode>,
);
