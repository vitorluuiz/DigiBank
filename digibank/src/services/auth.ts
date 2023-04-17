export const usuarioAutenticado = () =>
  localStorage.getItem('usuario-login-auth') !== null
    ? localStorage.getItem('usuario-login-auth')
    : 'não autenticado';

export const parseJwt = () => {
  const localStorageItem: string | null = localStorage.getItem('usuario-login-auth');
  const base64: string = localStorageItem != null ? localStorageItem.split('.')[1] : 'Erro';

  return JSON.parse(window.atob(base64));
};
