using System.ComponentModel.DataAnnotations;
using backend.model;

namespace backend.Model;

public class Receipt
{
    [Key]
    public int ReceiptId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public decimal Subtotal { get; set; }

    [Required]
    public decimal Discount { get; set; }

    [Required]
    public decimal Total { get; set; }

    // Navigation properties
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    //One to Many 
    public ICollection<ReceiptItem> Items { get; set; } = [];
}



public class ReceiptRequest
{
    [Required]
    [MinLength(1)]
    public ICollection<ReceiptItemRequest> Items { get; set; } = [];
}

public class ReceiptResponse
{
    public int ReceiptId { get; set; }

    public decimal Subtotal { get; set; }

    public decimal Discount { get; set; }

    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<ReceiptItemResponse> Items { get; set; } = [];

}