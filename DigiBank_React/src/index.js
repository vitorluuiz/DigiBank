import React from 'react';
import ReactDOM from 'react-dom';
// import './index.css';
import {
  Route,
  BrowserRouter as Router, Redirect,
  Switch
} from 'react-router-dom';
import './index.css';

import reportWebVitals from './reportWebVitals';
import { parseJwt, usuarioAutenticado } from './services/auth';

const routing = (
  <BrowserRouter>
    <Routes>
        <Route exact path="/" element={<TelaAcesso/>} />
    </Routes>
  </BrowserRouter>
)

ReactDOM.render(
  routing,
  document.getElementById('root')
);

reportWebVitals();
