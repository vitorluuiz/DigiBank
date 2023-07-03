import React from 'react';

import { GaleriaProps } from '../../@types/Post';
import { IMGROOT } from '../../services/api';

export default function GaleriaBlock({ galeria }: { galeria: GaleriaProps }) {
  return (
    <div className="store-item-galeria slim">
      <img alt="Logo da postagem recomendada" src={`${IMGROOT}${galeria}`} />
    </div>
  );
}
