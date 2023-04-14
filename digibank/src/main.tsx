import React from 'react';
import ReactDOM from 'react-dom/client';
// import { Provider } from 'react-redux';
import { BrowserRouter, Route, Routes } from 'react-router-dom';

import Login from './pages/Login/Login';
import App from './App';
import Home from './pages/Home/Home';

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    {/* <Provider store={store}> */}
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route path="/landing" element={<App />} />
        <Route path="/" element={<Home />} />
      </Routes>
    </BrowserRouter>
    {/* </Provider> */}
  </React.StrictMode>,
);
