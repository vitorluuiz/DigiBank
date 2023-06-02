import React, { useEffect, useState } from 'react';
import TextField from '@mui/material/TextField';
import { styled } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import StarIcon from '../../assets/img/star_icon.svg';
// import RL from '../../assets/video/RL.mp4';
import AddBookmarkIcon from '../../assets/img/bookmark-add_icon.svg';
import bannerDefault from '../../assets/img/defaultBanner.png';
// import Logo from '../../assets/img/logoVermelha.png';
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
      borderColor: 'transparent',
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

export default function CadastroPost() {
  const [idUsuario] = useState(parseJwt().role);
  const [usuario, setUsuario] = useState<UsuarioPublicoProps>();
  const [nome, setNome] = useState('');
  const [valor, setValor] = useState(0);
  const [descricao] = useState('salve');
  const [vendas] = useState(0);
  const [avaliacao] = useState(0);
  const [qntAvaliacoes] = useState(0);
  const [mainImg, setMainImg] = useState('');
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
  const CadastrarPost = (event: React.FormEvent) => {
    event.preventDefault();

    // setLoading(true);

    const formData = new FormData();

    const element = document.getElementById('mainImgInput') as HTMLInputElement;
    let file = null;

    if (element?.files && element.files.length > 0) {
      [file] = Array.from(element.files);
    }
    console.log(file);
    formData.append('imgPrincipal', file ?? '', file?.name);

    formData.append('idUsuario', idUsuario.toString());
    formData.append('nome', nome);
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
                value={nome}
                // size="small"
                onChange={(evt) => setNome(evt.target.value)}
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
                />
                <hr id="separador" />
                <button id="favoritar__btn">
                  <img alt="Botão adicionar produto à lista de desejos" src={AddBookmarkIcon} />
                  <span>Lista de desejos</span>
                </button>
              </div>
            </div>
            <span>{errorMessage}</span>

            {/* <button>Cadastrar</button> */}
          </section>
        </main>
      </form>
      <Footer />
    </div>
  );
}
