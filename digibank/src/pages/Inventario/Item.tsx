import React from 'react';

import ImgDefault from '../../assets/img/store-post - Copia.png';

export default function Item() {
  return (
    <div className="inventario-item">
      <img alt="Imagem do Item" src={ImgDefault} />
      <div className="item-infos-support">
        <div className="item-infos">
          <h2>Cartão pré pago de 1 mes da Netflix</h2>
          <h3>Digibank</h3>
        </div>
        <div className="item-options">
          <button className="btnComponent item-option">Comprar novamente</button>
          <button className="item-option">Acompanhar</button>
          <button className="item-option">Avaliar</button>
        </div>
      </div>
    </div>
  );
}
