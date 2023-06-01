// eslint-disable-next-line @typescript-eslint/no-unused-vars
import React, { useEffect, useState } from 'react';

// import { useParams } from 'react-router-dom';

// import Color from 'color-thief-react';
// import verificaTransparenciaImagem from '../../services/img';
// import { Rating } from '@mui/material';

// import { PostProps } from '../../@types/Post';
// import { CommentProps } from '../../@types/Comment';
import Header from '../../components/Header';
import Footer from '../../components/Footer';
// import api from '../../services/api';
// import { parseJwt } from '../../services/auth';
// import { CssTextField } from '../../assets/styledComponents/input';

import StarIcon from '../../assets/img/star_icon.svg';
import AddBookmarkIcon from '../../assets/img/bookmark-add_icon.svg';
import PostIcon from '../../assets/img/store-post.png';

// import SettingsIcon from '../../assets/img/list_icon.svg';

export default function Post() {
  // const { idPost } = useParams();
  // const [PostData, setPost] = useState<PostProps>();
  // const [Comments, setComments] = useState<CommentProps[]>([]);
  // const [nota, setNota] = useState<number | null>();
  // const [comentario, setComentario] = useState<string>();
  // const [isImgTransparent, setTransparent] = useState<boolean>(false);

  // function GetComments(id: number) {
  //   api(`Avaliacoes/AvaliacoesPost/${id}/1/10`).then((response) => {
  //     if (response.status === 200) {
  //       setComments(response.data);
  //     }
  //   });
  // }

  // function GetPost(id: string | undefined) {
  //   api(`Marketplace/${id}`).then((response) => {
  //     if (response.status === 200) {
  //       setPost(response.data);
  //       GetComments(response.data.idPost);

  //       // verificaTransparenciaImagem(`http://localhost:5000/img/${PostData?.mainImg}`).then(
  //       //   (isTransparent) => {
  //       //     if (isTransparent) {
  //       //       setTransparent(true);
  //       //     }
  //       //   },
  //       // );
  //     }
  //   });
  // }

  // function ComprarPost() {
  //   api.post(`Marketplace/Comprar/${idPost}/${parseJwt().role}`).then((response) => {
  //     if (response.status === 200) {
  //       GetPost(idPost);
  //     }
  //   });
  // }

  // function PostComentario(evt: any) {
  //   evt.preventDefault();

  //   api
  //     .post(`Avaliacoes`, {
  //       idUsuario: parseJwt().role,
  //       idPost: PostData?.idPost,
  //       nota,
  //       comentario,
  //     })
  //     .then((response) => {
  //       if (response.status === 201) {
  //         GetPost(idPost);
  //       }
  //     })
  //     .catch((error) => console.log(error));
  //   setComentario('');
  //   setNota(0);
  // }

  // useEffect(() => {
  //   GetPost(idPost);
  //   // eslint-disable-next-line react-hooks/exhaustive-deps
  // }, []);

  return (
    <div>
      <Header type="" />
      <main id="post">
        <section className="support-banner">
          <img
            id="fundo-banner"
            alt="Imagem de fundo do produto"
            src="https://images.wondershare.com/recoverit/article/2020/08/unhide-songs-on-spotify-1.jpg"
          />
          <div className="infos-banner container">
            <h1>3 Meses de Spotify</h1>
            <div className="post-stats-support">
              <img id="logo-frame" alt="Logo do produto" src={PostIcon} />
              <div className="post-stats">
                <h3 id="titulo">Digibank</h3>
                <hr id="separador" />
                <div id="avaliacao-support">
                  <div>
                    <span>4,3</span>
                    <img alt="Estrela de avaliação" src={StarIcon} />
                  </div>
                  <span>4,89 mil avaliações</span>
                </div>
              </div>
            </div>
            <div className="post-actions">
              <button id="adquirir__btn">29,80 BRL</button>
              <hr id="separador" />
              <button id="favoritar__btn">
                <img alt="Botão adicionar produto à lista de desejos" src={AddBookmarkIcon} />
                <span>Lista de desejos</span>
              </button>
            </div>
          </div>
        </section>
      </main>
      <Footer />
    </div>
  );
}
