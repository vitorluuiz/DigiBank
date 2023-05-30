import React, { useEffect, useState } from 'react';
import Pagination from '@mui/material/Pagination';
// import { useNavigate } from 'react-router-dom';
import { ThemeProvider, createTheme } from '@mui/material/styles';
import api from '../../services/api';
import { ExtratoProps } from '../../@types/Extrato';
import { parseJwt } from '../../services/auth';
import Header from '../../components/Header';
import Footer from '../../components/Footer';
import SideBar from '../../components/SideBar';

const theme = createTheme({
  palette: {
    primary: {
      main: '#c20014',
    },
  },
});

export default function Extratos() {
  //   const navigate = useNavigate();
  const [listaExtrato, setListaExtrato] = useState<ExtratoProps[]>([]);
  const [pagina, setPagina] = useState(1);
  const [qntItens] = useState(8);
  const [transacoesCount, setTransacoesCount] = useState(0);

  function ListarTransacao(pag: number) {
    api
      .get(`Transacoes/Listar/Minhas/${parseJwt().role}/${pag}/${qntItens}`, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('usuario-login-auth')}`,
        },
      })

      .then((resposta) => {
        if (resposta.status === 200) {
          setListaExtrato(resposta.data.transacoes);
          // console.log(respost.daa);
          console.log(resposta.headers);
          setTransacoesCount(resposta.data.transacoesCount);
        }
      })

      .catch((erro) => console.log(erro));
  }

  const handlePageChange = (event: React.ChangeEvent<unknown>, value: number) => {
    setPagina(value);
    ListarTransacao(value);
  };
  useEffect(() => {
    ListarTransacao(1);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);
  return (
    <div>
      <Header type="" />
      <main className="container mainExtratos">
        <h1>visualizacao do extrato</h1>
        {/* <svg width="100" height="100">
          <polygon points="10,30 30,30 40,10 10,10" fill="#c20014" />
        </svg> */}
        <div className="headerListagem">
          <p>Filtrar</p>
          <p>Buscar</p>
          <p>Ultimo Mês</p>
        </div>
        <div className="listExtrato">
          {listaExtrato.map((event) => (
            <div className="bodyListagem">
              <p>
                {event.descricao}
                {/* Transferencia de {event.nomePagane} para {event.nomeRecebente} */}
              </p>
              <div className="data-valor">
                <p>
                  {' '}
                  {new Date(event.dataTransacao).toLocaleDateString('pt-BR', {
                    day: '2-digit',
                    month: '2-digit',
                    year: 'numeric',
                  })}
                </p>
                <p
                  style={{
                    color:
                      event.idUsuarioPagante.toString() === parseJwt().role ? '#E40A0A' : '#12FE0D',
                  }}
                >
                  {` `}
                  {event.valor.toLocaleString('pt-BR', {
                    style: 'currency',
                    currency: 'BRL',
                  })}
                </p>
              </div>
            </div>
          ))}
        </div>
        <div className="paginacaoExtrato">
          <ThemeProvider theme={theme}>
            <Pagination
              page={pagina}
              onChange={handlePageChange}
              count={Math.ceil(transacoesCount / qntItens)}
              shape="rounded"
              // variant="outlined"
              color="primary"
              siblingCount={1}
            />
          </ThemeProvider>
        </div>
        <SideBar />
      </main>
      <Footer />
    </div>
  );
}
