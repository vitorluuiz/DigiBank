export const formatCurrency = (value: string) => {
  // Remove todos os caracteres não numéricos
  const numericValue = parseFloat(value.replace(/\D/g, '')) / 100;

  // Verifique se é um número válido
  if (Number.isNaN(numericValue)) {
    return 'R$ 0,00';
  }

  return numericValue.toLocaleString('pt-BR', {
    style: 'currency',
    currency: 'BRL',
    minimumFractionDigits: 2,
  });
};

export const parseCurrencyToFloat = (formattedValue: string) => {
  // Remove o símbolo de moeda e espaços em branco
  const formattedCurrency: string = formattedValue.replace('R$', '');
  const decimal: string = formattedCurrency.split(',')[1];
  const inteiro: string = formattedCurrency.split(',')[0].replaceAll('.', '');

  return parseFloat(`${inteiro}.${decimal}`);
};
