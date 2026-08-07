import { Outlet } from "react-router-dom";

import './inventoryLayout.scss'
import { Sidebar } from "../../components/sidebar/sidebar";

export const InventoryLayout = () => {
    return (
        <div className="main-container">
            <Sidebar />



            <div className="asd">
                <Outlet />
            </div>


        </div>
    )
}