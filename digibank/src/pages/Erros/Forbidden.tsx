import React from 'react';
import { Link } from 'react-router-dom';
import imagem403 from '../../assets/img/LogoErro403Verm.png';
import Header from '../../components/Header';
import Footer from '../../components/Footer';

function Forbidden() {
  return (
    <div>
      <Header type="" />
      <main id="erros" className="container">
        <section className="centralSection">
          <img alt="foto erro 403" src={imagem403} />
          <h2>Você não tem acesso a isto</h2>
          <p>Mantenha seu login atualizado.</p>
          <Link to="/login">Voltar</Link>
        </section>
      </main>
      <Footer />
    </div>
  );
}

export default Forbidden;
