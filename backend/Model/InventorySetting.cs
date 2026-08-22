using System.ComponentModel.DataAnnotations;
using backend.model;

namespace backend.Model;

public class InventorySetting
{
    [Key]
    public int InventorySettingId { get; set; }

    public int LowStockThreshold { get; set; } = 10;

    public int AverageStockThreshold { get; set; } = 50;
}

public class InventorySettingsPatchRequest
{
    [Range(1, int.MaxValue)]
    public int? LowStockThreshold { get; set; }

    [Range(1, int.MaxValue)]
    public int? AverageStockThreshold { get; set; }
}