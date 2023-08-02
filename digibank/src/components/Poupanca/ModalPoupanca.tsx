import { Dispatch, useEffect, useState } from 'react';
import { Dialog, DialogTitle } from '@mui/material';
import api from '../../services/api';
import { parseJwt } from '../../services/auth';
import { CssTextField2 } from '../../assets/styledComponents/input';
import pigIcon from '../../assets/img/PigIcon.png';

export default function ModalPoupanca({
  dispatch,
  type,
  dataInicio,
}: {
  dispatch: Dispatch<any>;
  type: string;
  dataInicio?: string | undefined;
}) {
  const [open, setOpen] = useState(false);
  const [quantidade, setQuantidade] = useState(0);
  const [inicio, setInicio] = useState('');
  const [fim, setFim] = useState('');
  const [ganhos, setGanhos] = useState<number | null>(0);

  const handleClickOpen = () => {
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  const calcularGanhos = (event: any) => {
    event.preventDefault();

    api
      .post(`Poupanca/Ganhos/${parseJwt().role}`, {
        inicio,
        fim,
      })
      .then((resposta) => {
        if (resposta.status === 200) {
          setGanhos(resposta.data.ganhos.ganhos);
          console.log(resposta.data);
        }
      });
  };
  const Sacar = (event: any) => {
    event?.preventDefault();
    api
      .post(`Poupanca/Sacar/${parseJwt().role}/${quantidade}`, {
        quantidade,
      })
      .then((resposta) => {
        if (resposta.status === 200) {
          dispatch({ type: 'update' });
        }
      });
  };
  const Depositar = (event: any) => {
    event?.preventDefault();
    api
      .post(`Poupanca/Depositar/${parseJwt().role}/${quantidade}`, {
        quantidade,
      })
      .then((resposta) => {
        if (resposta.status === 200) {
          dispatch({ type: 'update' });
        }
      });
  };
  useEffect(() => {
    const today = new Date();
    const umMes = new Date(today);
    umMes.setMonth(umMes.getMonth() + 1);

    const formattedDateInicio = today.toISOString().substr(0, 10);
    const formattedDateFim = umMes.toISOString().substr(0, 10);

    setInicio(formattedDateInicio);
    setFim(formattedDateFim);
  }, []);

  if (type === 'sacar') {
    return (
      <div>
        <button className="btnPoupanca" onClick={handleClickOpen}>
          Sacar
        </button>
        <Dialog open={open} onClose={handleClose}>
          <div className="bodyModalSacar">
            <div className="topModal">
              <img src={pigIcon} alt="icone cofrinho de porco" />
              <DialogTitle
                sx={{
                  color: '#000',
                  fontSize: '1.5rem',
                }}
              >
                Quanto deseja sacar?
              </DialogTitle>
            </div>
            <form className="formSacar" onSubmit={(event) => Sacar(event)}>
              <CssTextField2
                id="outlined-basic"
                label="Quantidade para sacar"
                variant="outlined"
                type="number"
                fullWidth
                value={quantidade}
                onChange={(evt) => setQuantidade(parseFloat(evt.target.value))}
              />
              <button type="submit" className="btnMetaModal" onClick={handleClose}>
                Sacar
              </button>
            </form>
          </div>
        </Dialog>
      </div>
    );
  }
  if (type === 'investir') {
    return (
      <div>
        <button className="btnPoupanca" onClick={handleClickOpen}>
          Investir Mais
        </button>
        <Dialog open={open} onClose={handleClose}>
          <div className="bodyModalSacar">
            <div className="topModal">
              <img src={pigIcon} alt="icone cofrinho de porco" />
              <DialogTitle
                sx={{
                  color: '#000',
                  fontSize: '1.5rem',
                }}
              >
                Quanto deseja investir?
              </DialogTitle>
            </div>
            <form className="formSacar" onSubmit={(event) => Depositar(event)}>
              <CssTextField2
                id="outlined-basic"
                label="Quantidade para investir"
                variant="outlined"
                type="number"
                fullWidth
                value={quantidade}
                onChange={(evt) => setQuantidade(parseFloat(evt.target.value))}
              />
              <button type="submit" className="btnMetaModal" onClick={handleClose}>
                Investir
              </button>
            </form>
          </div>
        </Dialog>
      </div>
    );
  }
  return (
    <div>
      <button className="btnPoupanca" onClick={handleClickOpen}>
        Calcular Ganhos
      </button>
      <Dialog open={open} onClose={handleClose}>
        <div className="bodyModalGanhos">
          <div className="topModal">
            <img src={pigIcon} alt="icone cofrinho de porco" />
            <DialogTitle
              sx={{
                color: '#000',
                fontSize: '2rem',
              }}
            >
              Calcule os Ganhos com a poupança
            </DialogTitle>
          </div>
          <form className="formGanhos" onSubmit={(event) => calcularGanhos(event)}>
            <div>
              <CssTextField2
                id="outlined-basic"
                label="Data de Inicio"
                variant="outlined"
                type="date"
                fullWidth
                value={inicio}
                inputProps={{ min: dataInicio }}
                onChange={(evt) => setInicio(evt.target.value)}
              />
              <CssTextField2
                id="outlined-basic"
                label="Data Final"
                variant="outlined"
                type="date"
                fullWidth
                value={fim}
                onChange={(evt) => setFim(evt.target.value)}
              />
              <div>
                <span>Ganhos previstos</span>
                <p>R${ganhos?.toFixed(2)}</p>
              </div>
            </div>
            <button type="submit" className="btnMetaModal">
              Calcular Ganhos
            </button>
          </form>
        </div>
      </Dialog>
    </div>
  );
}

ModalPoupanca.defaultProps = {
  dataInicio: '',
};
