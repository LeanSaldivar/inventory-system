import { DeleteIcon } from "../../assets/svg/deleteIcon";
import { EditIcon } from "../../assets/svg/editIcon";
import { ActionBtn } from "../../components/button/btn";
import { InventoryHeader } from "../../components/header/inventoryHeader";
import { ViewProd } from "../../components/table/addProd/viewProd";
import { useState } from "react";
import "./products.scss"
import { Modal } from "../../components/modal/modal";
import { AddProductModal } from "../modals/add-product-modal/AddProductModal";

export const Products = () => {
    const [isOpen, setIsOpen] = useState(false);

    return (
        <>
            <main id="inventory-view" className="container">
                <InventoryHeader text="Products" />
                <div className="product-wrapper">


                    <div className="stats">
                        <div className="stat-card">
                            <div className="dot-wrap green-bg">
                                <div className="dot green">
                                </div>
                            </div>
                            <div>
                                <h3>
                                    10
                                </h3>
                                <p>
                                    High Stock
                                    <span>
                                        At or above minimum
                                    </span>
                                </p>
                            </div>
                        </div>

                        <div className="stat-card">
                            <div className="dot-wrap orange-bg"><div className="dot orange"></div></div>
                            <div>
                                <h3>
                                    0
                                </h3>
                                <p>
                                    Moderate Stock
                                    <span>
                                        Below minimum, not empty
                                    </span>
                                </p>
                            </div>
                        </div>

                        <div className="stat-card">
                            <div className="dot-wrap red-bg"><div className="dot red"></div></div>
                            <div>
                                <h3>
                                    5
                                </h3>
                                <p>
                                    Low / Empty
                                    <span>
                                        Needs restocking now
                                    </span>
                                </p>
                            </div>
                        </div>
                    </div>

                    <div className="toolbar">
                        <div className="search-wrap">
                            <input type="text" placeholder="Search products..." />
                            <select>
                                <option>
                                    All Categories
                                </option>
                            </select>
                        </div>

                        <div className="btn-modal">
                            <button className="btn-add" onClick={() => setIsOpen(true)}>
                                + Add Product
                            </button>
                            <Modal isOpen={isOpen} onClose={() => setIsOpen(false)}>
                                <AddProductModal onClose={() => setIsOpen(false)}/>
                            </Modal>

                        </div>

                    </div>

                    <div className="table-card">
                        <table>
                            <thead>
                                <tr>
                                    <th>PRODUCT NAME</th>
                                    <th>Category</th>
                                    <th>QTY/MIN</th>
                                    <th>Unit Price</th>
                                    <th>Expiry</th>
                                    <th>Status</th>
                                    <th>Last Updated</th>
                                    <th>Actions</th>

                                </tr>
                            </thead>
                            <tbody>

                                <ViewProd
                                    productName="Paracetamol 500mg"
                                    productBrand="PRD001"
                                    productCategory="Biogesic"
                                    productQty="1,2500"
                                    productPrice="2.50"
                                    prodExpiry="2026-01-15"
                                    prodStatus="In Stock"
                                    pordLastUpdt="2026-07-30"
                                    editIcon={<ActionBtn icon={<EditIcon width={20} height={20} className="editIcon" />} />}
                                    trashIcon={<ActionBtn icon={<DeleteIcon width={20} height={20} className="trashIcon" />} />}
                                />

                                <ViewProd
                                    productName="Ibuprofen 200mg"
                                    productBrand="PRD002"
                                    productCategory="Advil"
                                    productQty="850"
                                    productPrice="5.00"
                                    prodExpiry="2027-03-20"
                                    prodStatus="In Stock"
                                    pordLastUpdt="2026-07-28"
                                    editIcon={
                                        <ActionBtn
                                            icon={<EditIcon width={20} height={20} className="editIcon" />}
                                        />
                                    }
                                    trashIcon={
                                        <ActionBtn
                                            icon={<DeleteIcon width={20} height={20} className="trashIcon" />}
                                        />
                                    }
                                />

                                <ViewProd
                                    productName="Amoxicillin 500mg"
                                    productBrand="PRD003"
                                    productCategory="Amoxil"
                                    productQty="420"
                                    productPrice="8.50"
                                    prodExpiry="2026-11-10"
                                    prodStatus="Low Stock"
                                    pordLastUpdt="2026-07-25"
                                    editIcon={
                                        <ActionBtn
                                            icon={<EditIcon width={20} height={20} className="editIcon" />}
                                        />
                                    }
                                    trashIcon={
                                        <ActionBtn
                                            icon={<DeleteIcon width={20} height={20} className="trashIcon" />}
                                        />
                                    }
                                />

                                <ViewProd
                                    productName="Cetirizine 10mg"
                                    productBrand="PRD004"
                                    productCategory="Zyrtec"
                                    productQty="675"
                                    productPrice="4.25"
                                    prodExpiry="2027-06-15"
                                    prodStatus="In Stock"
                                    pordLastUpdt="2026-07-22"
                                    editIcon={
                                        <ActionBtn
                                            icon={<EditIcon width={20} height={20} className="editIcon" />}
                                        />
                                    }
                                    trashIcon={
                                        <ActionBtn
                                            icon={<DeleteIcon width={20} height={20} className="trashIcon" />}
                                        />
                                    }
                                />

                                <ViewProd
                                    productName="Loperamide 2mg"
                                    productBrand="PRD005"
                                    productCategory="Diatabs"
                                    productQty="350"
                                    productPrice="6.50"
                                    prodExpiry="2026-10-05"
                                    prodStatus="Low Stock"
                                    pordLastUpdt="2026-07-20"
                                    editIcon={
                                        <ActionBtn
                                            icon={<EditIcon width={20} height={20} className="editIcon" />}
                                        />
                                    }
                                    trashIcon={
                                        <ActionBtn
                                            icon={<DeleteIcon width={20} height={20} className="trashIcon" />}
                                        />
                                    }
                                />

                                <ViewProd
                                    productName="Ascorbic Acid 500mg"
                                    productBrand="PRD006"
                                    productCategory="Ceelin"
                                    productQty="920"
                                    productPrice="3.75"
                                    prodExpiry="2027-01-25"
                                    prodStatus="In Stock"
                                    pordLastUpdt="2026-07-18"
                                    editIcon={
                                        <ActionBtn
                                            icon={<EditIcon width={20} height={20} className="editIcon" />}
                                        />
                                    }
                                    trashIcon={
                                        <ActionBtn
                                            icon={<DeleteIcon width={20} height={20} className="trashIcon" />}
                                        />
                                    }
                                />

                            </tbody>
                        </table>
                    </div>
                </div>
            </main>
        </>
    )
}