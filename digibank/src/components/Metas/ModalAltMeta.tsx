import * as React from 'react';
import TextField from '@mui/material/TextField';
import { styled } from '@mui/material';
import { Dispatch } from 'react';
import Dialog from '@mui/material/Dialog';
import DialogContent from '@mui/material/DialogContent';
import { toast } from 'react-toastify';
import { useParams } from 'react-router-dom';
import DialogContentText from '@mui/material/DialogContentText';
import DialogTitle from '@mui/material/DialogTitle';
import api from '../../services/api';
// import { parseJwt } from '../../services/auth';

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

export default function ModalAltMeta({ dispatch }: { dispatch: Dispatch<any> }) {
  const { idMeta } = useParams();
  const [open, setOpen] = React.useState(false);
  const [amount, setAmount] = React.useState<number>();
  const handleClickOpen = () => {
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  function AlterarMeta(event: any) {
    event.preventDefault();
    api
      .patch(`Metas/AlterarMeta/${idMeta}/${amount}`, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('usuario-login-auth')}`,
        },
      })

      .then((resposta) => {
        if (resposta.status === 200) {
          toast.success('Meta Alterada!');
          dispatch({ type: 'update' });
        }
      })

      .catch((resposta) => {
        console.log(resposta);
        toast.error('Erro ao Alterar Meta!');
      });
  }

  return (
    <div>
      <button className="btnsMetaWhite" onClick={handleClickOpen}>
        Alterar meta
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
          <form className="formMeta" onSubmit={(event) => AlterarMeta(event)}>
            <CssTextField2
              id="outlined-basic"
              label="Novo valor da Meta"
              variant="outlined"
              type="text"
              fullWidth
              value={amount}
              onChange={(evt) => setAmount(Number(evt.target.value))}
            />
            <button type="submit" className="btnMetaModal" onClick={handleClose}>
              Alterar
            </button>
          </form>
        </DialogContent>
      </Dialog>
    </div>
  );
}
