export interface User {
    userName: string;
    email: string;
    password: string;
}

export interface LoginRequest {
    userName: string;
    password: string;
}

export interface RegisterRequest {
    userName: string;
    password: string;
    confirmPassword: string;
}