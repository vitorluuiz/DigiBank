import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { parseJwt } from '../../services/auth';
import api from '../../services/api';

function Login() {
  const [cpf, setCpf] = useState('');
  const [senha, setSenha] = useState('');
  const navigate = useNavigate();

  const FazerLogin = (event: any) => {
    event.preventDefault();

    api
      .post('Login/Logar', {
        cpf,
        senha,
      })
      .then((resposta) => {
        if (resposta.status === 200) {
          localStorage.setItem('usuario-login-auth', resposta.data.token);
          if (parseJwt().role === '2') {
            navigate('/');
          } else if (parseJwt().role === '1') {
            navigate('/');
          }
        }
      })
      .catch((resposta) => {
        console.log(resposta);
      });
  };
  return (
    <div>
      <p>Login</p>
      <form onSubmit={(event) => FazerLogin(event)}>
        <input value={cpf} onChange={(evt) => setCpf(evt.target.value)} />
        <input value={senha} onChange={(evt) => setSenha(evt.target.value)} />
        <button type="submit">Entrar</button>
      </form>
    </div>
  );
}

export default Login;
