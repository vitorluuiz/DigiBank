import { useState } from 'react';
import TextField from '@mui/material/TextField';
import { styled } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { ToastContainer, toast } from 'react-toastify';
import mask from '../../components/mask';
import RedLogo from '../../assets/img/logoVermelha.png';
import passaroLogo from '../../assets/img/passaroLogo.png';
import api from '../../services/api';
import { parseJwt } from '../../services/auth';

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

  function handleChangeMask(event: any) {
    const { value } = event.target;

    setCpf(mask(value));
  }

  const FazerLogin = (event: any) => {
    event.preventDefault();

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
      .catch((resposta) => {
        console.log(resposta);
        toast.error('CPF ou senha incorretos');
      });
  };
  return (
    <div className="backColor">
      <ToastContainer position="top-center" autoClose={1800} />
      {/* <Header type="simples" /> */}
      <main className="mainLogin container">
        <div className="bannerLogin">
          <h1 className="titleLogin">bem-vindo ao DigiBank</h1>
          <p className="textLogin">acesse sua conta e tenha acesso a tudo sobre seu cartão</p>
          <img src={passaroLogo} alt="passaro Logo" />
        </div>
        <div className="formArea">
          <img src={RedLogo} alt="logo vermelha login" />
          <form className="formLogin" onSubmit={(event) => FazerLogin(event)}>
            <div className="double-input">
              <CssTextField
                inputProps={{ maxLength: 14 }}
                id="outlined-basic"
                label="CPF"
                variant="outlined"
                fullWidth
                // className={classes.textField}
                value={cpf}
                // eslint-disable-next-line react/jsx-no-bind
                onChange={handleChangeMask}
                // onChange={(evt) => setCpf(evt.target.value)}
              />
              <CssTextField
                id="outlined-basic"
                label="Senha"
                variant="outlined"
                type="password"
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
    </div>
  );
}

export default Login;
