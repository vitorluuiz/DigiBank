import React, { Dispatch, useState } from 'react';
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

import Dialog from '@mui/material/Dialog';
import { TextField, styled } from '@mui/material';

import { UsuarioPublicoProps } from '../../@types/Usuario';
import { FluxoProps } from '../../@types/FluxoBancario';

import api from '../../services/api';
import mask from '../mask';
import { parseJwt } from '../../services/auth';

const StyledTextField = styled(TextField)({
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

export default function ModalTransferir({ dispatch }: { dispatch: Dispatch<any> }) {
  const [open, setOpen] = useState<boolean>(false);
  const [Chave, setChave] = useState<string>();
  const [Valor, setValor] = useState<string>();
  const [Usuario, setUsuario] = useState<UsuarioPublicoProps>();
  const [Fluxo, setFluxo] = useState<FluxoProps>();
  const [isSearched, setSearched] = useState<boolean>();

  const handleClickOpen = () => {
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  function postTransferencia(event: any) {
    event.preventDefault();

    api
      .post('Transacoes/EfetuarTransacao/', {
        idUsuarioPagante: parseJwt().role,
        idUsuarioRecebente: Usuario?.idUsuario,
        valor: Valor,
      })
      .then((response) => {
        if (response.status === 201) {
          dispatch({ type: 'update' });
          handleClose();
          toast.success('Transferência realizada');
        }
      })
      .catch(() => {
        toast.error('Operação não concluída');
      });
  }

  function getBankFlow(uuid: number) {
    api(`Transacoes/FluxoEntreUsuarios/${parseJwt().role}/${uuid}`).then((response) => {
      if (response.status === 200) {
        setFluxo(response.data);
        setSearched(true);
      }
    });
  }

  function getUser(cpf: string) {
    api(`Usuarios/Cpf/${cpf}`)
      .then((response) => {
        if (response.status === 200) {
          setUsuario(response.data);
          getBankFlow(response.data.idUsuario);
        }
      })
      .catch(() => {
        toast.error('Usuário não encontrado');
      });
  }

  function handleChangeMask(event: any) {
    const { value } = event.target;

    setChave(mask(value));

    if (mask(value).length === 14) {
      getUser(mask(value).replaceAll('.', '').replace('-', '') ?? '0');
    } else {
      setSearched(false);
    }
  }

  return (
    <div id="transferencia" className="user-button">
      <button id="btnTransferencia" onClick={handleClickOpen}>
        Transferir
      </button>
      <Dialog open={open} onClose={handleClose}>
        <div id="suport-modal-transferir">
          <section
            id="left-modal-transferir"
            className={`left-modal-transferir ${isSearched ? 'visible' : 'invisible'}`}
          >
            <section className="modal-user-infos">
              <h1>{Usuario?.nomeCompleto}</h1>
              <h2>CPF: {mask(Usuario?.cpf ?? '')}</h2>
              <h2>{Usuario?.email}</h2>
            </section>
            <section className="modal-bank-flow">
              <span>Fluxo Financeiro com {Usuario?.nomeCompleto} nos últimos 30 dias</span>
              <div className="bank-flow">
                <div className="flow-box">
                  <h3>Recebido</h3>
                  <h3>
                    {Fluxo?.recebimentos.toLocaleString('pt-BR', {
                      currency: 'BRL',
                      style: 'currency',
                    })}
                  </h3>
                </div>
                <div className="flow-box">
                  <h3>Enviado</h3>
                  <h3>
                    {Fluxo?.pagamentos.toLocaleString('pt-BR', {
                      currency: 'BRL',
                      style: 'currency',
                    })}
                  </h3>
                </div>
                <div className="flow-box saldo">
                  <h3 className="total-title">Total</h3>
                  {Fluxo?.saldo != null && (
                    <h3 className={Fluxo.saldo >= 0 ? 'green' : 'red'}>
                      {Fluxo.saldo.toLocaleString('pt-BR', {
                        currency: 'BRL',
                        style: 'currency',
                      })}
                    </h3>
                  )}
                </div>
              </div>
            </section>
          </section>

          <section className="right-modal-transferir">
            <form onSubmit={postTransferencia}>
              <StyledTextField
                label="Chave PIX"
                type="text"
                variant="outlined"
                fullWidth
                value={Chave}
                inputProps={{ maxLength: 14 }}
                onChange={(evt) => {
                  handleChangeMask(evt);
                }}
              />
              <StyledTextField
                label="Valor"
                type="text"
                variant="outlined"
                fullWidth
                value={Valor}
                onChange={(evt) => setValor(evt.target.value)}
              />
              <button className="btnComponent" type="submit">
                Enviar
              </button>
            </form>
          </section>
        </div>
      </Dialog>
    </div>
  );
}
