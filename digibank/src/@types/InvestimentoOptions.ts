export interface MinimalOptionProps {
  idInvestimentoOption: number;
  idTipoInvestimento: number;
  idAreaInvestimento: number;
  nome: string;
  sigla: string;
  areaInvestimento: string;
  descricao: string;
  logo: string;
  mainImg: string;
  mainColorHex: string;
  valor: number;
  colaboradores: number;
  qntCotasTotais: number;
  fundacao: string;
  abertura: string;
  variacaoPercentual: number;
}

export interface FullOptionProps {
  idInvestimentoOption: number;
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

export interface StatsOptionProps {
  coeficienteVariativo: number;
  dividendos: number;
  marketCap: number;
  max: number;
  media: number;
  min: number;
  minMax: number;
  minMaxPercentual: number;
  valor: number;
  variacaoPeriodo: number;
  variacaoPeriodoPercentual: number;
}

export interface TitleOptionProps {
  idInvestimentoOption: number;
  nome: string;
  valor: number;
  sigla: string;
  logo: string;
  variacaoPercentual: number;
}

export interface InvestidoOptionProps {
  dataAquisicao: string;
  depositoInicial: number;
  IdInvestimento: number;
  idUsuario: number;
  isEntrada: boolean;
  qntCotas: number;
  idInvestimentoOptionNavigation: MinimalOptionProps;
}
export interface OptionPropsGenerico {
  dataAquisicao: string;
  depositoInicial: number;
  areaInvestimento: string;
  IdInvestimento: number;
  idUsuario: number;
  isEntrada: boolean;
  qntCotas: number;
  idInvestimentoOption: number;
  idTipoInvestimento: number;
  idAreaInvestimento: number;
  nome: string;
  sigla: string;
  descricao: string;
  logo: string;
  mainImg: string;
  mainColorHex: string;
  valor: number;
  colaboradores: number;
  qntCotasTotais: number;
  fundacao: string;
  abertura: string;
  variacaoPercentual: number;
}
