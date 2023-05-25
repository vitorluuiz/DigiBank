import React, { useEffect, useState } from 'react';
import Color from 'color-thief-react';
import { Link } from 'react-router-dom';
import verificaTransparenciaImagem from '../../services/img';

export function PostBlock({ img, link }: { img: string; link: string }) {
  const [isTransparente, setTransparente] = useState<boolean>(false);

  useEffect(() => {
    verificaTransparenciaImagem(img).then((temTransparencia) => {
      if (temTransparencia) {
        setTransparente(true);
      }
    });
  });

  return isTransparente ? (
    <Link to={link} className="store-item" style={{ backgroundColor: '#F2F2F2' }}>
      <img alt="Foto ilustrativa da postagem" src={img} style={{ width: '70%' }} />
    </Link>
  ) : (
    <Color src={img} format="hex">
      {({ data }) => (
        <Link to={link} className="store-item" style={{ backgroundColor: data }}>
          <img alt="Foto ilustrativa da postagem" src={img} />
        </Link>
      )}
    </Color>
  );
}

export function PostBlockSlim({ img, link }: { img: string; link: string }) {
  const [isTransparente, setTransparente] = useState<boolean>(false);

  useEffect(() => {
    verificaTransparenciaImagem(img).then((temTransparencia) => {
      if (temTransparencia) {
        setTransparente(true);
      }
    });
  });

  return isTransparente ? (
    <Link to={link} className="store-item slim" style={{ backgroundColor: '#F2F2F2' }}>
      <img alt="Foto ilustrativa da postagem" src={img} style={{ width: '70%' }} />
    </Link>
  ) : (
    <Color src={img} format="hex">
      {({ data }) => (
        <Link to={link} className="store-item slim" style={{ backgroundColor: data }}>
          <img alt="Foto ilustrativa da postagem" src={img} />
        </Link>
      )}
    </Color>
  );
}
