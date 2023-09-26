export function valueLabelFormat(value: number) {
  const units = ['M', 'B', 'T'];

  let unitIndex = 0;
  let scaledValue = value;

  while (scaledValue >= 1000 && unitIndex < units.length - 1) {
    unitIndex += 1;
    scaledValue /= 1000;
  }

  return `${scaledValue} ${units[unitIndex]}`;
}

export function calculateValue(value: number, peso: number) {
  let newValue = value;

  switch (Math.floor(value / 15)) {
    case 0:
      newValue *= 1;
      break;
    case 1:
      newValue *= 2;
      break;
    case 2:
      newValue *= 4;
      break;
    case 3:
      newValue *= 8;
      break;
    case 4:
      newValue *= 16;
      break;
    case 5:
      newValue *= 22;
      break;
    default:
      newValue *= 30;
      break;
  }

  return newValue * peso;
}
