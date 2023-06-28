// eslint-disable-next-line eslint-comments/disable-enable-pair
/* eslint-disable no-nested-ternary */
import React, { Dispatch } from 'react';
import Rating from '@mui/material/Rating';

import { CommentProps } from '../../@types/Comment';
import ModalComentario from './ModalComentarPost';
import { PostProps } from '../../@types/Post';
import Histograma from '../RatingStats';
import { RatingHistograma } from '../../@types/RatingHistogram';
import { parseJwt } from '../../services/auth';
import CommentPost from './Avaliacoes/Comments';

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
    <div id="mainAvaliacoes" className="support-avaliacoes-post">
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
          <CommentPost
            key={comment.idAvaliacao}
            onUpdate={() => dispatch({ type: 'update' })}
            comment={comment}
          />
        ))}
      </div>
    </div>
  );
}
