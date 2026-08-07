import type React from "react";
import "./transactionsTable.scss"

type transactionProps = {
    icon?: React.ReactNode;
    item: string;
    info: string;
    price: string;
}


export const TransactionsTable = ({
    icon,
    item,
    info,
    price
}: transactionProps) => {
    return (
        <div className="transaction">
            <div className="transaction-profile">
                <div className="transaction-icon">
                    {icon}
                </div>
                <div className="transaction-text">
                    <p>
                        {item}
                    </p>

                    <span>
                        {info}
                    </span>
                </div>
            </div>

            <div className="price">
                {price}
            </div>
        </div>
    )
}