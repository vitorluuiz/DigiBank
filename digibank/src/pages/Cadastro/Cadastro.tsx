import { useState } from 'react';
import TextField from '@mui/material/TextField';
import { styled } from '@mui/material';
import { NumericFormat } from 'react-number-format';
import { Link, useNavigate } from 'react-router-dom';
// import Logo from '../../assets/img/logoVermelha.png';
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

function NumberFormatCustom(props: any) {
  const { inputRef, onChange } = props;

  return (
    <NumericFormat
      getInputRef={inputRef}
      onValueChange={(rendaFixa) => {
        onChange({
          target: {
            // eslint-disable-next-line react/destructuring-assignment
            name: props.name,
            value: rendaFixa.value,
          },
        });
      }}
      thousandSeparator=","
      decimalSeparator="."
      prefix="R$ "
      // isNumericString
    />
  );
}

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
  const [rendaFixa, setRendaFixa] = useState(0);

  const [isLoading, setLoading] = useState<boolean>(false);
  const [errorMessage, setErrorMessage] = useState<string>('');
  const navigate = useNavigate();

  function handleChangeMask(event: any) {
    const { value } = event.target;

    setCpf(mask(value));
  }

  function formatarTelefone(numero: any) {
    const celular = numero.replace(/\D/g, ''); // Remove todos os caracteres não numéricos do telefone

    let telefoneFormatado = '';

    if (celular.length <= 2) {
      telefoneFormatado = celular;
    } else if (celular.length <= 6) {
      telefoneFormatado = `(${celular.slice(0, 2)}) ${celular.slice(2)}`;
    } else if (celular.length <= 10) {
      telefoneFormatado = `(${celular.slice(0, 2)}) ${celular.slice(2, 6)}-${celular.slice(6)}`;
    } else {
      telefoneFormatado = `(${celular.slice(0, 2)}) ${celular.slice(2, 7)}-${celular.slice(7, 11)}`;
    }

    return telefoneFormatado;
  }

  function handleChangeTelefone(event: any) {
    const { value } = event.target;
    const telefoneFormatado = formatarTelefone(value);
    setTelefone(telefoneFormatado);
  }

  function CadastrarUsuario(event: any) {
    event.preventDefault();

    setLoading(true);

    const cpfFormat = cpf.replaceAll('.', '').replace('-', '');
    const telefoneFormat = telefone
      .replace('(', '')
      .replace(')', '')
      .replace('-', '')
      .replace(' ', '');
    api
      .post('Usuarios', {
        idUsuario,
        nomeCompleto,
        apelido,
        telefoneFormat,
        email,
        senha,
        cpfFormat,
        digiPoints,
        saldo,
        rendaFixa,
      })
      .then((response) => {
        if (response.status === 201) {
          navigate('/');
        }
      })
      .catch(() => {
        setErrorMessage('Não foi possível efetuar o cadastro, usuário já existe');
        setLoading(false);
      });
  }

  return (
    <div>
      <Header type="simples" />
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
                  // eslint-disable-next-line react/jsx-no-bind
                  onChange={handleChangeTelefone}
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
              <div className="doubleInput">
                <CssTextField2
                  label="Renda Fixa"
                  required
                  variant="outlined"
                  fullWidth
                  type="number"
                  value={rendaFixa}
                  onChange={(evt) => setRendaFixa(parseFloat(evt.target.value))}
                  InputProps={{
                    inputComponent: NumberFormatCustom,
                  }}
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
