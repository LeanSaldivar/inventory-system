import { NotificationIcon } from "../../assets/svg/notificationIcon";
import { Initials } from "../card/initials";
import "./inventoryHeader.scss"

type HeaderProps = {
    text: string;
};


export const InventoryHeader = ({
    text
}: HeaderProps) => {
    return (
        <header className="header-container">
            <div className="title">
                <h1>
                    {text}
                </h1>
            </div>

            <div className="profile-wrapper">
                <div className="notif-icon">
                    <NotificationIcon width={20} height={20} className="notifs" />
                </div>
                <Initials />
            </div>
        </header>
    )
}