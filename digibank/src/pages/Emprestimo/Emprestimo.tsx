import React, { useEffect, useState } from 'react';
import 'react-toastify/dist/ReactToastify.css';

import { EmprestimoProps, OptionProps } from '../../@types/Emprestimo';

import { Emprestimo, EmprestimoOption } from '../../components/EmprestimoOption';
import Header from '../../components/Header';
import Footer from '../../components/Footer';
import api from '../../services/api';
import { parseJwt } from '../../services/auth';
import SideBar from '../../components/SideBar';
import Empty from '../../components/Empty';
import CustomSnackbar from '../../assets/styledComponents/snackBar';
import { useSnackBar } from '../../services/snackBarProvider';

function Emprestimos() {
  const [OptionsList, setOptionsList] = useState<OptionProps[]>([]);
  const [EmprestimosList, setEmprestimosList] = useState<EmprestimoProps[]>([]);

  const { currentMessage, handleCloseSnackBar } = useSnackBar();

  function GetOptions() {
    api(`Emprestimos/idUsuario/${parseJwt().role}`).then((response) => {
      if (response.status === 200) {
        setEmprestimosList(response.data);
      }
    });

    api(`EmprestimoOptions/${parseJwt().role}/1/10`).then((response) => {
      if (response.status) {
        setOptionsList(response.data);
      }
    });
  }

  useEffect(() => {
    GetOptions();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <div>
      <CustomSnackbar message={currentMessage} onClose={handleCloseSnackBar} />
      <Header type="" />
      <main id="emprestimos" className="container">
        <section className="left-menu-emprestimo">
          <h1 className="title-list">Pré-Aprovados</h1>
          <div className="list-emprestimos">
            {OptionsList.length !== 0 ? (
              OptionsList.map((option) => (
                <EmprestimoOption
                  onUpdate={() => GetOptions()}
                  key={option.idEmprestimoOption}
                  option={option}
                />
              ))
            ) : (
              <Empty type="pegarEmprestimo" />
            )}
          </div>
        </section>
        <section className="right-menu-emprestimo">
          <span className="title-list">Em aberto</span>
          <div className="list-emprestimos">
            {EmprestimosList.length !== 0 ? (
              EmprestimosList.map((emprestimo) => (
                <Emprestimo
                  onUpdate={() => GetOptions()}
                  key={emprestimo.idEmprestimo}
                  emprestimo={emprestimo}
                />
              ))
            ) : (
              <Empty type="pagarEmprestimo" />
            )}
          </div>
        </section>
        <SideBar />
      </main>
      <Footer />
    </div>
  );
}

export default Emprestimos;
