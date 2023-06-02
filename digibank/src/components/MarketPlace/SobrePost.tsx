import React from 'react';

import ExGaleriaImg from '../../assets/img/tela1.png';
import ExGaleriaImg2 from '../../assets/img/tela2.png';
import ExGaleriaImg3 from '../../assets/img/tela3.png';
import ExGaleriaImg4 from '../../assets/img/tela4.png';

export default function SobrePost() {
  return (
    <div className="support-sobre-post">
      <div className="galeria-post">
        <h2>Galeria</h2>
        <div className="support-galeria-post">
          <div className="support-img">
            <img alt="Imagem da galeria da postagem" src={ExGaleriaImg} />
          </div>
          <div className="support-img">
            <img alt="Imagem da galeria da postagem" src={ExGaleriaImg2} />
          </div>
          <div className="support-img">
            <img alt="Imagem da galeria da postagem" src={ExGaleriaImg3} />
          </div>
          <div className="support-img">
            <img alt="Imagem da galeria da postagem" src={ExGaleriaImg4} />
          </div>
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
