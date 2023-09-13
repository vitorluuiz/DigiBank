import React, { Dispatch } from 'react';
import { toast } from 'react-toastify';
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
  dispatch,
}: {
  comment: CommentProps;
  postProps: PostProps;
  onUpdate: () => void;
  dispatch: Dispatch<any>;
}) {
  const [anchorElDefault, setAnchorElDefault] = React.useState<null | HTMLElement>(null);
  const [anchorElUnDefault, setAnchorElUnDefault] = React.useState<null | HTMLElement>(null);
  const openDefault = Boolean(anchorElDefault);
  const openUnDefault = Boolean(anchorElUnDefault);

  const [anchorElUser, setAnchorElUser] = React.useState<null | HTMLElement>(null);
  const openUser = Boolean(anchorElUser);

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  const { postMessage } = useSnackBar();

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
        }
      })
      .catch((error) => toast.error(error.data.message));
  };

  const unReplieComment = (idComment: number) => {
    api
      .patch(`Avaliacoes/UnLike/${idComment}/${parseJwt().role}`)
      .then((response) => {
        if (response.status === 200) {
          onUpdate();
          handleClose();
        }
      })
      .catch((error) => toast.error(error.data.message));
  };

  const RemoveComment = (idComment: number) => {
    api
      .delete(`Avaliacoes/${idComment}`)
      .then((response) => {
        if (response.status === 204) {
          onUpdate();
          postMessage({ message: 'Comentário removido', severity: 'success', timeSpan: 2000 });
          handleClose();
        }
      })
      .catch((error) => console.log(error));
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
                  <MenuItem onClick={() => unReplieComment(comment.idAvaliacao)}>
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
                  <MenuItem onClick={() => ReplieComment(comment.idAvaliacao)}>
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
                  comments={comment}
                  dispatch={dispatch}
                  postProps={postProps}
                  type="atualizar"
                />
              </MenuItem>
              <MenuItem
                onClick={() => RemoveComment(comment.idAvaliacao)}
                sx={{ fontSize: '1rem', color: 'black', fontWeight: 400, fontFamily: 'Montserrat' }}
              >
                Apagar resenha
              </MenuItem>
            </Menu>
          </div>
        )}
      </div>
      <span>{comment.comentario}</span>
      <span id="replies">{`${comment.replies} pessoa${comment.replies === 1 ? '' : 's'} ${
        comment.replies === 1 ? 'achou' : 'acharam'
      } isto útil`}</span>
    </div>
  );
}
