import React, { useEffect, useState } from 'react';

import { useParams } from 'react-router-dom';

// import Color from 'color-thief-react';
// import verificaTransparenciaImagem from '../../services/img';
import { Rating } from '@mui/material';

import { PostProps } from '../../@types/Post';
import { CommentProps } from '../../@types/Comment';

import Header from '../../components/Header';
import Footer from '../../components/Footer';
import api from '../../services/api';
import { parseJwt } from '../../services/auth';
import { CssTextField } from '../../assets/styledComponents/input';

import StarIcon from '../../assets/img/star_icon.svg';
import SettingsIcon from '../../assets/img/list_icon.svg';

export default function Post() {
  const { idPost } = useParams();
  const [PostData, setPost] = useState<PostProps>();
  const [Comments, setComments] = useState<CommentProps[]>([]);
  const [nota, setNota] = useState<number | null>();
  const [comentario, setComentario] = useState<string>();
  // const [isImgTransparent, setTransparent] = useState<boolean>(false);

  function GetComments(id: number) {
    api(`Avaliacoes/AvaliacoesPost/${id}/1/10`).then((response) => {
      if (response.status === 200) {
        setComments(response.data);
      }
    });
  }

  function GetPost(id: string | undefined) {
    api(`Marketplace/${id}`).then((response) => {
      if (response.status === 200) {
        setPost(response.data);
        GetComments(response.data.idPost);

        // verificaTransparenciaImagem(`http://localhost:5000/img/${PostData?.mainImg}`).then(
        //   (isTransparent) => {
        //     if (isTransparent) {
        //       setTransparent(true);
        //     }
        //   },
        // );
      }
    });
  }

  function ComprarPost() {
    api.post(`Marketplace/Comprar/${idPost}/${parseJwt().role}`).then((response) => {
      if (response.status === 200) {
        GetPost(idPost);
      }
    });
  }

  function PostComentario(evt: any) {
    evt.preventDefault();

    api
      .post(`Avaliacoes`, {
        idUsuario: parseJwt().role,
        idPost: PostData?.idPost,
        nota,
        comentario,
      })
      .then((response) => {
        if (response.status === 201) {
          GetPost(idPost);
        }
      })
      .catch((error) => console.log(error));
    setComentario('');
    setNota(0);
  }

  useEffect(() => {
    GetPost(idPost);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <div>
      <Header type="" />
      <main id="post" className="container">
        <h1>{PostData?.nome}</h1>
        <div className="support-post">
          <section className="post-section">
            <div className="banner-post">
              <img
                alt="Imagem da postagem"
                src={`http://localhost:5000/img/${PostData?.mainImg}`}
              />
              <div className="banner-infos">
                <span>{PostData?.vendas} pedidos</span>
                <span>#{PostData?.idPost}</span>
              </div>
            </div>
            {/* {isImgTransparent ? (
              <div className="banner-post">
                <img
                  alt="Imagem da postagem"
                  src={`http://localhost:5000/img/${PostData?.mainImg}`}
                />
                <div className="banner-infos">
                  <span>{PostData?.vendas} pedidos</span>
                  <span>#{PostData?.idPost}</span>
                </div>
              </div>
            ) : (
              <Color src={`http://localhost:5000/img/${PostData?.mainImg}`} format="hex">
                {({ data }) => (
                  <div className="banner-post" style={{ backgroundColor: data }}>
                    <img
                      alt="Imagem da postagem"
                      src={`http://localhost:5000/img/${PostData?.mainImg}`}
                    />
                    <div className="banner-infos">
                      <span>{PostData?.vendas} pedidos</span>
                      <span>#{PostData?.idPost}</span>
                    </div>
                  </div>
                )}
              </Color>
            )} */}
            <div className="infos-post">
              <div className="support-infos">
                <div className="name-post">
                  <h2>{PostData?.nome}</h2>
                  <p>{PostData?.descricao}</p>
                </div>
                <div className="stats-post">
                  <div>
                    <Rating
                      name="simple-controlled"
                      precision={0.1}
                      value={PostData?.avaliacao ?? 0}
                      readOnly
                    />
                  </div>
                </div>
              </div>
              <div className="comprar-post">
                <span>{PostData?.valor} BRL</span>
                <button onClick={ComprarPost} className="btnComponent">
                  Adquirir
                </button>
              </div>
            </div>
          </section>
          <section className="comments-section">
            <div className="comments-support">
              {Comments.map((comment) => (
                <div key={comment.idAvaliacao} className="comment">
                  <span>{comment.comentario}</span>
                  <div className="comment-rank">
                    <span>{comment.nota}</span>
                    <img alt="Avaliação do comentário" src={StarIcon} />
                    <button id="comment-menu">
                      <img alt="Configurações extras" src={SettingsIcon} />
                    </button>
                  </div>
                </div>
              ))}
            </div>
            <form onSubmit={PostComentario} className="post-comment">
              <div>
                <CssTextField
                  id="comentario"
                  variant="standard"
                  placeholder="Novo comentário"
                  value={comentario}
                  onChange={(evt) => setComentario(evt.target.value)}
                  required
                />
                <Rating
                  name="simple-controlled"
                  precision={0.5}
                  value={nota ?? 0}
                  onChange={(evt, value) => setNota(value)}
                />
              </div>
              <button className="btnComponent">Publicar comentário</button>
            </form>
          </section>
        </div>
      </main>
      <Footer />
    </div>
  );
}
