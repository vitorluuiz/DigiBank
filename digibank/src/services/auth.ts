<<<<<<< HEAD
export const usuarioAutenticado = () =>
  localStorage.getItem('usuario-login-auth') !== null
    ? localStorage.getItem('usuario-login-auth')
    : 'nao autenticado';
=======
export const usuarioAutenticado = () => localStorage.getItem('usuario-login-auth') ?? 'undefined';
>>>>>>> 47cdda6ed062f7304e903299f7bcf051d38a59f8

export const parseJwt = () => {
  const localStorageItem: string = localStorage.getItem('usuario-login-auth') ?? 'undefined';
  const base64: string = localStorageItem.split('.')[1] ?? 'undefined';

  return JSON.parse(window.atob(base64));
};
