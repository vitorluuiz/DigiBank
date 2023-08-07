export default function formatNumber(num: number): string {
  let absNum = Math.abs(num);

  const units = ['BRL', 'K', 'M', 'B', 'T', 'Q'];
  let unitIndex = 0;

  while (absNum >= 1000 && unitIndex < units.length - 1) {
    absNum /= 1000;
    // eslint-disable-next-line no-plusplus
    unitIndex++;
  }

  const roundedNum = (num >= 0 ? absNum : -absNum).toFixed(2);
  return `${roundedNum}${units[unitIndex]}`;
}

// retirar o default caso haja mais funções futuras
