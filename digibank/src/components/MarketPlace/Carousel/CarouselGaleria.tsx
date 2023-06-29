import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import seta from '../../../assets/img/setaCarousel.svg';
import api from '../../../services/api';
import { GaleriaProps } from '../../../@types/Post';
import GaleriaBlock from '../GaleriaBlock';
import SkeletonComponent from '../Skeleton/Skeleton';

export default function CarouselGaleria() {
  const { idPost } = useParams();
  const [currentIndex, setCurrentIndex] = useState(0);
  const [galeria, setGaleria] = useState<GaleriaProps[]>([{ img: '' }]);
  const [loading, setLoading] = useState(true);

  function GetImagesPost(id: string) {
    api
      .get(`Marketplace/${id}`)
      .then((resposta) => {
        if (resposta.status === 200) {
          setGaleria(resposta.data.imgs);
          setLoading(false);
        }
      })
      .catch((erro) => console.log(erro));
  }

  function renderImagesGaleria() {
    const slicedImages = galeria.slice(currentIndex, currentIndex + 4);

    return slicedImages?.map((imagem) => (
      <GaleriaBlock key={slicedImages.indexOf(imagem)} galeria={imagem} />
    ));
  }

  const handleClickNextGaleria = () => {
    setCurrentIndex((prevIndex) => (prevIndex + 4) % galeria.length);
  };

  const handleClickPrevGaleria = () => {
    setCurrentIndex((prevIndex) => {
      const newIndex = prevIndex - 4;
      return newIndex < 0 ? galeria.length + newIndex : newIndex;
    });
  };

  useEffect(() => {
    GetImagesPost(idPost ?? '');
  }, [idPost]);

  if (loading) {
    return <SkeletonComponent />;
  }

  const shouldRenderImages = galeria.some(() => galeria && galeria.length > 0);

  if (!shouldRenderImages) {
    return null;
  }
  return (
    <div className="galeria-post">
      <h2>Galeria</h2>
      <div className="support-galeria-post">
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
              disabled={currentIndex + 4 >= galeria.length}
            >
              <img src={seta} alt="seta voltar Carousel" />
            </button>
          </div>
        </div>
      </div>
    </div>
  );
}
