import React, { useEffect, useState } from 'react';
import seta from '../../assets/img/setaCarousel.svg';
// import Nike from '../../assets/img/Nike.png';
// import Amazon from '../../assets/img/AmazonAcoes.png';
// import Google from '../../assets/img/Google.png';
// import CocaCola from '../../assets/img/CocaCola.png';
// import SpaceX from '../../assets/img/SpaceX.png';
// import Netflix from '../../assets/img/netflix.png';
import { PostBlock, PostBlockSlim } from './PostBlock';
import api from '../../services/api';

interface Image {
  idPost: number;
  mainImg: string;
}

const Carousel: React.FC<{ type: string }> = ({ type }) => {
  const [currentIndex, setCurrentIndex] = useState(0);
  const [images, setImages] = useState<Image[]>([]);

  /// //////////////////////// FUNCTION LISTAR
  function ListarPosts() {
    api
      .get(`Marketplace/${1}/${10}`, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('usuario-login-auth')}`,
        },
      })

      .then((resposta) => {
        if (resposta.status === 200) {
          setImages(resposta.data);
        }
      })

      .catch((erro) => console.log(erro));
  }

  /// //////////////////////// LISTAR IMAGES DESTAQUE

  function renderImages() {
    const slicedImages = images.slice(currentIndex, currentIndex + 3);

    return slicedImages.map((image) => (
      <PostBlock
        key={image.idPost}
        img={`http://localhost:5000/img/${image.mainImg}`}
        link={`/post/${image.idPost}`}
      />
    ));
  }
  const handleClickNext = () => {
    setCurrentIndex((prevIndex) => (prevIndex + 3) % images.length);
  };

  const handleClickPrev = () => {
    setCurrentIndex((prevIndex) => {
      const newIndex = prevIndex - 3;
      return newIndex < 0 ? images.length + newIndex : newIndex;
    });
  };

  const renderPaginationDots = () => {
    const dotsCount = Math.ceil(images.length / 3);

    return Array.from({ length: dotsCount }).map((_, index) => {
      const pageNumber = index + 1;
      const isActive = currentIndex === index * 3;
      const dotKey = `dot-${pageNumber}`;

      return (
        <button
          key={dotKey}
          className={`dot ${isActive ? 'active' : ''}`}
          onClick={() => setCurrentIndex(index * 3)}
          aria-label={`Página ${pageNumber}`}
        >
          {/* {pageNumber} */}
        </button>
      );
    });
  };

  /// //////////////////////// LISTAR IMAGES SLIM
  function renderImagesSlim() {
    const slicedImages = images.slice(currentIndex, currentIndex + 5);

    return slicedImages.map((image) => (
      <PostBlockSlim
        key={image.idPost}
        img={`http://localhost:5000/img/${image.mainImg}`}
        link={`/post/${image.idPost}`}
      />
    ));
  }

  const handleClickNextSlim = () => {
    setCurrentIndex((prevIndex) => (prevIndex + 5) % images.length);
  };

  const handleClickPrevSlim = () => {
    setCurrentIndex((prevIndex) => {
      const newIndex = prevIndex - 5;
      return newIndex < 0 ? images.length + newIndex : newIndex;
    });
  };

  const renderPaginationDotsSlim = () => {
    const dotsCount = Math.ceil(images.length / 5);

    return Array.from({ length: dotsCount }).map((_, index) => {
      const pageNumber = index + 1;
      const isActive = currentIndex === index * 5;
      const dotKey = `dot-${pageNumber}`;

      return (
        <button
          key={dotKey}
          className={`dot ${isActive ? 'active' : ''}`}
          onClick={() => setCurrentIndex(index * 5)}
          aria-label={`Página ${pageNumber}`}
        >
          {/* {pageNumber} */}
        </button>
      );
    });
  };

  /// //////////////////////// USE EFFECTS
  useEffect(() => {
    const interval = setInterval(() => {
      setCurrentIndex((prevIndex) => (prevIndex + 5) % images.length);
    }, 7000);

    return () => {
      clearInterval(interval);
    };
  }, [images.length]);

  useEffect(() => {
    ListarPosts();
  }, []);

  /// //////////////////////// RETURNS
  if (type === 'normal') {
    return (
      <div id="mainCarousel">
        <div className="suport-carousel">{renderImages()}</div>
        <div className="bottomCarousel">
          <button className="prevButton" onClick={handleClickPrev} disabled={currentIndex === 0}>
            <img src={seta} alt="seta voltar Carousel" />
          </button>
          <div className="pagination-dots">{renderPaginationDots()}</div>
          <button
            className="nextButton"
            onClick={handleClickNext}
            disabled={currentIndex + 3 >= images.length}
          >
            <img src={seta} alt="seta voltar Carousel" />
          </button>
        </div>
      </div>
    );
  }
  if (type === 'slim') {
    return (
      <div id="mainCarousel">
        <div className="suport-carousel">{renderImagesSlim()}</div>
        <div className="bottomCarousel">
          <button
            className="prevButton"
            onClick={handleClickPrevSlim}
            disabled={currentIndex + 5 >= images.length}
          >
            <img src={seta} alt="seta voltar Carousel" />
          </button>
          <div className="pagination-dots">{renderPaginationDotsSlim()}</div>
          <button
            className="nextButton"
            onClick={handleClickNextSlim}
            disabled={currentIndex + 3 >= images.length}
          >
            <img src={seta} alt="seta voltar Carousel" />
          </button>
        </div>
      </div>
    );
  }
  return null;
};
export default Carousel;
