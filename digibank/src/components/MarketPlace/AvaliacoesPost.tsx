import React from 'react';
import { Rating } from '@mui/material';

// import StarIcon from '../../assets/img/star_icon.svg';
import { CommentProps } from '../../@types/Comment';
import { RatingGraph } from '../RatingStats';

export default function AvaliacoesPost({
  avaliacao,
  votos,
  comments,
}: {
  avaliacao: number;
  votos: number;
  comments: CommentProps[];
}) {
  return (
    <div className="support-avaliacoes-post">
      <div className="avaliacao">
        <div className="avaliacoes-stats">
          <div id="avaliacoes-post">
            <div>
              <span id="nota">{avaliacao}</span>
              <Rating value={4 ?? 0} precision={0.1} size="large" readOnly />
            </div>
            <span>{votos} avaliações</span>
          </div>
          <RatingGraph />
        </div>
        <div className="btnComentar btnComponent">Comentar</div>
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
