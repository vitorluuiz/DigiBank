// eslint-disable-next-line eslint-comments/disable-enable-pair
/* eslint-disable consistent-return */
import { Dialog } from '@mui/material';
import React, { useEffect, useState } from 'react';
import FlowBox from '../FlowBox';
import FormEmprestimo from './Form';
import GetButton from './Button';
import {
  EmprestimoProps,
  EmprestimoPropsGenerico,
  EmprestimoSimulado,
  OptionProps,
} from '../../@types/Emprestimo';
import { formatDate } from '../../services/formater';
import api from '../../services/api';
import { parseJwt } from '../../services/auth';
import { useSnackBar } from '../../services/snackBarProvider';

const mapEmprestimo = (Data: EmprestimoProps | OptionProps): EmprestimoPropsGenerico => {
  let emprestimo: EmprestimoPropsGenerico;

  if ('idEmprestimoOptionsNavigation' in Data) {
    emprestimo = {
      idEmprestimo: Data.idEmprestimo,
      idUsuario: Data.idUsuario,
      idCondicao: Data.idCondicao,
      condicao: Data.idCondicaoNavigation.condicao,
      idEmprestimoOption: Data.idEmprestimoOption,
      valorPago: Data.valorPago,
      ultimoPagamento: Data.ultimoPagamento,
      dataInicial: Data.dataInicial,
      dataFinal: Data.dataFinal,
      valor: Data.idEmprestimoOptionsNavigation.valor,
      taxaJuros: Data.idEmprestimoOptionsNavigation.taxaJuros,
      rendaMinima: Data.idEmprestimoOptionsNavigation.rendaMinima,
      prazoEstimado: Data.idEmprestimoOptionsNavigation.prazoEstimado,
    };
  } else {
    emprestimo = {
      idEmprestimo: 0,
      idUsuario: 0,
      idCondicao: 0,
      condicao: '',
      idEmprestimoOption: Data.idEmprestimoOption,
      valorPago: 0,
      ultimoPagamento: new Date(),
      dataInicial: new Date(),
      dataFinal: new Date(),
      valor: Data.valor,
      taxaJuros: Data.taxaJuros,
      rendaMinima: Data.rendaMinima,
      prazoEstimado: Data.prazoEstimado,
    };
  }

  return emprestimo;
};

