using System.ComponentModel.DataAnnotations;
namespace backend.Model;

public class ReceiptItem
{
    [Key]
    public int ReceiptItemId { get; set; }

    public int ReceiptId { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    public decimal UnitPrice { get; set; }

    [Required]
    public decimal Subtotal { get; set; }

    //NavProperties
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public Receipt Receipt { get; set; } = null!;
}

public class ReceiptItemRequest
{
    [Required]
    public int ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}

public class ReceiptItemResponse
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public ProductStatus ProductStatus { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal Subtotal { get; set; }


}