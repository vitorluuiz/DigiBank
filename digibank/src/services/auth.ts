export const usuarioAutenticado = () => localStorage.getItem('usuario-login-auth') ?? 'undefined';

export const parseJwt = () => {
  const localStorageItem: string = localStorage.getItem('usuario-login-auth') ?? 'undefined';
  const base64: string = localStorageItem.split('.')[1] ?? 'undefined';

  return JSON.parse(window.atob(base64));
};
