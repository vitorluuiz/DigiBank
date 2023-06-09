import React, { useEffect, useState } from 'react';
import { NumericFormat } from 'react-number-format';
import TextField from '@mui/material/TextField';
import { styled } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import StarIcon from '../../assets/img/star_icon.svg';
// import RL from '../../assets/video/RL.mp4';
import AddBookmarkIcon from '../../assets/img/bookmark-add_icon.svg';
import Plus from '../../assets/img/Plus.png';
import bannerDefault from '../../assets/img/defaultBanner.png';
import api from '../../services/api';
import Footer from '../../components/Footer';
import Header from '../../components/Header';
import { parseJwt } from '../../services/auth';
import { UsuarioPublicoProps } from '../../@types/Usuario';

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
      fontSize: '2.25rem',
    },

    '& fieldset': {
      borderColor: '#fff',
      width: '23rem',
      borderRadius: '10px',
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
      thousandSeparator="."
      decimalSeparator=","
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
  const [descricao, setDescricao] = useState('');
  const [vendas] = useState(0);
  const [avaliacao] = useState(0);
  const [qntAvaliacoes] = useState(0);
  const [mainImg, setMainImg] = useState('');
  const [imgsPost, setImgsPost] = useState<{ id: number; img: string }[]>([]);
  const [isUpdatedImgs, setImgsUpdated] = useState(false);
  const [isHovered, setIsHovered] = useState(false);

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

  // const handleImgsPostChange = (event: React.ChangeEvent<HTMLInputElement>) => {
  //   const arquivo = event.target.files?.[0];

  //   if (arquivo) {
  //     const urlImgs = URL.createObjectURL(arquivo);
  //     setImgsPost(urlImgs);
  //   }
  // };

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
      console.log('batata');
      arquivos = Array.from(elemento.files) as File[];
    }

    // eslint-disable-next-line no-plusplus
    for (let n = 0; n < arquivos.length; n++) {
      formData.append('imgsPost', arquivos[n], arquivos[n].name);
    }

    formData.append('imgPrincipal', file ?? '', file?.name);
    // formData.append('imgsPost', arquivo ?? '', arquivo?.name);

    formData.append('idUsuario', idUsuario.toString());
    formData.append('titulo', titulo);
    formData.append('descricao', descricao);
    formData.append('valor', valor.toString() || '');
    formData.append('vendas', vendas.toString());
    formData.append('avaliacao', avaliacao.toString());
    formData.append('qntAvaliacoes', qntAvaliacoes.toString());

    api
      .post('Marketplace', formData)
      .then((response) => {
        if (response.status === 201) {
          const { idPost } = response.data;
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
  useEffect(() => {
    GetUserProps();
  }, []);
  return (
    <div>
      <Header type="" />
      <form onSubmit={CadastrarPost}>
        <main id="post">
          <section className="support-banner">
            {mainImg ? (
              <img id="fundo-banner" alt="Imagem de fundo do produto" src={mainImg} />
            ) : (
              // <div className="defaultImgDiv">
              <img id="fundo-banner" alt="Imagem de fundo do produto" src={bannerDefault} />
            )}
            {/* <video id="fundo-banner" controls loop autoPlay>
              <source src={RL} type="video/mp4" />
              <track kind="captions" src="legenda.vtt" label="Legenda" default />
              Seu navegador não suporta vídeos HTML5.
            </video> */}
            <div className="infos-banner container">
              <CssTextField1
                label="Nome do Produto"
                variant="outlined"
                required
                type="text"
                value={titulo}
                // size="small"
                onChange={(evt) => setTitulo(evt.target.value)}
              />
              <div className="post-stats-support">
                <label htmlFor="mainImgInput">
                  <div
                    className="postImgCad"
                    onMouseEnter={handleMainImgMouseEnter}
                    onMouseLeave={handleMainImgMouseLeave}
                  >
                    {mainImg ? (
                      <div>
                        <img src={mainImg} alt="Imagem selecionada" />
                        {isHovered && <span>Trocar</span>}
                      </div>
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
                <CssTextField2
                  label="Valor"
                  variant="outlined"
                  required
                  type="text"
                  value={valor.toString()}
                  onChange={(evt) => setValor(parseInt(evt.target.value, 10))}
                  InputProps={{
                    inputComponent: NumberFormatCustom,
                  }}
                />
                <hr id="separador" />
                <button id="favoritar__btn">
                  <img alt="Botão adicionar produto à lista de desejos" src={AddBookmarkIcon} />
                  <span>Lista de desejos</span>
                </button>
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
              <div className="descricao-post">
                <div className="textBox">
                  <h2>Sobre o produto</h2>
                  <textarea
                    cols={50}
                    rows={10}
                    onChange={(evt) => setDescricao(evt.target.value)}
                  />
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
