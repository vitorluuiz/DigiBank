// import { Button } from '@mui/material';
import { useEffect, useState } from 'react';
import ApexCharts from 'react-apexcharts';
import { ToastContainer } from 'react-toastify';
import { Link, useParams } from 'react-router-dom';
import 'react-toastify/dist/ReactToastify.css';
import Header from '../../components/Header';
import Footer from '../../components/Footer';
import SideBar from '../../components/SideBar';
// import Meta from '../../components/Meta';
import api from '../../services/api';
import { MetaProps } from '../../@types/Meta';
// import { parseJwt } from '../../services/auth';
// import { parseJwt } from '../../services/auth';

function MetaUnica() {
  const { idMeta } = useParams();
  const [meta, setMeta] = useState<MetaProps>({
    idMeta: 0,
    titulo: '',
    valorMeta: 0,
    arrecadado: 0,
  });
  function ListarMeta() {
    console.log(idMeta);
    api
      .get(`Metas/${idMeta}`, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('usuario-login-auth')}`,
        },
      })

      .then((resposta) => {
        if (resposta.status === 200) {
          setMeta(resposta.data);
          console.log(resposta);
        }
      })

      .catch((erro) => console.log(erro));
  }

  useEffect(ListarMeta, [idMeta]);

  return (
    <div>
      <ToastContainer position="top-center" autoClose={1800} />
      <Header type="" />
      <main id="metaUnica" className="container">
        <section className="leftSection">
          <div className="suport-metaUnica">
            <h2>{meta?.titulo}</h2>
            <div className="suport-infos">
              <div className="spans-meta">
                <span>
                  {meta?.arrecadado.toLocaleString('pt-BR', {
                    style: 'currency',
                    currency: 'BRL',
                  })}{' '}
                </span>
                <span>guardados</span>
              </div>
              <ApexCharts
                options={{
                  chart: {
                    type: 'radialBar',
                  },
                  plotOptions: {
                    radialBar: {
                      startAngle: -90,
                      endAngle: 90,
                      hollow: {
                        margin: 65,
                        size: '60%',
                      },
                      dataLabels: {
                        value: {
                          fontSize: '24px',
                        },
                      },
                    },
                  },
                  colors: ['#c20004'],
                  labels: ['Concluido'],
                  fill: {
                    colors: ['#c20004', '#f20519'],
                    type: 'gradient',
                    gradient: {
                      shade: 'light',
                      shadeIntensity: 0.4,
                      inverseColors: false,
                      opacityFrom: 1,
                      stops: [0, 50, 91],
                    },
                  },
                }}
                // eslint-disable-next-line no-unsafe-optional-chaining
                series={[Math.round((meta?.arrecadado * 1000) / meta?.valorMeta) / 10]}
                type="radialBar"
                height={250}
                width={250}
              />
              <div className="spans-meta">
                <span>
                  {meta.valorMeta.toLocaleString('pt-BR', {
                    style: 'currency',
                    currency: 'BRL',
                  })}{' '}
                </span>
                <span>meta</span>
              </div>
            </div>
          </div>
        </section>
        <section className="rightSection">
          <div className="supportButtons">
            <button className="btnsMeta">excluir meta</button>
            <button className="btnsMetaWhite"> Adicionar Saldo</button>
            <button className="btnsMetaWhite">Alterar Meta</button>
            <Link to="/metas" className="btnsMetaWhite">
              Voltar para metas
            </Link>
          </div>
        </section>
        <SideBar />
      </main>
      <Footer />
    </div>
  );
}

export default MetaUnica;
