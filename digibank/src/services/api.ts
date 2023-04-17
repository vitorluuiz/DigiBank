import axios from 'axios';
import { usuarioAutenticado } from './auth';

const headers = {
  Authorization: `Bearer ${usuarioAutenticado()}`,
};

const api = axios.create({
  baseURL: 'http://localhost:5000/api/',
  headers,
});

export default api;
