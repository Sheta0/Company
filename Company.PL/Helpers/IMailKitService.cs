namespace Company.PL.Helpers
{
    public interface IMailKitService
    {
        Task<bool> SendEmailAsync(Email email);
    }
}
