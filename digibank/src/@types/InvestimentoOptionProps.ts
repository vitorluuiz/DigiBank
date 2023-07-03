import { TipoInvestimentoProps } from './TipoInvestimentoProps';

export interface InvestimentoOptionProps {
  idInvestimentoOption: number;
  idTipoInvestimento: number;
  nome: string;
  descricao: string;
  codeId: string;
  img: string;
  valorInicial: number;
  indiceConfiabilidade: number;
  indicedividendos: number;
  indiceValorizacao: number;
  dividendos: number;
  idTipoInvestimentoNavigation: TipoInvestimentoProps;
}
