import { useState } from 'react';
import TextField from '@mui/material/TextField';
import { styled } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { ToastContainer, toast } from 'react-toastify';
import Logo from '../../assets/img/logoVermelha.png';
import mask from '../../components/mask';
import api from '../../services/api';
import Footer from '../../components/Footer';

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
  const navigate = useNavigate();

  function handleChangeMask(event: any) {
    const { value } = event.target;

    setCpf(mask(value));
  }

  function CadastrarUsuario(event: any) {
    event.preventDefault();

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
          console.log('usuário cadastrado!');
          navigate('/');
        }
      })
      .catch((resposta) => {
        console.log(resposta);
        toast.error('Usuario Não Cadastrado!');
      });
  }

  return (
    <div>
      <ToastContainer position="top-center" autoClose={1800} />
      <main className="container mainCadastro">
        <section className="sectionLeft">
          <img className="logoCadastro" src={Logo} alt="Logo Vermelha" />
          <div className="boxTextCadastro">
            <h1 className="titleCadastro">o novo banco que vai mudar a sua vida</h1>
            <p className="sloganCadastro">
              Conta digital, cartão de crédito, investimentos e mais: tudo em um só app
            </p>
          </div>
          <a href="/" className="btnCadastro">
            conheça os beneficios
          </a>
        </section>
        <section className="sectionRight">
          <h2>abra sua conta</h2>
          <form className="formCadastro" onSubmit={(event) => CadastrarUsuario(event)}>
            <div className="boxInputsCadastro">
              <div className="doubleInput">
                <CssTextField2
                  id="outlined-basic"
                  label="Nome Completo"
                  variant="outlined"
                  type="text"
                  fullWidth
                  value={nomeCompleto}
                  onChange={(evt) => setNomeCompleto(evt.target.value)}
                />
                <CssTextField2
                  id="outlined-basic"
                  label="Apelido"
                  variant="outlined"
                  type="text"
                  fullWidth
                  defaultValue={apelido}
                  onChange={(evt) => setApelido(evt.target.value)}
                />
              </div>
              <div className="doubleInput">
                <CssTextField2
                  id="outlined-basic"
                  label="Telefone"
                  variant="outlined"
                  type="text"
                  fullWidth
                  defaultValue={telefone}
                  onChange={(evt) => setTelefone(evt.target.value)}
                />
                <CssTextField2
                  id="outlined-basic"
                  label="Email"
                  variant="outlined"
                  type="text"
                  fullWidth
                  defaultValue={email}
                  onChange={(evt) => setEmail(evt.target.value)}
                />
              </div>
              <div className="doubleInput">
                <CssTextField2
                  inputProps={{ maxLength: 14 }}
                  id="outlined-basic"
                  label="CPF"
                  variant="outlined"
                  fullWidth
                  value={cpf}
                  // eslint-disable-next-line react/jsx-no-bind
                  onChange={handleChangeMask}
                  // onChange={(evt) => setCpf(evt.target.value)}
                />
                <CssTextField2
                  id="outlined-basic"
                  label="Senha"
                  variant="outlined"
                  type="password"
                  fullWidth
                  defaultValue={senha}
                  onChange={(evt) => setSenha(evt.target.value)}
                />
              </div>
            </div>
            <button type="submit" className="btnCadastro">
              cadastrar
            </button>
          </form>
        </section>
      </main>
      <Footer />
    </div>
  );
}
