import React from 'react';
import { Link } from 'react-router-dom';
import imagem503 from '../../assets/img/503.png';
import Header from '../../components/Header';
import Footer from '../../components/Footer';

function ServiceUnavailable() {
  return (
    <div>
      <Header type="" />
      <main id="erros" className="container">
        <section className="centralSection">
          <img alt="foto erro 503" src={imagem503} />
          <h2>O Serviço não está disponível</h2>
          <p>Pode ser que nossos servidores estejam enfrentando problemas.</p>
          <Link to="/login">Voltar</Link>
        </section>
      </main>
      <Footer />
    </div>
  );
}

export default ServiceUnavailable;
