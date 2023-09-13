import React from 'react';
import { useNavigate } from 'react-router-dom';
import imagem404 from '../../assets/img/LogoErro404Verm.png';
import Header from '../../components/Header';
import Footer from '../../components/Footer';

function NotFound() {
  const navigate = useNavigate();

  // Função para armazenar a última página visitada

  // Função para redirecionar para a última página visitada
  const redirectToLastVisitedPage = () => {
    navigate(-3);
  };

  return (
    <div>
      <Header type="" />
      <main id="erros" className="container">
        <section className="centralSection">
          <img alt="Ícone do erro 404 NotFound" src={imagem404} />
          <h2>pagina não encontrada</h2>
          <p>Pode ser que esta página não exista mais.</p>
          <button onClick={redirectToLastVisitedPage}>Voltar</button>
        </section>
      </main>
      <Footer />
    </div>
  );
}

export default NotFound;
