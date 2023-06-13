import React, { useEffect, useState } from 'react';

import api from '../../services/api';
import { PostProps } from '../../@types/Post';

import RecommendedBlock from './RecommendedPost';
import Carousel from './Carousel';

export default function RecomendadosPost() {
  const [deProprietarioList, setDeProprietario] = useState<PostProps[]>([]);
  // const [topRatingList, setTopRatingList] = useState<PostProps[]>([]);

  function GetDeProprietario() {
    api('Marketplace/1/41').then((response) => {
      if (response.status === 200) {
        setDeProprietario(response.data);
      }
    });

    // api('Marketplace/1/4/Avaliacao').then((response) => {
    //   if (response.status === 200) {
    //     setTopRatingList(response.data);
    //   }
    // });
  }
  useEffect(() => {
    GetDeProprietario();
    // eslint-disable-next-line prettier/prettier
  }, []);
  // console.log(idUsuario);

  return (
    <div className="support-recomendados-post">
      {/* Do mesmo anunciante */}
      <div className="recomendados-list-support">
        <h2>Do mesmo Anunciante</h2>
        <div className="recomendados-list">
          {/* Postagem */}
          {deProprietarioList.map((post) => (
            <RecommendedBlock key={post.idPost} post={post} />
          ))}
        </div>
      </div>

      {/* Recomendados */}
      <div className="recomendados-list-support">
        <h2>Produtos recomendados</h2>
        <div className="recomendados-list">
          {/* Postagem */}
          {/* Postagem */}
          {/* {topRatingList.map((post) => (
            <RecommendedBlock key={post.idPost} post={post} />
          ))} */}
          <Carousel type="slim" />
        </div>
      </div>
    </div>
  );
}
