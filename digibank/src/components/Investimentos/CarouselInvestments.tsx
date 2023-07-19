import { useEffect, useState } from 'react';
import Empty from '../Empty';
import api from '../../services/api';
import { parseJwt } from '../../services/auth';
import seta from '../../assets/img/setaCarousel.svg';
import SkeletonComponent from '../MarketPlace/Skeleton/Skeleton';
import { InvestimentoOptionsProps } from '../../@types/InvestimentoOptions';
import RecommendedInvestiment from './RecommendedInvestment';

export default function CarouselInvestimentos({
  type,
  typeInvestimento,
  maxValue,
}: {
  type: string;
  typeInvestimento: number | null;
  maxValue?: number;
}) {
  const [currentIndex, setCurrentIndex] = useState(0);
  const [qntdLista, setQntdLista] = useState(0);
  const [InvestimentoList, setInvestimentoList] = useState<InvestimentoOptionsProps[]>([]);
  const [loading, setLoading] = useState(true);

  function sliceImages() {
    const slicedImages = InvestimentoList.slice(currentIndex, currentIndex + qntdLista);

    if (slicedImages.length > 0) {
      return slicedImages.map((investimento) => {
        let recommendedInvestmentType = 'Big';
        if (typeInvestimento === 5) {
          recommendedInvestmentType = 'cripto';
        }
        if (typeInvestimento === 2) {
          recommendedInvestmentType = 'rendaFixa';
        }

        return (
          <RecommendedInvestiment
            type={recommendedInvestmentType}
            key={investimento.idInvestimentoOption}
            investimento={investimento}
          />
        );
      });
    }
    return <Empty type="marketplace" />;
  }
  const handleClickNext = () => {
    setCurrentIndex((prevIndex) => (prevIndex + qntdLista) % InvestimentoList.length);
  };

  const handleClickPrev = () => {
    setCurrentIndex((prevIndex) => {
      const newIndex = prevIndex - qntdLista;
      return newIndex < 0 ? InvestimentoList.length + newIndex : newIndex;
    });
  };

  const GetInvestimentoList = () => {
    setInvestimentoList([]);

    if (typeInvestimento === 2 || typeInvestimento === 5) {
      setQntdLista(2);
    } else {
      setQntdLista(3);
    }

    switch (type) {
      case 'emAlta':
        api.get(`Marketplace/${1}/${9}/Vendas`).then((response) => {
          if (response.status === 200) {
            const data = Array.isArray(response.data) ? response.data : [];
            setInvestimentoList(data);
            setLoading(false);
          }
        });
        break;
      case 'vendas':
        api.get(`InvestimentoOptions/${1}/${9}/${typeInvestimento}/vendas`).then((response) => {
          if (response.status === 200) {
            const data = Array.isArray(response.data) ? response.data : [];
            setInvestimentoList(data);

            setLoading(false);
          }
        });
        break;
      case 'comprados':
        if (parseJwt().role !== 'undefined') {
          api
            .get(`InvestimentoOptions/${1}/${1}/${typeInvestimento}/comprados/${parseJwt().role}`)
            .then((response) => {
              if (response.status === 200) {
                const data = Array.isArray(response.data) ? response.data : [];
                setInvestimentoList(data);
                console.log(response.data);

                setLoading(false);
              }
            });
        } else {
          setLoading(false);
        }
        break;
      case 'valor':
        api
          .get(`InvestimentoOptions/${1}/${12}/${typeInvestimento}/valor/${maxValue}`)
          .then((response) => {
            if (response.status === 200) {
              const data = Array.isArray(response.data) ? response.data : [];
              setInvestimentoList(data);

              setLoading(false);
            }
          });
        break;
      default:
        break;
    }
  };

  // eslint-disable-next-line react-hooks/exhaustive-deps
  useEffect(() => GetInvestimentoList(), [typeInvestimento]);

  if (loading) {
    return <SkeletonComponent />;
  }
  return (
    <div>
      <div id="mainCarousel">
        <div className="suport-carousel">
          <button
            className="prevButton btnCarousel"
            onClick={handleClickPrev}
            disabled={currentIndex === 0}
          >
            <img src={seta} alt="seta voltar Carousel" />
          </button>
          {sliceImages()}
          <button
            className="nextButton btnCarousel"
            onClick={handleClickNext}
            disabled={currentIndex + qntdLista >= InvestimentoList.length}
          >
            <img src={seta} alt="seta voltar Carousel" />
          </button>
        </div>
      </div>
    </div>
  );
}

CarouselInvestimentos.defaultProps = {
  maxValue: 0,
};
