import React, { useEffect, useReducer, useState } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import { ToastContainer, toast } from 'react-toastify';
import { Box, Rating } from '@mui/material';
import { TabContext, TabPanel } from '@mui/lab';

import api, { IMGROOT } from '../../services/api';
import { parseJwt } from '../../services/auth';
import reducer from '../../services/reducer';

import { PostProps } from '../../@types/Post';
import { CommentProps } from '../../@types/Comment';
import { RatingHistograma } from '../../@types/RatingHistogram';

import AddBookmarkIcon from '../../assets/img/bookmark-add_icon.svg';
import AddedBookmarkIcon from '../../assets/img/bookmark-added_icon.svg';

import { CustomTab, CustomTabs } from '../../assets/styledComponents/tabNavigator';
import Header from '../../components/Header';
import Footer from '../../components/Footer';
import SobrePost from '../../components/MarketPlace/SobrePost';
import AvaliacoesPost from '../../components/MarketPlace/Avaliacoes/AvaliacoesPost';
import RecomendadosPost from '../../components/MarketPlace/RecomendadosPost';
import ModalTransacao from '../../components/ModalEfetuarTransacao';

export interface WishlishedPost {
  idUsuario: number;
  idPost: number;
}

export default function Post({ tabID }: { tabID?: string }) {
  const { idPost } = useParams();
  const [PostData, setPost] = useState<PostProps>();
  const [Comments, setComments] = useState<CommentProps[]>([]);
  const [CommentsHistograma, setCommentsHistograma] = useState<RatingHistograma[]>([]);
  const [canComment, setCanComment] = useState<boolean>(false);
  const [isWishlisted, setWishlisted] = useState<boolean>(false);
  const [TabID, setTab] = useState(tabID ?? '1');

  const navigate = useNavigate();

  const updateStage = {
    count: 0,
  };

  const [updates, dispatch] = useReducer(reducer, updateStage);

  const VerifyIfWishlisted = (postData: PostProps) => {
    const db: WishlishedPost[] = localStorage.getItem('wishlist')
      ? JSON.parse(localStorage.getItem('wishlist') ?? '[]')
      : [];

    setWishlisted(db.some((item) => item.idPost === postData.idPost));
  };

  function GetComments(id: number) {
    api(
      `Avaliacoes/AvaliacoesPost/${id}/${
        parseJwt().role === 'undefined' ? 0 : parseJwt().role
      }/1/10`,
    ).then((response) => {
      if (response.status === 200) {
        setComments(response.data.avaliacoesList);
        setCommentsHistograma(response.data.ratingHistograma);
        setCanComment(response.data.canPostComment);
      }
    });
  }

  // function GetPost(id: string) {
  //   api(`Marketplace/${id}`).then((response) => {
  //     if (response.status === 200) {
  //       setPost(response.data);
  //       GetComments(response.data.idPost);
  //       VerifyIfWishlisted(response.data);
  //     } else {
  //       navigate('/404');
  //       console.log('batata');
  //     }
  //   });
  // }
  async function GetPost(id: string) {
    try {
      const response = await api(`Marketplace/${id}`);
      if (response.status === 200) {
        setPost(response.data);
        GetComments(response.data.idPost);
        VerifyIfWishlisted(response.data);
      }
    } catch (error) {
      navigate('/404');
    }
  }

  const AddToWishlist = () => {
    const db: WishlishedPost[] = localStorage.getItem('wishlist')
      ? JSON.parse(localStorage.getItem('wishlist') ?? '[]')
      : [];
    if (PostData !== undefined) {
      db.push({ idPost: PostData.idPost, idUsuario: parseJwt().role });
      localStorage.setItem('wishlist', JSON.stringify(db));
      setWishlisted(true);
      toast.success('Adicionado á Wishlist');
    }
  };

  const RemoveFromWishlist = () => {
    const db: WishlishedPost[] = localStorage.getItem('wishlist')
      ? JSON.parse(localStorage.getItem('wishlist') ?? '[]')
      : [];

    if (PostData !== undefined) {
      const updatedDb = db.filter((item) => item.idPost !== PostData.idPost);

      localStorage.setItem('wishlist', JSON.stringify(updatedDb));
      setWishlisted(false);
      toast.success('Removido da lista de desejos');
    }
  };

  useEffect(() => {
    GetPost(idPost ?? '0');
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [idPost, updates.count]);

  let postContent;
  if (parseJwt().role !== 'undefined') {
    if (PostData?.idUsuario.toString() !== parseJwt().role && PostData !== undefined) {
      postContent = (
        <>
          {/* Elementos JSX */}
          <ModalTransacao
            data={{
              titulo: `Confirmar compra de ${PostData.nome}?`,
              valor: PostData.valor,
              destino: parseInt(idPost ?? '0', 10),
              img: PostData.mainImg,
              mainColorHex: PostData.mainColorHex,
            }}
            type="post"
            onClose={() => GetPost(idPost ?? '0')}
          />
          <hr id="separador" />
          {isWishlisted === true ? (
            <button id="favoritar__btn" className="btnPressionavel" onClick={RemoveFromWishlist}>
              <img alt="Botão remover produto da lista de desejos" src={AddedBookmarkIcon} />
              <span>Lista de desejos</span>
            </button>
          ) : (
            <button id="favoritar__btn" className="btnPressionavel" onClick={AddToWishlist}>
              <img alt="Botão adicionar produto para a lista de desejos" src={AddBookmarkIcon} />
              <span>Lista de desejos</span>
            </button>
          )}
        </>
      );
    } else {
      postContent = (
        <div className="optionVendas">
          <p>
            Total de Vendas: <span>{PostData?.vendas}</span>
          </p>
        </div>
      );
    }
  } else {
    postContent = (
      <div className="loginPost">
        <span>Faça login ou cadastre-se para comprar o produto</span>
        <div className="boxBtnsLog">
          <Link to="/login" className="btnLogin">
            Login
          </Link>
          <Link to="/cadastro" className="btnCadastre">
            Cadastre-se
          </Link>
        </div>
      </div>
    );
  }

  return (
    <div>
      <ToastContainer position="top-center" autoClose={1800} />
      <Header type="" />
      <main id="post">
        {/* Banner do post */}
        <section className="support-banner">
          <img
            id="fundo-banner"
            alt="Imagem de fundo do produto"
            src={`${IMGROOT}/${PostData?.mainImg}`}
          />
          <div className="infos-banner container">
            <h1>{PostData?.nome}</h1>
            <div className="post-stats-support">
              <img
                id="logo-frame"
                alt="Logo do produto"
                src={`${IMGROOT}/${PostData?.mainImg}`}
                style={{ backgroundColor: `#${PostData?.mainColorHex}` }}
              />
              <div className="post-stats">
                <h3 id="titulo">{PostData?.apelidoProprietario}</h3>
                <hr id="separador" />
                <div id="avaliacao-support">
                  <div>
                    <span id="nota">{PostData?.avaliacao}</span>
                    <Rating value={PostData?.avaliacao ?? 0} precision={0.1} readOnly />
                  </div>
                  <span>{PostData?.qntAvaliacoes} avaliações</span>
                </div>
              </div>
            </div>
            <div className="post-actions">
              {postContent}
              {/* {PostData?.idUsuario.toString() !== parseJwt().role &&
              PostData !== undefined &&
              parseJwt().role !== 'undefined' ? (
                <>
                  <ModalTransacao
                    data={{
                      titulo: `Confirmar compra de ${PostData.nome}?`,
                      valor: PostData.valor,
                      destino: parseInt(idPost ?? '0', 10),
                      img: PostData.mainImg,
                      mainColorHex: PostData.mainColorHex,
                    }}
                    type="post"
                    onClose={() => GetPost(idPost ?? '0')}
                  />
                  <hr id="separador" />
                  {isWishlisted === true ? (
                    <button
                      id="favoritar__btn"
                      className="btnPressionavel"
                      onClick={RemoveFromWishlist}
                    >
                      <img
                        alt="Botão remover produto da lista de desejos"
                        src={AddedBookmarkIcon}
                      />
                      <span>Lista de desejos</span>
                    </button>
                  ) : (
                    <button id="favoritar__btn" className="btnPressionavel" onClick={AddToWishlist}>
                      <img
                        alt="Botão adicionar produto para a lista de desejos"
                        src={AddBookmarkIcon}
                      />
                      <span>Lista de desejos</span>
                    </button>
                  )}
                </>
              ) : parseJwt().role !== 'undefined' ? (
                <div className="optionVendas">
                  <p>
                    Total de Vendas: <span>{PostData?.vendas}</span>
                  </p>
                </div>
              ) : (
                // <span>faça login para comprar o produto</span>
              )} */}
            </div>
          </div>
        </section>
        <section className="post-infos container">
          <TabContext value={TabID}>
            <Box>
              <CustomTabs
                value={TabID}
                onChange={(evt, value) => setTab(value)}
                aria-label="Barra de navegação"
                variant="fullWidth"
              >
                <CustomTab label="Sobre" value="1" />
                <CustomTab label="Avaliações" value="2" />
                <CustomTab label="Recomendados" value="3" />
              </CustomTabs>
            </Box>
            <TabPanel value="1">
              <SobrePost postProps={PostData} />
            </TabPanel>
            <TabPanel value="2">
              <h2>Avaliações</h2>
              <AvaliacoesPost
                dispatch={dispatch}
                postProps={PostData}
                comments={Comments}
                canComment={canComment}
                commentsHistograma={CommentsHistograma}
              />
            </TabPanel>
            <TabPanel value="3">
              <RecomendadosPost postprops={PostData} />
            </TabPanel>
          </TabContext>
        </section>
      </main>
      <Footer />
    </div>
  );
}

Post.defaultProps = {
  tabID: '1',
};
