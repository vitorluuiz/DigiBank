import React, { useState } from 'react';
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

import Dialog from '@mui/material/Dialog';

import { UsuarioPublicoProps } from '../../@types/Usuario';
import { FluxoProps } from '../../@types/FluxoBancario';

import api from '../../services/api';
import mask from '../mask';
import { parseJwt } from '../../services/auth';
import ModalTransacao from '../ModalEfetuarTransacao';
import { StyledTextField } from '../../assets/styledComponents/input';

export default function ModalTransferir({ onClose }: { onClose: () => void }) {
  const [open, setOpen] = useState<boolean>(false);
  const [Chave, setChave] = useState<string>();
  const [Valor, setValor] = useState<number>();
  const [Usuario, setUsuario] = useState<UsuarioPublicoProps>();
  const [Fluxo, setFluxo] = useState<FluxoProps>();
  const [isSearched, setSearched] = useState<boolean>();
  const [isValidated, setValidated] = useState<boolean>(false);

  const handleClickOpenModal = () => {
    setOpen(true);
  };

  const handleCloseModal = () => {
    onClose();
    setOpen(false);
  };

  function getBankFlow(uuid: number) {
    api(`Transacoes/FluxoEntreUsuarios/${parseJwt().role}/${uuid}`).then((response) => {
      if (response.status === 200) {
        setFluxo(response.data);
        setSearched(true);
      }
    });
  }

  function getUser(cpf: string) {
    api(`Usuarios/cpf/${cpf}`)
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

  const validate = (event: any | undefined) => {
    event.preventDefault();

    if (!isValidated && isSearched) {
      setValidated(true);
    } else {
      setValidated(false);
    }
  };

  return (
    <div title="Realizar uma transferência" id="transferencia" className="user-button">
      <button id="btnTransferencia" onClick={handleClickOpenModal}>
        Transferir
      </button>
      <Dialog open={open} onClose={handleCloseModal}>
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
            <form onSubmit={validate}>
              <StyledTextField
                inputProps={{ maxLength: 14, minLength: 14 }}
                label="Chave PIX"
                required
                variant="outlined"
                fullWidth
                value={Chave}
                onChange={(evt) => {
                  handleChangeMask(evt);
                }}
              />
              <StyledTextField
                label="Valor"
                type="number"
                variant="outlined"
                fullWidth
                required
                value={Valor}
                onChange={(evt) =>
                  !Number.isNaN(evt.target.value)
                    ? setValor(parseInt(evt.target.value, 10))
                    : setValor(0)
                }
              />
              <ModalTransacao
                type="transacao"
                data={{
                  titulo: `Deseja efetuar uma transação para ${Usuario?.nomeCompleto}?`,
                  valor: Valor ?? 0,
                  destino: Usuario?.idUsuario ?? 0,
                }}
                disabled={!isValidated}
                onClose={() => {
                  getBankFlow(Usuario?.idUsuario ?? parseJwt().role);
                  setValidated(false);
                }}
              />
            </form>
          </section>
        </div>
      </Dialog>
    </div>
  );
}
