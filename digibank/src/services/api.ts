import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5000/api/',
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem('usuario-login-auth');
  if (token) {
    // eslint-disable-next-line no-param-reassign
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// api.interceptors.response.use((response) => {
//   if (response.status === 401) {
//     window.location.replace('/401');
//   } else if (response.status === 403) {
//     window.location.replace('/403');
//   }
//   return response;
// });

export default api;
