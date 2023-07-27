// mudar nome para InvestimentoOptionCompleto
export interface InvestidosProps {
  idOption: number;
  idTipo: number;
  idArea: number;
  tipo: string;
  area: string;
  nome: string;
  descricao: string;
  sigla: string;
  logo: string;
  mainImg: string;
  mainColorHex: string;
  colaboradores: string;
  valor: number;
  marketCap: number;
  fundacao: Date;
  abertura: Date;
  sede: string;
  fundador: string;
  dividendos: number;
  variacaoPercentual: number;
}
