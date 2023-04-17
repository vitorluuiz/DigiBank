export interface ExtratoProps {
  idTransacao: number;
  idUsuarioPagante: number;
  nomePagante: string;
  idUsuarioRecebente: number;
  nomeRecebente: string;
  valor: number;
  dataTransacao: Date;
  descricao: string;
}
