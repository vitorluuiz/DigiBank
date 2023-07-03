import React, { useRef } from 'react';
import { Link } from 'react-router-dom';
import Marquee from 'react-fast-marquee';
import Footer from './components/Footer';
import BannerLanding from './assets/img/bannerLanding.svg';
import storeLanding from './assets/img/storeLanding.svg';
import metasLanding from './assets/img/MetasLanding.svg';
import investLanding from './assets/img/InvestLanding.svg';
import Apple from './assets/img/apple.png';
import Playstation from './assets/img/playstation-6.svg';
import Xbox from './assets/img/Xbox.png';
import Nintendo from './assets/img/nintendo-2.svg';
import Amazon from './assets/img/amazon-2.svg';
import Spotify from './assets/img/spotify-logo.svg';
import Netflix from './assets/img/netflix-3.svg';
import Steam from './assets/img/steam.png';
import EpicGames from './assets/img/EpicGames.png';
import GooglePlay from './assets/img/google Play.png';
import cartaoLanding from './assets/img/cartaoDuploLanding.svg';
import Header from './components/Header';
import Slider from './components/Slider';

function App() {
  const scrollRef = useRef<HTMLDivElement>(null);

  const scrollToRef = () => {
    if (scrollRef.current) {
      window.scrollTo({
        top: scrollRef.current.offsetTop,
        behavior: 'smooth',
      });
    }
  };

  return (
    <div>
      <Header type="landing" />
      <main id="landing" className="container">
        <section className="bannerSection">
          <div className="leftSectionBanner">
            <h1>
              o novo banco que vai mudar <span>a sua vida.</span>
            </h1>
            <p>Conta digital, cartão de crédito, investimentos e mais: tudo em um só app</p>
            <div className="boxBotoes">
              <Link className="btnLanding" to="/cadastro">
                Crie sua conta
              </Link>
              <button className="btnLandingWhite" onClick={scrollToRef}>
                Explorar vantagens
              </button>
            </div>
          </div>
          <div className="rightSectionBanner">
            <img alt="icon home landing" src={BannerLanding} />
          </div>
        </section>
        <section className="storeLanding" ref={scrollRef} id="store">
          <h2>
            Loja com produtos <span>diversificados</span>
          </h2>
          <img alt="icone store landing page" src={storeLanding} className="imgStore" />
          <Link className="storeLanding" to="/digistore">
            <Marquee play direction="right">
              <div className="boxLogo">
                <img src={Apple} alt="alguma logo ai" className="imgLogos" />
              </div>
              <div className="boxLogo">
                <img src={Playstation} alt="alguma logo ai" className="imgLogos" />
              </div>
              <div className="boxLogo">
                <img src={Xbox} alt="alguma logo ai" className="imgLogos" />
              </div>
              <div className="boxLogo">
                <img src={Nintendo} alt="alguma logo ai" className="imgLogos" />
              </div>
              <div className="boxLogo">
                <img src={Steam} alt="alguma logo ai" className="imgLogos" />
              </div>
              <div className="boxLogo">
                <img src={EpicGames} alt="alguma logo ai" className="imgLogos" />
              </div>
              <div className="boxLogo">
                <img src={Amazon} alt="alguma logo ai" className="imgLogos" />
              </div>
              <div className="boxLogo">
                <img src={Spotify} alt="alguma logo ai" className="imgLogos" />
              </div>
              <div className="boxLogo">
                <img src={Netflix} alt="alguma logo ai" className="imgLogos" />
              </div>
              <div className="boxLogo">
                <img src={GooglePlay} alt="alguma logo ai" className="imgLogos" />
              </div>
            </Marquee>
            <Marquee play direction="left">
              <div className="boxLogo">
                <img src={GooglePlay} alt="alguma logo ai" className="imgLogos" />
              </div>
              <div className="boxLogo">
                <img src={Netflix} alt="alguma logo ai" className="imgLogos" />
              </div>
              <div className="boxLogo">
                <img src={Spotify} alt="alguma logo ai" className="imgLogos" />
              </div>
              <div className="boxLogo">
                <img src={Amazon} alt="alguma logo ai" className="imgLogos" />
              </div>
              <div className="boxLogo">
                <img src={EpicGames} alt="alguma logo ai" className="imgLogos" />
              </div>
              <div className="boxLogo">
                <img src={Steam} alt="alguma logo ai" className="imgLogos" />
              </div>
              <div className="boxLogo">
                <img src={Nintendo} alt="alguma logo ai" className="imgLogos" />
              </div>
              <div className="boxLogo">
                <img src={Xbox} alt="alguma logo ai" className="imgLogos" />
              </div>
              <div className="boxLogo">
                <img src={Playstation} alt="alguma logo ai" className="imgLogos" />
              </div>
              <div className="boxLogo">
                <img src={Apple} alt="alguma logo ai" className="imgLogos" />
              </div>
            </Marquee>
          </Link>
        </section>
        <section className="metasLanding" id="metas">
          <div className="leftMetas">
            <img className="imgMeta" alt="icone meta landing page" src={metasLanding} />
          </div>
          <div className="rightMetas">
            <h2>
              <span>Gerenciamento</span> de metas.
            </h2>
            <p>Estabeleça metas e tenha maior controle do seu dinheiro a qualquer momento</p>
          </div>
        </section>
        <section className="investLanding">
          <h2>
            Invista em ações e fundos de marcas <span>conhecidas mundialmente.</span>
          </h2>
          <div className="bottomInvest">
            {/* <img alt="icone invest landing page" className="imgInvest" src={investLanding} /> */}
            <Slider />
            <img alt="icone invest landing page" className="iconInvest" src={investLanding} />
          </div>
        </section>
      </main>
      <section className="finalBanner">
        <div className="container">
          <h2>Abra ja sua conta e tenha acesso a esta vasta quantidade de beneficios</h2>
          <div className="bottomFinal">
            <img alt="cartao duplo landing Page" src={cartaoLanding} />
            <div className="rightBottomFinal">
              <p>Abra Sua conta ou faça login para acessar o site e adquirir vantagens</p>
              <div className="buttonsFinal">
                <Link to="/cadastro">Crie sua conta</Link>
                <Link to="/login">Login</Link>
              </div>
            </div>
          </div>
        </div>
      </section>
      <Footer />
    </div>
  );
}

export default App;
