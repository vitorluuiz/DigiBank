import React, { Dispatch, FormEvent, useState } from 'react';
import { DialogContent, DialogTitle, TextField, styled } from '@mui/material';
import Dialog from '@mui/material/Dialog';

import api from '../../services/api';
import { parseJwt } from '../../services/auth';
import { useSnackBar } from '../../services/snackBarProvider';

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

export default function ModalNovoCartao({ dispatch }: { dispatch: Dispatch<any> }) {
  const [open, setOpen] = useState<boolean>(false);
  const [isLoading, setLoading] = useState<boolean>(false);
  const [nome, setNome] = useState<string>('');
  const [token, setToken] = useState<string>('');

  const { postMessage } = useSnackBar();

  const handleOpen = () => {
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  function GerarCartao(evt: FormEvent<HTMLFormElement>) {
    evt.preventDefault();
    setLoading(true);

    api
      .post('Cartao/GerarCartao', {
        nome,
        token,
        idUsuario: parseJwt().role,
      })
      .then((response) => {
        if (response.status === 201) {
          handleClose();
          postMessage({ message: 'Cartão geradodo', severity: 'success', timeSpan: 3000 });
          dispatch({ type: 'update' });
        }
      });

    setInterval(() => {
      setLoading(false);
    }, 2000);
  }

  return (
    <div title="Gerar um cartão personalizado" className="card-option" style={{ height: '100%' }}>
      <button onClick={handleOpen} style={{ padding: '1rem' }}>
        Gerar novo Cartão
      </button>
      <Dialog open={open} onClose={handleClose}>
        <DialogTitle>Gerar cartão</DialogTitle>
        <DialogContent>
          <form id="post-cartao-form" onSubmit={(evt) => GerarCartao(evt)}>
            <CssTextField2
              label="Nomeie seu cartão"
              variant="outlined"
              type="text"
              fullWidth
              value={nome}
              onChange={(evt) => setNome(evt.target.value)}
            />
            <CssTextField2
              label="Digite uma senha"
              variant="outlined"
              type="password"
              fullWidth
              required
              value={token}
              onChange={(evt) => setToken(evt.target.value)}
            />
            <button disabled={isLoading} className="btnComponent">
              Gerar Cartão
            </button>
          </form>
        </DialogContent>
      </Dialog>
    </div>
  );
}
