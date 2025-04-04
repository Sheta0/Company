using Twilio.Rest.Api.V2010.Account;

namespace Company.PL.Helpers
{
    public interface ITwilioService
    {
        Task<MessageResource> SendSMSAsync(SMS sms);
    }
}
