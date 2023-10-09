import { useState } from 'react';
import { NumericFormat } from 'react-number-format';
import { Link, useNavigate } from 'react-router-dom';
import showSenhaTrue from '../../assets/img/showSenhaTrue.svg';
import showSenhaFalse from '../../assets/img/showSenhaFalse.svg';
import mask from '../../components/mask';
import api from '../../services/api';
import Footer from '../../components/Footer';
import Header from '../../components/Header';
import { StyledTextField } from '../../assets/styledComponents/input';
import { formatCurrency } from '../../assets/styledComponents/DolarInput';

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

export function NumberFormatCustom(props: any) {
  const { inputRef, onChange } = props;

  return (
    <NumericFormat
      getInputRef={inputRef}
      onValueChange={(value) => {
        onChange({
          target: {
            // eslint-disable-next-line react/destructuring-assignment
            name: props.name,
            value: value.value,
          },
        });
      }}
      thousandSeparator="."
      decimalSeparator=","
      prefix="R$ "
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
  const [rendaFixa, setRendaFixa] = useState<string>('');
  const [showPassword, setShowPassword] = useState<boolean>(false);
  const [isLoading, setLoading] = useState<boolean>(false);
  const [errorMessage, setErrorMessage] = useState<string>('');
  const navigate = useNavigate();

  function handleChangeMask(event: any) {
    const { value } = event.target;

    setCpf(mask(value));
  }

  function handleChangeTelefone(event: any) {
    const { value } = event.target;
    const telefoneFormatado = formatarTelefone(value);
    setTelefone(telefoneFormatado);
  }

  const handleChangeRendaFixa = (newValue: string) => setRendaFixa(formatCurrency(newValue));

  function CadastrarUsuario(event: any) {
    event.preventDefault();

    setLoading(true);

    const CPF = cpf.replaceAll('.', '').replace('-', '');
    const TELEFONE = telefone.replace('(', '').replace(')', '').replace('-', '').replace(' ', '');
    api
      .post('Usuarios', {
        idUsuario,
        nomeCompleto,
        apelido,
        TELEFONE,
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
                <StyledTextField
                  label="Nome Completo"
                  variant="outlined"
                  required
                  type="text"
                  fullWidth
                  value={nomeCompleto}
                  onChange={(evt) => setNomeCompleto(evt.target.value)}
                />
                <StyledTextField
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
                <StyledTextField
                  label="Telefone"
                  variant="outlined"
                  required
                  type="text"
                  fullWidth
                  value={telefone}
                  // eslint-disable-next-line react/jsx-no-bind
                  onChange={handleChangeTelefone}
                />
                <StyledTextField
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
                <StyledTextField
                  inputProps={{ maxLength: 14, minLength: 14 }}
                  label="CPF"
                  required
                  variant="outlined"
                  fullWidth
                  value={cpf}
                  // eslint-disable-next-line react/jsx-no-bind
                  onChange={handleChangeMask}
                />
                <StyledTextField
                  label="Senha"
                  variant="outlined"
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
              <div className="doubleInput">
                <StyledTextField
                  label="Renda Fixa"
                  required
                  variant="outlined"
                  fullWidth
                  value={rendaFixa}
                  onChange={(evt) => handleChangeRendaFixa(evt.target.value)}
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
