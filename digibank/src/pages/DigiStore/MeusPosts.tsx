import React, { useEffect, useState } from 'react';

import api from '../../services/api';
import { parseJwt } from '../../services/auth';

import Header from '../../components/Header';
import Footer from '../../components/Footer';
import { ItemPublicados } from '../../components/Inventario/Item';
import { PostProps } from '../../@types/Post';
import CustomSnackbar from '../../assets/styledComponents/snackBar';
import { useSnackBar } from '../../services/snackBarProvider';

export default function MeusPosts() {
  const [MeusPostsList, setMeusPosts] = useState<PostProps[]>([]);

  const { currentMessage, handleCloseSnackBar } = useSnackBar();

  function GetPosts() {
    api(`Marketplace/Usuario/${parseJwt().role}/Meus`).then((response) => {
      if (response.status === 200) {
        setMeusPosts(response.data);
      }
    });
  }

  useEffect(() => {
    GetPosts();
  }, []);

  return (
    <div>
      <Header type="digiStore" />
      <CustomSnackbar message={currentMessage} onClose={handleCloseSnackBar} />
      <main id="inventario" className="container">
        <h1>Seus Produtos publicados</h1>
        <section className="inventario-list">
          {MeusPostsList.map((item) => (
            <ItemPublicados key={item.idPost} itemData={item} onUpdate={() => GetPosts()} />
          ))}
        </section>
      </main>
      <Footer />
    </div>
  );
}
