import React, { useState, useEffect } from 'react';
import Banner1 from '../../assets/img/Banner1.png';
import Banner2 from '../../assets/img/Banner2.png';
import Banner3 from '../../assets/img/Banner3.png';
import Banner4 from '../../assets/img/Banner4.png';

interface Banner {
  id: number;
  image: string;
}

const BannerStore: React.FC = () => {
  const slidesBanner: Banner[] = [
    {
      id: 1,
      image: Banner1,
    },
    {
      id: 2,
      image: Banner2,
    },
    {
      id: 3,
      image: Banner3,
    },
    {
      id: 4,
      image: Banner4,
    },
  ];

  const [currentSlide, setCurrentSlide] = useState(0);

  useEffect(() => {
    const interval = setInterval(() => {
      setCurrentSlide((prevSlide) => (prevSlide + 1) % slidesBanner.length);
    }, 8000);

    return () => clearInterval(interval);
  }, [slidesBanner.length]);

  return (
    <div className="bannerContent">
      <div className="bannersContainer">
        {slidesBanner.map((slide, index) => (
          <div key={slide.id} className={`banner ${index === currentSlide ? 'active' : 'hidden'}`}>
            <img src={slide.image} alt="imagem banner" />
          </div>
        ))}
      </div>
    </div>
  );
};

export default BannerStore;
