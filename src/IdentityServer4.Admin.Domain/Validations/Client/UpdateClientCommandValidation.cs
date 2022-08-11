using ByLearning.Admin.Domain.Commands.Clients;

namespace ByLearning.Admin.Domain.Validations.Client
{
    public class UpdateClientCommandValidation : ClientValidation<UpdateClientCommand>
    {
        public UpdateClientCommandValidation()
        {
            ValidateGrantType();
            ValidateOldClientId();
            ValidateIdentityTokenLifetime();
            ValidateAccessTokenLifetime();
            ValidateAuthorizationCodeLifetime();
            ValidateSlidingRefreshTokenLifetime();
            ValidateDeviceCodeLifetime();
            ValidateAbsoluteRefreshTokenLifetime();
            ValidatePostLogoutTrailingSlash();
            ValidateClientUriTrailingSlash();
        }
    }
}