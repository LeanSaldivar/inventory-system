import './inventoryLayout.scss'
import { Outlet } from "react-router-dom";

export const InventoryLayout = () => {
    return (
        <div className="main-container">
            <p></p>
            <div className="sticky-sidebar">
            </div>
            <Outlet/>
        </div>
    )
}