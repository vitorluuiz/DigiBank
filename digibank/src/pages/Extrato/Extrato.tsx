import React, { useEffect, useState } from 'react';
// import { useNavigate } from 'react-router-dom';
import api from '../../services/api';
import { ExtratoProps } from '../../@types/Extrato';
import { parseJwt } from '../../services/auth';
import Header from '../../components/Header';
import Footer from '../../components/Footer';
import SideBar from '../../components/SideBar';

export default function Extratos() {
  //   const navigate = useNavigate();
  const [listaExtrato, setListaExtrato] = useState<ExtratoProps[]>([]);
  const [pagina] = useState(1);
  const [qntItens] = useState(10);

  function ListarTransacao() {
    api
      .get(`Transacoes/Listar/Minhas/${parseJwt().role}/${pagina}/${qntItens}`, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('usuario-login-auth')}`,
        },
      })

      .then((resposta) => {
        if (resposta.status === 200) {
          setListaExtrato(resposta.data);
          console.log(resposta);
        }
      })

      .catch((erro) => console.log(erro));
  }

  useEffect(() => {
    ListarTransacao();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);
  return (
    <div>
      <Header type="" />
      <main className="container mainExtratos">
        <h1>visualizacao do extrato</h1>
        <div className="headerListagem">
          <p>Filtrar</p>
          <p>Buscar</p>
          <p>Ultimo Mês</p>
        </div>
        <div className="listExtrato">
          {listaExtrato.map((event) => (
            <div className="bodyListagem">
              <p>
                Transferencia de {event.nomePagante} para {event.nomeRecebente}
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
        <SideBar />
      </main>
      <Footer />
    </div>
  );
}