export default function ModalEmprestimo({
  data,
  type,
  onUpdate,
}: {
  data: EmprestimoProps | OptionProps;
  type: string;
  onUpdate: () => void;
}) {
  const [open, setOpen] = useState<boolean>(false);
  const [intervaloUpdater, setIntervaloUpdater] = useState<NodeJS.Timer | null>(null);
  const [isLoading, setLoading] = useState<boolean>(false);
  const [Emprestimo, setEmprestimo] = useState<EmprestimoPropsGenerico>(mapEmprestimo(data));
  const [Simulacao, setSimulacao] = useState<EmprestimoSimulado>({
    parcelaSugerida: 0,
    progressoPagamento: 0,
    progressoPrazo: 0,
    proximaParcela: new Date(),
    restanteAvista: 0,
    restanteParcelado: 0,
  });

  const { postMessage } = useSnackBar();

  const handleOpenModal = () => {
    setOpen(true);
  };

  const handleCloseModal = () => {
    setOpen(false);
    onUpdate();
  };

  function mapSimulacao(emprestimo: EmprestimoProps | OptionProps) {
    if ('idEmprestimoOptionsNavigation' in emprestimo) {
      api(`Emprestimos/Simular/${emprestimo.idEmprestimo}`).then((response) => {
        if (response.status === 200) {
          setSimulacao(response.data);
        }
      });
    } else {
      api(`EmprestimoOptions/Simular/${emprestimo.idEmprestimoOption}`).then((response) => {
        if (response.status === 200) {
          setSimulacao(response.data);
        }
      });
    }
  }

  const StatusColor = (status: string) => {
    let color: string;
    switch (status) {
      case 'À pagar':
        color = '#FFC524';
        break;
      case 'Pago':
        color = 'darkgreen';
        break;
      default:
        color = '#F56F22';
        break;
    }

    return color;
  };

  const PostEmprestimo = () => {
    setLoading(true);

    api
      .post(`Emprestimos`, {
        idUsuario: parseJwt().role,
        idEmprestimoOptions: Emprestimo.idEmprestimoOption,
      })
      .then((response) => {
        if (response.status === 201) {
          onUpdate();
          postMessage({ message: 'Empréstimo adquirido', severity: 'success', timeSpan: 2500 });
          handleCloseModal();
        }
      })
      .catch((error) => postMessage({ message: error, severity: error, timeSpan: 3000 }));

    setInterval(() => {
      setLoading(false);
    }, 2000);
  };

  const PagarParcelaEmprestimo = (valor: number) => {
    api
      .post(`Emprestimos/PagarParcela`, {
        idEmprestimo: Emprestimo.idEmprestimo,
        valor,
      })
      .then((response) => {
        if (response.status === 200) {
          setEmprestimo(mapEmprestimo(response.data.emprestimo));
          mapSimulacao(response.data.emprestimo);
          postMessage({
            message: `${response.data.message}`,
            severity: 'success',
            timeSpan: 2500,
          });
        }
      })
      .catch(() =>
        postMessage({
          message: 'Não foi possível concluir o pagamento',
          severity: 'error',
          timeSpan: 3000,
        }),
      );
  };

  const StartUpdater = (): NodeJS.Timer => setInterval(() => mapSimulacao(data), 10000);

  const StopUpdater = (updater: NodeJS.Timer) => clearInterval(updater);

  useEffect(() => (open ? mapSimulacao(data) : console.log('Componente fechado')), [open, data]);

  useEffect(() => {
    if (open) {
      setIntervaloUpdater(StartUpdater());
    } else if (intervaloUpdater) {
      StopUpdater(intervaloUpdater);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [open]);

  return (
    <div>
      <button onClick={handleOpenModal}>Ver mais</button>
      <Dialog open={open} onClose={handleCloseModal}>
        <div id="support-modal-emprestimo">
          <div className="support-title-bar">
            <div className="title">
              <h1>
                Empréstimo de{' '}
                {Emprestimo.valor.toLocaleString('pt-BR', {
                  style: 'currency',
                  currency: 'BRL',
                })}
              </h1>
              <h4>
                Parcelas dividas em até{' '}
                <span>
                  {Math.ceil(Emprestimo.prazoEstimado / 30)}{' '}
                  {`${Emprestimo.prazoEstimado / 30 >= 2 ? 'meses + ' : 'mês + '}`}
                  {formatDate(Emprestimo.prazoEstimado * 0.2)}
                </span>{' '}
                com taxa fixa de
                <span>
                  {' '}
                  {Emprestimo.taxaJuros}% + {(Emprestimo.taxaJuros * 0.2).toFixed(3)}% a.m
                </span>
              </h4>
            </div>
            {type === 'option' ? null : (
              <div className="status-box">
                <div className="status">
                  <div
                    id="status-circle"
                    style={{ backgroundColor: StatusColor(Emprestimo.condicao) }}
                  />
                  <h1>{Emprestimo.condicao}</h1>
                </div>
                <h4>
                  Última parcela sugerida{' '}
                  <span>
                    {new Date(Emprestimo.dataFinal).toLocaleDateString('pt-BR', {
                      day: '2-digit',
                      month: '2-digit',
                      year: 'numeric',
                    })}
                  </span>
                  <br />
                  Valor sugerido{' '}
                  <span>
                    {Simulacao.parcelaSugerida.toLocaleString('pt-BR', {
                      style: 'currency',
                      currency: 'BRL',
                    })}
                  </span>
                </h4>
                {Emprestimo.ultimoPagamento !== Emprestimo.dataInicial ? (
                  <h4>
                    Última parcela paga{' '}
                    <span>
                      {new Date(Emprestimo.ultimoPagamento).toLocaleDateString('pt-BR', {
                        day: '2-digit',
                        month: '2-digit',
                        year: 'numeric',
                      })}
                    </span>
                    <br />
                    Valor pago <span>R${Emprestimo.valorPago}</span>
                  </h4>
                ) : null}
              </div>
            )}
          </div>
          <div className="support-body">
            <div className="main-infos-emprestimo">
              {type === 'option' ? (
                <section className="bank-flow">
                  <FlowBox name="Parcela sugerida" valor={Simulacao.parcelaSugerida} />
                  <FlowBox name="Valor a receber" valor={Emprestimo.valor} saldo />
                </section>
              ) : (
                <section className="bank-flow">
                  <FlowBox name="Pagamento parcelado" valor={Simulacao.restanteParcelado} />
                  <FlowBox name="Pagamento realizado" valor={Emprestimo.valorPago} />
                  <FlowBox name="Restante" valor={Simulacao.restanteAvista} saldo />
                </section>
              )}
              <h4>
                Prazo de {formatDate(Emprestimo.prazoEstimado)} com extensão de mais{' '}
                {formatDate(Emprestimo.prazoEstimado * 0.2)} adicionais
              </h4>
            </div>
            <section className="main-actions-body">
              {type === 'option' ? (
                <GetButton
                  data={Simulacao.proximaParcela}
                  valor={Simulacao.parcelaSugerida}
                  isLoading={isLoading}
                  onPost={() => PostEmprestimo()}
                />
              ) : (
                <FormEmprestimo onSubmit={(valor) => PagarParcelaEmprestimo(valor)} />
              )}
            </section>
          </div>
        </div>
      </Dialog>
    </div>
  );
}
