// eslint-disable-next-line eslint-comments/disable-enable-pair
/* eslint-disable jsx-a11y/no-noninteractive-element-interactions */
// eslint-disable-next-line eslint-comments/disable-enable-pair
/* eslint-disable jsx-a11y/click-events-have-key-events */
import React, { useEffect, useState } from 'react';
import { NumericFormat } from 'react-number-format';
import TextField from '@mui/material/TextField';
import { styled } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import Color from 'color-thief-react';
// import StarIcon from '../../assets/img/star_icon.svg';
// import RL from '../../assets/video/RL.mp4';
// import AddBookmarkIcon from '../../assets/img/bookmark-add_icon.svg';
import Plus from '../../assets/img/Plus.png';
import bannerDefault from '../../assets/img/defaultBanner.png';
// eslint-disable-next-line @typescript-eslint/no-unused-vars
import imgDefault from '../../assets/img/ImgDefault.png';
import api from '../../services/api';
import Footer from '../../components/Footer';
import Header from '../../components/Header';
import { parseJwt } from '../../services/auth';
import { UsuarioPublicoProps } from '../../@types/Usuario';
import verificaTransparenciaImagem from '../../services/img';
// eslint-disable-next-line @typescript-eslint/no-unused-vars
import ModalPreviewPost from '../../components/MarketPlace/ModalConfirmarCadastro';
import { useSnackBar } from '../../services/snackBarProvider';
import CustomSnackbar from '../../assets/styledComponents/snackBar';
// import Carousel from '../../components/MarketPlace/Carousel';

const CssTextField1 = styled(TextField)({
  '& label': {
    color: '#ffffff',
    fontSize: '1rem',
  },
  '& label-selected': {
    color: '#ffffff',
    fontSize: '1rem',
  },
  '& .MuiOutlinedInput-root': {
    // width: '23rem',
    '& .MuiInputBase-input': {
      color: '#ffffff', // Defina a cor desejada aqui
      fontSize: '2rem',
    },

    '& fieldset': {
      border: 'none',
      borderBottom: '1px solid #fff',
    },
    '& placeholder': {
      color: '#ffffff',
    },
    '&:hover fieldset': {
      borderColor: 'transparent',
    },
    '&.Mui-focused fieldset': {
      borderColor: 'transparent',
    },
  },
});

const CssTextField2 = styled(TextField)({
  '& label': {
    color: '#ffffff',
    fontSize: '1rem',
  },
  '& label.Mui-focused': {
    color: '#ffffff',
  },
  '& .MuiInput-underline:after': {
    borderBottomColor: '#ffffff',
  },
  '& .MuiOutlinedInput-root': {
    width: '8.5rem',
    '& .MuiInputBase-input': {
      color: '#ffffff', // Defina a cor desejada aqui
    },
    '& fieldset': {
      borderColor: '#ffffff',
      width: '8.5rem',
      borderRadius: '10px',
    },
    '&:hover fieldset': {
      borderColor: '#ffffff',
    },
    '&.Mui-focused fieldset': {
      borderColor: '#ffffff',
    },
  },
});

function NumberFormatCustom(props: any) {
  const { inputRef, onChange } = props;

  const handleKeyPress = (event: React.KeyboardEvent<HTMLInputElement>) => {
    const keyCode = event.keyCode || event.which;
    const keyValue = String.fromCharCode(keyCode);

    if (!/^\d$/.test(keyValue)) {
      event.preventDefault();
    }
  };

  return (
    <NumericFormat
      getInputRef={inputRef}
      onValueChange={(valor) => {
        onChange({
          target: {
            // eslint-disable-next-line react/destructuring-assignment
            name: props.name,
            value: valor.floatValue,
          },
        });
      }}
      thousandSeparator=","
      decimalSeparator="."
      suffix=" BRL"
      onKeyPress={handleKeyPress}
    />
  );
}

interface GaleriaImage {
  id: number;
  img: File;
}

