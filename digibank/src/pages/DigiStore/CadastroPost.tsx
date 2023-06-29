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
import imgDefault from '../../assets/img/ImgDefault.png';
import api from '../../services/api';
import Footer from '../../components/Footer';
import Header from '../../components/Header';
import { parseJwt } from '../../services/auth';
import { UsuarioPublicoProps } from '../../@types/Usuario';
import verificaTransparenciaImagem from '../../services/img';
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

  return (
    <NumericFormat
      getInputRef={inputRef}
      onValueChange={(valor) => {
        onChange({
          target: {
            // eslint-disable-next-line react/destructuring-assignment
            name: props.name,
            value: valor.value,
          },
        });
      }}
      thousandSeparator=","
      decimalSeparator="."
      suffix=" BRL"
      // isNumericString
    />
  );
}

export default function CadastroPost() {
  const [idUsuario] = useState(parseJwt().role);
  const [usuario, setUsuario] = useState<UsuarioPublicoProps>();
  const [titulo, setTitulo] = useState('');
  const [valor, setValor] = useState(0);
  const [mainColorHex, setMainColorHex] = useState('');
  const [descricao, setDescricao] = useState('');
  const [vendas] = useState(0);
  const [avaliacao] = useState(0);
  const [qntAvaliacoes] = useState(0);
  const [mainImg, setMainImg] = useState('');
  const [imgsPost, setImgsPost] = useState<{ id: number; img: string }[]>([]);
  const [isUpdatedImgs, setImgsUpdated] = useState(false);
  const [isHovered, setIsHovered] = useState(false);
  const [isTransparente, setTransparente] = useState<boolean>(false);

  //   const [isLoading, setLoading] = useState<boolean>(false);
  const [errorMessage, setErrorMessage] = useState<string>('');
  const navigate = useNavigate();

  const handleMainImgChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];

    if (file) {
      const urlImg = URL.createObjectURL(file);
      setMainImg(urlImg);
    }
  };

  const handleImgsPostChange = () => {
    const imgsElement = document.getElementById('ImgsInput');
    if (imgsElement instanceof HTMLInputElement && imgsElement.files?.length !== 0) {
      const fileList = imgsElement.files;
      if (fileList !== null) {
        const urlImages = [];
        // eslint-disable-next-line no-plusplus
        for (let index = 0; index < fileList.length; index++) {
          const urlImage = {
            id: index,
            img: URL.createObjectURL(fileList[index]),
          };
          urlImages.push(urlImage);
        }
        if (!isUpdatedImgs) {
          setImgsPost(urlImages);
          setImgsUpdated(true);
        }
      }
    }
  };
  const handleRemoveImg = (id: number) => {
    // eslint-disable-next-line @typescript-eslint/no-shadow
    const updatedImgs = imgsPost.filter((img) => img.id !== id);
    setImgsPost(updatedImgs);
  };
  const CadastrarPost = (event: React.FormEvent) => {
    event.preventDefault();

    // setLoading(true);

    const formData = new FormData();

    const element = document.getElementById('mainImgInput') as HTMLInputElement;
    let file = null;

    if (element?.files && element.files.length > 0) {
      [file] = Array.from(element.files);
    }

    const elemento = document.getElementById('ImgsInput') as HTMLInputElement;
    let arquivos: File[] = [];

    if (elemento?.files && elemento.files.length > 0) {
      arquivos = Array.from(elemento.files) as File[];
    }

    // eslint-disable-next-line no-plusplus
    for (let n = 0; n < arquivos.length; n++) {
      formData.append('imgsPost', arquivos[n], arquivos[n].name);
    }

    formData.append('imgPrincipal', file || new File([], ''), file?.name);
    // formData.append('imgsPost', arquivo ?? '', arquivo?.name);

    const MainColorHex = mainColorHex.replace('#', '');

    formData.append('idUsuario', idUsuario.toString());
    formData.append('Titulo', titulo);
    formData.append('MainColorHex', MainColorHex);
    formData.append('Descricao', descricao);
    formData.append('Valor', valor.toString() || '');
    formData.append('vendas', vendas.toString());
    formData.append('avaliacao', avaliacao.toString());
    formData.append('qntAvaliacoes', qntAvaliacoes.toString());

    api
      .post('Marketplace', formData)
      .then((response) => {
        if (response.status === 201) {
          console.log(response.data.postData);
          const { idPost } = response.data.postData;
          console.log(idPost);
          navigate(`/post/${idPost}`);
        }
      })
      .catch(() => {
        setErrorMessage('Não foi possível efetuar o cadastro, post já existe');
        // setLoading(false);
      });
  };
  async function GetUserProps() {
    await api(`Usuarios/Cpf/${parseJwt().sub}`).then((response) => {
      if (response.status === 200) {
        setUsuario(response.data);
      }
    });
  }

  function handleMainImgMouseEnter() {
    setIsHovered(true);
  }

  function handleMainImgMouseLeave() {
    setIsHovered(false);
  }
  // const handleChange = (values: any) => {
  //   const inputValue = parseInt(values.value, 10);
  //   const newValue = !Number.isNaN(inputValue) ? inputValue : 0;
  //   setValor(newValue);
  // };

  const getInputWidth = () => `${titulo.length * 17.5}px`;

  useEffect(() => {
    GetUserProps();
  }, []);

  useEffect(() => {
    verificaTransparenciaImagem(mainImg).then((temTransparencia) => {
      if (temTransparencia) {
        setTransparente(true);
        console.log('banana');
      }
    });
  });

  return (
    <div>
      <Header type="" />
      <form onSubmit={CadastrarPost}>
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
            <div className="infos-banner container">
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
                        {({ data }) => (
                          <div style={{ backgroundColor: data }}>
                            <img src={mainImg} alt="Imagem selecionada" />
                            {isHovered && <span>Trocar</span>}
                          </div>
                        )}
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
                  value={valor.toString()}
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
            </div>
          </section>
          <section className="post-infos">
            <div className="support-sobre-post container">
              <div className="galeria-post">
                <h2>Galeria</h2>
                <div className="support-galeria-post">
                  {/* <img alt="Imagem da galeria da postagem" src={Logo} /> */}
                  {imgsPost ? (
                    <div>
                      {imgsPost.map((event) => (
                        <div className="support-img">
                          <img src={event.img} key={event.id} alt="imagens Galeria" />
                          <button onClick={() => handleRemoveImg(event.id)}>Remover</button>
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
                <div className="recomendado-support">
                  <div
                    className="postImgCad"
                    onMouseEnter={handleMainImgMouseEnter}
                    onMouseLeave={handleMainImgMouseLeave}
                  >
                    {/* eslint-disable-next-line no-nested-ternary */}
                    {mainImg && isTransparente === true ? (
                      <div style={{ backgroundColor: '#000' }}>
                        <img src={mainImg} alt="Imagem selecionada" />
                      </div>
                    ) : mainImg && isTransparente === false ? (
                      <Color src={mainImg} format="hex" quality={1}>
                        {({ data }) => {
                          if (data) {
                            setMainColorHex(data);
                          }
                          return (
                            <div>
                              <img
                                src={mainImg}
                                style={{ backgroundColor: data, borderRadius: '10px' }}
                                alt="Imagem selecionada"
                              />
                            </div>
                          );
                        }}
                      </Color>
                    ) : (
                      <img src={imgDefault} alt="imagem banner default" />
                    )}
                  </div>
                  <div className="recomendado-infos">
                    <div>
                      {titulo ? <h3>{titulo}</h3> : <h3>Titulo do produto</h3>}
                      <h4>{usuario?.apelido}</h4>
                    </div>
                    <div className="avaliacao-recomendado">
                      <span>4,3</span>
                      {/* <Rating value={post.avaliacao ?? 0} size="small" precision={0.1} readOnly /> */}
                      <h5>{valor}BRL</h5>
                    </div>
                  </div>
                </div>
                <button>Cadastrar</button>
              </div>
            </div>
          </section>
        </main>
      </form>
      <Footer />
    </div>
  );
}
