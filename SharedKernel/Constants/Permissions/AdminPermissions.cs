namespace SharedKernel.Constants.Permissions
{
    /// <summary>
    /// Identity/Admin Module Permissions
    /// Based on actual endpoints from Identity.Api controllers
    /// </summary>
    public static class AdminPermissions
    {
        public const string Module = "Admin";

        // ===== USERS =====
        public const string UsersView = "Admin.Users.View";
        public const string UsersCreate = "Admin.Users.Create";
        public const string UsersEdit = "Admin.Users.Edit";
        public const string UsersDelete = "Admin.Users.Delete";
        public const string UsersResetPassword = "Admin.Users.ResetPassword";
        public const string UsersAssignRoles = "Admin.Users.AssignRoles";
        public const string UsersRemoveRoles = "Admin.Users.RemoveRoles";

        // ===== ROLES =====
        public const string RolesView = "Admin.Roles.View";
        public const string RolesCreate = "Admin.Roles.Create";
        public const string RolesEdit = "Admin.Roles.Edit";
        public const string RolesDelete = "Admin.Roles.Delete";
        public const string RolesAssignPermissions = "Admin.Roles.AssignPermissions";
        public const string RolesRemovePermissions = "Admin.Roles.RemovePermissions";

        // ===== PERMISSIONS =====
        public const string PermissionsView = "Admin.Permissions.View";
        public const string PermissionsAssign = "Admin.Permissions.Assign";
        public const string PermissionsRemove = "Admin.Permissions.Remove";

        // ===== TENANTS =====
        public const string TenantsView = "Admin.Tenants.View";
        public const string TenantsCreate = "Admin.Tenants.Create";
        public const string TenantsEdit = "Admin.Tenants.Edit";
        public const string TenantsDelete = "Admin.Tenants.Delete";
        public const string TenantsManageUsers = "Admin.Tenants.ManageUsers";

        // ===== ACCOUNTS =====
        public const string AccountsView = "Admin.Accounts.View";
        public const string AccountsEdit = "Admin.Accounts.Edit";
    }
}
