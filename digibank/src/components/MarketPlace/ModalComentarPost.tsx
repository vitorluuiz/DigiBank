import React, { Dispatch, useState } from 'react';
import { toast } from 'react-toastify';
import { Dialog, Rating } from '@mui/material';
import { PostProps } from '../../@types/Post';
import { CssTextField } from '../../assets/styledComponents/input';
import api from '../../services/api';
import { parseJwt } from '../../services/auth';
import { CommentProps } from '../../@types/Comment';

export default function ModalComentario({
  postProps,
  dispatch,
  type,
  comments,
}: {
  comments?: CommentProps | null;
  postProps: PostProps | undefined;
  dispatch: Dispatch<any>;
  type: string;
}) {
  const [open, setOpen] = useState<boolean>(false);
  const [nota, setNota] = useState<number | null>(1);
  const [comentario, setComentario] = useState<string>();

  const handleClickOpenModal = (event: React.MouseEvent<HTMLButtonElement>) => {
    event.stopPropagation();
    setOpen(true);
  };

  const handleCloseModal = () => {
    setOpen(false);
  };

  const handleChangeRanting = (newRating: number | null) => {
    if (newRating === null) {
      setNota(1);
    } else {
      setNota(newRating);
    }
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
          toast.success('Comentario Enviado!');
          handleCloseModal();
        }
      });
  }

  function AtualizarComentario(event: any) {
    event.preventDefault();
    console.log(comments?.idAvaliacao);

    api
      .put(`Avaliacoes/${comments?.idAvaliacao}`, {
        idPost: postProps?.idPost,
        idUsuario: parseJwt().role,
        nota,
        comentario,
      })
      .then((response) => {
        if (response.status === 200) {
          dispatch({ type: 'update' });
          toast.success(response.data.message);
          handleCloseModal();
        }
      });
  }

  if (type === 'comentar') {
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
              <Rating
                value={nota}
                onChange={(evt, value) => handleChangeRanting(value)}
                size="large"
              />
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
  if (type === 'atualizar') {
    return (
      <div title="Faça um comentário na publicação" className="btnComentar">
        <button onClick={handleClickOpenModal} className="btnAtualizarModal">
          Atualizar resenha
        </button>
        <Dialog open={open} onClose={handleCloseModal}>
          <div id="support-modal-comentar">
            <div className="display-post-info">
              <img
                alt="Logo da postagem"
                src={`http://localhost:5000/img/${postProps?.mainImg ?? 'ImagemInvalida'}`}
              />
              <h2>{`Alterar avaliação de ${postProps?.nome}`}</h2>
            </div>
            <div className="input-rating-support">
              <Rating
                value={nota}
                onChange={(evt, value) => handleChangeRanting(value)}
                size="large"
              />
            </div>
            <form onSubmit={(evt) => AtualizarComentario(evt)}>
              <CssTextField
                fullWidth
                multiline
                required
                defaultValue={comments?.comentario}
                value={comentario}
                onChange={(evt) => setComentario(evt.target.value)}
              />
              <button type="submit" className="btnComponent">
                Atualizar resenha
              </button>
            </form>
          </div>
        </Dialog>
      </div>
    );
  }
  return null;
}
ModalComentario.defaultProps = {
  comments: null,
};
