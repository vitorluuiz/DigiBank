export const usuarioAutenticado = () => localStorage.getItem('usuario-login-auth') ?? 'undefined';

export const parseJwt = () => {
  const localStorageItem: string = localStorage.getItem('usuario-login-auth') ?? 'undefined';
  const base64: string = localStorageItem.split('.')[1] ?? 'undefined';
  if (base64 === 'undefined') {
    return { role: 'undefined' };
  }
  return JSON.parse(window.atob(base64));
};
