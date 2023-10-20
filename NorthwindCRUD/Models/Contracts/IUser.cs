namespace NorthwindCRUD.Models.Contracts
{
    public interface IUser
    {
        string Email { get; set; }

        string Password { get; set; }
    }
}
