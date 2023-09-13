import React, { useState } from 'react';
import { Link } from 'react-router-dom';

import { Rating } from '@mui/material';

import { PostProps } from '../../@types/Post';
import { IMGROOT } from '../../services/api';

export default function RecommendedBlock({ type, post }: { type: string; post: PostProps }) {
  const [hovered, setHovered] = useState(false);

  const handleMouseEnter = () => {
    setHovered(true);
  };

  const handleMouseLeave = () => {
    setHovered(false);
  };
  const handlePostClick = () => {
    window.scrollTo({
      top: 0,
      behavior: 'smooth',
    });
  };

  if (type === 'Big') {
    return (
      <Link
        to={`/post/${post.idPost}`}
        className="recomendado-support support-post-big"
        onClick={handlePostClick}
        onMouseEnter={handleMouseEnter}
        onMouseLeave={handleMouseLeave}
      >
        <img alt="Logo da postagem recomendada" src={`${IMGROOT}/${post.mainImg}`} />

        {hovered && (
          <div
            className="recomendado-infos big"
            style={{
              background: hovered
                ? `linear-gradient(360deg, #000 30%, #00000070 90%, transparent 100%)`
                : 'transparent',
            }}
          >
            <div>
              <h3>{post.nome}</h3>
              <h4>{post.apelidoProprietario}</h4>
            </div>
            <div className="avaliacao-recomendado">
              <div>
                <span>{post.avaliacao}</span>
                <Rating
                  value={post.avaliacao ?? 0}
                  size="small"
                  precision={0.1}
                  readOnly
                  sx={{ color: '#faaf00' }}
                />
              </div>
              <h5>{`${post.valor === 0 ? 'Grátis' : `${post.valor}BRL`}`}</h5>
            </div>
          </div>
        )}
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
            <div>
              <span>{post.avaliacao}</span>
              <Rating
                value={post.avaliacao ?? 0}
                size="small"
                precision={0.1}
                readOnly
                sx={{ color: '#faaf00' }}
              />
            </div>
            <h5>{`${post.valor === 0 ? 'Grátis' : `${post.valor}BRL`}`}</h5>
          </div>
        </div>
      </Link>
    );
  }
  return null;
}
