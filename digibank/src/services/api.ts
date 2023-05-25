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

<<<<<<< HEAD
// api.interceptors.response.use((response) => {
//   if (response.status === 401) {
//     window.location.replace('/401');
//   } else if (response.status === 403) {
//     window.location.replace('/403');
//   }
//   return response;
// });
=======
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response.status === 401) {
      const requestConfig = error.config;
      window.location.replace('/401');
      return axios(requestConfig);
    }
    if (error.response.status === 403) {
      const requestConfig = error.config;
      window.location.replace('/403');
      return axios(requestConfig);
    }

    return Promise.reject(error);
  },
);
>>>>>>> 47cdda6ed062f7304e903299f7bcf051d38a59f8

export default api;
