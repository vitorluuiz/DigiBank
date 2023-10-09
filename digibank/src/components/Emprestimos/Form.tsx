import React, { useState } from 'react';
import { StyledTextField } from '../../assets/styledComponents/input';
import ModalTransacao from '../ModalEfetuarTransacao';
import { formatCurrency, parseCurrencyToFloat } from '../../assets/styledComponents/DolarInput';

export default function FormEmprestimo({ onSubmit }: { onSubmit: (valor: number) => void }) {
  const [valor, setValor] = useState<string>('');
  const [isValidated, setValidated] = useState<boolean>();

  const validate = (event: any | undefined) => {
    event.preventDefault();

    if (!isValidated) {
      setValidated(true);
    } else {
      setValidated(false);
    }
  };

  const handleChangeValor = (newValue: string) => setValor(formatCurrency(newValue));

  return (
    <form onSubmit={validate}>
      <StyledTextField
        fullWidth
        autoComplete="off"
        variant="outlined"
        placeholder="Valor da parcela"
        value={valor}
        onChange={(evt) => handleChangeValor(evt.target.value)}
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
          valor: parseCurrencyToFloat(valor),
        }}
        type="pagarEmprestimo"
        disabled={!isValidated}
        onClose={(isSuccess) => {
          if (isSuccess) {
            onSubmit(parseCurrencyToFloat(valor));
          }
        }}
      />
    </form>
  );
}
