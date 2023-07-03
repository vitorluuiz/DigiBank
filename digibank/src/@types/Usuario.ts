import { MetaDestaque } from './MetaDestaque';

export interface UsuarioProps {
  idUsuario: number;
  nomeCompleto: string;
  apelido: string;
  cpf: string;
  telefone: string;
  email: string;
  digiPoints: number;
  saldo: number;
  rendaFixa: number;
  investido: number;
  metaDestaque: MetaDestaque;
}

export interface UsuarioPublicoProps {
  idUsuario: number;
  nomeCompleto: string;
  apelido: string;
  email: string;
  cpf: string;
  telefone: string;
}
