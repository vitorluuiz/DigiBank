import React, { useEffect, useState } from 'react';
import api from '../../../services/api';
import { PostProps } from '../../../@types/Post';
import RecommendedBlock from '../RecommendedPost';
import seta from '../../../assets/img/setaCarousel.svg';
import { parseJwt } from '../../../services/auth';
import Empty from '../../Empty';
import SkeletonComponent from '../Skeleton/Skeleton';

export default function CarouselPosts({
  type,
  idOwner,
  maxValue,
}: {
  type: string;
  idOwner?: number;
  maxValue?: number;
}) {
  const [currentIndex, setCurrentIndex] = useState(0);
  const [qntdLista, setQntdLista] = useState(0);
  const [PostsList, setPostsList] = useState<PostProps[]>([]);
  const [loading, setLoading] = useState(true);

  function sliceImages() {
    const slicedImages = PostsList.slice(currentIndex, currentIndex + qntdLista);

    if (slicedImages.length > 0) {
      return slicedImages.map((post) => (
        <RecommendedBlock type="slim" key={post.idPost} post={post} />
      ));
    }
    return <Empty type="marketplace" />;
  }
  const handleClickNext = () => {
    setCurrentIndex((prevIndex) => (prevIndex + qntdLista) % PostsList.length);
  };

  const handleClickPrev = () => {
    setCurrentIndex((prevIndex) => {
      const newIndex = prevIndex - qntdLista;
      return newIndex < 0 ? PostsList.length + newIndex : newIndex;
    });
  };

  const GetPostsList = () => {
    switch (type) {
      case 'normal':
        api.get(`Marketplace/${1}/${9}/Vendas`).then((response) => {
          if (response.status === 200) {
            setPostsList(response.data);
            setQntdLista(3);
            setLoading(false);
          }
        });
        break;
      case 'slim':
        api.get(`Marketplace/${1}/${12}/Avaliacao`).then((response) => {
          if (response.status === 200) {
            setPostsList(response.data);
            setQntdLista(4);
            setLoading(false);
          }
        });
        break;
      case 'comprados':
        if (parseJwt().role !== 'undefined') {
          api.get(`Marketplace/${1}/${12}/comprados/${parseJwt().role}`).then((response) => {
            if (response.status === 200) {
              setPostsList(response.data);
              setQntdLista(4);
              setLoading(false);
            }
          });
        } else {
          setLoading(false);
        }
        break;
      case 'anunciante':
        api.get(`Marketplace/Usuario/${idOwner}`).then((response) => {
          if (response.status === 200) {
            setPostsList(response.data);
            setQntdLista(4);
            setLoading(false);
          }
        });
        break;
      case 'valor':
        api.get(`Marketplace/${1}/${12}/valor/${maxValue}`).then((response) => {
          if (response.status === 200) {
            setPostsList(response.data);
            setQntdLista(4);
            setLoading(false);
          }
        });
        break;
      default:
        break;
    }
  };

  // eslint-disable-next-line react-hooks/exhaustive-deps
  useEffect(() => GetPostsList(), []);

  if (loading) {
    return <SkeletonComponent />;
  }
  return (
    <div id="mainCarousel">
      <div className="suport-carousel">
        <button
          className="prevButton btnCarousel"
          onClick={handleClickPrev}
          disabled={currentIndex === 0}
        >
          <img src={seta} alt="seta voltar Carousel" />
        </button>
        {sliceImages()}
        <button
          className="nextButton btnCarousel"
          onClick={handleClickNext}
          disabled={currentIndex + qntdLista >= PostsList.length}
        >
          <img src={seta} alt="seta voltar Carousel" />
        </button>
      </div>
    </div>
  );
}

CarouselPosts.defaultProps = {
  idOwner: 0,
  maxValue: 0,
};
