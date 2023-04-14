import React, { useState } from 'react';
import TextField from '@mui/material/TextField';
import { styled } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { parseJwt } from '../../services/auth';
import RedLogo from '../../assets/img/logoVermelha.png';
import api from '../../services/api';
import Header from '../../components/Header';

const CssTextField = styled(TextField)({
  '& label.Mui-focused': {
    color: '#b3b3b3',
  },
  '& .MuiInput-underline:after': {
    borderBottomColor: '#b3b3b3',
  },
  '& .MuiOutlinedInput-root': {
    '& fieldset': {
      borderColor: '#b3b3b3',
    },
    '&:hover fieldset': {
      borderColor: '#b3b3b3',
    },
    '&.Mui-focused fieldset': {
      borderColor: '#b3b3b3',
    },
  },
});

function Login() {
  const [cpf, setCpf] = useState('');
  const [senha, setSenha] = useState('');
  const navigate = useNavigate();
  // eslint-disable-next-line no-bitwise

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
    <body>
      <Header type="simples" />
      <main className="mainLogin container">
        <div className="bannerLogin">
          <img src={RedLogo} alt="logo header vermelha" />
        </div>
        <div className="formArea">
          <h1>login</h1>
          <p className="textLogin">acesse sua conta e tenha acesso a tudo sobre seu cartão</p>
          <form className="formLogin" onSubmit={(event) => FazerLogin(event)}>
            <div className="double-input">
              <CssTextField
                id="outlined-basic"
                label="CPF"
                variant="outlined"
                required
                fullWidth
                // className={classes.textField}
                value={cpf}
                onChange={(evt) => setCpf(evt.target.value)}
              />
              <CssTextField
                id="outlined-basic"
                label="Senha"
                variant="outlined"
                required
                fullWidth
                // className="inputLogin"
                value={senha}
                onChange={(evt) => setSenha(evt.target.value)}
              />
            </div>
            <button className="btnLogin" type="submit">
              Entrar
            </button>
          </form>
        </div>
      </main>
    </body>
  );
}

export default Login;
