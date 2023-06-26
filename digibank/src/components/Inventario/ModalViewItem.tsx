import { Dialog } from '@mui/material';
import React, { useState } from 'react';

export default function ModalViewItem() {
  const [open, setOpen] = useState<boolean>(false);

  const handleClickOpenModal = (event: React.MouseEvent<HTMLButtonElement>) => {
    event.stopPropagation();
    setOpen(true);
  };

  const handleCloseModal = () => {
    setOpen(false);
  };

  return (
    <div className="menu-option">
      <button className="modal-button" onClick={handleClickOpenModal}>
        Ver Mais
      </button>
      <Dialog open={open} onClose={handleCloseModal}>
        <h1>Opa</h1>
      </Dialog>
    </div>
  );
}
