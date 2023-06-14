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

api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (!error.response) {
      window.location.replace('/503');
    } else if (error.response.status === 401) {
      const requestConfig = error.config;
      window.location.replace('/401');
      return axios(requestConfig);
    } else if (error.response.status === 403) {
      const requestConfig = error.config;
      window.location.replace('/403');
      return axios(requestConfig);
    }

    // return axios(requestConfig);
    return Promise.reject();
  },
);

export const IMGROOT = 'http://localhost:5000/img/';

export default api;
