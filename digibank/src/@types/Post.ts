export interface PostProps {
  idPost: number;
  idUsuario: number;
  apelidoProprietario: string;
  nome: string;
  descricao: string;
  mainImg: string;
  mainColorHex: string;
  imgs: [string];
  valor: number;
  isVirtual: boolean;
  vendas: number;
  qntAvaliacoes: number;
  avaliacao: number;
}
