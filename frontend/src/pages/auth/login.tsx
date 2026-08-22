import AuthBtn from "../../components/button/btn";
import "./auth.scss"
import { Link, useNavigate } from 'react-router-dom';
import { useState } from 'react';
import { useAuth } from "../../api/authContext";
import * as React from "react";
import type { LoginRequest } from "../../types/user";


export const Login = () => {
    const [formData, setFormData] = useState<LoginRequest>({
        userName: '',
        password: '',
    }); const [error, setError] = useState("");
    const [isLoading, setIsLoading] = useState(false);
    const navigate = useNavigate();
    const { login } = useAuth();

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setFormData({
            ...formData,
            [e.target.name]: e.target.value
        });
    };

    const handleLogin = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        try {
            if (!formData.userName || !formData.password) {
                setError('Please enter both username and password.');
                return;
            }
            setIsLoading(true);
            setError("");
            console.log("Authenticating...")


            const response = await login(formData);
            console.log('Login successful:', response);
            console.log("Redirecting user: ", response);
            navigate('/reisa');
        } catch (error) {
            console.error('Error logging in:', error);
            setError('Invalid username or password.');
        } finally {
            setIsLoading(false);
        }
    };


    return (
        <>
            <form onSubmit={handleLogin} action="" className="login-form">
                <header className="form-header">
                    <h2>
                        Welcome Back!
                    </h2>

                    <span>
                        Sign in to your account to continue
                    </span>
                </header>

                <label htmlFor="userName">
                    USERNAME
                </label>
                <input
                    type="text"
                    name="userName"
                    className="userName"
                    id="userName"
                    placeholder="John Doe"
                    onChange={handleChange}
                    required
                />

                <label htmlFor="password">
                    PASSWORD
                </label>
                <input
                    type="password"
                    name="password"
                    id="password"
                    placeholder="password"
                    onChange={handleChange}
                    required
                />

                <div className="btn-wrapper">
                    <AuthBtn
                        text={isLoading ? 'Loading...' : 'Login'}
                        disabled={isLoading}
                        type="submit"
                    />


                    <a href="https://localhost:7153/api/oauth2/auth/google" className="google">
                        Continue to Google
                    </a>
                </div>

                <div className="register-link">
                    <Link to="/auth/register" className="register">
                        Create Account
                    </Link>
                </div>

                {error && <p className="text-danger">{error}</p>} {/* Render error message if exists */}
            </form>
        </>
    )
}