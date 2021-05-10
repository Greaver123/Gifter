import axios from 'axios';

export const axiosDevInstance = axios.create({
  baseURL: 'https://192.168.1.32:44327/api',
  timeout: 120000,
});
