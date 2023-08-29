import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import { Menu, MenuItem } from '@mui/material';

import LockIcon from '../../assets/img/lock_icon_light.svg';

import UnLockIcon from '../../assets/img/unlock_icon_light.svg';

import api, { IMGROOT } from '../../services/api';
import { ItemProps } from '../../@types/Inventario';
import ModalViewItem from './ModalViewItem';
import { PostProps } from '../../@types/Post';
import { useSnackBar } from '../../services/snackBarProvider';

export function ItemInventario({
  itemData,
  onDelete,
}: {
  itemData: ItemProps;
  onDelete: () => void;
}) {
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const open = Boolean(anchorEl);

  const { postMessage } = useSnackBar();

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
        postMessage({ message: 'Item deletado', severity: 'success', timeSpan: 3000 });
        onDelete();
      }
    });
  }

  return (
    <div className="inventario-item">
      <div
        className="support-logo"
        style={{ backgroundColor: `#${itemData.idPostNavigation.mainColorHex}` }}
      >
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
              <ModalViewItem key={itemData.idInventario} idPost={itemData.idInventario} />
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

export function ItemPublicados({
  itemData,
  onUpdate,
}: {
  itemData: PostProps;
  onUpdate: () => void;
}) {
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const open = Boolean(anchorEl);

  const { postMessage } = useSnackBar();

  const handleClick = (event: React.MouseEvent<HTMLButtonElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleClose = () => {
    setAnchorEl(null);
  };

  function DeleteItem(idItem: number) {
    api.delete(`Marketplace/${idItem}`).then((response) => {
      if (response.status === 204) {
        handleClose();
        postMessage({ message: 'Publicação deletada', severity: 'success', timeSpan: 2000 });
        onUpdate();
      }
    });
  }

  const PrivarPost = (idPost: number) => {
    api.patch(`Marketplace/Privar/${idPost}`).then((response) => {
      if (response.status === 200) {
        postMessage({ message: 'Publicação privada', severity: 'success', timeSpan: 2000 });
        onUpdate();
      }
    });
  };

  const DesprivarPost = (idPost: number) => {
    api.patch(`Marketplace/Desprivar/${idPost}`).then((response) => {
      if (response.status === 200) {
        postMessage({ message: 'Publicação desprivada', severity: 'success', timeSpan: 2000 });
        onUpdate();
      }
    });
  };

  return (
    <div className="inventario-item">
      <div className="support-logo" style={{ backgroundColor: `#${itemData.mainColorHex}` }}>
        <img alt="Imagem do Item" src={`${IMGROOT}/${itemData.mainImg}`} />
      </div>
      <div className="item-infos-support">
        <div className="item-infos">
          <h2>{itemData.nome}</h2>
          <h3>{itemData.apelidoProprietario}</h3>
        </div>
        <div className="item-options">
          <img
            alt="Icone de visibilidade da postagem"
            src={`${itemData.isActive ? UnLockIcon : LockIcon}`}
          />
          <button className="btnComponent item-option" onClick={handleClick}>
            Mostrar mais
          </button>
          <Menu anchorEl={anchorEl} open={open} onClose={handleClose}>
            <MenuItem className="item-option" onClick={handleClose}>
              <Link className="menu-option" to={`/post/${itemData.idPost}`}>
                Ver produto
              </Link>
            </MenuItem>
            <MenuItem
              id={`${!itemData.isActive ? 'disabled' : ''}`}
              className="menu-option"
              onClick={handleClose}
            >
              <button
                className="menu-option"
                disabled={!itemData.isActive}
                onClick={() => PrivarPost(itemData.idPost)}
              >
                Privar produto
              </button>
            </MenuItem>
            <MenuItem
              id={`${itemData.isActive ? 'disabled' : ''}`}
              className="menu-option"
              onClick={handleClose}
            >
              <button
                className="menu-option"
                disabled={itemData.isActive}
                onClick={() => DesprivarPost(itemData.idPost)}
              >
                Desprivar produto
              </button>
            </MenuItem>
            <MenuItem id="deletar" className="menu-option">
              <button onClick={() => DeleteItem(itemData.idPost)} className="menu-option">
                Deletar
              </button>
            </MenuItem>
          </Menu>
        </div>
      </div>
    </div>
  );
}
