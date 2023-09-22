import React, { useEffect, useState } from 'react';
import Footer from '../../../components/Footer';
import Header from '../../../components/Header';
import api from '../../../services/api';
import { parseJwt } from '../../../services/auth';
import RecommendedInvestiment from '../../../components/Investimentos/RecommendedInvestment';
import { WishlishedInvestment } from '../InvestPost';
import { MinimalOptionProps } from '../../../@types/InvestimentoOptions';
import AsideInvest from '../../../components/Investimentos/AsideInvest';

export default function FavortosInvest() {
  const [investList, setInvestList] = useState<MinimalOptionProps[]>([]);
  const [componenteExibido, setComponenteExibido] = useState<number | null>(3);

  const exibirComponente = (componente: number) => {
    setComponenteExibido(componente);
  };

  function ListFavoritos() {
    return investList.map((investimento) => {
      let recommendedInvestmentType = 'Big';
      if (componenteExibido === 5) {
        recommendedInvestmentType = 'cripto';
      }
      if (componenteExibido === 2) {
        recommendedInvestmentType = 'rendaFixa';
      }

      return (
        <RecommendedInvestiment type={recommendedInvestmentType} investimento={investimento} />
      );
    });
  }

  const GetWishlistFromServer = (idsInvestimento: number[]) => {
    api
      .post(`InvestimentoOptions/Favoritos/${componenteExibido}`, idsInvestimento)
      .then((response) => {
        if (response.status === 200) {
          setInvestList(response.data.optionsList);
        }
      });
  };

  const GetWishlistFromLocal = () => {
    if (localStorage.getItem('wishlistInvest')) {
      const localData: WishlishedInvestment[] = JSON.parse(
        localStorage.getItem('wishlistInvest') ?? '[]',
      );
      const idInvestimentos: number[] = [];
      localData.forEach((item) => {
        if (item.idUsuario === parseJwt().role) {
          idInvestimentos.push(item.idInvestimentoOption);
        }
      });
      GetWishlistFromServer(idInvestimentos);
    } else {
      localStorage.setItem('wishlistInvest', '[]');
    }
  };

  useEffect(() => {
    GetWishlistFromLocal();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [componenteExibido]);

  return (
    <div>
      <Header type="" />
      <main id="investFavoritos" className="container">
        <AsideInvest
          type="favoritos"
          componenteExibido={componenteExibido}
          exibirComponente={exibirComponente}
        />
        <div className="containerCarousels">
          <div className="boxCarousel">{ListFavoritos()}</div>
        </div>
      </main>
      <Footer />
    </div>
  );
}
