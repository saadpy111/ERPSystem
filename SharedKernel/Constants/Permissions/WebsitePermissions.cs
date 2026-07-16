namespace SharedKernel.Constants.Permissions
{
    /// <summary>
    /// Website Module Permissions — strict CRUD-based.
    /// No "Manage" abstractions. Each HTTP verb maps to exactly one permission:
    ///   GET    → View
    ///   POST   → Create
    ///   PUT    → Edit
    ///   DELETE → Delete
    /// </summary>
    public static class WebsitePermissions
    {
        public const string Module = "Website";

        // ===== WEBSITE CONFIG =====
        public const string ConfigView       = "Website.Config.View";
        public const string ConfigEdit       = "Website.Config.Edit";
        public const string ConfigApplyTheme = "Website.Config.ApplyTheme";
        public const string ConfigPublish    = "Website.Config.Publish";
        public const string WebsiteBuilder    = "Website.Config.Build";
        // Newsletter

        public const string NewsletterView = "Website.Newsletter.View";
        public const string NewsletterDelete = "Website.Newsletter.Delete";

        // ===== BRANDS =====
        public const string BrandsView   = "Website.Brands.View";
        public const string BrandsCreate = "Website.Brands.Create";
        public const string BrandsEdit   = "Website.Brands.Edit";
        public const string BrandsDelete = "Website.Brands.Delete";

        // ===== TESTIMONIALS =====
        public const string TestimonialsView   = "Website.Testimonials.View";
        public const string TestimonialsCreate = "Website.Testimonials.Create";
        public const string TestimonialsEdit   = "Website.Testimonials.Edit";
        public const string TestimonialsDelete = "Website.Testimonials.Delete";
        public const string TestimonialsChangeVisibility = "Website.Testimonials.ChangeVisibility";
        public const string TestimonialsReorder = "Website.Testimonials.Reorder";

        // ===== PRODUCTS =====
        public const string ProductsView      = "Website.Products.View";
        public const string ProductsCreate    = "Website.Products.Create";   // publish from Inventory
        public const string ProductsEdit      = "Website.Products.Edit";
        public const string ProductsDelete    = "Website.Products.Delete";   // unpublish / remove

        // ===== CATEGORIES =====
        public const string CategoriesView   = "Website.Categories.View";
        public const string CategoriesCreate = "Website.Categories.Create";  // publish
        public const string CategoriesEdit   = "Website.Categories.Edit";    // update, unpublish, republish
        public const string CategoriesDelete = "Website.Categories.Delete";

        // ===== COLLECTIONS =====
        public const string CollectionsView   = "Website.Collections.View";
        public const string CollectionsCreate = "Website.Collections.Create";
        public const string CollectionsEdit   = "Website.Collections.Edit";
        public const string CollectionsDelete = "Website.Collections.Delete";

        // ===== OFFERS =====
        public const string OffersView   = "Website.Offers.View";
        public const string OffersCreate = "Website.Offers.Create";
        public const string OffersEdit   = "Website.Offers.Edit";
        public const string OffersDelete = "Website.Offers.Delete";

        // ===== ORDERS =====
        public const string OrdersView = "Website.Orders.View";
        public const string OrdersEdit = "Website.Orders.Edit";   // update status

        // ===== COUPONS =====
        public const string CouponsView   = "Website.Coupons.View";
        public const string CouponsCreate = "Website.Coupons.Create";

        // ===== CUSTOMERS =====
        public const string CustomersView = "Website.Customers.View";

        // ===== ANALYTICS & DASHBOARD =====
        public const string AnalyticsView = "Website.Analytics.View";
        public const string DashboardView = "Website.Dashboard.View";

        // ===== WALLET =====
        public const string WalletView      = "Website.Wallet.View";
        public const string WalletTransactionView = "Website.WalletTransaction.View";
        public const string WalletWithdraw  = "Website.Wallet.Withdraw";
        public const string WithdrawalsView   = "Website.Withdrawals.View";

        //// ===== WITHDRAWALS (admin) =====
        //public const string WithdrawalsApprove = "Website.Withdrawals.Approve";
        //public const string WithdrawalsReject  = "Website.Withdrawals.Reject";
    }
}
