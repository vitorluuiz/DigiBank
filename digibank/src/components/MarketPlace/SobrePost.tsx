import React from 'react';
import Carousel from './Carousel';

export default function SobrePost() {
  return (
    <div className="support-sobre-post">
      <div className="galeria-post">
        <h2>Galeria</h2>
        <div className="support-galeria-post">
          <Carousel type="galeria" />
        </div>
      </div>
      <div className="descricao-post">
        <h2>Sobre o produto</h2>
        <p>
          Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam nec justo diam. Sed
          eleifend, risus id tempus imperdiet, nulla est.Lorem ipsum dolor sit amet, consectetur
          adipiscing elit. Etiam nec justo diam. Sed eleifend, risus id tempus imperdiet, nulla est.
          Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam nec justo diam. Sed
          eleifend, risus id tempus imperdiet, nulla est...
        </p>
      </div>
    </div>
  );
}
