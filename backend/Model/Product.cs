using System.ComponentModel.DataAnnotations;
using backend.model;
using backend.model;

namespace backend.Model;

public enum ProductStatus
{
    NoStocks,
    LowOnStocks,
    AverageOnStocks,
    HighOnStocks
};

public enum ProductUnit
{
    Tablet,
    Capsule,
    Bottle,
    Tube,
    Vial,
    Sachet
}

public enum ProductCategory
{
    Analgaesic,
    Antibiotic,
    NSAID,
    Antidiabetic,
    Antihypertensive,
    Antacid,
    Anthistamine,
    Supplement,
    Other

}

public class Product
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string ProductName { get; set; } = string.Empty;

    [Required]
    public string ProductBrand { get; set; } = string.Empty;

    [Required]
    public ProductCategory ProductCategory { get; set; }

    [Required]
    public ProductUnit ProductUnit { get; set; }

    [Required]
    public int ProductQuantity { get; set; }

    [Required]
    public decimal ProductPrice { get; set; }

    public ProductStatus? ProductStatus { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? LastUpdatedAt { get; set; }

    public DateTime? ProductExpiry { get; set; }

    public void UpdateStatus(InventorySetting settings)
    {
        if (ProductQuantity <= 0)
        {
            ProductStatus = Model.ProductStatus.NoStocks;
        }
        else if (ProductQuantity <= settings.LowStockThreshold)
        {
            ProductStatus = Model.ProductStatus.LowOnStocks;
        }
        else if (ProductQuantity <= settings.AverageStockThreshold)
        {
            ProductStatus = Model.ProductStatus.AverageOnStocks;
        }
        else
        {
            ProductStatus = Model.ProductStatus.HighOnStocks;
        }
    }

    //Nav properties
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    //Many to Many with RecieptItems
    public ICollection<ReceiptItem> ProductItems { get; set; } = [];


}


public class ProductInventoryRequestDTO
{
    [Required]
    [Display(Name = "Product Name")]
    [MaxLength(50)]
    public string ProductName { get; set; } = null!;

    [Required]
    [Display(Name = "Product Brand")]
    [MaxLength(50)]
    public string ProductBrand { get; set; } = null!;

    [Required]
    [Display(Name = "Product Category")]
    public ProductCategory ProductCategory { get; set; }

    [Required]
    [Display(Name = "Product Unit")]
    public ProductUnit ProductUnit { get; set; }


    [Required]
    [Display(Name = "Product Quantity")]
    public int ProductQuantity { get; set; }

    [Required]
    [Display(Name = "Product Price")]
    public decimal ProductPrice { get; set; }
}

public class ProductInventoryResponseDTO
{
    public int Id { get; set; }

    public string ProductCode => $"RX{Id:D4}";

    public string ProductName { get; set; } = string.Empty;

    public string ProductBrand { get; set; } = string.Empty;

    public ProductCategory ProductCategory { get; set; }

    public ProductUnit ProductUnit { get; set; }

    public int ProductQuantity { get; set; }

    public decimal ProductPrice { get; set; }

    public ProductStatus? ProductStatus { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? LastUpdatedAt { get; set; }

    public DateTime? ProductExpiry { get; set; }


}

public class ProductSalesResponseDTO
{
    public int Id { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public string ProductBrand { get; set; } = string.Empty;

    public decimal ProductPrice { get; set; }

    public int ProductQuantity { get; set; }
}

public class CreateSaleRequestDTO
{
    [Required]
    [MinLength(1)]
    public List<SaleItemRequestDTO> Items { get; set; } = [];
}

public class SaleItemRequestDTO
{
    [Required]
    public int ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}