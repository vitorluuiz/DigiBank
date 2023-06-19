import React from 'react';
import { Link } from 'react-router-dom';

import { Rating } from '@mui/material';

import { PostProps } from '../../@types/Post';

export default function RecommendedBlock({ post }: { post: PostProps }) {
  return (
    <Link to={`/post/${post.idPost}`} className="recomendado-support">
      <img
        alt="Logo da postagem recomendada"
        style={{ backgroundColor: `#${post.mainColorHex}` }}
        src={`http://localhost:5000/img/${post.mainImg}`}
      />

      <div className="recomendado-infos">
        <div>
          <h3>{post.nome}</h3>
          <h4>{post.apelidoProprietario}</h4>
        </div>
        <div className="avaliacao-recomendado">
          <span>{post.avaliacao}</span>
          <Rating value={post.avaliacao ?? 0} size="small" precision={0.1} readOnly />
          <h5>{post.valor}BRL</h5>
        </div>
      </div>
    </Link>
  );
}