export default function CadastroPost() {
  const [idUsuario] = useState(parseJwt().role);
  const [usuario, setUsuario] = useState<UsuarioPublicoProps>();
  const [titulo, setTitulo] = useState('');
  const [valorPost, setValor] = useState(0);
  const [mainColorHex, setMainColorHex] = useState<string>('');
  const [descricao, setDescricao] = useState('');
  const [vendas] = useState(0);
  const [avaliacao] = useState(0);
  const [qntAvaliacoes] = useState(0);
  const [mainImg, setMainImg] = useState('');
  const [imgsPost, setImgsPost] = useState<GaleriaImage[]>([]);
  const [isHovered, setIsHovered] = useState(false);
  const [isTransparente, setTransparente] = useState<boolean>(false);
  const [idBlob, setIdBlob] = useState<number>(0);
  const [errorMessage, setErrorMessage] = useState<string>('');
  const navigate = useNavigate();

  const { currentMessage, postMessage, handleCloseSnackBar } = useSnackBar();

  const handleMainImgChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    let mainImg1: string;
    if (file) {
      const urlImg = URL.createObjectURL(file);
      mainImg1 = urlImg;

      verificaTransparenciaImagem(mainImg1).then((temTransparencia) => {
        setTransparente(false);
        if (temTransparencia) {
          setTransparente(true);
        }
        setMainImg(urlImg);
      });
    }
  };

  const handleImgsPostChange = () => {
    const imgsElement = document.getElementById('ImgsInput');

    if (imgsElement instanceof HTMLInputElement && imgsElement.files?.length !== 0) {
      const fileList = imgsElement.files;

      if (fileList !== null) {
        const urlImages: GaleriaImage[] = [];

        let idBlobRender: number = idBlob;
        // eslint-disable-next-line no-plusplus
        for (let index = 0; index < fileList.length; index++) {
          // const urlImage: GaleriaImage = {
          //   id: idBlobRender,
          //   img: URL.createObjectURL(fileList[index]),
          // };
          const urlImage: GaleriaImage = {
            id: idBlobRender,
            img: fileList[index],
          };
          // eslint-disable-next-line no-plusplus
          idBlobRender++;
          urlImages.push(urlImage);
        }
        setIdBlob(idBlobRender);

        // eslint-disable-next-line no-plusplus
        for (let index = 0; index < urlImages.length; index++) {
          imgsPost.push(urlImages[index]);
          // Adiciona cada imagem ao indexedDb
        }
      }
    }
  };

  const handleRemoveImage = (idImage: number) => {
    const attImgsPost = imgsPost.filter((img) => img.id !== idImage);
    setImgsPost(attImgsPost);
  };

  const CadastrarPost = (event: React.FormEvent) => {
    event.preventDefault();

    const formData = new FormData();
    const element = document.getElementById('mainImgInput') as HTMLInputElement;
    let file = null;

    if (element?.files && element.files.length > 0) {
      [file] = Array.from(element.files);
    }

    imgsPost.forEach((image, index) => {
      const blob = new Blob([image.img], { type: 'image/*' });
      formData.append(`imgsPost`, blob, `imagem${index}.jpg`);
    });

    formData.append('imgPrincipal', file || new File([], ''), file?.name);
    // formData.append('imgsPost', arquivo ?? '', arquivo?.name);

    const MainColorHex = mainColorHex.replace('#', '');

    formData.append('idUsuario', idUsuario.toString());
    formData.append('Titulo', titulo);
    formData.append('MainColorHex', MainColorHex);
    formData.append('Descricao', descricao);
    formData.append('Valor', valorPost.toString() || '');
    formData.append('vendas', vendas.toString());
    formData.append('avaliacao', avaliacao.toString());
    formData.append('qntAvaliacoes', qntAvaliacoes.toString());

    api
      .post('Marketplace', formData)
      .then((response) => {
        if (response.status === 201) {
          const { idPost } = response.data.postData;
          navigate(`/post/${idPost}`);
        }
      })
      .catch(() => {
        setErrorMessage('Não foi possível efetuar o cadastro');
        postMessage({
          message: 'Não foi possível efetuar o cadastro',
          severity: 'error',
          timeSpan: 3000,
        });
      });
  };
  async function GetUserProps() {
    await api(`Usuarios/Cpf/${parseJwt().sub}`).then((response) => {
      if (response.status === 200) {
        setUsuario(response.data);
      }
    });
  }

  const handleChangeMainColor = (color: string) => {
    setMainColorHex(color);
  };

  function handleMainImgMouseEnter() {
    setIsHovered(true);
  }

  function handleMainImgMouseLeave() {
    setIsHovered(false);
  }

  const getInputWidth = () => `${titulo.length * 17.5}px`;

  useEffect(() => {
    GetUserProps();
  }, []);

  return (
    <div>
      <CustomSnackbar message={currentMessage} onClose={handleCloseSnackBar} />
      <Header type="digiStore" />
      <form id="post" onSubmit={CadastrarPost}>
        <main id="post">
          <section className="support-banner">
            {/* eslint-disable-next-line no-nested-ternary */}
            {mainImg ? (
              <img
                id="fundo-banner"
                alt="Imagem de fundo do produto"
                src={mainImg}
                style={{ backgroundColor: 'transparent' }}
              />
            ) : (
              <img id="fundo-banner" alt="Imagem de fundo do produto" src={bannerDefault} />
            )}
            {/* <video id="fundo-banner" controls loop autoPlay>
              <source src={RL} type="video/mp4" />
              <track kind="captions" src="legenda.vtt" label="Legenda" default />
              Seu navegador não suporta vídeos HTML5.
            </video> */}
            <section className="infos-banner container">
              <CssTextField1
                label="Titulo do Produto"
                variant="outlined"
                required
                multiline
                maxRows={2}
                type="text"
                value={titulo}
                // size="small"
                inputProps={{ maxLength: 55 }}
                onChange={(evt) => setTitulo(evt.target.value)}
                style={{
                  minWidth: '23rem',
                  width: getInputWidth(),
                  maxWidth: '33rem',
                  height: '100%',
                  lineHeight: '2.25rem',
                }}
              />
              <div className="post-stats-support">
                <label htmlFor="mainImgInput">
                  <div
                    className="postImgCad"
                    onMouseEnter={handleMainImgMouseEnter}
                    onMouseLeave={handleMainImgMouseLeave}
                  >
                    {/* eslint-disable-next-line no-nested-ternary */}
                    {mainImg && isTransparente === true ? (
                      <div style={{ backgroundColor: '#000' }}>
                        <img src={mainImg} alt="Imagem selecionada" />
                        {isHovered && <span>{isTransparente}</span>}
                      </div>
                    ) : mainImg && isTransparente === false ? (
                      <Color src={mainImg} format="hex" quality={1}>
                        {({ data }) => {
                          if (data && mainColorHex === '') {
                            setMainColorHex(data);
                          }
                          return (
                            <div style={{ backgroundColor: mainColorHex }}>
                              <img src={mainImg} alt="Imagem selecionada" />
                              {isHovered && <span>Trocar</span>}
                            </div>
                          );
                        }}
                      </Color>
                    ) : (
                      <span>Selecionar Imagem</span>
                    )}
                  </div>
                  <input
                    id="mainImgInput"
                    type="file"
                    accept="image/*, video/*"
                    style={{ display: 'none' }}
                    onChange={handleMainImgChange}
                  />
                </label>
                <div className="post-stats">
                  <h3 id="titulo">{usuario?.apelido}</h3>
                  {/* <hr id="separador" /> */}
                  {/* <div id="avaliacao-support">
                    <div>
                      <span>4,3</span>
                      <img alt="Estrela de avaliação" src={StarIcon} />
                    </div>
                    <span>4,89 mil avaliações</span>
                  </div> */}
                </div>
              </div>
              <div className="post-actions">
                <CssTextField2
                  label="Valor"
                  variant="outlined"
                  required
                  type="number"
                  value={valorPost.toString()}
                  onChange={(evt) => setValor(parseFloat(evt.target.value))}
                  InputProps={{
                    inputComponent: NumberFormatCustom,
                  }}
                />
                {/* <hr id="separador" />
                <button id="favoritar__btn">
                  <img alt="Botão adicionar produto à lista de desejos" src={AddBookmarkIcon} />
                  <span>Lista de desejos</span>
                </button> */}
              </div>
              <span>{errorMessage}</span>
            </section>
          </section>
          <section className="post-infos">
            <section className="support-sobre-post container">
              <div className="galeria-post">
                <h2>Galeria</h2>
                <div className="support-galeria-post">
                  {/* <img alt="Imagem da galeria da postagem" src={Logo} /> */}
                  {imgsPost ? (
                    <div>
                      {imgsPost.map((event) => (
                        <div className="support-img">
                          <img
                            src={URL.createObjectURL(event.img)}
                            key={event.id}
                            alt="imagens da Galeria"
                            onClick={() => handleRemoveImage(event.id)}
                          />
                        </div>
                      ))}

                      <div className="support-img">
                        <div className="boxCadastro">
                          <label htmlFor="ImgsInput">
                            <img src={Plus} alt="simbolo mais" />
                            <input
                              id="ImgsInput"
                              type="file"
                              accept="image/*, video/*"
                              style={{ display: 'none' }}
                              onChange={handleImgsPostChange}
                              multiple
                            />
                          </label>
                        </div>
                      </div>
                    </div>
                  ) : (
                    <div className="support-img">
                      <div className="boxCadastro">
                        <label htmlFor="ImgsInput">
                          <img src={Plus} alt="simbolo mais" />
                          <input
                            id="ImgsInput"
                            type="file"
                            accept="image/*, video/*"
                            style={{ display: 'none' }}
                            onChange={handleImgsPostChange}
                            multiple
                          />
                        </label>
                      </div>
                    </div>
                  )}
                </div>
              </div>
              <div className="descricao-post-cad">
                <div className="textBox">
                  <h2>Sobre o produto</h2>
                  <textarea
                    cols={50}
                    rows={10}
                    onChange={(evt) => setDescricao(evt.target.value)}
                  />
                </div>
                <ModalPreviewPost
                  canPost
                  onChangeColor={(color) => handleChangeMainColor(color)}
                  isTransparente={isTransparente}
                  mainImg={mainImg}
                  postData={{
                    idPost: 0,
                    idUsuario: parseJwt().role,
                    nome: titulo,
                    valor: valorPost,
                    apelidoProprietario: '',
                    avaliacao: 0,
                    descricao: '',
                    imgs: [],
                    isActive: true,
                    isVirtual: true,
                    mainColorHex,
                    mainImg,
                    vendas: 0,
                    qntAvaliacoes: 0,
                  }}
                />
              </div>
            </section>
          </section>
        </main>
      </form>
      <Footer />
    </div>
  );
}
