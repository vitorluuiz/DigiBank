import { useNavigate } from 'react-router-dom';
import React, { useEffect, useState } from 'react';
import mask from '../../components/mask';
import RedLogo from '../../assets/img/logoVermelha.png';
import showSenhaTrue from '../../assets/img/showSenhaTrue.svg';
import showSenhaFalse from '../../assets/img/showSenhaFalse.svg';
import passaroLogo from '../../assets/img/passaroLogo.png';
import api from '../../services/api';
import { CssTextField } from '../../assets/styledComponents/input';

function Login() {
  const [cpf, setCpf] = useState('');
  const [senha, setSenha] = useState('');
  const [saveLogin, setSaveLogin] = useState<boolean>(false);
  const [isLoading, setLoading] = useState<boolean>(false);
  const [errorMessage, setErrorMessage] = useState<string>('');
  const [showPassword, setShowPassword] = useState<boolean>(false);
  const navigate = useNavigate();

  const refreshToken = () => {
    api('Login/RefreshToken')
      .then((response) => {
        if (response.status === 200) {
          localStorage.setItem('usuario-login-auth', response.data.token);
          navigate('/home');
        }
      })
      .catch((error) => console.log(error));
  };

  const handleChangeSaveLogin = () => {
    if (saveLogin) {
      setSaveLogin(false);
    } else {
      setSaveLogin(true);
    }
  };

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
          if (saveLogin) {
            localStorage.setItem('save-login?', 'true');
          } else {
            localStorage.setItem('save-login?', 'false');
          }
          navigate('/home');
        }
      })
      .catch(() => {
        setErrorMessage('Usuário ou senha inválidos');
        setLoading(false);
      });
  };

  useEffect(() => {
    if (localStorage.getItem('save-login?') === 'true') {
      refreshToken();
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <div className="backColor">
      <main className="mainLogin container">
        <div className="bannerLogin">
          <h1 className="titleLogin">bem-vindo ao DigiBank</h1>
          <p className="textLogin">Acesse sua conta e tenha o controle de suas finanças</p>
          <img src={passaroLogo} alt="passaro Logo" />
        </div>
        <div className="formArea">
          <img src={RedLogo} alt="logo vermelha login" className="logoForm" />
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
              {/* <CssTextField
                label="Senha"
                variant="outlined"
                required
                type="password"
                fullWidth
                // className="inputLogin"
                value={senha}
                onChange={(evt) => setSenha(evt.target.value)}
              /> */}
              <CssTextField
                label="Senha"
                variant="outlined"
                className="inputSenha"
                required
                type={showPassword ? 'text' : 'password'}
                fullWidth
                value={senha}
                onChange={(evt) => setSenha(evt.target.value)}
                InputProps={{
                  endAdornment: (
                    <button
                      type="button"
                      className="show-password-button"
                      onClick={() => setShowPassword(!showPassword)}
                    >
                      {showPassword ? (
                        <img src={showSenhaTrue} alt="ver senha on" />
                      ) : (
                        <img src={showSenhaFalse} alt="ver senha off" />
                      )}
                    </button>
                  ),
                }}
              />
            </div>
            <div className="support-save-pwd">
              <label htmlFor="save-pwd">
                <input id="save-pwd" type="checkbox" onChange={() => handleChangeSaveLogin()} />
                Manter conectado
              </label>
            </div>
            <span>{errorMessage}</span>
            <button disabled={isLoading} className="btnComponent btnLogin" type="submit">
              Entrar
            </button>
          </form>
        </div>
      </main>
    </div>
  );
}

export default Login;
