namespace membership.Service.internalinterface
{
    public interface IEmailService
{
    Task SendEmailAsync(string email, string subject, string message);
}

}