import React from 'react';
import Header from '../../components/Header';
import Footer from '../../components/Footer';
import Item from './Item';

export default function Inventario() {
  return (
    <div>
      <Header type="" />
      <main id="inventario" className="container">
        <h1>Seus Produtos</h1>
        <section className="inventario-list">
          <Item />
          <Item />
        </section>
      </main>
      <Footer />
    </div>
  );
}
