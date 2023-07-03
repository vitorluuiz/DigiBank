import React, { useState, useEffect } from 'react';
import Nike from '../assets/img/Nike.png';
import Amazon from '../assets/img/AmazonAcoes.png';
import Google from '../assets/img/Google.png';
import CocaCola from '../assets/img/CocaCola.png';

interface Slide {
  id: number;
  image: string;
}

const Slider: React.FC = () => {
  const slides: Slide[] = [
    {
      id: 1,
      image: CocaCola,
    },
    {
      id: 2,
      image: Amazon,
    },
    {
      id: 3,
      image: Nike,
    },
    {
      id: 4,
      image: Google,
    },
  ];

  const [currentSlide, setCurrentSlide] = useState(0);

  useEffect(() => {
    const interval = setInterval(() => {
      setCurrentSlide((prevSlide) => (prevSlide + 1) % slides.length);
    }, 3000);

    return () => clearInterval(interval);
  }, [slides.length]);

  return (
    <div className="sliderContent">
      <div className="slidesContainer">
        {slides.map((slide, index) => (
          <div key={slide.id} className={`slide ${index === currentSlide ? 'active' : ''}`}>
            <img src={slide.image} alt="imagem slide" />
          </div>
        ))}
      </div>
    </div>
  );
};

export default Slider;
