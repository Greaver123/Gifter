import axios from 'axios';

export const axiosDevInstance = axios.create({
  baseURL: 'https://192.168.1.32:5001/api',
  timeout: 60000,
});
