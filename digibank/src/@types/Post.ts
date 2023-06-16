export interface PostProps {
  idPost: number;
  idUsuario: number;
  apelidoProprietario: string;
  nome: string;
  descricao: string;
  mainImg: string;
  imgs: GaleriaProps[];
  valor: number;
  isVirtual: boolean;
  vendas: number;
  qntAvaliacoes: number;
  avaliacao: number;
}

export interface GaleriaProps {
  img: string;
}
