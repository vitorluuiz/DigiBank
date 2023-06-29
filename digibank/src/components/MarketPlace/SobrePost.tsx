import React from 'react';
import { PostProps } from '../../@types/Post';
import CarouselGaleria from './Carousel/CarouselGaleria';

export default function SobrePost({ postProps }: { postProps: PostProps | undefined }) {
  return (
    <div className="support-sobre-post">
      <CarouselGaleria />
      <div className="descricao-post">
        <h2>Sobre o produto</h2>
        <p>{postProps?.descricao}</p>
      </div>
    </div>
  );
}
