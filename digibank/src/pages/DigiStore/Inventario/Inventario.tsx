import React, { useEffect, useState } from 'react';
import api from '../../../services/api';
import { parseJwt } from '../../../services/auth';

import { ItemProps } from '../../../@types/Inventario';

import Header from '../../../components/Header';
import Footer from '../../../components/Footer';
import { ItemInventario } from '../../../components/Inventario/Item';

export default function Inventario() {
  const [InventarioList, setInventarioList] = useState<ItemProps[]>([]);

  function GetInventario() {
    api(`Inventario/MeuInventario/${parseJwt().role}/1/100`).then((response) => {
      if (response.status === 200) {
        setInventarioList(response.data);
      }
    });
  }

  useEffect(() => {
    GetInventario();
  }, []);

  return (
    <div>
      <Header type="digiStore" />
      <main id="inventario" className="container">
        <h1>Seus Produtos comprados</h1>
        <section className="inventario-list">
          {InventarioList.map((item) => (
            <ItemInventario
              key={item.idInventario}
              itemData={item}
              onDelete={() => GetInventario()}
            />
          ))}
        </section>
      </main>
      <Footer />
    </div>
  );
}
