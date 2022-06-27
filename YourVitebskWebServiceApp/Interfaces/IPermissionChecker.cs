namespace YourVitebskWebServiceApp.Interfaces
{
    public interface IPermissionChecker
    {
        bool CheckRolePermission(string permission);
    }
}
