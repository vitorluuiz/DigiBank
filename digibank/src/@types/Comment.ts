export interface CommentProps {
  idAvaliacao: number;
  idUsuario: number;
  publicador: string;
  nota: number;
  dataPostagem: Date;
  replies: number;
  isReplied: boolean;
  comentario: string;
}
