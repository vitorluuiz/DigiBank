import React from 'react';
import { Link } from 'react-router-dom';
import imagem401 from '../../assets/img/LogoErro401Verm.png';
import Header from '../../components/Header';
import Footer from '../../components/Footer';

function Unauthorized() {
  return (
    <div>
      <Header type="" />
      <main id="erros" className="container">
        <section className="centralSection">
          <img alt="foto erro 401" src={imagem401} />
          <h2>Você pode não estar logado</h2>
          <Link to="/home">Voltar</Link>
        </section>
      </main>
      <Footer />
    </div>
  );
}

export default Unauthorized;
