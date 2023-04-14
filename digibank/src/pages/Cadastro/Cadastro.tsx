import { useState } from 'react';
import TextField from '@mui/material/TextField';
import { styled } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import Logo from '../../assets/img/logoVermelha.png';
// import mask from '../../components/mask';
import api from '../../services/api';
import Footer from '../../components/Footer';

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

  // function handleChangeMask(event: any) {
  //   const { value } = event.target;

  //   setCpf(mask(value));
  // }

  function CadastrarUsuario(event: any) {
    event.preventDefault();

    api
      .post('Usuarios', {
        idUsuario,
        nomeCompleto,
        apelido,
        telefone,
        email,
        senha,
        cpf,
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
      .catch((erro) => console.log(erro));
  }

  return (
    <div>
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
                <CssTextField
                  id="outlined-basic"
                  label="Nome Completo"
                  variant="outlined"
                  type="text"
                  fullWidth
                  defaultValue={nomeCompleto}
                  onChange={(evt) => setNomeCompleto(evt.target.defaultValue)}
                />
                <CssTextField
                  id="outlined-basic"
                  label="Apelido"
                  variant="outlined"
                  type="text"
                  fullWidth
                  defaultValue={apelido}
                  onChange={(evt) => setApelido(evt.target.defaultValue)}
                />
              </div>
              <div className="doubleInput">
                <CssTextField
                  id="outlined-basic"
                  label="Telefone"
                  variant="outlined"
                  type="text"
                  fullWidth
                  defaultValue={telefone}
                  onChange={(evt) => setTelefone(evt.target.defaultValue)}
                />
                <CssTextField
                  id="outlined-basic"
                  label="Email"
                  variant="outlined"
                  type="text"
                  fullWidth
                  defaultValue={email}
                  onChange={(evt) => setEmail(evt.target.defaultValue)}
                />
              </div>
              <div className="doubleInput">
                <CssTextField
                  // inputProps={{ maxLength: 14 }}
                  id="outlined-basic"
                  label="CPF"
                  variant="outlined"
                  fullWidth
                  defaultValue={cpf}
                  // eslint-disable-next-line react/jsx-no-bind
                  // onChange={handleChangeMask}
                  onChange={(evt) => setCpf(evt.target.defaultValue)}
                />
                <CssTextField
                  id="outlined-basic"
                  label="Senha"
                  variant="outlined"
                  type="password"
                  fullWidth
                  defaultValue={senha}
                  onChange={(evt) => setSenha(evt.target.defaultValue)}
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
