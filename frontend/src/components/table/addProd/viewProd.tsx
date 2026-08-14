import "./viewProd.scss"
import type React from "react";

type prodType = {
    productName: string;
    productBrand: string;
    productCategory: string;
    productQty: string;
    productPrice: string;
    prodExpiry: string;
    prodStatus: string;
    pordLastUpdt: string;
    editIcon: React.ReactNode;
    trashIcon: React.ReactNode;
}

export const ViewProd = ({
    productName,
    productBrand,
    productCategory,
    productQty,
    productPrice,
    prodExpiry,
    prodStatus,
    pordLastUpdt,
    editIcon,
    trashIcon
}: prodType) => {
    return (
        <tr>
            <td>
                <b>{productName}</b>
                <br />
                <small>{productBrand}</small>
            </td>
            <td>{productCategory}</td>
            <td>
                <b>
                    {productQty}
                </b>
            </td>

            <td>
                ₱{productPrice}
            </td>

            <td>
                {prodExpiry}
            </td>

            <td>
                {prodStatus}
            </td>

            <td>
                {pordLastUpdt}
            </td>

            <td className="btn-wrapper">
               {editIcon}
               {trashIcon}
            </td>
        </tr>
    )
}