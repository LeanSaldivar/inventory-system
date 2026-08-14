import "./AddProductModal.scss"

interface AddProductModalProps {
    onClose: () => void;
}

export const AddProductModal = ({
    onClose
}: AddProductModalProps) => {
    return (
        <>
            <div className="add-product-modal-container">
                <h3>
                    Add New Product
                </h3>

                <hr />
                <form action="" className="product-form">
                    <label htmlFor="productName">
                        PRODUCT NAME
                    </label>
                    <input
                        type="text"
                        name="productName"
                        className="productName"
                        id="productName"
                        placeholder="e.g Paracetamol 500mg"
                        required
                    />

                    <div className="form-wrapper">
                        <div className="left">
                            <div className="label-wrapper">
                                <label htmlFor="productBrand">
                                    BRAND
                                </label>
                                <input
                                    type="text"
                                    name="productBrand"
                                    className="productBrand"
                                    id="productBrand"
                                    placeholder="e.g Biogesic"
                                    required
                                />
                            </div>

                            <div className="label-wrapper">
                                <label htmlFor="productUnit">
                                    UNIT
                                </label>
                                <select
                                    name="productUnit"
                                    className="productUnit"
                                    id="productUnit"
                                    defaultValue=""
                                    required
                                >
                                    <option value="" disabled>
                                        Select a unit
                                    </option>
                                    <option value="piece">Piece</option>
                                    <option value="box">Box</option>
                                    <option value="bottle">Bottle</option>
                                    <option value="pack">Pack</option>
                                    <option value="strip">Strip</option>
                                </select>
                            </div>


                        </div>

                        <div className="right">
                            <div className="label-wrapper">
                                <label htmlFor="productCategory">
                                    CATEGORY
                                </label>
                                <select
                                    name="productCategory"
                                    className="productCategory"
                                    id="productCategory"
                                    defaultValue=""
                                    required >
                                    <option value="" disabled> Select a category </option>
                                    <option value="analgesics">Analgesics</option>
                                    <option value="antibiotics">Antibiotics</option>
                                    <option value="antihistamines">Antihistamines</option>
                                    <option value="antacids">Antacids</option>
                                    <option value="vitamins">Vitamins</option>
                                    <option value="cough-and-cold">Cough & Cold</option>
                                    <option value="first-aid">First Aid</option>
                                </select>
                            </div>

                            <div className="label-wrapper">
                                <label htmlFor="productStock">
                                    Minimum Stock Level
                                </label>
                                <input
                                    type="number"
                                    name="productStock"
                                    className="productStock"
                                    id="productStock"
                                    placeholder="100"
                                    required
                                />
                            </div>
                        </div>
                    </div>

                    <div className="button-wrapper">
                        <button className="cancel" onClick={onClose}>
                            <p>
                                Cancel
                            </p>
                        </button>

                        <button className="addProduct">
                            <p>
                                Add Product
                            </p>
                        </button>
                    </div>


                </form>
            </div>
        </>
    )
}