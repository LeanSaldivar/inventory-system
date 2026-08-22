using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Data.migrations
{
    /// <inheritdoc />
    public partial class inventorySettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InventorySettings",
                columns: table => new
                {
                    InventorySettingId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LowStockThreshold = table.Column<int>(type: "INTEGER", nullable: false),
                    AverageStockThreshold = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventorySettings", x => x.InventorySettingId);
                    table.CheckConstraint("CK_InventorySettings_LowLessThanAverage", "LowStockThreshold < AverageStockThreshold");
                    table.CheckConstraint("CK_InventorySettings_PositiveThresholds", "LowStockThreshold > 0 AND AverageStockThreshold > 0");
                });

            migrationBuilder.InsertData(
                table: "InventorySettings",
                columns: new[] { "InventorySettingId", "AverageStockThreshold", "LowStockThreshold" },
                values: new object[] { 1, 50, 10 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventorySettings");
        }
    }
}
