import { Link } from "react-router-dom";
import AuthBtn from "../../components/button/btn";
import "./auth.scss"

export const Register = () => {
    return (
        <>
            <form action="" className="login-form">
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
                    required
                />

                <label htmlFor="password">
                    PASSWORD
                </label>
                <input
                    type="password"
                    name="password"
                    id="password"
                    required
                />

                <label htmlFor="password">
                    CONFIRM PASSWORD
                </label>
                <input
                    type="password"
                    name="password"
                    id="password"
                    required
                />

                <AuthBtn text="Create Account!" />

                <div className="register-link">
                    <Link to="/reisa/login" className="register">
                        Sign-in
                    </Link>
                </div>
            </form>
        </>
    )
}