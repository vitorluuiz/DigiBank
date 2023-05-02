const mask = (v: string) => {
  let result = v.replace(/\D/g, '');

  if (result.length <= 11) {
    result = result.replace(/(\d{3})(\d)/, '$1.$2');
    result = result.replace(/(\d{3})(\d)/, '$1.$2');
    result = result.replace(/(\d{3})(\d{1,2})$/, '$1-$2');
  }
  // else {
  //   result = result.substring(0, 14); // limita em 14 números
  //   result = result.replace(/^(\d{2})(\d)/, '$1.$2');
  //   result = result.replace(/^(\d{2})\.(\d{3})(\d)/, '$1.$2.$3');
  //   result = result.replace(/\.(\d{3})(\d)/, '.$1/$2');
  //   result = result.replace(/(\d{4})(\d)/, '$1-$2');
  // }

  return result;
};
export default mask;
