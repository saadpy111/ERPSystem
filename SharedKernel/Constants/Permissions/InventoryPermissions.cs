namespace SharedKernel.Constants.Permissions
{
    /// <summary>
    /// Inventory Module Permissions
    /// Based on actual endpoints from Inventory.Api controllers
    /// </summary>
    public static class InventoryPermissions
    {
        public const string Module = "Inventory";

        // ===== PRODUCTS =====
        public const string ProductsView = "Inventory.Products.View";
        public const string ProductsCreate = "Inventory.Products.Create";
        public const string ProductsEdit = "Inventory.Products.Edit";
        public const string ProductsDelete = "Inventory.Products.Delete";
        public const string ProductsExport = "Inventory.Products.Export";
        public const string ProductsDeactivate = "Inventory.Products.Deactivate";
        public const string ProductsManageAttributes = "Inventory.Products.ManageAttributes";
        public const string ProductsManageBarcodes = "Inventory.Products.ManageBarcodes";
        public const string ProductsViewCostHistory = "Inventory.Products.ViewCostHistory";

        // ===== CATEGORIES =====
        public const string CategoriesView = "Inventory.Categories.View";
        public const string CategoriesCreate = "Inventory.Categories.Create";
        public const string CategoriesEdit = "Inventory.Categories.Edit";
        public const string CategoriesDelete = "Inventory.Categories.Delete";

        // ===== WAREHOUSES =====
        public const string WarehousesView = "Inventory.Warehouses.View";
        public const string WarehousesCreate = "Inventory.Warehouses.Create";
        public const string WarehousesEdit = "Inventory.Warehouses.Edit";
        public const string WarehousesDelete = "Inventory.Warehouses.Delete";

        // ===== LOCATIONS =====
        public const string LocationsView = "Inventory.Locations.View";
        public const string LocationsCreate = "Inventory.Locations.Create";
        public const string LocationsEdit = "Inventory.Locations.Edit";
        public const string LocationsDelete = "Inventory.Locations.Delete";

        // ===== STOCK MOVEMENTS =====
        public const string StockMovesView = "Inventory.StockMoves.View";
        public const string StockMovesCreate = "Inventory.StockMoves.Create";

        // ===== STOCK ADJUSTMENTS =====
        public const string StockAdjustmentsView = "Inventory.StockAdjustments.View";
        public const string StockAdjustmentsCreate = "Inventory.StockAdjustments.Create";
        public const string StockAdjustmentsApprove = "Inventory.StockAdjustments.Approve";

        // ===== STOCK QUANTITIES =====
        public const string StockQuantitiesView = "Inventory.StockQuantities.View";

        // ===== QUARANTINE =====
        public const string QuarantineView = "Inventory.Quarantine.View";
        public const string QuarantineCreate = "Inventory.Quarantine.Create";
        public const string QuarantineRelease = "Inventory.Quarantine.Release";

        // ===== PRODUCT ATTRIBUTES =====
        public const string AttributesView = "Inventory.Attributes.View";
        public const string AttributesCreate = "Inventory.Attributes.Create";
        public const string AttributesEdit = "Inventory.Attributes.Edit";
        public const string AttributesDelete = "Inventory.Attributes.Delete";
    }
}
