import { useEffect, useReducer, useState } from 'react';
import ApexCharts from 'react-apexcharts';
import TextField from '@mui/material/TextField';
import { styled } from '@mui/material';
import { ToastContainer, toast } from 'react-toastify';
import { Link, useParams, useNavigate } from 'react-router-dom';
import Dialog from '@mui/material/Dialog';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import DialogTitle from '@mui/material/DialogTitle';
import 'react-toastify/dist/ReactToastify.css';
import Header from '../../components/Header';
import Footer from '../../components/Footer';
import SideBar from '../../components/SideBar';
import api from '../../services/api';
import { MetaProps } from '../../@types/Meta';
import reducer from '../../services/reducer';
import ModalAltMeta from '../../components/Metas/ModalAltMeta';

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

function MetaUnica() {
  const { idMeta } = useParams();
  const [open, setOpen] = useState(false);
  const [amount, setAmount] = useState<number>();
  const navigate = useNavigate();
  const [meta, setMeta] = useState<MetaProps>({
    idMeta: 0,
    titulo: '',
    valorMeta: 0,
    arrecadado: 0,
  });
  const handleClickOpen = () => {
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };
  const updateStage = {
    count: 0,
  };

  const [updates, dispatch] = useReducer(reducer, updateStage);
  function ListarMeta() {
    // console.log(idMeta);
    api
      .get(`Metas/${idMeta}`, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('usuario-login-auth')}`,
        },
      })

      .then((resposta) => {
        if (resposta.status === 200) {
          setMeta(resposta.data);
          // toast.success('Meta Excluida!');
          console.log(resposta);
          // dispatch({ type: 'update' });
        }
      })

      .catch((resposta) => {
        console.log(resposta);
        // toast.error('Erro ao Excluir Meta!');
      });
  }

  function ExcluirMeta() {
    api
      .delete(`Metas/${idMeta}`, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('usuario-login-auth')}`,
        },
      })

      .then((resposta) => {
        if (resposta.status === 200) {
          toast.success('Meta Excluida!');
          console.log('deletou');
          setTimeout(() => {
            navigate('/metas');
          }, 2200);
        }
      })

      .catch((resposta) => {
        console.log(resposta);
        toast.error('Erro ao Excluir Meta!');
      });
  }
  function AdicionarSaldo(event: any) {
    event.preventDefault();
    api
      .patch(`Metas/AdicionarSaldo/${idMeta}/${amount}`, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('usuario-login-auth')}`,
        },
      })

      .then((resposta) => {
        if (resposta.status === 200) {
          toast.success('Saldo Adicionado!');
          ListarMeta();
        }
      })

      .catch((resposta) => {
        console.log(resposta);
        toast.error('Erro ao Adicionar Saldo!');
      });
  }

  useEffect(ListarMeta, [idMeta, updates.count]);

  return (
    <div>
      <Dialog open={open} onClose={handleClose}>
        <DialogTitle
          sx={{
            marginBottom: 2,
            color: '#000',
          }}
        >
          Adicionar Saldo
        </DialogTitle>
        <DialogContent>
          <DialogContentText
            sx={{
              marginBottom: 2,
              color: '#000',
            }}
          >
            Adicione Saldo para cumprir a sua meta estipulada.
          </DialogContentText>
          <form className="formMeta" onSubmit={(event) => AdicionarSaldo(event)}>
            <CssTextField2
              id="outlined-basic"
              label="Saldo em R$"
              variant="outlined"
              type="number"
              fullWidth
              value={amount}
              onChange={(evt) => setAmount(Number(evt.target.value))}
            />
            <button type="submit" className="btnMetaModal" onClick={handleClose}>
              Adicionar
            </button>
          </form>
        </DialogContent>
      </Dialog>
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
                        margin: 0,
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
            <button
              className="btnsMeta"
              onClick={() => {
                ExcluirMeta();
              }}
            >
              excluir meta
            </button>
            <button className="btnsMetaWhite" onClick={handleClickOpen}>
              Adicionar Saldo
            </button>
            <ModalAltMeta dispatch={dispatch} />
            {/* <button className="btnsMetaWhite" onClick={}>Alterar Meta</button> */}
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
