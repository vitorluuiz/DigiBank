import React, { useState } from 'react';
import { StyledTextField } from '../../assets/styledComponents/input';
// import { NumberFormatCustom } from '../../pages/Cadastro/Cadastro';

export default function FormEmprestimo({
  onSubmit,
}: {
  onSubmit: (evt: any, valor: number) => void;
}) {
  const [valor, setValor] = useState<number>();

  return (
    <form onSubmit={(evt) => onSubmit(evt, valor || 0)}>
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
      <button
        type="submit"
        disabled={valor === undefined || valor === 0 || Number.isNaN(valor)}
        className="btnComponent"
      >
        Pagar empréstimo
      </button>
    </form>
  );
}
