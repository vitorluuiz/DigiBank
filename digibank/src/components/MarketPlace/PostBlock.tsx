import React, { useEffect, useState } from 'react';
import Color from 'color-thief-react';
// import Skeleton from '@mui/material/Skeleton';
// import { Rating } from '@mui/material';
import { Link } from 'react-router-dom';
import verificaTransparenciaImagem from '../../services/img';
import verificaFundoBrancoImagem from '../../services/imgWhite';
// import { PostProps } from '../../@types/Post';

export function PostBlock({ img, link }: { img: string; link: string }) {
  const [isTransparente, setTransparente] = useState<boolean>(false);
  const [isWhite, setWhite] = useState<boolean>(false);

  useEffect(() => {
    verificaTransparenciaImagem(img).then((temTransparencia) => {
      if (temTransparencia) {
        setTransparente(true);
        console.log('banana');
      }
    });
  });
  useEffect(() => {
    verificaFundoBrancoImagem(img).then((temBranco) => {
      if (temBranco) {
        setWhite(true);
      }
    });
  });

  if (isTransparente === true) {
    return (
      <Link to={link} className="store-item" style={{ backgroundColor: '#FFF' }}>
        <img alt="Foto ilustrativa da postagem" src={img} style={{ width: '70%' }} />

        {/* <div className="recomendado-infos">
          <div>
            <h3>{post.nome}</h3>
            <h4>{post.apelidoProprietario}</h4>
          </div>
          <div className="avaliacao-recomendado">
            <span>{post.avaliacao}</span>
            <Rating value={post.avaliacao ?? 0} size="small" readOnly />
            <h5>{post.valor}BRL</h5>
          </div>
        </div> */}
      </Link>
    );
  }
  if (isWhite === true) {
    return (
      <Link to={link} className="store-item" style={{ backgroundColor: '#FFFFFF' }}>
        <img alt="Foto ilustrativa da postagem" src={img} style={{ width: '70%' }} />
        {/* <div className="recomendado-infos">
          <div>
            <h3>{post.nome}</h3>
            <h4>{post.apelidoProprietario}</h4>
          </div>
          <div className="avaliacao-recomendado">
            <span>{post.avaliacao}</span>
            <Rating value={post.avaliacao ?? 0} size="small" readOnly />
            <h5>{post.valor}BRL</h5>
          </div>
        </div> */}
      </Link>
    );
  }
  return (
    <Color src={img} format="rgbString" quality={1}>
      {({ data }) => (
        <Link to={link} className="store-item" style={{ backgroundColor: data }}>
          <img alt="Foto ilustrativa da postagem" src={img} />
          {/* <div className="recomendado-infos">
            <div>
              <h3>{post.nome}</h3>
              <h4>{post.apelidoProprietario}</h4>
            </div>
            <div className="avaliacao-recomendado">
              <span>{post.avaliacao}</span>
              <Rating value={post.avaliacao ?? 0} size="small" readOnly />
              <h5>{post.valor}BRL</h5>
            </div>
          </div> */}
        </Link>
      )}
    </Color>
  );
}

export function PostBlockSlim({ img, link }: { img: string; link: string }) {
  const [isTransparente, setTransparente] = useState<boolean>(false);
  const [isWhite, setWhite] = useState<boolean>(false);

  useEffect(() => {
    verificaTransparenciaImagem(img).then((temTransparencia) => {
      if (temTransparencia) {
        setTransparente(true);
      }
    });
  });
  useEffect(() => {
    verificaFundoBrancoImagem(img).then((temBranco) => {
      if (temBranco) {
        setWhite(true);
      }
    });
  });

  if (isTransparente === true) {
    return (
      <Link to={link} className="store-item slim" style={{ backgroundColor: '#F2F2F2' }}>
        <img alt="Foto ilustrativa da postagem" src={img} style={{ width: '70%' }} />
        {/* <div className="recomendado-infos">
          <div>
            <h3>{post.nome}</h3>
            <h4>{post.apelidoProprietario}</h4>
          </div>
          <div className="avaliacao-recomendado">
            <span>{post.avaliacao}</span>
            <Rating value={post.avaliacao ?? 0} size="small" readOnly />
            <h5>{post.valor}BRL</h5>
          </div>
        </div> */}
      </Link>
    );
  }
  if (isWhite === true) {
    return (
      <Link to={link} className="store-item slim" style={{ backgroundColor: '#FFFFFF' }}>
        <img alt="Foto ilustrativa da postagem" src={img} style={{ width: '70%' }} />
        {/* <div className="recomendado-infos">
          <div>
            <h3>{post.nome}</h3>
            <h4>{post.apelidoProprietario}</h4>
          </div>
          <div className="avaliacao-recomendado">
            <span>{post.avaliacao}</span>
            <Rating value={post.avaliacao ?? 0} size="small" readOnly />
            <h5>{post.valor}BRL</h5>
          </div>
        </div> */}
      </Link>
    );
  }
  return (
    <Color src={img} format="rgbString" quality={1}>
      {({ data }) => (
        <Link to={link} className="store-item slim" style={{ backgroundColor: data }}>
          <img alt="Foto ilustrativa da postagem" src={img} />
          {/* <div className="recomendado-infos">
            <div>
              <h3>{post.nome}</h3>
              <h4>{post.apelidoProprietario}</h4>
            </div>
            <div className="avaliacao-recomendado">
              <span>{post.avaliacao}</span>
              <Rating value={post.avaliacao ?? 0} size="small" readOnly />
              <h5>{post.valor}BRL</h5>
            </div>
          </div> */}
        </Link>
      )}
    </Color>
  );
}
