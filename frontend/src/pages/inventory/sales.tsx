import { CategBtn } from "../../components/button/btn";
import { PosCard } from "../../components/card/posCard/posCard";
import { InventoryHeader } from "../../components/header/inventoryHeader";
import "./sales.scss"

export const Sales = () => {
    return (
        <>
            <main id="pos-view" className="pos-container">
                <InventoryHeader text="POS" />
                <hr />
                <div className="pos-wrapper">
                    <div className="pos-content">
                        <div className="search-bar">
                            <input
                                type="text"
                                className="pos-search"
                                placeholder="Search medicines..."
                            />

                            <div className="categories">
                                <button className="cat-btn active">All</button>
                                <CategBtn text="Analgesic" />
                                <CategBtn text="Antibiotic" />
                                <CategBtn text="NSAID" />
                                <CategBtn text="Antihistamine" />
                                <CategBtn text="Antacid" />
                                <CategBtn text="Antipyretic" />
                                <CategBtn text="Antidiarrheal" />
                                <CategBtn text="Vitamins" />
                                <CategBtn text="Cough & Cold" />
                                <CategBtn text="Dermatological" />
                            </div>

                        </div>

                        <div className="product-grid">

                            <PosCard
                                productName="Paracetamol 500mg"
                                productCategory="Biogesic"
                                price="2.50"
                                stock="1250"
                            />

                            <PosCard
                                productName="Paracetamol 500mg"
                                productCategory="Biogesic"
                                price="2.50"
                                stock="1250"
                            />

                            <PosCard
                                productName="Ibuprofen 200mg"
                                productCategory="Advil"
                                price="5.00"
                                stock="850"
                            />

                            <PosCard
                                productName="Amoxicillin 500mg"
                                productCategory="Amoxil"
                                price="8.50"
                                stock="420"
                            />

                            <PosCard
                                productName="Cetirizine 10mg"
                                productCategory="Zyrtec"
                                price="4.25"
                                stock="675"
                            />

                            <PosCard
                                productName="Loperamide 2mg"
                                productCategory="Diatabs"
                                price="6.50"
                                stock="350"
                            />

                            <PosCard
                                productName="Ascorbic Acid 500mg"
                                productCategory="Ceelin"
                                price="3.75"
                                stock="920"
                            />

                            <PosCard
                                productName="Mefenamic Acid 500mg"
                                productCategory="Ponstan"
                                price="7.25"
                                stock="540"
                            />

                            <PosCard
                                productName="Omeprazole 20mg"
                                productCategory="Losec"
                                price="9.50"
                                stock="280"
                            />

                            <PosCard
                                productName="Antacid Tablet"
                                productCategory="Kremil-S"
                                price="4.00"
                                stock="760"
                            />

                            <PosCard
                                productName="Diphenhydramine 25mg"
                                productCategory="Benadryl"
                                price="5.50"
                                stock="190"
                            />
                        </div>
                    </div>

                    <div className="pos-sidebar">
                        <div className="cart-top">
                            <h3>Current Sale</h3>
                            <p>0 items selected</p>
                        </div>
                        <div className="cart-empty">
                            <div className="cart-icon">🛒</div>
                            <p>Cart is empty</p>
                            <span>Click a product to add it</span>
                        </div>
                        <div className="cart-bottom">
                            <div className="total-row">
                                <span>Total</span>
                                <span className="price-blue">₱0.00</span>
                            </div>
                            <button className="btn-complete" disabled>Complete Sale</button>
                        </div>
                    </div>
                </div>
            </main>
        </>
    )
}