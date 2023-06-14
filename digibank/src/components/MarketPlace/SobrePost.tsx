import React from 'react';
import Carousel from './Carousel';
import { PostProps } from '../../@types/Post';

export default function SobrePost({ postProps }: { postProps: PostProps | undefined }) {
  return (
    <div className="support-sobre-post">
      <Carousel type="galeria" />
      <div className="descricao-post">
        <h2>Sobre o produto</h2>
        <p>{postProps?.descricao}</p>
      </div>
    </div>
  );
}
