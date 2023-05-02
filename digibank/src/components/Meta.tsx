// import React from 'react';

import { useEffect, useState } from 'react';
// import Graphic from './Grafico';
import ApexCharts from 'react-apexcharts';
import api from '../services/api';
import { parseJwt } from '../services/auth';

function Meta() {
  const [listaMetas, setListaMetas] = useState([]);
  // const [titulo, setTitulo] = useState();
  // const [valorMeta, setValorMeta] = useState();
  // const [Arrecadado, setrArrecadado] = useState();

  function ListarMeta() {
    api
      .get(`Metas/Minhas/${parseJwt().role}`, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('usuario-login-auth')}`,
        },
      })

      .then((resposta) => {
        if (resposta.status === 200) {
          setListaMetas(resposta.data);
          console.log(resposta);
        }
      })

      .catch((erro) => console.log(erro));
  }

  useEffect(() => {
    ListarMeta();
  }, []);

  return (
    <div className="box-meta">
      {listaMetas.map((event: any) => (
        <div className="suport-meta">
          <h2>{event.titulo}</h2>
          <div className="suport-infos">
            <div className="spans-meta">
              <span>
                {event.arrecadado.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}{' '}
              </span>
              <span>guardados</span>
            </div>
            {/* <Graphic /> */}
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
                      margin: 75,
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
                    opacityTo: 1,
                    stops: [0, 50, 53, 91],
                  },
                },
              }}
              series={[Math.round((event.arrecadado * 100) / event.valorMeta)]}
              type="radialBar"
              height={250}
              width={250}
            />
            <div className="spans-meta">
              <span>
                {event.valorMeta.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}{' '}
              </span>
              <span>meta</span>
            </div>
          </div>
        </div>
      ))}
    </div>
  );
}

export default Meta;
