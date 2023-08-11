import React, { useEffect, useState } from 'react';
// import api from '../../../services/api';
// import { parseJwt } from '../../../services/auth';

import { ThemeProvider, createTheme } from '@mui/material/styles';
import Pagination from '@mui/material/Pagination';
import Header from '../../../components/Header';
import Footer from '../../../components/Footer';
import AsideInvest from '../../../components/Investimentos/AsideInvest';
import { parseJwt } from '../../../services/auth';
import api from '../../../services/api';
// import { InvestidosProps } from '../../../@types/Investidos';
import RecommendedInvestiment from '../../../components/Investimentos/RecommendedInvestment';
// import { InvestimentoOptionsProps } from '../../../@types/InvestimentoOptions';
import { MinimalOptionProps } from '../../../@types/InvestimentoOptions';
// import Empty from '../../../components/Empty';

const theme = createTheme({
  palette: {
    primary: {
      main: '#c20014',
    },
  },
});

export default function Investidos() {
  const [InvestidosList, setInvestidosList] = useState<MinimalOptionProps[]>([]);
  const [componenteExibido, setComponenteExibido] = useState<number | null>(3);
  const [pagina, setPagina] = useState(1);
  const [investidosCount, setInvestidosCount] = useState(1);
  const [qntItens] = useState(3);

  const exibirComponente = (componente: number) => {
    setComponenteExibido(componente);
  };

  function GetInvestidos(pag: number) {
    setInvestidosList([]);
    api(
      `/InvestimentoOptions/${pag}/${qntItens}/${componenteExibido}/comprados/${parseJwt().role}`,
    ).then((response) => {
      if (response.status === 200) {
        setInvestidosList(response.data.optionList);
        console.log(response.data.optionList);
        setInvestidosCount(response.data.optionCount);
        console.log(investidosCount);
      }
    });
  }

  const handlePageChange = (event: React.ChangeEvent<unknown>, value: number) => {
    setPagina(value);
    GetInvestidos(value);
  };

  useEffect(() => {
    GetInvestidos(1);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [componenteExibido]);

  let typeBlock = 'Big';
  if (componenteExibido === 5) {
    typeBlock = 'cripto';
  }
  if (componenteExibido === 2) {
    typeBlock = 'rendaFixa';
  }

  return (
    <div>
      <Header type="digInvest" />
      <main id="investidos" className="container">
        <AsideInvest
          type="investidos"
          componenteExibido={componenteExibido}
          exibirComponente={exibirComponente}
        />
        <div className="containerCarousels">
          <div className="boxCarousel">
            {InvestidosList.map((invest) => (
              <RecommendedInvestiment
                type={typeBlock}
                key={invest.idInvestimentoOption}
                investimento={invest}
              />
            ))}
          </div>
          <div className="paginacaoInvestidos">
            <ThemeProvider theme={theme}>
              <Pagination
                page={pagina}
                onChange={handlePageChange}
                count={Math.ceil(investidosCount / qntItens)}
                shape="rounded"
                // variant="outlined"
                color="primary"
                siblingCount={1}
              />
            </ThemeProvider>
          </div>
        </div>
      </main>
      <Footer />
    </div>
  );
}
