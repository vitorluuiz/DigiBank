import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

import { EmprestimoPost, EmprestimoProps, OptionProps } from '../../@types/Emprestimo';

import { Emprestimo, EmprestimoOption } from '../../components/EmprestimoOption';
import Header from '../../components/Header';
import Footer from '../../components/Footer';
import api from '../../services/api';
import { parseJwt } from '../../services/auth';
import SideBar from '../../components/SideBar';
import Empty from '../../components/Empty';

function Emprestimos() {
  const [OptionsList, setOptionsList] = useState<OptionProps[]>([]);
  const [EmprestimosList, setEmprestimosList] = useState<EmprestimoProps[]>([]);
  const navigate = useNavigate();

  async function GetOptions() {
    await api(`Emprestimos/idUsuario/${parseJwt().role}`)
      .then((response) => {
        if (response.status === 200) {
          setEmprestimosList(response.data);
        }
      })
      .catch(() => {
        navigate('/');
      });

    await api(`EmprestimoOptions/${parseJwt().role}/1/10`).then((response) => {
      if (response.status) {
        setOptionsList(response.data);
      }
    });
  }

  function PostEmprestimo(emprestimo: EmprestimoPost) {
    api
      .post('Emprestimos', {
        idUsuario: emprestimo.idUsuario,
        idEmprestimoOptions: emprestimo.idEmprestimoOptions,
      })
      .then((response) => {
        if (response.status === 201) {
          GetOptions();
          toast.success('Empréstimo adquirido');
        }
      })
      .catch(() => {
        toast.error(`Empréstimo em atraso, ou limite simultâneo atingido`);
      });
  }

  function PayEmprestimo(idEmprestimo: number) {
    api
      .post(`Emprestimos/Concluir/${idEmprestimo}`)
      .then((response) => {
        if (response.status === 200) {
          GetOptions();
          toast.success('Empréstimo concluído');
        }
      })
      .catch((error) => {
        toast.error(error.Message);
      });
  }

  useEffect(() => {
    GetOptions();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <div>
      <ToastContainer position="top-center" autoClose={2000} />
      <Header type="" />
      <main id="emprestimos" className="container">
        <section className="left-menu-emprestimo">
          <h1 className="title-list">Pré-Aprovados</h1>
          <div className="list-emprestimos">
            {OptionsList.length !== 0 ? (
              OptionsList.map((option) => (
                <EmprestimoOption
                  key={option.idEmprestimoOption}
                  option={option}
                  onClick={() => {
                    PostEmprestimo({
                      idUsuario: parseInt(parseJwt().role, 10),
                      idEmprestimoOptions: option.idEmprestimoOption,
                    });
                  }}
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
                  key={emprestimo.idEmprestimo}
                  emprestimo={emprestimo}
                  onClick={() => PayEmprestimo(emprestimo.idEmprestimo)}
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
