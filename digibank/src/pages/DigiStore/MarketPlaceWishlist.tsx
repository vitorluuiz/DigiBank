import React, { useEffect, useState } from 'react';
import Footer from '../../components/Footer';
import Header from '../../components/Header';
import { PostProps } from '../../@types/Post';
import RecommendedBlock from '../../components/MarketPlace/RecommendedPost';

export default function Wishlist() {
  const [PostList, setPostList] = useState<PostProps[]>([]);

  const GetWishlist = () => {
    if (localStorage.getItem('wishlist')) {
      setPostList(JSON.parse(localStorage.getItem('wishlist') ?? '[]'));
    } else {
      localStorage.setItem('wishlist', '[]');
    }
  };

  useEffect(() => {
    GetWishlist();
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
