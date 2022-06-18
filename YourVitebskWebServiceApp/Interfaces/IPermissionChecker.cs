namespace YourVitebskWebServiceApp.Interfaces
{
    public interface IPermissionChecker
    {
        bool CheckRolePermission(string userEmail, string permission);
    }
}
