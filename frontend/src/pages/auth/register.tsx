import { Link } from "react-router-dom";
import AuthBtn from "../../components/button/btn";
import "./auth.scss"
import { useState } from "react";
import type { RegisterRequest } from "../../types/user";
import { register } from "../../api/authService";

export const Register = () => {
    const [formData, setFormData] = useState<RegisterRequest>({
        userName: '',
        password: '',
        confirmPassword: '',
    });

    const [error, setError] = useState("");
    const [isLoading, setIsLoading] = useState(false);


    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setFormData({
            ...formData,
            [e.target.name]: e.target.value
        });
    };

    const handleRegister = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        if (formData.password !== formData.confirmPassword) {
            setError('Passwords do not match.');
            return;
        }

        setIsLoading(true);
        try {
            const response = await register(formData);
            console.log(response.data);
        } catch (err) {
            setError('Registration failed. Please try again.');
            console.error(err);
        } finally {
            setIsLoading(false);
        }
    };


    return (
        <>
            <form onSubmit={handleRegister} className="login-form">
                <header className="form-header">
                    <h2>
                        Welcome Back!
                    </h2>

                    <span>
                        Create an account to continue
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

                <label htmlFor="email">
                    EMAIL
                </label>
                <input
                    type="text"
                    name="email"
                    className="email"
                    id="email"
                    placeholder="please enter a valid email"
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
                    onChange={handleChange}
                    required
                />

                <label htmlFor="confirmPassword">
                    CONFIRM PASSWORD
                </label>
                <input
                    type="confirmPassword"
                    name="confirmPassword"
                    id="confirmPassword"
                    onChange={handleChange}
                    required
                />

                <AuthBtn
                    text={isLoading ? 'Loading...' : 'Register'}
                    disabled={isLoading}
                    type="submit"
                />

                <div className="register-link">
                    <Link to="/auth/login" className="register">
                        Sign-in
                    </Link>
                </div>

                {error && <p className="text-danger">{error}</p>} {/* Render error message if exists */}

            </form>
        </>
    )
}