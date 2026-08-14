import type React from "react";
import { SvgBtn } from "../../button/btn";
import { PillIcon } from "../../../assets/svg/pillIcon";
import "./posCard.scss"

type posType = {
    productName: string;
    productCategory: string;
    price: string;
    stock: string;
}

export const PosCard = ({
    productName,
    productCategory,
    price,
    stock
}: posType) => {
    return (
        <>
            <div className="p-card">
                <SvgBtn icon={<PillIcon className="p-icon" />} />
                <h4>{productName}</h4>
                <p>{productCategory}</p>
                <div className="price">
                    ₱{price}
                </div>

                <div className="p-stock">
                    {stock} in stock
                </div>
            </div>
        </>
    )
}