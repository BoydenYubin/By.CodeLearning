### IdentityServer4项目预览

> ​	该项目主要借鉴作者`brunobritodev`,包含用户注册，Admin管理等界面
>
> [JPProject.IdentityServer4.SSO](https://github.com/brunobritodev/JPProject.IdentityServer4.SSO)

- 项目还原过程主要通过git提交记录
- UserManagement和AdminUI主要通过Angular完成(后续可以尝试用Vue完成)

>  **以下为辅助理解部分**

#### 1、SSOContext主要DbContext类

<img src="https://raw.githubusercontent.com/BoydenYubin/PicGoHub/master/SSOContext.svg"/>

- `IConfigurationDbContext`和`IPersistedGrantDbContext`为`IdentityServer4`持久化部分的Context
- `IEventStoreContext`和`ISSOContext`是应用层持久化Context
- 继承的`IdentityDbContext`为`MiMicrosoft.AspNetCore.Identity`持久化Context
- `ISecurityKeyContext`和`IDataProtectionKeyContext`为数据保护部分持久化Context

#### 2、主要持久化对象`DbSet<XXX>`

- **Application**
  - `ISSOContext`
    - `DbSet<Template> Templates`
    - `DbSet<Email> Emails `
    - `DbSet<GlobalConfigurationSettings> GlobalConfigurationSettings`
  - `IEventStoreContext`
    - `DbSet<StoredEvent> StoredEvent`
    - `DbSet<EventDetails> StoredEventDetails`
- **Identity**
  - `IdentityDbContext`
    - `DbSet<TUserRole> UserRoles`
    - `DbSet<TRole> Roles`
    - `DbSet<TRoleClaim> RoleClaims`
  - `IdentityUserContext`
    - `DbSet<TUser> Users`
    - `DbSet<TUserClaim> UserClaims`
    -  `DbSet<TUserLogin> UserLogins`
    -  `DbSet<TUserToken> UserTokens`
- **IdentityServer4**
  - `IPersistedGrantDbContext`
    - `DbSet<PersistedGrant> PersistedGrants`
    - `DbSet<DeviceFlowCodes> DeviceFlowCodes`
  - `IConfigurationDbContext`
    - `DbSet<Client> Clients`
    - `DbSet<IdentityResource> IdentityResources`
    - `DbSet<ApiResource> ApiResources`
- **DataProtection(TBD)**
  - `IDataProtectionKeyContext`
  - `ISecurityKeyContext`

#### 3、Identity中`User`和`Role`管理

​	Identity中主要包括User和Role的管理，用于授权和认证。

<img src="https://raw.githubusercontent.com/BoydenYubin/PicGoHub/master/IdentityDiagram.svg" width=40% height=40% />

#### 4、IdentityServer4服务

##### 4.1、AuthorizeEndpoint时序

​		IdentityServer4主要认证流程集中在AuthorizeEnpoint和TokenEndpoint两个端点，其相应Services和Validator与端点路由配合进行认证相关服务和交互。以下是`AuthorizeEndpoint`端点的交互时序图。

<img src="https://raw.githubusercontent.com/BoydenYubin/PicGoHub/master/IDentityServer4.AuthorizeEndpoint%E6%97%B6%E5%BA%8F%E5%9B%BE.svg"/>

​		其中，针对重要验证部分配合设置参数做一些阐述，以解释说明在配置IdentityServer4客户端参数时需要注意的部分：

###### 4.1.1、 LoadClientAsync

​		`LoadClientAsync`函数主要针对`ClientId`以及是否是`valid client`进行校验，否则验证失败；

```c#
private async Task<AuthorizeRequestValidationResult> LoadClientAsync(ValidatedAuthorizeRequest request)
{
    //////////////////////////////////////////////////////////
    // client_id must be present
    /////////////////////////////////////////////////////////
    var clientId = request.Raw.Get(OidcConstants.AuthorizeRequest.ClientId);
    //省略部分代码
    //LogError("client_id is missing or too long", request);
    request.ClientId = clientId;
    //////////////////////////////////////////////////////////
    // check for valid client
    //////////////////////////////////////////////////////////
    var client = await _clients.FindEnabledClientByIdAsync(request.ClientId);
    //省略部分代码
    //LogError("Unknown client or not enabled", request.ClientId, request);
    request.SetClient(client);
    return Valid(request);
}
```
###### 4.1.2、LoadRequestObjectAsync

​		`LoadRequestObjectAsync`函数主要针对请求对象进行验证

```C#
private async Task<AuthorizeRequestValidationResult> LoadRequestObjectAsync(ValidatedAuthorizeRequest request)
{
    var jwtRequest = request.Raw.Get(OidcConstants.AuthorizeRequest.Request);
    var jwtRequestUri = request.Raw.Get(OidcConstants.AuthorizeRequest.RequestUri);
    
    if (jwtRequest.IsPresent() && jwtRequestUri.IsPresent())
    {
        LogError("Both request and request_uri are present", request);
        return Invalid(request, description: "Only one request parameter is allowed");
    }

    if (_options.Endpoints.EnableJwtRequestUri)
    {
        if (jwtRequestUri.IsPresent())
        {
            // 512 is from the spec
            if (jwtRequestUri.Length > 512)
            {
                LogError("request_uri is too long", request);
                return Invalid(request, error: OidcConstants.AuthorizeErrors.InvalidRequestUri, description: "request_uri is too long");
            }

            var jwt = await _jwtRequestUriHttpClient.GetJwtAsync(jwtRequestUri, request.Client);
            if (jwt.IsMissing())
            {
                LogError("no value returned from request_uri", request);
                return Invalid(request, error: OidcConstants.AuthorizeErrors.InvalidRequestUri, description: "no value returned from request_uri");
            }

            jwtRequest = jwt;
        }
    }
    else if (jwtRequestUri.IsPresent())
    {
        LogError("request_uri present but config prohibits", request);
        return Invalid(request, error: OidcConstants.AuthorizeErrors.RequestUriNotSupported);
    }

    // check length restrictions
    if (jwtRequest.IsPresent())
    {
        if (jwtRequest.Length >= _options.InputLengthRestrictions.Jwt)
        {
            LogError("request value is too long", request);
            return Invalid(request, error: OidcConstants.AuthorizeErrors.InvalidRequestObject, description: "Invalid request value");
        }
    }

    request.RequestObject = jwtRequest;
    return Valid(request);
}
```
###### 4.1.3、ValidateRequestObjectAsync

```C#
private async Task<AuthorizeRequestValidationResult> ValidateRequestObjectAsync(ValidatedAuthorizeRequest request)
{
    //////////////////////////////////////////////////////////
    // validate request object
    /////////////////////////////////////////////////////////
    if (request.RequestObject.IsPresent())
    {
        // validate the request JWT for this client
        var jwtRequestValidationResult = await _jwtRequestValidator.ValidateAsync(request.Client, request.RequestObject);
        if (jwtRequestValidationResult.IsError)
        {
            LogError("request JWT validation failure", request);
            return Invalid(request, error: OidcConstants.AuthorizeErrors.InvalidRequestObject, description: "Invalid JWT request");
        }

        // validate response_type match
        var responseType = request.Raw.Get(OidcConstants.AuthorizeRequest.ResponseType);
        if (responseType != null)
        {
            if (jwtRequestValidationResult.Payload.TryGetValue(OidcConstants.AuthorizeRequest.ResponseType, out var payloadResponseType))
            {
                if (payloadResponseType != responseType)
                {
                    LogError("response_type in JWT payload does not match response_type in request", request);
                    return Invalid(request, description: "Invalid JWT request");
                }
            }
        }

        // validate client_id mismatch
        if (jwtRequestValidationResult.Payload.TryGetValue(OidcConstants.AuthorizeRequest.ClientId, out var payloadClientId))
        {
            if (!string.Equals(request.Client.ClientId, payloadClientId, StringComparison.Ordinal))
            {
                LogError("client_id in JWT payload does not match client_id in request", request);
                return Invalid(request, description: "Invalid JWT request");
            }
        }
        else
        {
            LogError("client_id is missing in JWT payload", request);
            return Invalid(request, error: OidcConstants.AuthorizeErrors.InvalidRequestObject, description: "Invalid JWT request");
        }

        var ignoreKeys = new[]
        {
            JwtClaimTypes.Issuer,
            JwtClaimTypes.Audience
        };

        // merge jwt payload values into original request parameters
        foreach (var key in jwtRequestValidationResult.Payload.Keys)
        {
            if (ignoreKeys.Contains(key)) continue; 
            var value = jwtRequestValidationResult.Payload[key];
            var qsValue = request.Raw.Get(key);
            if (qsValue != null)
            {
                if (!string.Equals(value, qsValue, StringComparison.Ordinal))
                {
                    LogError("parameter mismatch between request object and query string parameter.", request);
                    return Invalid(request, description: "Parameter mismatch in JWT request");
                }
            }
            request.Raw.Set(key, value);
        }
        request.RequestObjectValues = jwtRequestValidationResult.Payload;
    }
    return Valid(request);
}
```
###### 4.1.4、ValidateClientAsync

```C#
private async Task<AuthorizeRequestValidationResult> ValidateClientAsync(ValidatedAuthorizeRequest request)
{
    //////////////////////////////////////////////////////////
    // check request object requirement
    //////////////////////////////////////////////////////////
    if (request.Client.RequireRequestObject)
    {
        if (!request.RequestObjectValues.Any())
        {
            return Invalid(request, description: "Client must use request object, but no request or request_uri parameter present");
        }
    }

    //////////////////////////////////////////////////////////
    // redirect_uri must be present, and a valid uri
    //////////////////////////////////////////////////////////
    var redirectUri = request.Raw.Get(OidcConstants.AuthorizeRequest.RedirectUri);

    if (redirectUri.IsMissingOrTooLong(_options.InputLengthRestrictions.RedirectUri))
    {
        LogError("redirect_uri is missing or too long", request);
        return Invalid(request, description: "Invalid redirect_uri");
    }

    if (!Uri.TryCreate(redirectUri, UriKind.Absolute, out _))
    {
        LogError("malformed redirect_uri", redirectUri, request);
        return Invalid(request, description: "Invalid redirect_uri");
    }

    //////////////////////////////////////////////////////////
    // check if client protocol type is oidc
    //////////////////////////////////////////////////////////
    if (request.Client.ProtocolType != IdentityServerConstants.ProtocolTypes.OpenIdConnect)
    {
        LogError("Invalid protocol type for OIDC authorize endpoint", request.Client.ProtocolType, request);
        return Invalid(request, OidcConstants.AuthorizeErrors.UnauthorizedClient, description: "Invalid protocol");
    }

    //////////////////////////////////////////////////////////
    // check if redirect_uri is valid
    //////////////////////////////////////////////////////////
    if (await _uriValidator.IsRedirectUriValidAsync(redirectUri, request.Client) == false)
    {
        LogError("Invalid redirect_uri", redirectUri, request);
        return Invalid(request, OidcConstants.AuthorizeErrors.InvalidRequest, "Invalid redirect_uri");
    }

    request.RedirectUri = redirectUri;

    return Valid(request);
}
```
###### 4.1.5、ValidateCoreParameters

```C#
private AuthorizeRequestValidationResult ValidateCoreParameters(ValidatedAuthorizeRequest request)
{
    //////////////////////////////////////////////////////////
    // check state
    //////////////////////////////////////////////////////////
    var state = request.Raw.Get(OidcConstants.AuthorizeRequest.State);
    if (state.IsPresent())
    {
        request.State = state;
    }

    //////////////////////////////////////////////////////////
    // response_type must be present and supported
    //////////////////////////////////////////////////////////
    var responseType = request.Raw.Get(OidcConstants.AuthorizeRequest.ResponseType);
    if (responseType.IsMissing())
    {
        LogError("Missing response_type", request);
        return Invalid(request, OidcConstants.AuthorizeErrors.UnsupportedResponseType, "Missing response_type");
    }

    // The responseType may come in in an unconventional order.
    // Use an IEqualityComparer that doesn't care about the order of multiple values.
    // Per https://tools.ietf.org/html/rfc6749#section-3.1.1 -
    // 'Extension response types MAY contain a space-delimited (%x20) list of
    // values, where the order of values does not matter (e.g., response
    // type "a b" is the same as "b a").'
    // http://openid.net/specs/oauth-v2-multiple-response-types-1_0-03.html#terminology -
    // 'If a response type contains one of more space characters (%20), it is compared
    // as a space-delimited list of values in which the order of values does not matter.'
    if (!Constants.SupportedResponseTypes.Contains(responseType, _responseTypeEqualityComparer))
    {
        LogError("Response type not supported", responseType, request);
        return Invalid(request, OidcConstants.AuthorizeErrors.UnsupportedResponseType, "Response type not supported");
    }

    // Even though the responseType may have come in in an unconventional order,
    // we still need the request's ResponseType property to be set to the
    // conventional, supported response type.
    request.ResponseType = Constants.SupportedResponseTypes.First(
        supportedResponseType => _responseTypeEqualityComparer.Equals(supportedResponseType, responseType));

    //////////////////////////////////////////////////////////
    // match response_type to grant type
    //////////////////////////////////////////////////////////
    request.GrantType = Constants.ResponseTypeToGrantTypeMapping[request.ResponseType];

    // set default response mode for flow; this is needed for any client error processing below
    request.ResponseMode = Constants.AllowedResponseModesForGrantType[request.GrantType].First();

    //////////////////////////////////////////////////////////
    // check if flow is allowed at authorize endpoint
    //////////////////////////////////////////////////////////
    if (!Constants.AllowedGrantTypesForAuthorizeEndpoint.Contains(request.GrantType))
    {
        LogError("Invalid grant type", request.GrantType, request);
        return Invalid(request, description: "Invalid response_type");
    }

    //////////////////////////////////////////////////////////
    // check if PKCE is required and validate parameters
    //////////////////////////////////////////////////////////
    if (request.GrantType == GrantType.AuthorizationCode || request.GrantType == GrantType.Hybrid)
    {
        _logger.LogDebug("Checking for PKCE parameters");

        /////////////////////////////////////////////////////////////////////////////
        // validate code_challenge and code_challenge_method
        /////////////////////////////////////////////////////////////////////////////
        var proofKeyResult = ValidatePkceParameters(request);

        if (proofKeyResult.IsError)
        {
            return proofKeyResult;
        }
    }

    //////////////////////////////////////////////////////////
    // check response_mode parameter and set response_mode
    //////////////////////////////////////////////////////////

    // check if response_mode parameter is present and valid
    var responseMode = request.Raw.Get(OidcConstants.AuthorizeRequest.ResponseMode);
    if (responseMode.IsPresent())
    {
        if (Constants.SupportedResponseModes.Contains(responseMode))
        {
            if (Constants.AllowedResponseModesForGrantType[request.GrantType].Contains(responseMode))
            {
                request.ResponseMode = responseMode;
            }
            else
            {
                LogError("Invalid response_mode for response_type", responseMode, request);
                return Invalid(request, OidcConstants.AuthorizeErrors.InvalidRequest, description: "Invalid response_mode for response_type");
            }
        }
        else
        {
            LogError("Unsupported response_mode", responseMode, request);
            return Invalid(request, OidcConstants.AuthorizeErrors.UnsupportedResponseType, description: "Invalid response_mode");
        }
    }


    //////////////////////////////////////////////////////////
    // check if grant type is allowed for client
    //////////////////////////////////////////////////////////
    if (!request.Client.AllowedGrantTypes.Contains(request.GrantType))
    {
        LogError("Invalid grant type for client", request.GrantType, request);
        return Invalid(request, OidcConstants.AuthorizeErrors.UnauthorizedClient, "Invalid grant type for client");
    }

    //////////////////////////////////////////////////////////
    // check if response type contains an access token,
    // and if client is allowed to request access token via browser
    //////////////////////////////////////////////////////////
    var responseTypes = responseType.FromSpaceSeparatedString();
    if (responseTypes.Contains(OidcConstants.ResponseTypes.Token))
    {
        if (!request.Client.AllowAccessTokensViaBrowser)
        {
            LogError("Client requested access token - but client is not configured to receive access tokens via browser", request);
            return Invalid(request, description: "Client not configured to receive access tokens via browser");
        }
    }

    return Valid(request);
}
```
###### 4.1.6、ValidateScopeAsync

```C#
private async Task<AuthorizeRequestValidationResult> ValidateScopeAsync(ValidatedAuthorizeRequest request)
{
    //////////////////////////////////////////////////////////
    // scope must be present
    //////////////////////////////////////////////////////////
    var scope = request.Raw.Get(OidcConstants.AuthorizeRequest.Scope);
    if (scope.IsMissing())
    {
        LogError("scope is missing", request);
        return Invalid(request, description: "Invalid scope");
    }

    if (scope.Length > _options.InputLengthRestrictions.Scope)
    {
        LogError("scopes too long.", request);
        return Invalid(request, description: "Invalid scope");
    }

    request.RequestedScopes = scope.FromSpaceSeparatedString().Distinct().ToList();

    if (request.RequestedScopes.Contains(IdentityServerConstants.StandardScopes.OpenId))
    {
        request.IsOpenIdRequest = true;
    }

    //////////////////////////////////////////////////////////
    // check scope vs response_type plausability
    //////////////////////////////////////////////////////////
    var requirement = Constants.ResponseTypeToScopeRequirement[request.ResponseType];
    if (requirement == Constants.ScopeRequirement.Identity ||
        requirement == Constants.ScopeRequirement.IdentityOnly)
    {
        if (request.IsOpenIdRequest == false)
        {
            LogError("response_type requires the openid scope", request);
            return Invalid(request, description: "Missing openid scope");
        }
    }

    //////////////////////////////////////////////////////////
    // check if scopes are valid/supported and check for resource scopes
    //////////////////////////////////////////////////////////
    var validatedResources = await _resourceValidator.ValidateRequestedResourcesAsync(new ResourceValidationRequest
    {
        Client = request.Client,
        Scopes = request.RequestedScopes
    });

    if (!validatedResources.Succeeded)
    {
        return Invalid(request, OidcConstants.AuthorizeErrors.InvalidScope, "Invalid scope");
    }

    if (validatedResources.Resources.IdentityResources.Any() && !request.IsOpenIdRequest)
    {
        LogError("Identity related scope requests, but no openid scope", request);
        return Invalid(request, OidcConstants.AuthorizeErrors.InvalidScope, "Identity scopes requested, but openid scope is missing");
    }

    if (validatedResources.Resources.ApiScopes.Any())
    {
        request.IsApiResourceRequest = true;
    }

    //////////////////////////////////////////////////////////
    // check id vs resource scopes and response types plausability
    //////////////////////////////////////////////////////////
    var responseTypeValidationCheck = true;
    switch (requirement)
    {
        case Constants.ScopeRequirement.Identity:
            if (!validatedResources.Resources.IdentityResources.Any())
            {
                _logger.LogError("Requests for id_token response type must include identity scopes");
                responseTypeValidationCheck = false;
            }
            break;
        case Constants.ScopeRequirement.IdentityOnly:
            if (!validatedResources.Resources.IdentityResources.Any() || validatedResources.Resources.ApiScopes.Any())
            {
                _logger.LogError("Requests for id_token response type only must not include resource scopes");
                responseTypeValidationCheck = false;
            }
            break;
        case Constants.ScopeRequirement.ResourceOnly:
            if (validatedResources.Resources.IdentityResources.Any() || !validatedResources.Resources.ApiScopes.Any())
            {
                _logger.LogError("Requests for token response type only must include resource scopes, but no identity scopes.");
                responseTypeValidationCheck = false;
            }
            break;
    }

    if (!responseTypeValidationCheck)
    {
        return Invalid(request, OidcConstants.AuthorizeErrors.InvalidScope, "Invalid scope for response type");
    }

    request.ValidatedResources = validatedResources;

    return Valid(request);
}
```
###### 4.1.7、ValidateOptionalParametersAsync
```C#
private async Task<AuthorizeRequestValidationResult> ValidateOptionalParametersAsync(ValidatedAuthorizeRequest request)
{
    //////////////////////////////////////////////////////////
    // check nonce
    //////////////////////////////////////////////////////////
    var nonce = request.Raw.Get(OidcConstants.AuthorizeRequest.Nonce);
    if (nonce.IsPresent())
    {
        if (nonce.Length > _options.InputLengthRestrictions.Nonce)
        {
            LogError("Nonce too long", request);
            return Invalid(request, description: "Invalid nonce");
        }

        request.Nonce = nonce;
    }
    else
    {
        if (request.GrantType == GrantType.Implicit ||
            request.GrantType == GrantType.Hybrid)
        {
            // only openid requests require nonce
            if (request.IsOpenIdRequest)
            {
                LogError("Nonce required for implicit and hybrid flow with openid scope", request);
                return Invalid(request, description: "Invalid nonce");
            }
        }
    }


    //////////////////////////////////////////////////////////
    // check prompt
    //////////////////////////////////////////////////////////
    var prompt = request.Raw.Get(OidcConstants.AuthorizeRequest.Prompt);
    if (prompt.IsPresent())
    {
        var prompts = prompt.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (prompts.All(p => Constants.SupportedPromptModes.Contains(p)))
        {
            if (prompts.Contains(OidcConstants.PromptModes.None) && prompts.Length > 1)
            {
                LogError("prompt contains 'none' and other values. 'none' should be used by itself.", request);
                return Invalid(request, description: "Invalid prompt");
            }

            request.PromptModes = prompts;
        }
        else
        {
            _logger.LogDebug("Unsupported prompt mode - ignored: " + prompt);
        }
    }

    //////////////////////////////////////////////////////////
    // check ui locales
    //////////////////////////////////////////////////////////
    var uilocales = request.Raw.Get(OidcConstants.AuthorizeRequest.UiLocales);
    if (uilocales.IsPresent())
    {
        if (uilocales.Length > _options.InputLengthRestrictions.UiLocale)
        {
            LogError("UI locale too long", request);
            return Invalid(request, description: "Invalid ui_locales");
        }

        request.UiLocales = uilocales;
    }

    //////////////////////////////////////////////////////////
    // check display
    //////////////////////////////////////////////////////////
    var display = request.Raw.Get(OidcConstants.AuthorizeRequest.Display);
    if (display.IsPresent())
    {
        if (Constants.SupportedDisplayModes.Contains(display))
        {
            request.DisplayMode = display;
        }

        _logger.LogDebug("Unsupported display mode - ignored: " + display);
    }

    //////////////////////////////////////////////////////////
    // check max_age
    //////////////////////////////////////////////////////////
    var maxAge = request.Raw.Get(OidcConstants.AuthorizeRequest.MaxAge);
    if (maxAge.IsPresent())
    {
        if (int.TryParse(maxAge, out var seconds))
        {
            if (seconds >= 0)
            {
                request.MaxAge = seconds;
            }
            else
            {
                LogError("Invalid max_age.", request);
                return Invalid(request, description: "Invalid max_age");
            }
        }
        else
        {
            LogError("Invalid max_age.", request);
            return Invalid(request, description: "Invalid max_age");
        }
    }

    //////////////////////////////////////////////////////////
    // check login_hint
    //////////////////////////////////////////////////////////
    var loginHint = request.Raw.Get(OidcConstants.AuthorizeRequest.LoginHint);
    if (loginHint.IsPresent())
    {
        if (loginHint.Length > _options.InputLengthRestrictions.LoginHint)
        {
            LogError("Login hint too long", request);
            return Invalid(request, description: "Invalid login_hint");
        }

        request.LoginHint = loginHint;
    }

    //////////////////////////////////////////////////////////
    // check acr_values
    //////////////////////////////////////////////////////////
    var acrValues = request.Raw.Get(OidcConstants.AuthorizeRequest.AcrValues);
    if (acrValues.IsPresent())
    {
        if (acrValues.Length > _options.InputLengthRestrictions.AcrValues)
        {
            LogError("Acr values too long", request);
            return Invalid(request, description: "Invalid acr_values");
        }

        request.AuthenticationContextReferenceClasses = acrValues.FromSpaceSeparatedString().Distinct().ToList();
    }

    //////////////////////////////////////////////////////////
    // check custom acr_values: idp
    //////////////////////////////////////////////////////////
    var idp = request.GetIdP();
    if (idp.IsPresent())
    {
        // if idp is present but client does not allow it, strip it from the request message
        if (request.Client.IdentityProviderRestrictions != null && request.Client.IdentityProviderRestrictions.Any())
        {
            if (!request.Client.IdentityProviderRestrictions.Contains(idp))
            {
                _logger.LogWarning("idp requested ({idp}) is not in client restriction list.", idp);
                request.RemoveIdP();
            }
        }
    }

    //////////////////////////////////////////////////////////
    // check session cookie
    //////////////////////////////////////////////////////////
    if (_options.Endpoints.EnableCheckSessionEndpoint)
    {
        if (request.Subject.IsAuthenticated())
        {
            var sessionId = await _userSession.GetSessionIdAsync();
            if (sessionId.IsPresent())
            {
                request.SessionId = sessionId;
            }
            else
            {
                LogError("Check session endpoint enabled, but SessionId is missing", request);
            }
        }
        else
        {
            request.SessionId = ""; // empty string for anonymous users
        }
    }

    return Valid(request);
}
```

###### 4.1.8、CreateCodeFlowResponseAsync



```C#
protected virtual async Task<AuthorizeResponse> CreateCodeFlowResponseAsync(ValidatedAuthorizeRequest request)
{
    Logger.LogDebug("Creating Authorization Code Flow response.");

    var code = await CreateCodeAsync(request);
    var id = await AuthorizationCodeStore.StoreAuthorizationCodeAsync(code);

    var response = new AuthorizeResponse
    {
        Request = request,
        Code = id,
        SessionState = request.GenerateSessionStateValue()
    };

    return response;
}
```

###### 4.1.9、CreateImplicitFlowResponseAsync

```c#
protected virtual async Task<AuthorizeResponse> CreateImplicitFlowResponseAsync(ValidatedAuthorizeRequest request, string authorizationCode = null)
{
    Logger.LogDebug("Creating Implicit Flow response.");

    string accessTokenValue = null;
    int accessTokenLifetime = 0;

    var responseTypes = request.ResponseType.FromSpaceSeparatedString();

    if (responseTypes.Contains(OidcConstants.ResponseTypes.Token))
    {
        var tokenRequest = new TokenCreationRequest
        {
            Subject = request.Subject,
            ValidatedResources = request.ValidatedResources,

            ValidatedRequest = request
        };

        var accessToken = await TokenService.CreateAccessTokenAsync(tokenRequest);
        accessTokenLifetime = accessToken.Lifetime;

        accessTokenValue = await TokenService.CreateSecurityTokenAsync(accessToken);
    }

    string jwt = null;
    if (responseTypes.Contains(OidcConstants.ResponseTypes.IdToken))
            {
                string stateHash = null;
                if (request.State.IsPresent())
                {
                    var credential = await KeyMaterialService.GetSigningCredentialsAsync(request.Client.AllowedIdentityTokenSigningAlgorithms);
                    if (credential == null)
                    {
                        throw new InvalidOperationException("No signing credential is configured.");
                    }

                    var algorithm = credential.Algorithm;
                    stateHash = CryptoHelper.CreateHashClaimValue(request.State, algorithm);
                }

                var tokenRequest = new TokenCreationRequest
                {
                    ValidatedRequest = request,
                    Subject = request.Subject,
                    ValidatedResources = request.ValidatedResources,
                    Nonce = request.Raw.Get(OidcConstants.AuthorizeRequest.Nonce),
                    IncludeAllIdentityClaims = !request.AccessTokenRequested,
                    AccessTokenToHash = accessTokenValue,
                    AuthorizationCodeToHash = authorizationCode,
                    StateHash = stateHash
                };

                var idToken = await TokenService.CreateIdentityTokenAsync(tokenRequest);
                jwt = await TokenService.CreateSecurityTokenAsync(idToken);
            }
    var response = new AuthorizeResponse
    {
        Request = request,
        AccessToken = accessTokenValue,
        AccessTokenLifetime = accessTokenLifetime,
        IdentityToken = jwt,
        SessionState = request.GenerateSessionStateValue()
    };
    return response;
}
```

4.1.10、CreateHybridFlowResponseAsync

```C#
 protected virtual async Task<AuthorizeResponse> CreateHybridFlowResponseAsync(ValidatedAuthorizeRequest request)
 {
     Logger.LogDebug("Creating Hybrid Flow response.");

     var code = await CreateCodeAsync(request);
     var id = await AuthorizationCodeStore.StoreAuthorizationCodeAsync(code);

     var response = await CreateImplicitFlowResponseAsync(request, id);
     response.Code = id;
     return response;
 }
```

