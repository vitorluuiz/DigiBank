import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import seta from '../../assets/img/setaCarousel.svg';
// import SkeletonComponent from '../../Skeleton';
// import { PostBlock } from './PostBlock';
import api from '../../../services/api';
import RecommendedBlock from '../RecommendedPost';
import { GaleriaProps, PostProps } from '../../../@types/Post';
import GaleriaBlock from '../GaleriaBlock';
import { parseJwt } from '../../../services/auth';

const Carousel: React.FC<{ type: string; postprops?: PostProps }> = ({ type, postprops }) => {
  const [currentIndex, setCurrentIndex] = useState(0);
  const [galeria, setGaleria] = useState<GaleriaProps[]>([{ img: '' }]);
  const [listaAvaliados, setListaAvaliados] = useState<PostProps[]>([]);
  const [listaAnunciante, setListaAnunciante] = useState<PostProps[]>([]);
  const [listaVendas, setListaVendas] = useState<PostProps[]>([]);
  const [listaComprarNovamente, setListaComprarNovamente] = useState<PostProps[]>([]);
  const [listaValor50, setListaValor50] = useState<PostProps[]>([]);
  const [listaValor25, setListaValor25] = useState<PostProps[]>([]);
  const [listaValor5, setListaValor5] = useState<PostProps[]>([]);
  // const [showSkeleton, setShowSkeleton] = useState(true);
  const { idPost } = useParams();
  // const [loading, setLoading] = useState(true);
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
            // setLoading(false); // Atualiza o estado para interromper a exibição do Skeleton
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
  function ListarComprarNovamente() {
    api
      .get(`Marketplace/${1}/${12}/comprados/${parseJwt().role}`, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('usuario-login-auth')}`,
        },
      })

      .then((resposta) => {
        if (resposta.status === 200) {
          setListaComprarNovamente(resposta.data);
          // console.log(resposta.data);
        }
      })

      .catch((erro) => console.log(erro));
  }
  function ListarPorValor() {
    api
      .get(`Marketplace/${1}/${12}/valor/${50}`, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('usuario-login-auth')}`,
        },
      })

      .then((resposta) => {
        if (resposta.status === 200) {
          setListaValor50(resposta.data);
          // console.log(resposta.data);
        }
      })

      .catch((erro) => console.log(erro));
    api
      .get(`Marketplace/${1}/${12}/valor/${25}`, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('usuario-login-auth')}`,
        },
      })

      .then((resposta) => {
        if (resposta.status === 200) {
          setListaValor25(resposta.data);
          // console.log(resposta.data);
        }
      });
    api
      .get(`Marketplace/${1}/${12}/valor/${5}`, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('usuario-login-auth')}`,
        },
      })

      .then((resposta) => {
        if (resposta.status === 200) {
          setListaValor5(resposta.data);
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
    ListarComprarNovamente();
  }, []);
  useEffect(() => {
    ListarPorValor();
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

  const handleClickNext = () => {
    setCurrentIndex((prevIndex) => (prevIndex + 3) % listaVendas.length);
  };

  const handleClickPrev = () => {
    setCurrentIndex((prevIndex) => {
      const newIndex = prevIndex - 3;
      return newIndex < 0 ? listaVendas.length + newIndex : newIndex;
    });
  };

  /// //////////////////////// LISTAR IMAGES SLIM

  const handleClickNextSlim = () => {
    setCurrentIndex((prevIndex) => (prevIndex + 4) % listaAvaliados.length);
  };

  const handleClickPrevSlim = () => {
    setCurrentIndex((prevIndex) => {
      const newIndex = prevIndex - 4;
      return newIndex < 0 ? listaAvaliados.length + newIndex : newIndex;
    });
  };

  /// //////////////////////// LISTAR IMAGES GALERIA

  const handleClickNextGaleria = () => {
    setCurrentIndex((prevIndex) => (prevIndex + 4) % galeria.length);
  };

  const handleClickPrevGaleria = () => {
    setCurrentIndex((prevIndex) => {
      const newIndex = prevIndex - 4;
      return newIndex < 0 ? galeria.length + newIndex : newIndex;
    });
  };

  /// //////////////////////// LISTAR IMAGES Comprar Novamente
  function renderImages() {
    const slicedImages = listaVendas.slice(currentIndex, currentIndex + 3);

    return slicedImages.map((post) => (
      <RecommendedBlock type="Big" key={post.idPost} post={post} />
    ));
  }
  function renderImagesSlim() {
    const slicedImages = listaAvaliados.slice(currentIndex, currentIndex + 4);
    return slicedImages.map((post) => (
      <RecommendedBlock type="slim" key={post.idPost} post={post} />
    ));
  }
  function renderImagesNovamente() {
    const slicedImages = listaComprarNovamente.slice(currentIndex, currentIndex + 4);
    return slicedImages.map((post) => (
      <RecommendedBlock type="slim" key={post.idPost} post={post} />
    ));
  }
  function renderImages50$() {
    const slicedImages = listaValor50.slice(currentIndex, currentIndex + 4);
    return slicedImages.map((post) => (
      <RecommendedBlock type="slim" key={post.idPost} post={post} />
    ));
  }
  function renderImages25$() {
    const slicedImages = listaValor25.slice(currentIndex, currentIndex + 4);
    return slicedImages.map((post) => (
      <RecommendedBlock type="slim" key={post.idPost} post={post} />
    ));
  }
  function renderImages5$() {
    const slicedImages = listaValor5.slice(currentIndex, currentIndex + 4);
    return slicedImages.map((post) => (
      <RecommendedBlock type="slim" key={post.idPost} post={post} />
    ));
  }
  function renderImagesGaleria() {
    const slicedImages = galeria.slice(currentIndex, currentIndex + 4);

    return slicedImages?.map((imagem) => (
      <GaleriaBlock key={slicedImages.indexOf(imagem)} galeria={imagem} />
    ));
  }
  function renderImagesAnunciante() {
    const slicedImages = listaAnunciante.slice(currentIndex, currentIndex + 4);

    return slicedImages.map((post) => (
      <RecommendedBlock type="slim" key={post.idPost} post={post} />
    ));
  }

  useEffect(() => {
    ListarPostsId(idPost);
  }, [idPost]);

  /// //////////////////////// RETURNS
  // if (loading) {
  //   return <SkeletonComponent />;
  // }
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
      </div>
    );
  }
  if (type === 'novamente') {
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
          {renderImagesNovamente()}
          <button
            className="nextButton btnCarousel"
            onClick={handleClickNextSlim}
            disabled={currentIndex + 4 >= listaAvaliados.length}
          >
            <img src={seta} alt="seta voltar Carousel" />
          </button>
        </div>
      </div>
    );
  }
  if (type === 'Valor50') {
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
          {renderImages50$()}
          <button
            className="nextButton btnCarousel"
            onClick={handleClickNextSlim}
            disabled={currentIndex + 4 >= listaAvaliados.length}
          >
            <img src={seta} alt="seta voltar Carousel" />
          </button>
        </div>
      </div>
    );
  }
  if (type === 'Valor50') {
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
          {renderImages50$()}
          <button
            className="nextButton btnCarousel"
            onClick={handleClickNextSlim}
            disabled={currentIndex + 4 >= listaAvaliados.length}
          >
            <img src={seta} alt="seta voltar Carousel" />
          </button>
        </div>
      </div>
    );
  }
  if (type === 'Valor25') {
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
          {renderImages25$()}
          <button
            className="nextButton btnCarousel"
            onClick={handleClickNextSlim}
            disabled={currentIndex + 4 >= listaAvaliados.length}
          >
            <img src={seta} alt="seta voltar Carousel" />
          </button>
        </div>
      </div>
    );
  }
  if (type === 'Valor5') {
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
          {renderImages5$()}
          <button
            className="nextButton btnCarousel"
            onClick={handleClickNextSlim}
            disabled={currentIndex + 4 >= listaAvaliados.length}
          >
            <img src={seta} alt="seta voltar Carousel" />
          </button>
        </div>
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
            onClick={handleClickPrevSlim}
            disabled={currentIndex === 0}
          >
            <img src={seta} alt="seta voltar Carousel" />
          </button>
          {renderImagesAnunciante()}
          <button
            className="nextButton btnCarousel"
            onClick={handleClickNextSlim}
            disabled={currentIndex + 4 >= listaAnunciante.length}
          >
            <img src={seta} alt="seta voltar Carousel" />
          </button>
        </div>
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
