import axios from 'axios';
import { usuarioAutenticado } from './auth';

const api = axios.create({
  baseURL: 'http://localhost:5000/api/',
});

api.defaults.headers.common = { Authorization: `Bearer ${usuarioAutenticado()}` };

api.interceptors.request.use((config) => {
  const token = localStorage.getItem('usuario-login-auth');
  if (token) {
    // eslint-disable-next-line no-param-reassign
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default api;
