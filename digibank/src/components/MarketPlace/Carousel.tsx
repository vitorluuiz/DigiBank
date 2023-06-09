import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
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
interface Galerias {
  idPost: number;
  mainImg: string;
  imgs?: string[];
}

const Carousel: React.FC<{ type: string }> = ({ type }) => {
  const [currentIndex, setCurrentIndex] = useState(0);
  const [images, setImages] = useState<Image[]>([]);
  const [galeria, setGaleria] = useState<Galerias[]>([]);
  const { idPost } = useParams();
  // console.log(idPost);

  /// //////////////////////// FUNCTION LISTAR
  function ListarPosts() {
    api
      .get(`Marketplace/${1}/${30}`, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('usuario-login-auth')}`,
        },
      })

      .then((resposta) => {
        if (resposta.status === 200) {
          setImages(resposta.data);
          console.log(resposta.data);
        }
      })

      .catch((erro) => console.log(erro));
  }

  function ListarPostsId(id: string | undefined) {
    api
      .get(`Marketplace/${id}`, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('usuario-login-auth')}`,
        },
      })
      .then((resposta) => {
        if (resposta.status === 200) {
          const { data } = resposta;
          if (Array.isArray(data)) {
            setGaleria(data);
          } else {
            setGaleria([data]);
          }
          console.log(data);
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

  /// //////////////////////// LISTAR IMAGES GALERIA
  function renderImagesGaleria() {
    const slicedImages = galeria.slice(currentIndex, currentIndex + 4);

    return slicedImages.map((image) =>
      image.imgs?.map((img, index) => (
        <PostBlockSlim
          // eslint-disable-next-line react/no-array-index-key
          key={`${image.idPost}-${index}`}
          img={`http://localhost:5000/img/${img}`}
          link="/"
        />
      )),
    );
  }

  const handleClickNextGaleria = () => {
    setCurrentIndex((prevIndex) => (prevIndex + 4) % images.length);
  };

  const handleClickPrevGaleria = () => {
    setCurrentIndex((prevIndex) => {
      const newIndex = prevIndex - 4;
      return newIndex < 0 ? images.length + newIndex : newIndex;
    });
  };

  const renderPaginationDotsGaleria = () => {
    const dotsCount = Math.ceil(images.length / 4);

    return Array.from({ length: dotsCount }).map((_, index) => {
      const pageNumber = index + 1;
      const isActive = currentIndex === index * 4;
      const dotKey = `dot-${pageNumber}`;

      return (
        <button
          key={dotKey}
          className={`dot ${isActive ? 'active' : ''}`}
          onClick={() => setCurrentIndex(index * 4)}
          aria-label={`Página ${pageNumber}`}
        >
          {/* {pageNumber} */}
        </button>
      );
    });
  };

  /// //////////////////////// USE EFFECTS
  // useEffect(() => {
  //   const interval = setInterval(() => {
  //     setCurrentIndex((prevIndex) => (prevIndex + 5) % images.length);
  //   }, 7000);

  //   return () => {
  //     clearInterval(interval);
  //   };
  // }, [images.length]);

  useEffect(() => {
    ListarPosts();
  }, []);
  useEffect(() => {
    ListarPostsId(idPost);
  }, [idPost]);

  /// //////////////////////// RETURNS
  if (type === 'normal') {
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
          {renderImages()}
          <button
            className="nextButton btnCarousel"
            onClick={handleClickNext}
            disabled={currentIndex + 3 >= images.length}
          >
            <img src={seta} alt="seta voltar Carousel" />
          </button>
        </div>
        <div className="bottomCarousel">
          <div className="pagination-dots">{renderPaginationDots()}</div>
        </div>
      </div>
    );
  }
  if (type === 'slim') {
    return (
      <div id="mainCarousel">
        <div className="suport-carousel">
          <button
            className="prevButton btnCarousel"
            onClick={handleClickPrevSlim}
            disabled={currentIndex === 0}
          >
            <img src={seta} alt="seta voltar Carousel" />
          </button>
          {renderImagesSlim()}
          <button
            className="nextButton btnCarousel"
            onClick={handleClickNextSlim}
            disabled={currentIndex + 5 >= images.length}
          >
            <img src={seta} alt="seta voltar Carousel" />
          </button>
        </div>
        <div className="bottomCarousel">
          <div className="pagination-dots">{renderPaginationDotsSlim()}</div>
        </div>
      </div>
    );
  }
  if (type === 'recomendados') {
    return (
      <div id="mainCarousel">
        <div className="suport-carousel">
          <button
            className="prevButton btnCarousel"
            onClick={handleClickPrevSlim}
            disabled={currentIndex === 0}
          >
            <img src={seta} alt="seta voltar Carousel" />
          </button>
          {renderImagesSlim()}
          <button
            className="nextButton btnCarousel"
            onClick={handleClickNextSlim}
            disabled={currentIndex + 4 >= images.length}
          >
            <img src={seta} alt="seta voltar Carousel" />
          </button>
        </div>
        <div className="bottomCarousel">
          <div className="pagination-dots">{renderPaginationDotsSlim()}</div>
        </div>
      </div>
    );
  }
  if (type === 'galeria') {
    return (
      <div id="mainCarousel">
        <div className="suport-carousel">
          <button
            className="prevButton btnCarousel"
            onClick={handleClickPrevGaleria}
            disabled={currentIndex === 0}
          >
            <img src={seta} alt="seta voltar Carousel" />
          </button>
          {renderImagesGaleria()}
          <button
            className="nextButton btnCarousel"
            onClick={handleClickNextGaleria}
            disabled={currentIndex + 4 >= images.length}
          >
            <img src={seta} alt="seta voltar Carousel" />
          </button>
        </div>
        <div className="bottomCarousel">
          <div className="pagination-dots">{renderPaginationDotsGaleria()}</div>
        </div>
      </div>
    );
  }
  if (type === 'recomendados') {
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
