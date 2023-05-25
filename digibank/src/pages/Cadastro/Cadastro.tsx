import { useState } from 'react';
import TextField from '@mui/material/TextField';
import { styled } from '@mui/material';
<<<<<<< HEAD
import { useNavigate } from 'react-router-dom';
import { ToastContainer, toast } from 'react-toastify';
import Logo from '../../assets/img/logoVermelha.png';
=======
import { Link, useNavigate } from 'react-router-dom';
// import Logo from '../../assets/img/logoVermelha.png';
>>>>>>> 47cdda6ed062f7304e903299f7bcf051d38a59f8
import mask from '../../components/mask';
import api from '../../services/api';
import Footer from '../../components/Footer';
import Header from '../../components/Header';

const CssTextField2 = styled(TextField)({
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

export default function Cadastro() {
  const [idUsuario] = useState(0);
  const [nomeCompleto, setNomeCompleto] = useState('');
  const [apelido, setApelido] = useState('');
  const [email, setEmail] = useState('');
  const [senha, setSenha] = useState('');
  const [cpf, setCpf] = useState('');
  const [telefone, setTelefone] = useState('');
  const [digiPoints] = useState(0);
  const [saldo] = useState(0);
  const [rendaFixa] = useState(0);

  const [isLoading, setLoading] = useState<boolean>(false);
  const [errorMessage, setErrorMessage] = useState<string>('');
  const navigate = useNavigate();

  function handleChangeMask(event: any) {
    const { value } = event.target;

    setCpf(mask(value));
  }

  function CadastrarUsuario(event: any) {
    event.preventDefault();

    setLoading(true);

    const CPF = cpf.replaceAll('.', '').replace('-', '');
    api
      .post('Usuarios', {
        idUsuario,
        nomeCompleto,
        apelido,
        telefone,
        email,
        senha,
        CPF,
        digiPoints,
        saldo,
        rendaFixa,
      })
      .then((response) => {
        if (response.status === 201) {
          navigate('/');
        }
      })
<<<<<<< HEAD
      .catch((resposta) => {
        console.log(resposta);
        toast.error('Usuario Não Cadastrado!');
=======
      .catch(() => {
        setErrorMessage('Não foi possível efetuar o cadastro, usuário já existe');
        setLoading(false);
>>>>>>> 47cdda6ed062f7304e903299f7bcf051d38a59f8
      });
  }

  return (
    <div>
<<<<<<< HEAD
      <ToastContainer position="top-center" autoClose={1800} />
=======
      <Header type="simples" />
>>>>>>> 47cdda6ed062f7304e903299f7bcf051d38a59f8
      <main className="container mainCadastro">
        <section className="sectionLeft">
          <div className="boxTextCadastro">
            <h1 className="titleCadastro">o novo banco que vai mudar a sua vida</h1>
            <p className="sloganCadastro">
              Conta digital, cartão de crédito, investimentos e mais: tudo em um só app
            </p>
          </div>
          <Link to="/" className="btnComponent">
            conheça os beneficios
          </Link>
        </section>
        <section className="sectionRight">
          <h2>abra sua conta</h2>
          <form className="formCadastro" onSubmit={(event) => CadastrarUsuario(event)}>
            <div className="boxInputsCadastro">
              <div className="doubleInput">
                <CssTextField2
                  label="Nome Completo"
                  variant="outlined"
                  required
                  type="text"
                  fullWidth
                  value={nomeCompleto}
                  onChange={(evt) => setNomeCompleto(evt.target.value)}
                />
                <CssTextField2
                  label="Apelido"
                  variant="outlined"
                  required
                  type="text"
                  fullWidth
                  value={apelido}
                  onChange={(evt) => setApelido(evt.target.value)}
                />
              </div>
              <div className="doubleInput">
                <CssTextField2
                  label="Telefone"
                  variant="outlined"
                  required
                  type="text"
                  fullWidth
                  value={telefone}
                  onChange={(evt) => setTelefone(evt.target.value)}
                />
                <CssTextField2
                  label="Email"
                  variant="outlined"
                  required
                  type="email"
                  fullWidth
                  value={email}
                  onChange={(evt) => setEmail(evt.target.value)}
                />
              </div>
              <div className="doubleInput">
                <CssTextField2
                  inputProps={{ maxLength: 14, minLength: 14 }}
                  label="CPF"
                  required
                  variant="outlined"
                  fullWidth
                  value={cpf}
                  // eslint-disable-next-line react/jsx-no-bind
                  onChange={handleChangeMask}
                  // onChange={(evt) => setCpf(evt.target.value)}
                />
                <CssTextField2
                  label="Senha"
                  variant="outlined"
                  required
                  type="password"
                  fullWidth
                  value={senha}
                  onChange={(evt) => setSenha(evt.target.value)}
                />
              </div>
              <span>{errorMessage}</span>
            </div>
            <button disabled={isLoading} type="submit" className="btnComponent">
              cadastrar
            </button>
          </form>
        </section>
      </main>
      <Footer />
    </div>
  );
}
