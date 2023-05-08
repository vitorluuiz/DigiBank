import * as React from 'react';
// import { useNavigate } from 'react-router-dom';
// import Button from '@mui/material/Button';
import TextField from '@mui/material/TextField';
import { styled } from '@mui/material';
import { Dispatch } from 'react';
import Dialog from '@mui/material/Dialog';
// import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import { toast } from 'react-toastify';
import DialogContentText from '@mui/material/DialogContentText';
import DialogTitle from '@mui/material/DialogTitle';
import api from '../services/api';
import { parseJwt } from '../services/auth';

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

export default function ModalMeta({ dispatch }: { dispatch: Dispatch<any> }) {
  const [open, setOpen] = React.useState(false);
  const [idMeta] = React.useState(0);
  const [idUsuario] = React.useState(parseJwt().role);
  const [titulo, setTitulo] = React.useState('');
  const [valorMeta, setValorMeta] = React.useState('');
  //   const navigate = useNavigate();

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
          console.log('meta adicionada!');
          toast.success('Meta Adicionada!');
          dispatch({ type: 'update' });
        }
      })
      .catch((resposta) => {
        console.log(resposta);
        toast.error('Erro ao Adicionar Meta!');
      });
  }

  return (
    <div>
      <button className="btnMeta" onClick={handleClickOpen}>
        Adicionar meta
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
            <CssTextField2
              id="outlined-basic"
              label="Nome da Meta"
              variant="outlined"
              type="text"
              fullWidth
              value={titulo}
              onChange={(evt) => setTitulo(evt.target.value)}
            />
            <CssTextField2
              id="outlined-basic"
              label="Qual seu objetivo?"
              variant="outlined"
              type="text"
              fullWidth
              value={valorMeta}
              onChange={(evt) => setValorMeta(evt.target.value)}
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
