export interface CartaoProps {
  idCartao: number;
  nome: string;
  numero: string;
  cvv: string;
  dataExpira: Date;
  isValid: boolean;
}
