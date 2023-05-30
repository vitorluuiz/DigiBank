import React, { useEffect, useState } from 'react';
import seta from '../../assets/img/setaCarousel.svg';
import Nike from '../../assets/img/Nike.png';
import Amazon from '../../assets/img/AmazonAcoes.png';
import Google from '../../assets/img/Google.png';
import CocaCola from '../../assets/img/CocaCola.png';
import SpaceX from '../../assets/img/SpaceX.png';
import Netflix from '../../assets/img/netflix.png';
import { PostBlock, PostBlockSlim } from './PostBlock';

const Carousel: React.FC<{ type: string }> = ({ type }) => {
  const [currentIndex, setCurrentIndex] = useState(0);
  const images = [
    { id: 'image1', src: Nike, linka: '/home' },
    { id: 'image2', src: Amazon, linka: '/extrato' },
    { id: 'image3', src: Google, linka: '/404' },
    { id: 'image4', src: CocaCola, linka: '/403' },
    { id: 'image5', src: SpaceX, linka: '/401' },
    { id: 'image6', src: Netflix, linka: '/' },
    { id: 'image7', src: Netflix, linka: '/' },
    { id: 'image8', src: Netflix, linka: '/' },
    { id: 'image9', src: Netflix, linka: '/' },
    { id: 'image10', src: Netflix, linka: '/' },
  ];

  const handleClickNext = () => {
    setCurrentIndex((prevIndex) => (prevIndex + 3) % images.length);
  };

  const handleClickPrev = () => {
    setCurrentIndex((prevIndex) => {
      const newIndex = prevIndex - 3;
      return newIndex < 0 ? images.length + newIndex : newIndex;
    });
  };

  const renderImages = () => {
    const endIndex = currentIndex + 3 > images.length ? images.length : currentIndex + 3;
    return images
      .slice(currentIndex, endIndex)
      .map(({ id, src, linka }) => <PostBlock key={id} img={src} link={linka} />);
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

  const handleClickNextSlim = () => {
    setCurrentIndex((prevIndex) => (prevIndex + 5) % images.length);
  };

  const handleClickPrevSlim = () => {
    setCurrentIndex((prevIndex) => {
      const newIndex = prevIndex - 5;
      return newIndex < 0 ? images.length + newIndex : newIndex;
    });
  };

  const renderImagesSlim = () => {
    const endIndex = currentIndex + 5 > images.length ? images.length : currentIndex + 5;
    return images
      .slice(currentIndex, endIndex)
      .map(({ id, src, linka }) => <PostBlockSlim key={id} img={src} link={linka} />);
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
  useEffect(() => {
    const interval = setInterval(() => {
      setCurrentIndex((prevIndex) => (prevIndex + 5) % images.length);
    }, 7000);

    return () => {
      clearInterval(interval);
    };
  }, [images.length]);

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
