import React, { Dispatch, useState } from 'react';
import { toast } from 'react-toastify';
import { Dialog, Rating } from '@mui/material';
import { PostProps } from '../../@types/Post';
import { CssTextField } from '../../assets/styledComponents/input';
import api from '../../services/api';
import { parseJwt } from '../../services/auth';

export default function ModalComentario({
  postProps,
  dispatch,
}: {
  postProps: PostProps | undefined;
  dispatch: Dispatch<any>;
}) {
  const [open, setOpen] = useState<boolean>(false);
  const [nota, setNota] = useState<number | null>(1);
  const [comentario, setComentario] = useState<string>('');

  const handleClickOpenModal = () => {
    setOpen(true);
  };

  const handleCloseModal = () => {
    setOpen(false);
  };

  function Comentar(event: any) {
    event.preventDefault();

    api
      .post('Avaliacoes', {
        idPost: postProps?.idPost,
        idUsuario: parseJwt().role,
        nota,
        comentario,
      })
      .then((response) => {
        if (response.status === 201) {
          dispatch({ type: 'update' });
          toast.success(response.data.message);
          handleCloseModal();
        }
      });
  }

  return (
    <div title="Faça um comentário na publicação" className="btnComentar">
      <button onClick={handleClickOpenModal} className="btnComentar btnComponent">
        Comentar
      </button>
      <Dialog open={open} onClose={handleCloseModal}>
        <div id="support-modal-comentar">
          <div className="display-post-info">
            <img
              alt="Logo da postagem"
              src={`http://localhost:5000/img/${postProps?.mainImg ?? 'ImagemInvalida'}`}
            />
            <h2>{`Avaliar ${postProps?.nome}`}</h2>
          </div>
          <div className="input-rating-support">
            <Rating value={nota} onChange={(evt, value) => setNota(value)} size="large" />
          </div>
          <form onSubmit={(evt) => Comentar(evt)}>
            <CssTextField
              fullWidth
              multiline
              required
              value={comentario}
              onChange={(evt) => setComentario(evt.target.value)}
            />
            <button type="submit" className="btnComponent">
              Comentar
            </button>
          </form>
        </div>
      </Dialog>
    </div>
  );
}
