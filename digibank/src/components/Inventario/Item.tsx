import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import { Menu, MenuItem } from '@mui/material';

import api, { IMGROOT } from '../../services/api';
import { ItemProps } from '../../@types/Inventario';

export default function Item({
  itemData,
  onDelete,
}: {
  itemData: ItemProps;
  onDelete: () => void;
}) {
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const open = Boolean(anchorEl);

  const handleClick = (event: React.MouseEvent<HTMLButtonElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleClose = () => {
    setAnchorEl(null);
  };

  function DeleteItem(idItem: number) {
    api.delete(`Inventario/${idItem}`).then((response) => {
      if (response.status === 204) {
        handleClose();
        onDelete();
      }
    });
  }

  return (
    <div className="inventario-item">
      <div className="support-logo">
        <img alt="Imagem do Item" src={`${IMGROOT}/${itemData.idPostNavigation.mainImg}`} />
      </div>
      <div className="item-infos-support">
        <div className="item-infos">
          <h2>{itemData.idPostNavigation.nome}</h2>
          <h3>{itemData.idPostNavigation.apelidoProprietario}</h3>
        </div>
        <div className="item-options">
          <button className="btnComponent item-option" onClick={handleClick}>
            Mostrar mais
          </button>
          <Menu anchorEl={anchorEl} open={open} onClose={handleClose}>
            <MenuItem className="item-option" onClick={handleClose}>
              <Link className="menu-option" to={`/post/${itemData.idPost}`}>
                Comprar novamente
              </Link>
            </MenuItem>
            <MenuItem className="item-option" onClick={handleClose}>
              <Link className="menu-option" to={`/post/${itemData.idPost}`}>
                Avaliar produto
              </Link>
            </MenuItem>
            <MenuItem onClick={handleClose}>
              <button className="menu-option">Ver produto</button>
            </MenuItem>
            <MenuItem id="deletar" className="menu-option">
              <button onClick={() => DeleteItem(itemData.idInventario)} className="menu-option">
                Deletar
              </button>
            </MenuItem>
          </Menu>
        </div>
      </div>
    </div>
  );
}
