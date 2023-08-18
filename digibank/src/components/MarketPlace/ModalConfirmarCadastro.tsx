// eslint-disable-next-line eslint-comments/disable-enable-pair
/* eslint-disable jsx-a11y/no-static-element-interactions */
// eslint-disable-next-line eslint-comments/disable-enable-pair
/* eslint-disable jsx-a11y/click-events-have-key-events */
import { Palette } from 'color-thief-react';
import { Dialog } from '@mui/material';
import React, { useState } from 'react';

import imgDefault from '../../assets/img/ImgDefault.png';
import { PostProps } from '../../@types/Post';

export default function ModalPreviewPost({
  postData,
  isTransparente,
  mainImg,
  canPost,
  onChangeColor,
}: {
  postData: PostProps;
  isTransparente: boolean;
  mainImg: string;
  canPost: boolean;
  onChangeColor: (color: string) => void;
}) {
  const [open, setOpen] = useState<boolean>(false);

  const handleClickModal = () => {
    setOpen(true);
  };

  const handleCloseModal = () => {
    setOpen(false);
  };

  return (
    <div>
      <button type="button" onClick={handleClickModal}>
        Cadastrar
      </button>
      <Dialog open={open} onClose={handleCloseModal}>
        <div className="support-modal-preview">
          <div className="preview-post">
            <div className="recomendado-support preview">
              <div className="postImgCad">
                {/* eslint-disable-next-line no-nested-ternary */}
                {mainImg && isTransparente === true ? (
                  <div style={{ backgroundColor: '#000' }}>
                    <img src={mainImg} alt="Imagem selecionada" />
                  </div>
                ) : mainImg && isTransparente === false ? (
                  <div>
                    <img
                      style={{ backgroundColor: postData.mainColorHex, borderRadius: '10px' }}
                      src={mainImg}
                      alt="Imagem selecionada"
                    />
                  </div>
                ) : (
                  <img src={imgDefault} alt="imagem banner default" />
                )}
              </div>
              <div className="recomendado-infos">
                <div>
                  {postData.nome ? <h3>{postData.nome}</h3> : <h3>Titulo do produto</h3>}
                  <h4>{postData.apelidoProprietario}</h4>
                </div>
                <div className="avaliacao-recomendado">
                  <h5>{postData.valor}BRL</h5>
                </div>
              </div>
            </div>

            <Palette src={mainImg} colorCount={4} format="hex" quality={1}>
              {({ data }) => {
                const colorList: string[] = [];
                if (data) {
                  // eslint-disable-next-line array-callback-return
                  data.map((color) => {
                    colorList.push(color);
                  });
                }
                return (
                  <div className="color-options">
                    {colorList.map((color: string) => (
                      <div
                        onClick={() => onChangeColor(color)}
                        className="color-option"
                        style={{ backgroundColor: color }}
                      />
                    ))}
                    <div
                      onClick={() => onChangeColor('#000')}
                      className="color-option"
                      style={{ backgroundColor: '#000' }}
                    />
                    <div
                      onClick={() => onChangeColor('#FFF')}
                      className="color-option"
                      style={{ backgroundColor: '#FFF' }}
                    />
                  </div>
                );
              }}
            </Palette>
          </div>
          <button form="post" type="submit" disabled={!canPost} className="btnComponent">
            Cadastrar
          </button>
        </div>
      </Dialog>
    </div>
  );
}
