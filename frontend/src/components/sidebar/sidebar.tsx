import { DashboardIcon } from '../../assets/svg/dashboardIcon';
import { MenuIcon } from '../../assets/svg/menuIcon';
import { PillIcon } from '../../assets/svg/pillIcon';
import { ProductIcon } from '../../assets/svg/productIcont';
import { ShoppingCartIcon } from '../../assets/svg/shoppingCartIcon';
import { Link } from "react-router-dom";
import "./sidebar.scss"
import { LogoutBtn } from '../button/btn';
import { LogoutIcon } from '../../assets/svg/logoutIcon';
import { useState } from "react";
import { Initials } from '../card/initials';

export const Sidebar = () => {
    const [collapsed, setCollapsed] = useState(false);
    return (
        <nav className={`sticky-sidebar ${collapsed ? "collapsed" : ""}`}>
            <header className="sidebar-header">
                <div className="logo-icon">
                    <PillIcon width={20} height={20} className="mini-logo" />
                </div>

                <div className="text-wrapper">
                    <div className="text">
                        <p>
                            Reisa Drugstore
                        </p>

                        <span>
                            Pharmacy IMS
                        </span>
                    </div>


                </div>
                <div className="menu">
                    <MenuIcon width={30} height={30} className="menu-icon" onClick={() => setCollapsed(!collapsed)} />
                </div>
            </header>

            <hr />

            <div className="header-pages">
                <ul className="pages-container">
                    <Link to="/reisa/dashboard" className="pages-item">
                        <DashboardIcon width={30} height={30} className="pages-logo" />
                        <span className="pages-url">Dashboard</span>
                    </Link>
                    <Link to="/reisa/products" className="pages-item">
                        <ProductIcon width={25} height={25} className="pages-logo" />
                        <span className="pages-url">Products</span>
                    </Link>
                    <Link to="/reisa/sales" className="pages-item">
                        <ShoppingCartIcon width={25} height={25} className="pages-logo" />
                        <span className="pages-url">Sales/POS</span>
                    </Link>
                </ul>
            </div>



            <div className="logout">
                <hr />

                <Initials />

                <div className="logout-btn">
                    <LogoutBtn icon={<LogoutIcon width={20} height={20} />} text={"Logout"} />
                </div>
            </div>
        </nav>
    )
}