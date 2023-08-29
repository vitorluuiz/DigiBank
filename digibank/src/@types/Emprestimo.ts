export interface OptionProps {
  idEmprestimoOption: number;
  valor: number;
  taxaJuros: number;
  rendaMinima: number;
  prazoEstimado: number;
}

interface StatusEmprestimo {
  idCondicao: number;
  condicao: string;
}

export interface EmprestimoProps {
  idEmprestimo: number;
  idUsuario: number;
  idCondicao: number;
  idEmprestimoOption: number;
  valorPago: number;
  ultimoPagamento: Date;
  dataInicial: Date;
  dataFinal: Date;
  idCondicaoNavigation: StatusEmprestimo;
  idEmprestimoOptionsNavigation: OptionProps;
}

export interface EmprestimoSimulado {
  restanteParcelado: number;
  parcelaSugerida: number;
  progressoPagamento: number;
  progressoPrazo: number;
  restanteAvista: number;
  proximaParcela: Date;
}

export interface EmprestimoPropsGenerico {
  idEmprestimo: number;
  idUsuario: number;
  idCondicao: number;
  idEmprestimoOption: number;
  valorPago: number;
  ultimoPagamento: Date;
  dataInicial: Date;
  dataFinal: Date;
  valor: number;
  taxaJuros: number;
  rendaMinima: number;
  prazoEstimado: number;
  condicao: string;
}

export interface EmprestimoPost {
  idUsuario: number;
  idEmprestimoOptions: number;
}
