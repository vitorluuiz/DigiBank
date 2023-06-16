import { Modal } from '@mui/material';
import React, { useState } from 'react';

export default function ModalViewItem() {
  const [open, setOpen] = useState<boolean>(false);

  const handleClickModal = () => {
    setOpen(true);
  };

  const handleCloseModal = () => {
    setOpen(false);
  };

  return (
    <div className="menu-option">
      <button onClick={handleClickModal}>AbrirModal</button>
      <Modal open={open} onClose={handleCloseModal}>
        <h1>Opa</h1>
      </Modal>
    </div>
  );
}
