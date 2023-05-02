import React from 'react';

import Header from '../../components/Header';
import Footer from '../../components/Footer';
import SideBar from '../../components/SideBar';
import Meta from '../../components/Meta';

function Metas() {
  return (
    <div>
      <Header type="" />
      <main id="metas" className="container">
        <div className="header-page">
          <h1>Minhas metas</h1>
          <button>Adicionar meta</button>
        </div>
        <section className="meta-list">
          <Meta />
        </section>
        <SideBar />
      </main>
      <Footer />
    </div>
  );
}

export default Metas;
