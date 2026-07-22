import { PillIcon } from "../assets/svg/pillIcon";
import "./login.scss"

export const Login = () => {
    return (
        <div className="login-container">
            <header className="login-header">

                <div className="iconLogo">
                    <PillIcon className="pillIconLogo" />
                </div>

                <h1>
                    Reisa DrugStore & General Merchandise
                </h1>

                <span>
                    Inventory Management System
                </span>
            </header>
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

             
                <button className="sign-in">
                    Sign In
                </button>
            </form>
        </div>
    )
}