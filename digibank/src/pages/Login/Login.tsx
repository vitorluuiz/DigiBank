<<<<<<< HEAD
import { useState } from 'react';
import TextField from '@mui/material/TextField';
import { styled } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { ToastContainer, toast } from 'react-toastify';
=======
import React, { useState } from 'react';

import { useNavigate } from 'react-router-dom';
import 'react-toastify/dist/ReactToastify.css';
>>>>>>> 47cdda6ed062f7304e903299f7bcf051d38a59f8
import mask from '../../components/mask';
import RedLogo from '../../assets/img/logoVermelha.png';
import passaroLogo from '../../assets/img/passaroLogo.png';
import api from '../../services/api';
import { parseJwt } from '../../services/auth';
<<<<<<< HEAD

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
=======
import { CssTextField } from '../../assets/styledComponents/input';
>>>>>>> 47cdda6ed062f7304e903299f7bcf051d38a59f8

function Login() {
  const [cpf, setCpf] = useState('');
  const [senha, setSenha] = useState('');
  const [isLoading, setLoading] = useState<boolean>(false);
  const [errorMessage, setErrorMessage] = useState<string>('');
  const navigate = useNavigate();

  function handleChangeMask(event: any) {
    const { value } = event.target;

    setCpf(mask(value));
  }

  const FazerLogin = (event: any) => {
    event.preventDefault();

    setLoading(true);

    const CPF = cpf.replaceAll('.', '').replace('-', '');
    api
      .post('Login/Logar', {
        CPF,
        senha,
      })
      .then((resposta) => {
        if (resposta.status === 200) {
          localStorage.setItem('usuario-login-auth', resposta.data.token);
          if (parseJwt().role !== '1') {
            navigate('/home');
          } else if (parseJwt().role === '1') {
            navigate('/home');
          }
        }
      })
<<<<<<< HEAD
      .catch((resposta) => {
        console.log(resposta);
        toast.error('CPF ou senha incorretos');
=======
      .catch(() => {
        setErrorMessage('Usuário ou senha inválidos');
        setLoading(false);
>>>>>>> 47cdda6ed062f7304e903299f7bcf051d38a59f8
      });
  };
  return (
    <div className="backColor">
      <ToastContainer position="top-center" autoClose={1800} />
      {/* <Header type="simples" /> */}
      <main className="mainLogin container">
        <div className="bannerLogin">
          <h1 className="titleLogin">bem-vindo ao DigiBank</h1>
          <p className="textLogin">Acesse sua conta e tenha o controle de suas finanças</p>
          <img src={passaroLogo} alt="passaro Logo" />
        </div>
        <div className="formArea">
          <img src={RedLogo} alt="logo vermelha login" />
          <form className="formLogin" onSubmit={(event) => FazerLogin(event)}>
            <div className="double-input">
              <CssTextField
                inputProps={{ maxLength: 14, minLength: 14 }}
                label="CPF"
                required
                variant="outlined"
                fullWidth
                // className={classes.textField}
                value={cpf}
                // eslint-disable-next-line react/jsx-no-bind
                onChange={handleChangeMask}
                // onChange={(evt) => setCpf(evt.target.value)}
              />
              <CssTextField
                label="Senha"
                variant="outlined"
                required
                type="password"
                fullWidth
                // className="inputLogin"
                value={senha}
                onChange={(evt) => setSenha(evt.target.value)}
              />
            </div>
            <span>{errorMessage}</span>
            <button disabled={isLoading} className="btnComponent" type="submit">
              Entrar
            </button>
          </form>
        </div>
      </main>
    </div>
  );
}

export default Login;
