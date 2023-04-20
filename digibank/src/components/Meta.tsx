// import React from 'react';

import Graphic from './Grafico';

function Meta() {
  return (
    <div className="suport-meta">
      <h2>Bicicleta nova</h2>
      <div className="suport-infos">
        <span>R$450,00 Guardados</span>
        <div>
          {/* <img alt="Gráfico com o progresso da meta" src={grafico} /> */}
          <Graphic />
        </div>
        <span>R$900,00 Guardados</span>
      </div>
    </div>
  );
}

export default Meta;
