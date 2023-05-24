import React from 'react';
import { Link } from 'react-router-dom';
import imagem404 from '../../assets/img/LogoErro404Verm.png';
import Header from '../../components/Header';
import Footer from '../../components/Footer';

function NotFound() {
  return (
    <div>
      <Header type="" />
      <main id="erros" className="container">
        <section className="centralSection">
          <img alt="foto erro 404" src={imagem404} />
          <h2>pagina não encontrada</h2>
          <p>Ops, aparentemente não tem nada aqui.</p>
          {/* <div>
            <p>Retorne para a tela inicial</p>
          </div> */}
          <Link to="/home">Voltar</Link>
        </section>
        {/* <section className="rightSection"> */}

        {/* </section> */}
      </main>
      <Footer />
    </div>
  );
}

export default NotFound;
