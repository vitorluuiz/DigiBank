import { Dispatch, useState } from 'react';
import Dialog from '@mui/material/Dialog';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import DialogTitle from '@mui/material/DialogTitle';
import api from '../../services/api';
import { parseJwt } from '../../services/auth';
import { StyledTextField } from '../../assets/styledComponents/input';

export default function ModalMeta({ dispatch }: { dispatch: Dispatch<any> }) {
  const [open, setOpen] = useState(false);
  const [idMeta] = useState(0);
  const [idUsuario] = useState(parseJwt().role);
  const [titulo, setTitulo] = useState('');
  const [valorMeta, setValorMeta] = useState<number>(0);
  const handleClickOpen = () => {
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  function CadastrarMeta(event: any) {
    event.preventDefault();
    api
      .post('Metas', {
        idMeta,
        idUsuario,
        titulo,
        valorMeta,
      })
      .then((resposta) => {
        if (resposta.status === 201) {
          dispatch({ type: 'update' });
        }
      })
      .catch((resposta) => {
        console.log(resposta);
      });
  }

  return (
    <div>
      <button className="btnMeta" onClick={handleClickOpen}>
        Adicionar Meta
      </button>
      <Dialog open={open} onClose={handleClose}>
        <DialogTitle
          sx={{
            marginBottom: 2,
            color: '#000',
          }}
        >
          Adicione uma Meta
        </DialogTitle>
        <DialogContent>
          <DialogContentText
            sx={{
              marginBottom: 2,
              color: '#000',
            }}
          >
            Adicione uma Meta e tenha maior controle sobre os seus objetivos.
          </DialogContentText>
          <form className="formMeta" onSubmit={(event) => CadastrarMeta(event)}>
            <StyledTextField
              id="outlined-basic"
              label="Nome da Meta"
              variant="outlined"
              type="text"
              fullWidth
              value={titulo}
              onChange={(evt) => setTitulo(evt.target.value)}
            />
            <StyledTextField
              label="Qual seu objetivo?"
              required
              variant="outlined"
              fullWidth
              type="number"
              value={valorMeta}
              onChange={(evt) => setValorMeta(parseFloat(evt.target.value))}
            />
            <button type="submit" className="btnMetaModal" onClick={handleClose}>
              Cadastrar
            </button>
          </form>
        </DialogContent>
      </Dialog>
    </div>
  );
}
