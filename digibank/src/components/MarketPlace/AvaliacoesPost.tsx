import React, { Dispatch } from 'react';
import { Rating } from '@mui/material';

// import StarIcon from '../../assets/img/star_icon.svg';
import { CommentProps } from '../../@types/Comment';
import ModalComentario from './ModalComentarPost';
import { PostProps } from '../../@types/Post';
import Histograma from '../RatingStats';
import { RatingHistograma } from '../../@types/RatingHistogram';
import { parseJwt } from '../../services/auth';

export default function AvaliacoesPost({
  postProps,
  comments,
  commentsHistograma,
  dispatch,
}: {
  postProps: PostProps | undefined;
  comments: CommentProps[];
  commentsHistograma: RatingHistograma[];
  dispatch: Dispatch<any>;
}) {
  return (
    <div className="support-avaliacoes-post">
      <div className="avaliacao">
        <div className="avaliacoes-stats">
          <div id="avaliacoes-post">
            <div>
              <span id="nota">{postProps?.avaliacao}</span>
              <Rating value={postProps?.avaliacao ?? 0} precision={0.1} size="large" readOnly />
            </div>
            <span>{postProps?.qntAvaliacoes} avaliações</span>
          </div>
          <Histograma histograma={commentsHistograma} />
        </div>
        {postProps?.idUsuario.toString() !== parseJwt().role ? (
          <ModalComentario dispatch={dispatch} postProps={postProps} />
        ) : null}
      </div>
      <div className="comments-list">
        {comments.map((comment) => (
          <div key={comment.idAvaliacao} className="comment">
            <span>{comment.comentario}</span>
            <div className="comment-settings">
              <span>{comment.nota}</span>
              <Rating value={comment.nota ?? 0} readOnly size="small" />
              {/* <img alt="Icone de avaliação" src={StarIcon} /> */}
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
