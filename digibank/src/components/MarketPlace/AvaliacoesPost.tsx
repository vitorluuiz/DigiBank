import React, { Dispatch } from 'react';
import { Rating } from '@mui/material';

// import StarIcon from '../../assets/img/star_icon.svg';
import { CommentProps } from '../../@types/Comment';
import { RatingGraph } from '../RatingStats';
import ModalComentario from './ModalComentarPost';
import { PostProps } from '../../@types/Post';

export default function AvaliacoesPost({
  postProps,
  comments,
  dispatch,
}: {
  postProps: PostProps | undefined;
  comments: CommentProps[];
  dispatch: Dispatch<any>;
}) {
  return (
    <div className="support-avaliacoes-post">
      <div className="avaliacao">
        <div className="avaliacoes-stats">
          <div id="avaliacoes-post">
            <div>
              <span id="nota">{postProps?.avaliacao}</span>
              <Rating value={4 ?? 0} precision={0.1} size="large" readOnly />
            </div>
            <span>{postProps?.qntAvaliacoes} avaliações</span>
          </div>
          <RatingGraph />
        </div>
        <ModalComentario dispatch={dispatch} postProps={postProps} />
      </div>
      <div className="comments-list">
        {comments.map((comment) => (
          <div className="comment">
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
