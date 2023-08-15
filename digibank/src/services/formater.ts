// eslint-disable-next-line eslint-comments/disable-enable-pair
/* eslint-disable no-else-return */
export function formatNumber(num: number): string {
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

export function formatDate(days: number): string {
  if (days < 1) {
    return `${days} dias`;
  }

  const years = Math.floor(days / 365);
  const remainingDaysAfterYears = days % 365;

  const months = Math.floor(remainingDaysAfterYears / 30);
  const remainingDays = remainingDaysAfterYears % 30;

  const parts = [];

  if (years > 0) {
    parts.push(`${years} ano${years === 1 ? '' : 's'}`);
  }

  if (months > 0) {
    parts.push(`${months} ${months === 1 ? 'mês' : 'meses'}`);
  }

  if (remainingDays > 0) {
    parts.push(`${remainingDays} dia${remainingDays === 1 ? '' : 's'}`);
  }

  return parts.join(' e ');
}
