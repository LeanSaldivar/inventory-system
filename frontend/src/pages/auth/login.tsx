import AuthBtn from "../../components/button/btn";
import "./auth.scss"
import { Link } from 'react-router-dom';

export const Login = () => {
    return (
        <>
            <form action="" className="login-form">
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
                    required
                />

                <AuthBtn text="Sign-in" />

                <div className="register-link">
                    <Link to="/reisa/register" className="register">
                        Create Account
                    </Link>
                </div>
            </form>
        </>
    )
}