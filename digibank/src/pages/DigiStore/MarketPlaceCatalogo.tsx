import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';

import { PostProps } from '../../@types/Post';
import Header from '../../components/Header';
import Footer from '../../components/Footer';
import RecommendedBlock from '../../components/MarketPlace/RecommendedPost';
import api from '../../services/api';
import { parseJwt } from '../../services/auth';

export default function Catalogo() {
  const { filtro } = useParams();
  const [PostList, setPostList] = useState<PostProps[]>([]);
  const navigate = useNavigate();

  function GetCatalogo() {
    switch (filtro) {
      case 'vendas':
      case 'avaliacao':
        api(`Marketplace/1/30/${filtro}`).then((response) => {
          if (response.status === 200) {
            setPostList(response.data);
          }
        });
        break;
      case 'comprados':
        api(`Marketplace/1/30/comprados/${parseJwt().role}`).then((response) => {
          if (response.status === 200) {
            setPostList(response.data);
          }
        });
        break;
      default:
        api(`Marketplace/1/30/valor/${filtro}`)
          .then((response) => {
            if (response.status === 200) {
              setPostList(response.data);
            }
          })
          .catch(() => {
            navigate('/404');
          });
        break;
    }
  }
  // eslint-disable-next-line react-hooks/exhaustive-deps
  useEffect(() => GetCatalogo(), []);

  let filterLabel;
  if (filtro === 'vendas' || filtro === 'avaliacao') {
    filterLabel = `Produtos classificados por ${filtro}`;
  } else if (filtro === 'comprados') {
    filterLabel = `Produtos ${filtro} anteriormente`;
  } else {
    filterLabel = `Produtos abaixo de R$${filtro}`;
  }

  return (
    <div>
      <Header type="digiStore" />
      <main id="catalogo" className="container">
        <div className="support-recomendados-post">
          <div className="recomendados-list-support">
            {filterLabel && <h2>{filterLabel}</h2>}
            <div className="recomendados-list extended-list">
              {/* Postagem */}
              {PostList.map((post) => (
                <RecommendedBlock type="slim" key={post.idPost} post={post} />
              ))}
            </div>
          </div>
        </div>
      </main>
      <Footer />
    </div>
  );
}
