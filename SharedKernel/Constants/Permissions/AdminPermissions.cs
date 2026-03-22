namespace SharedKernel.Constants.Permissions
{
    /// <summary>
    /// Identity/Admin Module Permissions
    /// Strictly aligned with AccountManagement controllers
    /// </summary>
    public static class AdminPermissions
    {
        public const string Module = "Admin";

        // ===== USERS =====
        public const string UsersView = "Admin.Users.View";
        public const string UsersCreate = "Admin.Users.Create";
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
    }
}