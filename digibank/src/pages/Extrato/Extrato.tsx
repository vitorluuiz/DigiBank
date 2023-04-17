import React, { useEffect, useState } from 'react';
// import { useNavigate } from 'react-router-dom';
import api from '../../services/api';
import { ExtratoProps } from '../../@types/Extrato';
import { parseJwt } from '../../services/auth';

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
    <div className="container mainExtratos">
      <h1>visualizacao do extrato</h1>
      <div className="headerListagem">
        <p> </p>
        <p>Buscar</p>
        <p>Ultimo Mês</p>
      </div>
      {listaExtrato.map((event) => (
        <div className="">
          <p>Transferencia de {event.nomePagante}</p>
          <p>
            {' '}
            {new Date(event.dataTransacao).toLocaleDateString('pt-BR', {
              day: '2-digit',
              month: '2-digit',
              year: 'numeric',
            })}
          </p>
          <p>
            {` `}
            {event.valor.toLocaleString('pt-BR', {
              style: 'currency',
              currency: 'BRL',
            })}
          </p>
        </div>
      ))}
    </div>
  );
}
