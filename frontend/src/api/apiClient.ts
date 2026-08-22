import {default as axios} from 'axios';

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5091';

export const apiClient = axios.create({
    baseURL: API_BASE_URL,
    withCredentials: true
})