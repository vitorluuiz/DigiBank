import axios from 'axios';

const api = axios.create({
  baseURL: 'https://858d-2804-14c-c1-94e0-ed84-1eaa-5b97-f59b.ngrok-free.app/api/',
});

export default api;
