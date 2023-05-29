import React, { useEffect, useState } from 'react';
import Color from 'color-thief-react';
import Skeleton from '@mui/material/Skeleton';
import { Link } from 'react-router-dom';
import verificaTransparenciaImagem from '../../services/img';
import verificaFundoBrancoImagem from '../../services/imgWhite';

export function PostBlock({ img, link }: { img: string; link: string }) {
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
      <Link to={link} className="store-item" style={{ backgroundColor: '#FFF' }}>
        <img alt="Foto ilustrativa da postagem" src={img} style={{ width: '70%' }} />
      </Link>
    );
  }
  if (isWhite === true) {
    return (
      <Link to={link} className="store-item" style={{ backgroundColor: '#FFFFFF' }}>
        <img alt="Foto ilustrativa da postagem" src={img} style={{ width: '70%' }} />
      </Link>
    );
  }
  return (
    <Color src={img} format="rgbString" quality={1}>
      {({ data, loading }) => (
        <Link to={link} className="store-item" style={{ backgroundColor: data }}>
          {loading ? (
            <Skeleton variant="rectangular" animation="wave" />
          ) : (
            <img alt="Foto ilustrativa da postagem" src={img} />
          )}
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
      </Link>
    );
  }
  if (isWhite === true) {
    return (
      <Link to={link} className="store-item slim" style={{ backgroundColor: '#FFFFFF' }}>
        <img alt="Foto ilustrativa da postagem" src={img} style={{ width: '70%' }} />
      </Link>
    );
  }
  return (
    <Color src={img} format="rgbString" quality={1}>
      {({ data }) => (
        <Link to={link} className="store-item slim" style={{ backgroundColor: data }}>
          <img alt="Foto ilustrativa da postagem" src={img} />
        </Link>
      )}
    </Color>
  );
}
