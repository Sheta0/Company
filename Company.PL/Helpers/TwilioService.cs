using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Company.PL.Helpers
{
    public class TwilioService : ITwilioService
    {
        private readonly IOptions<TwilioSettings> _twilioSettings;

        public TwilioService(IOptions<TwilioSettings> twilioSettings)
        {
            _twilioSettings = twilioSettings;
        }
        public async Task<MessageResource> SendSMSAsync(SMS sms)
        {
            // Initialize Connection

            TwilioClient.Init(_twilioSettings.Value.AccountSID, _twilioSettings.Value.AuthToken);

            // build message
            var message = await MessageResource.CreateAsync(
                body: sms.Body,
                from: new Twilio.Types.PhoneNumber(_twilioSettings.Value.PhoneNumber),
                to: new Twilio.Types.PhoneNumber(sms.To)
            );
            return message;
        }
    }
}
