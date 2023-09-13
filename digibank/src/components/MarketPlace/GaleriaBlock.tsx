import React, { useState } from 'react';

import { Dialog } from '@mui/material';
import { GaleriaProps } from '../../@types/Post';
import { IMGROOT } from '../../services/api';

export default function GaleriaBlock({ galeria }: { galeria: GaleriaProps }) {
  const [open, setOpen] = useState<boolean>(false);

  const handleClickOpenModal = (event: React.MouseEvent<HTMLButtonElement>) => {
    event.stopPropagation();
    setOpen(true);
  };

  const handleCloseModal = () => {
    setOpen(false);
  };
  return (
    <div className="store-item-galeria slim">
      <button onClick={handleClickOpenModal} className="imgBtn">
        <img alt="Logo da postagem recomendada" src={`${IMGROOT}${galeria}`} />
      </button>
      <Dialog id="modal" open={open} onClose={handleCloseModal}>
        <div className="galeria-modal">
          <img alt="Logo da postagem recomendada" src={`${IMGROOT}${galeria}`} />
        </div>
      </Dialog>
    </div>
  );
}
