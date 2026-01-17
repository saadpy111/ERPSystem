namespace SharedKernel.Constants.Permissions
{
    /// <summary>
    /// Website Module Permissions
    /// Based on actual endpoints from Website.Api controllers
    /// </summary>
    public static class WebsitePermissions
    {
        public const string Module = "Website";

        // ===== THEMES =====
        public const string ThemesView = "Website.Themes.View";
        public const string ThemesCreate = "Website.Themes.Create";
        public const string ThemesEdit = "Website.Themes.Edit";
        public const string ThemesDelete = "Website.Themes.Delete";

        // ===== WEBSITE CONFIG =====
        public const string ConfigView = "Website.Config.View";
        public const string ConfigEdit = "Website.Config.Edit";
        public const string ConfigApplyTheme = "Website.Config.ApplyTheme";
        public const string ConfigPublish = "Website.Config.Publish";

        // ===== PRODUCTS (E-Commerce) =====
        public const string ProductsView = "Website.Products.View";
        public const string ProductsPublish = "Website.Products.Publish";
        public const string ProductsEdit = "Website.Products.Edit";
        public const string ProductsUnpublish = "Website.Products.Unpublish";

        // ===== CATEGORIES (E-Commerce) =====
        public const string CategoriesView = "Website.Categories.View";
        public const string CategoriesPublish = "Website.Categories.Publish";
        public const string CategoriesEdit = "Website.Categories.Edit";
        public const string CategoriesDelete = "Website.Categories.Delete";
        public const string CategoriesManage = "Website.Categories.Manage";

        // ===== COLLECTIONS (E-Commerce) =====
        public const string CollectionsView = "Website.Collections.View";
        public const string CollectionsManage = "Website.Collections.Manage";

        // ===== OFFERS (E-Commerce) =====
        public const string OffersView = "Website.Offers.View";
        public const string OffersManage = "Website.Offers.Manage";

        // ===== ORDERS (E-Commerce) =====
        public const string OrdersView = "Website.Orders.View";
        public const string OrdersManage = "Website.Orders.Manage";
    }
}

