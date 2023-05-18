import React, { Dispatch, FormEvent, useState } from 'react';
import { DialogContent, DialogTitle, TextField, styled } from '@mui/material';
import { toast } from 'react-toastify';
import Dialog from '@mui/material/Dialog';

import api from '../../services/api';

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

export default function ModalAltSenha({
  dispatch,
  idCartao,
}: {
  dispatch: Dispatch<any>;
  idCartao: number;
}) {
  const [open, setOpen] = useState<boolean>(false);
  const [newToken, setToken] = useState<string>('');

  const handleOpen = () => {
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  function AlterarSenha(evt: FormEvent<HTMLFormElement>) {
    evt.preventDefault();

    api
      .patch(`Cartao/AlterarSenha/${idCartao}`, {
        newToken,
      })
      .then((response) => {
        if (response.status === 200) {
          handleClose();
          toast.success('Senha alterada');
          dispatch({ type: 'update' });
        }
      });
  }

  return (
    <div className="card-option">
      <button onClick={handleOpen}>Alterar Senha</button>
      <Dialog open={open} onClose={handleClose}>
        <DialogTitle>AlterarSenha</DialogTitle>
        <DialogContent>
          <form id="post-cartao-form" onSubmit={(evt) => AlterarSenha(evt)}>
            <CssTextField2
              label="Digite uma nova senha"
              variant="outlined"
              type="password"
              fullWidth
              required
              value={newToken}
              onChange={(evt) => setToken(evt.target.value)}
            />
            <button className="btnComponent">Alterar</button>
          </form>
        </DialogContent>
      </Dialog>
    </div>
  );
}
