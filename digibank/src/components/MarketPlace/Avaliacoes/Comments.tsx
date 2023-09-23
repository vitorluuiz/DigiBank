import React from 'react';
import { IconButton, Menu, MenuItem, Rating } from '@mui/material';
import { CommentProps } from '../../../@types/Comment';

import ListIcon from '../../../assets/img/list_icon.svg';
import OwnerIcon from '../../../assets/img/owner_icon.svg';
import PublicIcon from '../../../assets/img/public_icon.svg';
import { parseJwt } from '../../../services/auth';
import api from '../../../services/api';
import ModalComentario from '../ModalComentarPost';
import { PostProps } from '../../../@types/Post';
import { useSnackBar } from '../../../services/snackBarProvider';

export default function CommentPost({
  comment,
  postProps,
  onUpdate,
}: {
  comment: CommentProps;
  postProps: PostProps;
  onUpdate: () => void;
}) {
  const [anchorElDefault, setAnchorElDefault] = React.useState<null | HTMLElement>(null);
  const [anchorElUnDefault, setAnchorElUnDefault] = React.useState<null | HTMLElement>(null);
  const openDefault = Boolean(anchorElDefault);
  const openUnDefault = Boolean(anchorElUnDefault);
  const [anchorElUser, setAnchorElUser] = React.useState<null | HTMLElement>(null);
  const openUser = Boolean(anchorElUser);

  const [isLoading, setLoading] = React.useState<boolean>(false);

  const { postMessage } = useSnackBar();

  const handleClearLoading = () => setTimeout(() => setLoading(false), 500);

  const handleClickUser = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorElUser(event.currentTarget);
  };

  const handleClose = () => {
    setAnchorElDefault(null);
    setAnchorElUnDefault(null);
    setAnchorElUser(null);
  };

  const ReplieComment = (idComment: number) => {
    api
      .patch(`Avaliacoes/Like/${idComment}/${parseJwt().role}`)
      .then((response) => {
        if (response.status === 200) {
          onUpdate();
          handleClose();
          handleClearLoading();
          postMessage({
            message: 'Comentário alavancado!',
            severity: 'success',
            timeSpan: 2000,
            open: true,
          });
        }
      })
      .catch(() => {
        handleClearLoading();
        postMessage({
          message: 'Não foi possivel alavancar esta avaliação',
          severity: 'error',
          timeSpan: 2000,
          open: true,
        });
      });
  };

  const unReplieComment = (idComment: number) => {
    setLoading(true);

    api
      .patch(`Avaliacoes/UnLike/${idComment}/${parseJwt().role}`)
      .then((response) => {
        if (response.status === 200) {
          onUpdate();
          handleClearLoading();
          handleClose();
          postMessage({
            message: 'Like removido!',
            severity: 'success',
            timeSpan: 2000,
            open: true,
          });
        }
      })
      .catch(() => {
        handleClearLoading();
        postMessage({
          message: 'Erro ao remover like!',
          severity: 'success',
          timeSpan: 2000,
          open: true,
        });
      });
  };

  const RemoveComment = (idComment: number) => {
    setLoading(true);

    api
      .delete(`Avaliacoes/${idComment}`)
      .then((response) => {
        if (response.status === 204) {
          postMessage({
            message: 'Comentário Removido!',
            severity: 'success',
            timeSpan: 2000,
            open: true,
          });
          onUpdate();
          postMessage({ message: 'Comentário removido', severity: 'success', timeSpan: 2000 });
          handleClose();
          handleClearLoading();
        }
      })
      .catch(() => {
        handleClearLoading();
        postMessage({
          message: 'Erro ao remover comentário!',
          severity: 'error',
          timeSpan: 2000,
          open: true,
        });
      });
  };

  const handleClickDefault = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorElDefault(event.currentTarget);
  };

  const handleClickUnDefault = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorElUnDefault(event.currentTarget);
  };

  return (
    <div className={`comment ${comment.idUsuario.toString() === parseJwt().role ? 'owned' : ''}`}>
      <div className="comment-settings">
        <div className="comment-rating">
          <div>
            <span>{comment.publicador}</span>
            <span id="data-publicado">
              {new Date(comment.dataPostagem).toLocaleDateString('pt-BR', {
                day: '2-digit',
                month: '2-digit',
                year: 'numeric',
              })}
            </span>
          </div>
          {comment.idUsuario.toString() === parseJwt().role ? (
            <img alt="Comentário próprio" src={OwnerIcon} />
          ) : (
            <img alt="Comentário da comunidade" src={PublicIcon} />
          )}
          <Rating value={comment.nota ?? 0} readOnly size="small" />
        </div>
        {comment.idUsuario.toString() !== parseJwt().role ? (
          <div>
            {comment.isReplied ? (
              <div>
                <IconButton onClick={handleClickUnDefault}>
                  <img alt="Mais opções" src={ListIcon} />
                </IconButton>
                <Menu anchorEl={anchorElUnDefault} open={openUnDefault} onClose={handleClose}>
                  <MenuItem
                    disabled={isLoading}
                    onClick={() => unReplieComment(comment.idAvaliacao)}
                  >
                    Este comentário não foi útil
                  </MenuItem>
                </Menu>
              </div>
            ) : (
              <div>
                <IconButton onClick={handleClickDefault}>
                  <img alt="Mais opções" src={ListIcon} />
                </IconButton>
                <Menu anchorEl={anchorElDefault} open={openDefault} onClose={handleClose}>
                  <MenuItem disabled={isLoading} onClick={() => ReplieComment(comment.idAvaliacao)}>
                    Este comentário foi útil
                  </MenuItem>
                </Menu>
              </div>
            )}
          </div>
        ) : (
          <div>
            <IconButton onClick={handleClickUser}>
              <img alt="Mais opções" src={ListIcon} />
            </IconButton>
            <Menu anchorEl={anchorElUser} open={openUser} onClose={handleClose}>
              <MenuItem>
                <ModalComentario
                  comment={comment}
                  onUpdate={() => onUpdate()}
                  postProps={postProps}
                  type="atualizar"
                />
              </MenuItem>
              <MenuItem
                onClick={() => RemoveComment(comment.idAvaliacao)}
                sx={{ fontSize: '1rem', color: 'black', fontWeight: 400, fontFamily: 'Montserrat' }}
                disabled={isLoading}
              >
                Apagar resenha
              </MenuItem>
            </Menu>
          </div>
        )}
      </div>
      <p>{comment.comentario}</p>
      <span id="replies">{`${comment.replies} pessoa${comment.replies === 1 ? '' : 's'} ${
        comment.replies === 1 ? 'achou' : 'acharam'
      } isto útil`}</span>
    </div>
  );
}
