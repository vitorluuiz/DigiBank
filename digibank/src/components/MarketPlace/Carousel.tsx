import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { Skeleton } from '@mui/material';
import seta from '../../assets/img/setaCarousel.svg';
// import { PostBlock } from './PostBlock';
import api from '../../services/api';
import RecommendedBlock from './RecommendedPost';
import { GaleriaProps, PostProps } from '../../@types/Post';
import GaleriaBlock from './GaleriaBlock';

const Carousel: React.FC<{ type: string; postprops?: PostProps }> = ({ type, postprops }) => {
  const [currentIndex, setCurrentIndex] = useState(0);
  const [galeria, setGaleria] = useState<GaleriaProps[]>([{ img: '' }]);
  const [listaAvaliados, setListaAvaliados] = useState<PostProps[]>([]);
  const [listaAnunciante, setListaAnunciante] = useState<PostProps[]>([]);
  const [listaVendas, setListaVendas] = useState<PostProps[]>([]);
  // const [showSkeleton, setShowSkeleton] = useState(true);
  const { idPost } = useParams();
  const [loading, setLoading] = useState(true);
  // console.log(idPost);

  /// //////////////////////// FUNCTION LISTAR
  useEffect(() => {
    // Simulando a requisição de lista de vendas
    function listarPostsVendas() {
      api
        .get(`Marketplace/${1}/${9}/Vendas`, {
          headers: {
            Authorization: `Bearer ${localStorage.getItem('usuario-login-auth')}`,
          },
        })
        .then((resposta) => {
          if (resposta.status === 200) {
            setListaVendas(resposta.data);
            // console.log(resposta.data);
            setLoading(false); // Atualiza o estado para interromper a exibição do Skeleton
          }
        })
        .catch((erro) => console.log(erro));
    }

    listarPostsVendas();
  }, [type]);

  function ListarPostsAvaliacao() {
    api
      .get(`Marketplace/${1}/${12}/Avaliacao`, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('usuario-login-auth')}`,
        },
      })

      .then((resposta) => {
        if (resposta.status === 200) {
          setListaAvaliados(resposta.data);
          // console.log(resposta.data);
        }
      })

      .catch((erro) => console.log(erro));
  }

  function ListarPostsId(id: string | undefined) {
    api
      .get(`Marketplace/${id}`, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('usuario-login-auth')}`,
        },
      })
      .then((resposta) => {
        if (resposta.status === 200) {
          setGaleria(resposta.data.imgs);
        }
      })

      .catch((erro) => console.log(erro));
  }

  useEffect(() => {
    ListarPostsAvaliacao();
  }, []);

  useEffect(() => {
    function GetDeProprietario() {
      api(`Marketplace/Usuario/${postprops?.idUsuario}`).then((response) => {
        if (response.status === 200) {
          setListaAnunciante(response.data);
        }
      });
    }
    GetDeProprietario();
  }, [postprops]);

  /// //////////////////////// LISTAR IMAGES DESTAQUE

  function renderImages() {
    const slicedImages = listaVendas.slice(currentIndex, currentIndex + 3);

    return slicedImages.map((post) => (
      <RecommendedBlock type="Big" key={post.idPost} post={post} />
    ));
  }
  const handleClickNext = () => {
    setCurrentIndex((prevIndex) => (prevIndex + 3) % listaVendas.length);
  };

  const handleClickPrev = () => {
    setCurrentIndex((prevIndex) => {
      const newIndex = prevIndex - 3;
      return newIndex < 0 ? listaVendas.length + newIndex : newIndex;
    });
  };

  // const renderPaginationDots = () => {
  //   const dotsCount = Math.ceil(listaVendas.length / 3);

  //   return Array.from({ length: dotsCount }).map((_, index) => {
  //     const pageNumber = index + 1;
  //     const isActive = currentIndex === index * 3;
  //     const dotKey = `dot-${pageNumber}`;

  //     return (
  //       <button
  //         key={dotKey}
  //         className={`dot ${isActive ? 'active' : ''}`}
  //         onClick={() => setCurrentIndex(index * 3)}
  //         aria-label={`Página ${pageNumber}`}
  //       >
  //         {/* {pageNumber} */}
  //       </button>
  //     );
  //   });
  // };
  function renderImagesAnunciante() {
    const slicedImages = listaAnunciante.slice(currentIndex, currentIndex + 4);

    return slicedImages.map((post) => (
      // <PostBlockSlim
      //   key={image.idPost}
      //   img={`http://localhost:5000/img/${image.mainImg}`}
      //   link={`/post/${image.idPost}`}
      // />
      <RecommendedBlock type="slim" key={post.idPost} post={post} />
    ));
  }
  const handleClickNextAnunciante = () => {
    setCurrentIndex((prevIndex) => (prevIndex + 4) % listaAnunciante.length);
  };

  const handleClickPrevAnunciante = () => {
    setCurrentIndex((prevIndex) => {
      const newIndex = prevIndex - 4;
      return newIndex < 0 ? listaAnunciante.length + newIndex : newIndex;
    });
  };

  // const renderPaginationDotsAnunciante = () => {
  //   const dotsCount = Math.ceil(listaAnunciante.length / 4);

  //   return Array.from({ length: dotsCount }).map((_, index) => {
  //     const pageNumber = index + 1;
  //     const isActive = currentIndex === index * 4;
  //     const dotKey = `dot-${pageNumber}`;

  //     return (
  //       <button
  //         key={dotKey}
  //         className={`dot ${isActive ? 'active' : ''}`}
  //         onClick={() => setCurrentIndex(index * 4)}
  //         aria-label={`Página ${pageNumber}`}
  //       >
  //         {/* {pageNumber} */}
  //       </button>
  //     );
  //   });
  // };
  /// //////////////////////// LISTAR IMAGES SLIM
  function renderImagesSlim() {
    const slicedImages = listaAvaliados.slice(currentIndex, currentIndex + 4);

    return slicedImages.map((post) => (
      <RecommendedBlock type="slim" key={post.idPost} post={post} />
    ));
  }

  const handleClickNextSlim = () => {
    setCurrentIndex((prevIndex) => (prevIndex + 4) % listaAvaliados.length);
  };

  const handleClickPrevSlim = () => {
    setCurrentIndex((prevIndex) => {
      const newIndex = prevIndex - 4;
      return newIndex < 0 ? listaAvaliados.length + newIndex : newIndex;
    });
  };

  // const renderPaginationDotsSlim = () => {
  //   const dotsCount = Math.ceil(listaAvaliados.length / 4);

  //   return Array.from({ length: dotsCount }).map((_, index) => {
  //     const pageNumber = index + 1;
  //     const isActive = currentIndex === index * 4;
  //     const dotKey = `dot-${pageNumber}`;

  //     return (
  //       <button
  //         key={dotKey}
  //         className={`dot ${isActive ? 'active' : ''}`}
  //         onClick={() => setCurrentIndex(index * 4)}
  //         aria-label={`Página ${pageNumber}`}
  //       >
  //         {/* {pageNumber} */}
  //       </button>
  //     );
  //   });
  // };

  /// //////////////////////// LISTAR IMAGES GALERIA
  function renderImagesGaleria() {
    const slicedImages = galeria.slice(currentIndex, currentIndex + 4);

    return slicedImages?.map((imagem) => (
      <GaleriaBlock key={slicedImages.indexOf(imagem)} galeria={imagem} />
    ));
  }

  const handleClickNextGaleria = () => {
    setCurrentIndex((prevIndex) => (prevIndex + 4) % galeria.length);
  };

  const handleClickPrevGaleria = () => {
    setCurrentIndex((prevIndex) => {
      const newIndex = prevIndex - 4;
      return newIndex < 0 ? galeria.length + newIndex : newIndex;
    });
  };

  // const renderPaginationDotsGaleria = () => {
  //   const dotsCount = Math.ceil(galeria.length / 4);

  //   return Array.from({ length: dotsCount }).map((_, index) => {
  //     const pageNumber = index + 1;
  //     const isActive = currentIndex === index * 4;
  //     const dotKey = `dot-${pageNumber}`;

  //     return (
  //       <button
  //         key={dotKey}
  //         className={`dot ${isActive ? 'active' : ''}`}
  //         onClick={() => setCurrentIndex(index * 4)}
  //         aria-label={`Página ${pageNumber}`}
  //       >
  //         {/* {pageNumber} */}
  //       </button>
  //     );
  //   });
  // };

  useEffect(() => {
    ListarPostsId(idPost);
  }, [idPost]);

  // useEffect(() => {
  //   const skeletonDuration = 3000;
  //   const timeoutId = setTimeout(() => {
  //     setShowSkeleton(false);
  //   }, skeletonDuration);
  //   return () => clearTimeout(timeoutId);
  // }, []);
  // if (showSkeleton) {
  //   return (
  //     <div style={{ display: 'flex', justifyContent: 'space-between', gap: 20 }}>
  //       <Skeleton variant="rectangular" width="20%" height={445} />
  //       <Skeleton variant="rectangular" width="20%" height={445} />
  //       <Skeleton variant="rectangular" width="20%" height={445} />
  //     </div>
  //   );
  // }

  /// //////////////////////// RETURNS
  if (loading) {
    return (
      <div style={{ display: 'flex', justifyContent: 'space-between', height: '100%' }}>
        {/* Renderizar o Skeleton do tamanho e estilo desejados */}
        <div style={{ width: '20%', height: '100%' }}>
          <Skeleton variant="rectangular" width="80%" height={225} />
          <Skeleton variant="text" width="75%" sx={{ fontSize: '3rem' }} />
          <Skeleton variant="text" width="25%" sx={{ fontSize: '2rem' }} />
        </div>
        <div style={{ width: '20%', height: '100%' }}>
          <Skeleton variant="rectangular" width="80%" height={225} />
          <Skeleton variant="text" width="75%" sx={{ fontSize: '3rem' }} />
          <Skeleton variant="text" width="25%" sx={{ fontSize: '2rem' }} />
        </div>
        <div style={{ width: '20%', height: '100%' }}>
          <Skeleton variant="rectangular" width="80%" height={225} />
          <Skeleton variant="text" width="75%" sx={{ fontSize: '3rem' }} />
          <Skeleton variant="text" width="25%" sx={{ fontSize: '2rem' }} />
        </div>
        <div style={{ width: '20%', height: '100%' }}>
          <Skeleton variant="rectangular" width="80%" height={225} />
          <Skeleton variant="text" width="75%" sx={{ fontSize: '3rem' }} />
          <Skeleton variant="text" width="25%" sx={{ fontSize: '2rem' }} />
        </div>
      </div>
    );
  }
  if (type === 'normal') {
    return (
      <div id="mainCarousel">
        <div className="suport-carousel">
          <button
            className="prevButton btnCarousel"
            onClick={handleClickPrev}
            disabled={currentIndex === 0}
          >
            <img src={seta} alt="seta voltar Carousel" />
          </button>
          {renderImages()}
          <button
            className="nextButton btnCarousel"
            onClick={handleClickNext}
            disabled={currentIndex + 3 >= listaVendas.length}
          >
            <img src={seta} alt="seta voltar Carousel" />
          </button>
        </div>
        {/* <div className="bottomCarousel">
          <div className="pagination-dots">{renderPaginationDots()}</div>
        </div> */}
      </div>
    );
  }
  if (type === 'slim') {
    return (
      <div id="mainCarousel">
        <div className="suport-carousel">
          <button
            className="prevButton btnCarousel"
            onClick={handleClickPrevSlim}
            disabled={currentIndex === 0}
          >
            <img src={seta} alt="seta voltar Carousel" />
          </button>
          {renderImagesSlim()}
          <button
            className="nextButton btnCarousel"
            onClick={handleClickNextSlim}
            disabled={currentIndex + 5 >= listaAvaliados.length}
          >
            <img src={seta} alt="seta voltar Carousel" />
          </button>
        </div>
        {/* <div className="bottomCarousel">
          <div className="pagination-dots">{renderPaginationDotsSlim()}</div>
        </div> */}
      </div>
    );
  }
  if (type === 'recomendados') {
    return (
      <div id="mainCarousel">
        <div className="suport-carousel">
          <button
            className="prevButton btnCarousel"
            onClick={handleClickPrevSlim}
            disabled={currentIndex === 0}
          >
            <img src={seta} alt="seta voltar Carousel" />
          </button>
          {renderImagesSlim()}
          <button
            className="nextButton btnCarousel"
            onClick={handleClickNextSlim}
            disabled={currentIndex + 4 >= listaAvaliados.length}
          >
            <img src={seta} alt="seta voltar Carousel" />
          </button>
        </div>
        {/* <div className="bottomCarousel">
          <div className="pagination-dots">{renderPaginationDotsSlim()}</div>
        </div> */}
      </div>
    );
  }
  if (type === 'galeria') {
    const shouldRenderImages = galeria.some(() => galeria && galeria.length > 0);

    if (!shouldRenderImages) {
      return null;
    }
    return (
      <div className="galeria-post">
        <h2>Galeria</h2>
        <div className="support-galeria-post">
          <div id="mainCarousel">
            <div className="suport-carousel">
              <button
                className="prevButton btnCarousel"
                onClick={handleClickPrevGaleria}
                disabled={currentIndex === 0}
              >
                <img src={seta} alt="seta voltar Carousel" />
              </button>
              {renderImagesGaleria()}
              <button
                className="nextButton btnCarousel"
                onClick={handleClickNextGaleria}
                disabled={currentIndex + 4 >= galeria.length}
              >
                <img src={seta} alt="seta voltar Carousel" />
              </button>
            </div>
            {/* <div className="bottomCarousel">
              <div className="pagination-dots">{renderPaginationDotsGaleria()}</div>
            </div> */}
          </div>
        </div>
      </div>
    );
  }
  if (type === 'anunciante') {
    return (
      <div id="mainCarousel">
        <div className="suport-carousel">
          <button
            className="prevButton btnCarousel"
            onClick={handleClickPrevAnunciante}
            disabled={currentIndex === 0}
          >
            <img src={seta} alt="seta voltar Carousel" />
          </button>
          {renderImagesAnunciante()}
          <button
            className="nextButton btnCarousel"
            onClick={handleClickNextAnunciante}
            disabled={currentIndex + 4 >= listaAnunciante.length}
          >
            <img src={seta} alt="seta voltar Carousel" />
          </button>
        </div>
        {/* <div className="bottomCarousel">
          <div className="pagination-dots">{renderPaginationDotsAnunciante()}</div>
        </div> */}
      </div>
    );
  }
  return null;
};
export default Carousel;

Carousel.defaultProps = {
  postprops: undefined,
};
// nao fazer requisicao
