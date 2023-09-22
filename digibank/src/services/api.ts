import axios from 'axios';

const api = axios.create({
  baseURL: 'http://191.180.171.214:5001/api/',
});

async function renewToken() {
  try {
    const response = await api.get('/Login/RefreshToken');
    const newToken = response.data.token;

    return newToken;
  } catch (error) {
    window.location.replace('401');
    return null;
  }
}

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
  async (error) => {
    if (!error.response) {
      window.location.replace('/503');
    } else if (error.response.status === 401) {
      const newToken: string = await renewToken();

      if (newToken) {
        const requestConfig = error.config;
        requestConfig.headers.Authorization = `Bearer ${newToken}`;
        localStorage.setItem('usuario-login-auth', newToken);

        // eslint-disable-next-line @typescript-eslint/return-await
        return await api(requestConfig).catch(() => {
          window.location.replace('401');
        });
      }
    } else if (error.response.status === 403) {
      const requestConfig = error.config;
      window.location.replace('/403');
      return axios(requestConfig);
    }

    // return axios(requestConfig);
    return Promise.reject();
  },
);

export const IMGROOT = 'http://191.180.171.214:5001/img/';

export default api;
