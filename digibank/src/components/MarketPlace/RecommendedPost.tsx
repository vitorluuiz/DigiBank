import React from 'react';
import { Link } from 'react-router-dom';

import { Rating } from '@mui/material';

import { PostProps } from '../../@types/Post';
import { IMGROOT } from '../../services/api';

export default function RecommendedBlock({ type, post }: { type: string; post: PostProps }) {
  const handlePostClick = () => {
    window.scrollTo({
      top: 0,
      behavior: 'smooth',
    });
  };

  if (type === 'Big') {
    return (
      <Link to={`/post/${post.idPost}`} className="recomendado-support" onClick={handlePostClick}>
        <img
          alt="Logo da postagem recomendada"
          src={`${IMGROOT}/${post.mainImg}`}
          style={{ backgroundColor: `#${post.mainColorHex}` }}
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
  if (type === 'slim') {
    return (
      <Link to={`/post/${post.idPost}`} className="recomendado-support" onClick={handlePostClick}>
        <img
          alt="Logo da postagem recomendada"
          src={`${IMGROOT}/${post.mainImg}`}
          style={{ backgroundColor: `#${post.mainColorHex}` }}
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
  return null;
}
