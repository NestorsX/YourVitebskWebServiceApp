namespace YourVitebskWebServiceApp.APIModels
{
    public class User
    {
        public int? UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsVisible { get; set; }
        public byte[] Image { get; set; }
    }
}
