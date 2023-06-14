import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';

import { PostProps } from '../../@types/Post';
import Header from '../../components/Header';
import Footer from '../../components/Footer';
import RecommendedBlock from '../../components/MarketPlace/RecommendedPost';
import api from '../../services/api';

export default function Catalogo() {
  const { filtro } = useParams();
  const [PostList, setPostList] = useState<PostProps[]>([]);

  function GetCatalogo() {
    api(`Marketplace/1/10/${filtro}`).then((response) => {
      if (response.status === 200) {
        setPostList(response.data);
        console.log(response.data);
      }
    });
  }

  // eslint-disable-next-line react-hooks/exhaustive-deps
  useEffect(() => GetCatalogo(), []);

  return (
    <div>
      <Header type="" />
      <main id="catalogo" className="container">
        <div className="support-recomendados-post">
          <div className="recomendados-list-support">
            <h2>Do mesmo Anunciante</h2>
            <div className="recomendados-list extended-list">
              {/* Postagem */}
              {PostList.map((post) => (
                <RecommendedBlock key={post.idPost} post={post} />
              ))}
            </div>
          </div>
        </div>
      </main>
      <Footer />
    </div>
  );
}
