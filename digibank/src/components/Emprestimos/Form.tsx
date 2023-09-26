import React, { useState } from 'react';
import { StyledTextField } from '../../assets/styledComponents/input';
import ModalTransacao from '../ModalEfetuarTransacao';
// import { NumberFormatCustom } from '../../pages/Cadastro/Cadastro';

export default function FormEmprestimo({ onSubmit }: { onSubmit: (valor: number) => void }) {
  const [valor, setValor] = useState<number>();
  const [isValidated, setValidated] = useState<boolean>();

  const validate = (event: any | undefined) => {
    event.preventDefault();

    if (!isValidated) {
      setValidated(true);
    } else {
      setValidated(false);
    }
  };
  return (
    <form onSubmit={validate}>
      <StyledTextField
        fullWidth
        autoComplete="off"
        variant="outlined"
        placeholder="Valor da parcela"
        type="number"
        value={valor}
        onChange={(evt) => setValor(parseFloat(evt.target.value))}
        // InputProps={{
        //   inputComponent: NumberFormatCustom,
        // }}
      />
      {/* <button
        type="submit"
        disabled={valor === undefined || valor === 0 || Number.isNaN(valor)}
        className="btnComponent"
      >
        Pagar empréstimo
      </button> */}
      <ModalTransacao
        data={{
          titulo: `Deseja pagar R$${valor} do seu emprestimo?`,
          valor: valor ?? 0,
        }}
        type="pagarEmprestimo"
        disabled={!isValidated}
        onClose={(isSuccess) => {
          if (isSuccess) {
            onSubmit(valor ?? 0);
          }
        }}
      />
    </form>
  );
}
