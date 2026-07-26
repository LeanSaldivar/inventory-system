import { Outlet } from "react-router-dom";
import "./authLayout.scss"
import { PillIcon } from "../../assets/svg/pillIcon";

export const AuthLayout = () => {
    return (
        <div className="auth-container">
            <header className="auth-header">

                <div className="iconLogo">
                    <PillIcon  className="pillIconLogo" />
                </div>

                <h1>
                    Reisa DrugStore & General Merchandise
                </h1>

                <span>
                    Inventory Management System
                </span>
            </header>

            
            <Outlet />
        </div>
    )
}