namespace SharedKernel.Constants.Permissions
{
    /// <summary>
    /// Procurement Module Permissions
    /// Based on actual endpoints from Procurement.Api controllers
    /// </summary>
    public static class ProcurementPermissions
    {
        public const string Module = "Procurement";

        // ===== VENDORS =====
        public const string VendorsView = "Procurement.Vendors.View";
        public const string VendorsCreate = "Procurement.Vendors.Create";
        public const string VendorsEdit = "Procurement.Vendors.Edit";
        public const string VendorsDelete = "Procurement.Vendors.Delete";

        // ===== PURCHASE ORDERS =====
        public const string PurchaseOrdersView = "Procurement.PurchaseOrders.View";
        public const string PurchaseOrdersCreate = "Procurement.PurchaseOrders.Create";
        public const string PurchaseOrdersEdit = "Procurement.PurchaseOrders.Edit";
        public const string PurchaseOrdersDelete = "Procurement.PurchaseOrders.Delete";
        public const string PurchaseOrdersApprove = "Procurement.PurchaseOrders.Approve";
        public const string PurchaseOrdersCancel = "Procurement.PurchaseOrders.Cancel";

        // ===== PURCHASE ORDER ITEMS =====
        public const string PurchaseOrderItemsView = "Procurement.PurchaseOrderItems.View";
        public const string PurchaseOrderItemsCreate = "Procurement.PurchaseOrderItems.Create";
        public const string PurchaseOrderItemsEdit = "Procurement.PurchaseOrderItems.Edit";
        public const string PurchaseOrderItemsDelete = "Procurement.PurchaseOrderItems.Delete";

        // ===== PURCHASE REQUISITIONS =====
        public const string PurchaseRequisitionsView = "Procurement.PurchaseRequisitions.View";
        public const string PurchaseRequisitionsCreate = "Procurement.PurchaseRequisitions.Create";
        public const string PurchaseRequisitionsEdit = "Procurement.PurchaseRequisitions.Edit";
        public const string PurchaseRequisitionsDelete = "Procurement.PurchaseRequisitions.Delete";
        public const string PurchaseRequisitionsApprove = "Procurement.PurchaseRequisitions.Approve";
        public const string PurchaseRequisitionsReject = "Procurement.PurchaseRequisitions.Reject";

        // ===== PURCHASE INVOICES =====
        public const string PurchaseInvoicesView = "Procurement.PurchaseInvoices.View";
        public const string PurchaseInvoicesCreate = "Procurement.PurchaseInvoices.Create";
        public const string PurchaseInvoicesEdit = "Procurement.PurchaseInvoices.Edit";
        public const string PurchaseInvoicesDelete = "Procurement.PurchaseInvoices.Delete";

        // ===== GOODS RECEIPTS =====
        public const string GoodsReceiptsView = "Procurement.GoodsReceipts.View";
        public const string GoodsReceiptsCreate = "Procurement.GoodsReceipts.Create";
        public const string GoodsReceiptsEdit = "Procurement.GoodsReceipts.Edit";
        public const string GoodsReceiptsDelete = "Procurement.GoodsReceipts.Delete";
    }
}
