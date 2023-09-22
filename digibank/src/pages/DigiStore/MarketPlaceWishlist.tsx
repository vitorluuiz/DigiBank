import React, { useEffect, useState } from 'react';
import Footer from '../../components/Footer';
import Header from '../../components/Header';
import { PostProps } from '../../@types/Post';
import RecommendedBlock from '../../components/MarketPlace/RecommendedPost';
import api from '../../services/api';
import { WishlishedPost } from './Post';
import { parseJwt } from '../../services/auth';

export default function Wishlist() {
  const [PostList, setPostList] = useState<PostProps[]>([]);

  const GetWishlistFromServer = (idsPosts: number[]) => {
    api.post(`Marketplace/ListarPorIds`, idsPosts).then((response) => {
      if (response.status === 200) {
        setPostList(response.data);
      }
    });
  };

  const GetWishlistFromLocal = () => {
    if (localStorage.getItem('wishlist')) {
      const localData: WishlishedPost[] = JSON.parse(localStorage.getItem('wishlist') ?? '[]');
      const idPosts: number[] = [];
      localData.forEach((item) => {
        if (item.idUsuario === parseJwt().role) {
          idPosts.push(item.idPost);
        }
      });
      GetWishlistFromServer(idPosts);
    } else {
      localStorage.setItem('wishlist', '[]');
    }
  };

  useEffect(() => {
    GetWishlistFromLocal();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <div>
      <Header type="" />
      <main id="catalogo" className="container">
        <div className="support-recomendados-post">
          <div className="recomendados-list-support">
            <h2>Classificado por itens desejados</h2>
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
