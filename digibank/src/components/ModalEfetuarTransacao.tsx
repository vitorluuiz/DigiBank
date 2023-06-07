import React, { useEffect, useState } from 'react';
import { Dialog } from '@mui/material';
import { toast } from 'react-toastify';

// import TransferIcon from '../assets/img/transfer_icon.svg';
import { PostProps } from '../@types/Post';
import { parseJwt } from '../services/auth';
import api from '../services/api';
import { UsuarioProps } from '../@types/Usuario';

export default function ModalTransacao({ postData }: { postData: PostProps | undefined }) {
  const [open, setOpen] = useState<boolean>(false);
  const [userData, setUserData] = useState<UsuarioProps>();
  const [error, setError] = useState<string>('');

  function GetUserData() {
    api(`Usuarios/Infos/${parseJwt().role}`).then((response) => {
      if (response.status === 200) {
        setUserData(response.data);
      }
    });
  }

  function ComprarPost(id: number | undefined) {
    api
      .post(`Marketplace/Comprar/${id}/${parseJwt().role}`)
      .then((response) => {
        if (response.status === 200) {
          toast.success('Compra efetivada');
          GetUserData();
        }
      })
      .catch(() => {
        setError('Saldo insuficiente');
      });
  }

  const handleClickOpenModal = () => {
    setOpen(true);
  };

  const handleCloseModal = () => {
    setOpen(false);
  };

  // eslint-disable-next-line prettier/prettier
  useEffect(() => {
    GetUserData();
  }, []);

  return (
    <div title="Comprar produto da loja" id="adquirir__btn" className="btnPressionavel">
      <button onClick={handleClickOpenModal} className="btnComentar">
        {postData?.valor}BRL
      </button>
      <Dialog open={open} onClose={handleCloseModal}>
        <div id="support-modal-transacao">
          <div className="display-destino-support">
            <img
              alt="Destino da transação"
              src={`http://localhost:5000/img/${postData?.mainImg}`}
            />
            <h2>Compra de {postData?.nome}</h2>
          </div>
          <div className="display-bank-flow">
            <div className="bank-flow">
              <div className="flow-box">
                <h3>Saldo Disponível</h3>
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
                  {/* {postData?.valor}BRL */}
                  {postData?.valor.toLocaleString('pt-BR', {
                    currency: 'BRL',
                    style: 'currency',
                  })}
                </h3>
              </div>
              <div className="flow-box saldo">
                <h3 className="total-title">Saldo final</h3>
                {((userData?.saldo ?? 0) - (postData?.valor ?? 0)).toLocaleString('pt-BR', {
                  currency: 'BRL',
                  style: 'currency',
                })}
              </div>
            </div>
          </div>
          <div className="support-transfer-options">
            <span>{error}</span>
            <div className="display-options">
              <button onClick={() => ComprarPost(postData?.idPost)} className="btnComponent">
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
