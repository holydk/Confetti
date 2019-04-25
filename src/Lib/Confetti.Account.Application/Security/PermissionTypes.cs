namespace Confetti.Account.Application.Security
{
    public class PermissionTypes
    {
        #region User category of permissions
        
        public const string ViewUsers = "users.view";
        public const string ManageUsers = "users.manage";

        #endregion

        #region Roles category of permissions
            
        public const string ViewRoles = "roles.view";
        public const string ManageRoles = "roles.manage";
        public const string AssignRoles = "roles.assign";

        #endregion
    }
}