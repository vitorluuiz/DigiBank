import React, { useEffect, useRef, useState } from 'react';
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

import Dialog from '@mui/material/Dialog';
import { MenuItem } from '@mui/material';

import api from '../../services/api';
import { parseJwt } from '../../services/auth';
import { MinimalOptionProps, TitleOptionProps } from '../../@types/InvestimentoOptions';
import { HistoryOptionProps } from '../../@types/HistoryOption';
import HistoryGraph from '../Investimentos/HistoryGraph';
import { CssTextField } from '../../assets/styledComponents/input';
import ModalTransacao from '../ModalEfetuarTransacao';
import CustomSnackbar from '../../assets/styledComponents/snackBar';
import { useSnackBar } from '../../services/snackBarProvider';

export interface TitleInvestimentoProps {
  idInvestimentoOptionNavigation: MinimalOptionProps;
  qntCotas: number;
}

export default function ModalVendaCotas({ onClose }: { onClose: () => void }) {
  const [open, setOpen] = useState<boolean>(false);
  const [Investimento, setInvestimento] = useState<string>('');
  const [InvestimentoOption, setInvestimentoOption] = useState<TitleOptionProps>();
  const [listaInvestimento, setListaInvestimento] = useState<TitleInvestimentoProps[]>([]);
  const [qntdCotas, setQntdCotas] = useState<number>(0);
  const [Valor, setValor] = useState<number>();
  const [minhasCotas, setMinhasCotas] = useState<number[]>([]);
  const [idTipo] = useState<number>(-1);
  const [pagina] = useState<number>(1);
  const [qntItens] = useState<number>(100);
  const [isSearched, setSearched] = useState<boolean>();
  const [isValidated, setValidated] = useState<boolean>(false);
  const [historyData, setHistoryData] = useState<HistoryOptionProps[]>([]);
  const [parentWidth, setParentWidth] = useState(100);

  const resetFields = () => {
    setInvestimento('');
    setInvestimentoOption(undefined);
    setQntdCotas(0);
    setValor(undefined);
    setMinhasCotas([]);
    setSearched(false);
    setValidated(false);
    setHistoryData([]);
  };
  const { currentMessage, handleCloseSnackBar } = useSnackBar();

  const handleClickOpenModal = () => {
    setOpen(true);
  };

  const handleCloseModal = () => {
    setOpen(false);
    onClose();
  };

  function getInvestidos() {
    api(`/Investimento/Usuario/${parseJwt().role}/${idTipo}`, {
      params: {
        pagina,
        qntItens,
      },
    })
      .then((response) => {
        if (response.status === 200) {
          setListaInvestimento(response.data.investimentosList);
        }
      })
      .catch(() => {
        toast.error('Usuário não encontrado');
      });
  }

  function getOption(idOption: number) {
    api(`InvestimentoOptions/${idOption}/Dias/365`)
      .then((response) => {
        if (response.status === 200) {
          setInvestimentoOption(response.data.option);
          setSearched(true);
          api(`HistoryInvest/Option/${idOption}/365`).then((responseHistory) => {
            if (responseHistory.status === 200) {
              const history: HistoryOptionProps[] = responseHistory.data.historyList;
              setHistoryData(history);
            }
          });
        }
      })
      .catch(() => {
        toast.error('Usuário não encontrado');
      });
  }

  const validate = (event: any | undefined) => {
    event.preventDefault();

    if (!isValidated && isSearched) {
      setValidated(true);
    } else {
      setValidated(false);
    }
  };
  const parentRef = useRef(null);

  useEffect(() => {
    getInvestidos();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  useEffect(() => {
    const parentElement = parentRef.current;

    if (!parentElement) return;

    // Função para ser executada quando o tamanho do elemento for alterado
    const handleResize = (entries: ResizeObserverEntry[]) => {
      entries.forEach((entry) => {
        setParentWidth(entry.contentRect.width);
      });
    };

    const resizeObserver = new ResizeObserver(handleResize);
    resizeObserver.observe(parentElement);

    // eslint-disable-next-line consistent-return
    return () => {
      resizeObserver.unobserve(parentElement);
    };
  }, [isSearched]);

  return (
    <div>
      <div title="Vender cotas de um investimento" id="vendaCota" style={{ width: '100%' }}>
        <button id="btnVendaCota" onClick={handleClickOpenModal}>
          Vender Cotas
        </button>
        <Dialog open={open} onClose={handleCloseModal}>
          <CustomSnackbar message={currentMessage} onClose={handleCloseSnackBar} />
          <div id="suport-modal-transferir">
            <section
              id="left-modal-transferir"
              className={`left-modal-transferir ${isSearched ? 'visible' : 'invisible'}`}
            >
              <section className="modal-invest-infos">
                <img src={InvestimentoOption?.logo} alt="Logo do investimento" />
                <div className="box-name-invest">
                  <h1>{InvestimentoOption?.nome}</h1>
                  <h2>{InvestimentoOption?.sigla}</h2>
                </div>
              </section>
              <section ref={parentRef} className="modal-invest-flow">
                <span>Valores no ultimo ano:</span>
                {historyData.length !== 0 ? (
                  <HistoryGraph historyData={historyData} height={200} width={parentWidth} />
                ) : null}
              </section>
            </section>

            <section className="right-modal-transferir">
              <form onSubmit={validate}>
                <CssTextField
                  inputProps={{ maxLength: 14, minLength: 14 }}
                  label="Investimento"
                  required
                  variant="outlined"
                  fullWidth
                  type="select"
                  value={Investimento}
                  select
                  onChange={(evt) => {
                    const selectedInvestmentId = Number(evt.target.value);
                    setInvestimento(String(selectedInvestmentId));
                    getOption(selectedInvestmentId);
                    const selectedInvestment = listaInvestimento.find(
                      (investiment) =>
                        investiment.idInvestimentoOptionNavigation.idInvestimentoOption ===
                        selectedInvestmentId,
                    );
                    if (selectedInvestment) {
                      setMinhasCotas(
                        Array.from(
                          { length: selectedInvestment.qntCotas },
                          (_, index) => index + 1,
                        ),
                      );
                    }
                  }}
                >
                  {listaInvestimento.map((investiment) => (
                    <MenuItem
                      value={investiment.idInvestimentoOptionNavigation.idInvestimentoOption}
                      key={investiment.idInvestimentoOptionNavigation.idInvestimentoOption}
                    >
                      {investiment.idInvestimentoOptionNavigation.nome} -{' '}
                      {investiment.idInvestimentoOptionNavigation.sigla}
                    </MenuItem>
                  ))}
                </CssTextField>
                <CssTextField
                  label="Quantidade de cotas"
                  type="number"
                  variant="outlined"
                  fullWidth
                  select
                  required
                  value={qntdCotas}
                  onChange={(evt) => {
                    const selectedQntdCotas = Number(evt.target.value);
                    setQntdCotas(selectedQntdCotas);
                    if (InvestimentoOption) {
                      setValor(InvestimentoOption.valor);
                    }
                  }}
                >
                  {minhasCotas.map((qntd) => (
                    <MenuItem value={qntd} key={qntd}>
                      {qntd}
                    </MenuItem>
                  ))}
                </CssTextField>
                <CssTextField
                  label="Valor Total"
                  type="number"
                  variant="outlined"
                  fullWidth
                  disabled
                  value={
                    InvestimentoOption && typeof qntdCotas === 'number'
                      ? qntdCotas * InvestimentoOption.valor
                      : 0
                  }
                />
                <ModalTransacao
                  type="vendaCotas"
                  data={{
                    img: InvestimentoOption?.logo,
                    titulo: `Deseja vender ${qntdCotas} cota(s) de ${InvestimentoOption?.nome}`,
                    valor: Valor ?? 0,
                    option: InvestimentoOption?.idInvestimentoOption,
                    preCotas: minhasCotas[minhasCotas.length - 1],
                    qntCotas: qntdCotas,
                  }}
                  onClose={() => {
                    getInvestidos();
                    setValidated(false);
                    resetFields();
                  }}
                />
              </form>
            </section>
          </div>
        </Dialog>
      </div>
    </div>
  );
}
