import {apiClient} from "./apiClient.ts";
import type {LoginRequest, RegisterRequest} from "../types/user.ts";

export const login = (credentials: LoginRequest) =>
{
    return apiClient.post('/api/auth/login', credentials)
}

export const register = (userData: RegisterRequest) =>
{
    return apiClient.post('/api/auth/register', userData);
}

export const logout = () =>
{
    return apiClient.post('/api/auth/logout')
}