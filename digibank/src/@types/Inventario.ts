import { PostProps } from './Post';

export interface ItemProps {
  idInventario: number;
  idUsuario: number;
  idPost: number;
  valor: number;
  dataAquisicao: Date;
  mainColorHex: string;
  idPostNavigation: PostProps;
}
