using Microsoft.Extensions.Options;

namespace WebAPI.Configuration;

public class SendGridSettingsValidator : IValidateOptions<SendGridSettings>
{
    public ValidateOptionsResult Validate(string? name, SendGridSettings options)
    {
        // SendGrid is optional - only validate if ApiKey is set (indicating email is being used)
        if (!string.IsNullOrWhiteSpace(options.ApiKey) && string.IsNullOrWhiteSpace(options.FromEmail))
        {
            return ValidateOptionsResult.Fail("SendGrid FromEmail is required when ApiKey is set.");
        }

        return ValidateOptionsResult.Success;
    }
}

