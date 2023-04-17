export interface OptionProps {
  idEmprestimoOption: number;
  valor: number;
  taxaJuros: number;
  rendaMinima: number;
  prazoEstimado: number;
}

export interface EmprestimoProps {
  idEmprestimo: number;
  idUsuario: number;
  idEmprestimoOption: number;
  idEmprestimoOptionsNavigation: OptionProps;
  valorPago: number;
  dataInicial: Date;
  dataFinal: Date;
  ultimoPagamento: Date;
}

export interface EmprestimoPost {
  idUsuario: number;
  idEmprestimoOptions: number;
}
