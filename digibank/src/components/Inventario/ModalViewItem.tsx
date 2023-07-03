import { Dialog, DialogTitle, ImageList, ImageListItem } from '@mui/material';
import React, { useEffect, useState } from 'react';
import { GaleriaProps } from '../../@types/Post';
import api, { IMGROOT } from '../../services/api';

export default function ModalViewItem({ idPost }: { idPost: number }) {
  const [open, setOpen] = useState<boolean>(false);
  const [Imgs, setImgs] = useState<GaleriaProps[]>([]);

  const getImagesPost = () => {
    api(`Inventario/${idPost}`).then((response) => {
      if (response.status === 200) {
        setImgs(response.data.imgs);
      }
    });
  };

  const handleClickOpenModal = (event: React.MouseEvent<HTMLButtonElement>) => {
    event.stopPropagation();
    setOpen(true);
  };

  const handleCloseModal = () => {
    setOpen(false);
  };

  // eslint-disable-next-line react-hooks/exhaustive-deps
  useEffect(() => getImagesPost(), []);

  return (
    <div className="menu-option">
      <button className="modal-button" onClick={handleClickOpenModal}>
        Ver mais
      </button>
      <Dialog open={open} onClose={handleCloseModal}>
        <DialogTitle>Listagem do Item</DialogTitle>
        <ImageList>
          {Imgs.map((item) => (
            <ImageListItem key={item.img}>
              <img
                style={{ objectFit: 'scale-down' }}
                src={`${IMGROOT}${item}`}
                alt="Imagem da postagem"
              />
            </ImageListItem>
          ))}
        </ImageList>
      </Dialog>
    </div>
  );
}
