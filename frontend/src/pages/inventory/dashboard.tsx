import { CartIcon } from '../../assets/svg/cartIcon';
import { FilterBtn } from '../../components/button/btn';
import { AiHeader } from '../../components/header/aiHeader';
import { InventoryHeader } from '../../components/header/inventoryHeader';
import { ProductTable } from '../../components/table/productTable';
import { TransactionsTable } from '../../components/table/transactionsTable';
import './dashboard.scss'

export const Dashboard = () => {
    return (
        <>
            <InventoryHeader text={"Dashboard"} />
            <div className="dashboard-container">
                <header className="dashboard-subheader">
                    <div className="subheader-text">
                        <h2>
                            Sales Summary
                        </h2>

                        <p>
                            Track Transactions Overtime
                        </p>
                    </div>

                    <div className="filter-wrapper">
                        <FilterBtn text={"Daily"} />
                        <FilterBtn text={"Weekly"} />
                        <FilterBtn text={"Monthly"} />
                    </div>
                </header>

                <div className="sales-card">
                    <div className="total-sales">
                        <p>
                            12,450
                        </p>

                        <span>
                            Total Sales
                        </span>
                    </div>

                    <div className="transactions">
                        <p>
                            38
                        </p>

                        <span>
                            Transactions
                        </span>
                    </div>

                    <div className="average">
                        <p>
                            327.63
                        </p>

                        <span>
                            Average / Sale
                        </span>
                    </div>
                </div>

                <div className="transaction-container">
                    <p>
                        Recent Transactions
                    </p>

                    <TransactionsTable
                        icon={<CartIcon width={20} height={20} className="shopping-icon" />}
                        item="4 items"
                        info="09:12AM M.Santos"
                        price="215.50"
                    />
                    <TransactionsTable
                        icon={<CartIcon width={20} height={20} className="shopping-icon" />}
                        item="4 items"
                        info="09:12AM M.Santos"
                        price="215.50"
                    />
                    <TransactionsTable
                        icon={<CartIcon width={20} height={20} className="shopping-icon" />}
                        item="4 items"
                        info="09:12AM M.Santos"
                        price="215.50"
                    />
                    <TransactionsTable
                        icon={<CartIcon width={20} height={20} className="shopping-icon" />}
                        item="4 items"
                        info="09:12AM M.Santos"
                        price="215.50"
                    />
                    <TransactionsTable
                        icon={<CartIcon width={20} height={20} className="shopping-icon" />}
                        item="4 items"
                        info="09:12AM M.Santos"
                        price="215.50"
                    />
                    <TransactionsTable
                        icon={<CartIcon width={20} height={20} className="shopping-icon" />}
                        item="4 items"
                        info="09:12AM M.Santos"
                        price="215.50"
                    />
                    <TransactionsTable
                        icon={<CartIcon width={20} height={20} className="shopping-icon" />}
                        item="4 items"
                        info="09:12AM M.Santos"
                        price="215.50"
                    />
                    <TransactionsTable
                        icon={<CartIcon width={20} height={20} className="shopping-icon" />}
                        item="4 items"
                        info="09:12AM M.Santos"
                        price="215.50"
                    />
                </div>
            </div>

            <div className="dashboard-container">
                <div className="top-products-container">
                    <div className="products-text">
                        <p>
                            Best-selling products
                        </p>

                        <span>
                            Ranked By Units Sold
                        </span>
                    </div>

                    <div className="filter-wrapper">
                        <FilterBtn text={"Daily"} />
                        <FilterBtn text={"Weekly"} />
                        <FilterBtn text={"Monthly"} />
                    </div>
                </div>

                <div className="product-table">
                    <ProductTable
                        num="1"
                        product="Paracetamol 500mg"
                        units="248 units"
                        price="620.00"
                    />

                    <ProductTable
                        num="1"
                        product="Paracetamol 500mg"
                        units="248 units"
                        price="620.00"
                    />

                    <ProductTable
                        num="1"
                        product="Paracetamol 500mg"
                        units="248 units"
                        price="620.00"
                    />

                    <ProductTable
                        num="1"
                        product="Paracetamol 500mg"
                        units="248 units"
                        price="620.00"
                    />

                    <ProductTable
                        num="1"
                        product="Paracetamol 500mg"
                        units="248 units"
                        price="620.00"
                    />
                </div>
            </div>

            <div className="ai dashboard-container">
                <AiHeader/>
                
            </div>

        </>
    )
}