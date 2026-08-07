import type React from "react";
import "./productTable.scss"

type productType = {
    num: string;
    product: string;
    units: string;
    price: string;
}

export const ProductTable = ({
    num,
    product,
    units,
    price
}: productType) => {
    return (
        <div className="product-item">
            <div className="num">
                {num}
            </div>

            <div className="product-wrapper">
                <div className="product-text">
                    <p>
                        {product}
                    </p>

                    <span>
                        {units}
                    </span>
                </div>

                <div className="progress">

                </div>
            </div>

            <div className="price">
                <p>
                    {price}
                </p>
            </div>
        </div>
    )
}
