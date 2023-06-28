// eslint-disable-next-line eslint-comments/disable-enable-pair
/* eslint-disable react/destructuring-assignment */
import React, { useEffect, useState } from 'react';
import { Dialog } from '@mui/material';
import { toast } from 'react-toastify';

import TransferIcon from '../assets/img/transfer_icon.svg';
import { parseJwt } from '../services/auth';
import api, { IMGROOT } from '../services/api';
import { UsuarioProps } from '../@types/Usuario';
import { TransferenciaProps } from '../@types/TransferenciaProps';

export default function ModalTransacao({
  data,
  type,
  onClose,
  disabled,
}: {
  data: TransferenciaProps;
  type: string;
  onClose: () => void;
  disabled?: boolean;
}) {
  const [open, setOpen] = useState<boolean>(false);
  const [userData, setUserData] = useState<UsuarioProps>();
  const [error, setError] = useState<string>('');
  const [isLoading, setLoading] = useState<boolean>(false);

  const handleClickOpenModal = () => {
    setOpen(true);
  };

  const handleCloseModal = () => {
    setTimeout(() => {
      setLoading(false);
    }, 1000);
    setOpen(false);
    setError('');
    onClose();
  };

  function GetUserData() {
    api(`Usuarios/Infos/${parseJwt().role}`).then((response) => {
      if (response.status === 200) {
        setUserData(response.data);
      }
    });
  }

  function ComprarPost(id: number) {
    setLoading(true);

    api
      .post(`Marketplace/Comprar/${id}/${parseJwt().role}`)
      .then((response) => {
        if (response.status === 200) {
          toast.success('Compra efetivada');
          handleCloseModal();
          GetUserData();
        }
      })
      .catch(() => {
        setLoading(false);
        setError('Saldo insuficiente');
      });
  }

  function postTransferencia(event: any) {
    event.preventDefault();
    setLoading(true);

    api
      .post('Transacoes/EfetuarTransacao/', {
        idUsuarioPagante: parseJwt().role,
        idUsuarioRecebente: data.destino,
        valor: data.valor,
      })
      .then((response) => {
        if (response.status === 201) {
          toast.success('Transferência realizada');
          handleCloseModal();
        }
      })
      .catch((erro) => {
        setLoading(false);
        setError(erro.message);
        toast.error('Operação não concluída');
      });
  }

  useEffect(() => {
    GetUserData();
  }, []);

  useEffect(() => {
    if (!disabled) {
      handleClickOpenModal();
      setError('');
      setLoading(false);
    }
  }, [disabled]);

  return (
    <div title="Comprar produto da loja" id="adquirir__btn" className="btnPressionavel">
      {type === 'transacao' ? (
        <button type="submit" className="btnComponent">
          Enviar
        </button>
      ) : (
        <button onClick={handleClickOpenModal} className="btnComentar">
          {data.valor}BRL
        </button>
      )}

      <Dialog open={open} onClose={handleCloseModal}>
        <div id="support-modal-transacao">
          <div className="display-destino-support">
            {type === 'transacao' ? (
              <img alt="Destino da transação" src={TransferIcon} />
            ) : (
              <img
                alt="Destino da transação"
                src={`${IMGROOT}/${data.img}`}
                style={{ backgroundColor: `#${data.mainColorHex}` }}
              />
            )}
            <h2>{data.titulo}</h2>
          </div>
          <div className="display-bank-flow">
            <div className="bank-flow">
              <div className="flow-box">
                <h3>Saldo atual</h3>
                <h3>
                  {userData?.saldo.toLocaleString('pt-BR', {
                    currency: 'BRL',
                    style: 'currency',
                  })}
                </h3>
              </div>
              <div className="flow-box">
                <h3>Valor gasto</h3>
                <h3>
                  {data?.valor.toLocaleString('pt-BR', {
                    currency: 'BRL',
                    style: 'currency',
                  })}
                </h3>
              </div>
              <div className="flow-box saldo">
                <h3 className="total-title">Saldo final</h3>
                {((userData?.saldo ?? 0) - (data?.valor ?? 0)).toLocaleString('pt-BR', {
                  currency: 'BRL',
                  style: 'currency',
                })}
              </div>
            </div>
          </div>
          <div className="support-transfer-options">
            <span>{error}</span>
            <div className="display-options">
              <button
                onClick={(evt) =>
                  type === 'transacao' ? postTransferencia(evt) : ComprarPost(data.destino)
                }
                className="btnComponent"
                disabled={isLoading}
              >
                Efetuar transação
              </button>
              <button onClick={handleCloseModal} id="cancelar">
                Voltar
              </button>
            </div>
          </div>
        </div>
      </Dialog>
    </div>
  );
}

ModalTransacao.defaultProps = {
  disabled: true,
};
