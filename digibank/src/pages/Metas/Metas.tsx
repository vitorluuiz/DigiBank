import { useEffect, useReducer, useState } from 'react';
import ApexCharts from 'react-apexcharts';
import 'react-toastify/dist/ReactToastify.css';
import { Link } from 'react-router-dom';
import Header from '../../components/Header';
import Footer from '../../components/Footer';
import SideBar from '../../components/SideBar';
import ModalMeta from '../../components/Metas/ModalAddMeta';
import api from '../../services/api';
import reducer from '../../services/reducer';
import { parseJwt } from '../../services/auth';
import Empty from '../../components/Empty';
import CustomSnackbar from '../../assets/styledComponents/snackBar';
import { useSnackBar } from '../../services/snackBarProvider';

function Metas() {
  const [listaMetas, setListaMetas] = useState([]);
  const updateStage = {
    count: 0,
  };

  const { currentMessage, handleCloseSnackBar } = useSnackBar();

  const [updates, dispatch] = useReducer(reducer, updateStage);
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
        }
      })
      .catch((erro) => console.warn(erro));
  }

  useEffect(() => {
    ListarMeta();
  }, [updates.count]);
  return (
    <div>
      <CustomSnackbar message={currentMessage} onClose={handleCloseSnackBar} />
      <Header type="" />
      <main id="metas" className="container">
        <div className="header-page">
          <h1>Minhas metas</h1>
          <ModalMeta dispatch={dispatch} />
        </div>
        <section className="meta-list box-meta">
          {listaMetas.length !== 0 ? (
            listaMetas.map((event: any) => (
              <Link to={`/meta/${event.idMeta}`} key={event.idMeta} className="suport-meta">
                <h2>{event.titulo}</h2>
                <div className="suport-infos">
                  <div className="spans-meta">
                    <span>
                      {event.arrecadado.toLocaleString('pt-BR', {
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
                          stops: [0, 50, 91],
                        },
                      },
                    }}
                    series={[Math.round((event.arrecadado * 1000) / event.valorMeta) / 10]}
                    type="radialBar"
                    height={250}
                    width={250}
                  />
                  <div className="spans-meta">
                    <span>
                      {event.valorMeta.toLocaleString('pt-BR', {
                        style: 'currency',
                        currency: 'BRL',
                      })}{' '}
                    </span>
                    <span>meta</span>
                  </div>
                </div>
              </Link>
            ))
          ) : (
            <Empty type="metas" />
          )}
        </section>
        <SideBar />
      </main>
      <Footer />
    </div>
  );
}

export default Metas;
