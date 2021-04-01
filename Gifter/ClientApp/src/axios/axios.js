import axios from 'axios';

export const axiosDevInstance = axios.create({
  baseURL: 'https://localhost:44327/api',
  timeout: 60000,
});
